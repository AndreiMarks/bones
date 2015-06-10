using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnityDesigner
{
	public class Star : Shape {

		[Space(10)]
		[SerializeField]
		[Range(3,48)]
		int _points = 5;

		[Range(0,1)]
		[SerializeField]
		float _innerRadius = .5f;
		

		public override void DrawOutline()
		{
			drawer.Clear();
			float angle = 0;
			float angleStep = 360 / (float)(points * 2) * Mathf.Deg2Rad;
			
			bool inner = false;
			for(int i = 0; i <= points * 2; i++ )
			{
				float s = Mathf.Sin(angle);
				float c = Mathf.Cos(angle);

				Vector2 point = new Vector2(s * size.x * 0.5f, c * size.y * 0.5f);
				if( inner ) point *= innerRadius;
				drawer.Add( point );
				
				angle += angleStep;
				inner = !inner;
			}

		}
		
		public override void BuildFaces()
		{
			int offset = 0;
			int edgeLength = drawer.numPoints;
			ShapeFill frontFill = GetFill( ShapeFillSide.Front );

			if( frontFill.isVisible )
			{
				List<Vector3> frontVertices = drawer.GetVertices(0);

				frontVertices.Insert(0, Vector3.zero);
				AddFace( Face.MakeFanFace( frontVertices, 0, frontFill, true) );
				offset += edgeLength + 1;
			}


			if( depth > 0 )
			{
				ShapeFill backFill = GetFill( ShapeFillSide.Back );

				if( backFill.isVisible )
				{		
					List<Vector3> backVertices = drawer.GetVertices( depth ); 
				
					backVertices.Insert(0, new Vector3(0,0,depth));
					AddFace( Face.MakeFanFace( backVertices, offset, backFill,  false) );
					offset += edgeLength + 1;
				}

				ShapeFill sideFill = GetFill( ShapeFillSide.Side );
				if( sideFill.isVisible )
				{		
					List<Vector3> edgeVertices = new List<Vector3>();
					edgeVertices.AddRange( drawer.GetVertices( depth ) );
					edgeVertices.AddRange( drawer.GetVertices( 0 ) );

					Face face = Face.MakeEdgeFace( edgeVertices, edgeLength, offset, sideFill, true );
					face.FacetQuads( true );
					AddFace( face );
				}
			}
		}

		public override string[] serializedPropertyNames
		{
			get{
				return new string[]{ "_points", "_innerRadius" };
			}
		}
		
		
		public int points
		{
			get{ return _points; }
			set
			{
				if( _points == value ) return;
				_points = Mathf.Max( 3, value );

				Draw();
			}
		} 
		
		public float innerRadius
		{
			get{ return _innerRadius; }
			set
			{
				if( _innerRadius == value ) return;
				_innerRadius = value;
				Draw();
			}
		} 

	}
}