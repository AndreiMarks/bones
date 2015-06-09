using UnityEngine;
using System.Collections.Generic;
using System.Collections;


namespace UnityDesigner
{

	[RequireComponent (typeof (MeshFilter))]
	[RequireComponent (typeof (MeshRenderer))]
	[ExecuteInEditMode]
	abstract public class Shape : MonoBehaviour 
	{

		[Range(0,1)]
		[SerializeField]

		float _fidelity;

		[SerializeField]
		float _depth = 0;

		[SerializeField]
		private Vector2 _size = Vector2.one;	

		private MeshFilter _meshFilter;
		private MeshRenderer _meshRenderer;
		
		[SerializeField]
		[HideInInspector]
		private int _instanceId;
		
		[SerializeField]
		[HideInInspector]
		private Mesh _mesh;
		
		public string[] sharedSerializedPropertyNames = new string[]{ "_fidelity", "_depth", "_size" };
		private float _aspectRatio;

		float minSegmentsPerUnit = 5;
		float maxSegmentsPerUnit = 30;
		
		Drawer _drawer = new Drawer();

		private static Material _sharedMaterial;

		[HideInInspector]
		public ShapeFill allFill = new ShapeFill();
		[HideInInspector]
		public ShapeFill frontFill = new ShapeFill();
		[HideInInspector]
		public ShapeFill sideFill = new ShapeFill();
		[HideInInspector]
		public ShapeFill backFill = new ShapeFill();
		

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Color> colors = new List<Color>();
		List<Vector2> uv = new List<Vector2>();
		List<Face> faces = new List<Face>();

		public virtual void DrawOutline() { }
		public virtual void BuildFaces() { }
		public virtual Vector3 GetOffset(){ return Vector3.zero; }

		public static T CreateShape<T>( Vector3 position ) where T : Shape 
		{
			GameObject go = new GameObject( typeof(T).ToString() );
			go.transform.position = position;
			T shape = go.AddComponent<T>();
			return shape;
		}


		void Start(){
			if( _instanceId != GetInstanceID() )
			{
				_mesh = null;
				_instanceId = GetInstanceID();
			}
			
			Draw();
		}
		

			
		public void Draw() {

			#if UNITY_EDITOR
			if (UnityEditor.EditorUtility.IsPersistent(this)) return;
			#endif
			
			if(_meshFilter == null || _mesh == null) GetReferences();
			ClearMesh();
			GetMesh( _mesh );
			
			UpdateMaterials();
			
		}

		void GetMesh( Mesh mesh )
		{
			ClearArrays();
			DrawOutline();
			BuildFaces();

			mesh.vertices = vertices.ToArray();

			if( useMultiFaceFill )
			{
				mesh.subMeshCount = faces.Count;
				for(int i = 0; i < faces.Count; i++)
				{
					mesh.SetTriangles( faces[i].GetTriangles().ToArray(), i );
				}
			} else {
				mesh.subMeshCount = 1;
				mesh.triangles = triangles.ToArray();
			}
			
			mesh.colors = colors.ToArray();
			mesh.uv = uv.ToArray();

			mesh.RecalculateNormals();

		}

		public void ClearArrays()
		{
			faces.Clear();
			vertices.Clear();
			triangles.Clear();
			colors.Clear();
			uv.Clear();
		}


		public void AddFace( Face face )
		{
			faces.Add( face );

			vertices.AddRange( face.GetVertices() );
			triangles.AddRange( face.GetTriangles() );
			colors.AddRange( face.GetColors( size ) );
			uv.AddRange( face.GetUV( size ) );
		}

		public void UpdateMaterials()
		{
			if(_meshRenderer == null) GetReferences();
			

			Material[] materials = _meshRenderer.sharedMaterials;
			Material[] newMaterials = new Material[ useMultiFaceFill ? 3 : 1 ];

			if( useMultiFaceFill )
			{
				
				ShapeFillSide[] sides = new ShapeFillSide[]{ ShapeFillSide.Front, ShapeFillSide.Back, ShapeFillSide.Side };

				for(int i = 0; i < 3; i++)
				{
					if( i >= materials.Length || IsMaterialGenerated( materials[i]) )
					{
						newMaterials[i] = GetFillByType( sides[i] ).material;
					} else {
						newMaterials[i] = materials[i];
					}
				}

			} else {
				if( materials.Length == 0 || IsMaterialGenerated( materials[0] ) )
				{
					newMaterials[0] = allFill.material;
				} else {
					newMaterials[0] = materials[0];
				}
			}

			_meshRenderer.sharedMaterials = newMaterials;
		}

		public void SetColor( Color value ) 
		{
			color = value;
		}
		
		public void ClearMesh()
		{
			_mesh.Clear();
		}

		bool IsMaterialGenerated(Material material)
		{
			if( material == null ) return true;
			else return material.GetTag("ShapesGenerated", false, "False") == "true";
		}

		public virtual void ResizeToBounds( Bounds previous, Bounds bounds )
		{
			Bounds currentBounds = this.shapeBounds;
			Vector3 sizeRatio = new Vector3( bounds.size.x / previous.size.x, bounds.size.y / previous.size.y, 1 );
			Vector3 centerOffset = transform.position - previous.center;

			Vector3 newOffset = Vector3.Scale( centerOffset, sizeRatio );
			Vector3 newSize = Vector3.Scale( currentBounds.size, sizeRatio );

			Vector3 newPosition = bounds.center + newOffset;

			size = newSize;
			transform.position = newPosition;
		}
		
		



		public int GetSegmentsForRadius( float radius, float arcAngle )
		{
			float arcLength = Mathf.Deg2Rad * arcAngle * radius;
			
			float segmentsPerUnit = Mathf.Lerp( minSegmentsPerUnit, maxSegmentsPerUnit, fidelity );
			return (int)( arcLength * segmentsPerUnit / radius);
		}

		private void GetReferences(){
			if(_mesh == null)
			{
				_mesh = new Mesh();
			//	_mesh.MarkDynamic();
			}

			if(_meshFilter == null ) _meshFilter = GetComponent<MeshFilter>();
			if(_meshFilter.sharedMesh != _mesh) _meshFilter.sharedMesh = _mesh;
			
			_meshRenderer = GetComponent<MeshRenderer>();
			
		}

		public ShapeFill GetFillByType( ShapeFillSide side )
		{
			ShapeFill result = allFill;

			switch( side )
			{
				case ShapeFillSide.All:
					result = allFill;
				break;

				case ShapeFillSide.Front:
					result = frontFill;
				break;
			
				case ShapeFillSide.Side:
					result = sideFill;
				break;
			
				case ShapeFillSide.Back:
					result = backFill;
				break;
			}
			
			return result;
		}

		public ShapeFill GetFill( ShapeFillSide side )
		{
			ShapeFill result = allFill;

			if( useMultiFaceFill )
			{
				switch( side )
				{
					case ShapeFillSide.Front:
						result = frontFill;
					break;
				
					case ShapeFillSide.Side:
						result = sideFill;
					break;
				
					case ShapeFillSide.Back:
						result = backFill;
					break;
				}
			}
			
			return result;
		}



		public virtual Bounds localBounds
		{
			get
			{
				Quaternion rotation = transform.rotation;
				transform.rotation = Quaternion.identity;
				Bounds bounds = this.bounds;
				transform.rotation = rotation;
				return bounds;
			}
		}


		public virtual Bounds shapeBounds
		{
			get
			{
				Bounds bounds = new Bounds( transform.position, new Vector3( size.x, size.y, depth ) );
				return bounds;
			}
		}

		public virtual Bounds bounds
		{
			get
			{
				return GetComponent<Renderer>().bounds;
			}
		}


		void OnDestroy()
		{
			DestroyImmediate( _mesh );
		}



		public Color color
		{
			get
			{
				return allFill.color;
			}

			set
			{
				if( allFill.color == value ) return;
				
				allFill.color = value;
				Draw();
			}
		}

		public float fidelity
		{
			get{ return _fidelity; }
			set
			{
				if( _fidelity == value ) return;
				_fidelity = value;
				Draw();
			}
		}



		public float depth
		{
			get{ return _depth; }
			set
			{
				if( _depth == value ) return;
				_depth = value;
				Draw();
			}
		}

		/**
		* Size of shape.
		* # Markdown Test
		* ## WHAT
		* `Test`
		*/
		public Vector2 size
		{
			get{ return _size; }
			set
			{
				if( _size == value ) return;
				_size = value;

				Draw();
			}
		}
		
		public bool useMultiFaceFill
		{
			get{ return !allFill.isVisible; }
			set
			{
				if( allFill.isVisible == !value ) return;
				allFill.isVisible = !value;
				Draw();
			}
		}
		
		

		public virtual string[] serializedPropertyNames
		{
			get{
				return new string[]{  };
			}
		}

		public Drawer drawer{ get { return _drawer; }}
	}

	public enum ShapeAnchorPoint
	{
		TopLeft,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public enum ShapePivotPoint
	{
		Base,
		Center
	}

	public enum ShapeFillSide
	{
		All,
		Front,
		Side,
		Back
	}

}


