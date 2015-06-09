using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityDesigner
{

	public class Triangle : Shape {

		//Distance from foot to midpoint
		[SerializeField]
		[Range(-1,1)]
		[Space(10)]
		private float _shear = 0;
		
		[SerializeField]
		private float _cornerRadius = 0;

		[SerializeField]
		private Vector3 _cornerHandlePosition;

		public override void DrawOutline()
		{

			Vector2 first = firstVertex;
			Vector2 second = secondVertex;
			Vector2 third = thirdVertex;

			Vector2 firstDirection = (second - first).normalized;
			float   firstAngle = Vector2.Angle( firstDirection, Vector2.right );
			float   firstOffset = GetCornerOffset( firstAngle );
			float	firstLength =  Vector2.Distance( first, second );
			
			Vector2 secondDirection = (third - second).normalized;
			float   secondAngle = 180 - Vector2.Angle( secondDirection, firstDirection );
			float   secondOffset = GetCornerOffset( secondAngle );
			float	secondLength =  Vector2.Distance( second, third );


			Vector2 thirdDirection = -Vector2.right;
			float   thirdAngle = 180 - Vector2.Angle( secondDirection, thirdDirection );
			float   thirdOffset = GetCornerOffset( thirdAngle );
			float	thirdLength = Vector2.Distance( first, third );


			_cornerHandlePosition = first + Vector2.right * firstOffset;
			
			drawer.Clear();

			drawer.Start( first + firstDirection * firstOffset, firstDirection );

			drawer.Move( firstLength - (firstOffset + secondOffset) );
			drawer.Bevel( secondAngle - 180 , cornerRadius, GetSegmentsForRadius( cornerRadius, 180-secondAngle ) ) ;

			drawer.Move( secondLength - (secondOffset + thirdOffset) );
			drawer.Bevel( thirdAngle - 180, cornerRadius, GetSegmentsForRadius( cornerRadius, 180-thirdAngle ) ) ;

			drawer.Move( thirdLength - (thirdOffset + firstOffset ) );
			drawer.Bevel( firstAngle - 180, cornerRadius, GetSegmentsForRadius( cornerRadius, 180-firstAngle ) ) ;

			//drawer.points.RemoveAt( drawer.points.Count - 1 );
		}



		public override void BuildFaces()
		{

			Vector2 centroid = ( firstVertex + secondVertex + thirdVertex )/3;

			int offset = 0;
			int edgeLength = drawer.numPoints;	

			ShapeFill frontFill = GetFill( ShapeFillSide.Front );
			if( frontFill.isVisible )
			{
				List<Vector3> frontVertices = drawer.GetVertices(0);

				if( cornerRadius > 0 )
				{
					frontVertices.Insert(0, new Vector3(centroid.x, centroid.y, 0) );
					AddFace( Face.MakeFanFace( frontVertices, 0,  frontFill, true) );
					offset += edgeLength + 1;
				} else {
					AddFace( Face.MakeFace( frontVertices, 0,  frontFill, false) );
					offset += edgeLength;
				}

			}

		
				
			ShapeFill backFill = GetFill( ShapeFillSide.Back );
			if( backFill.isVisible )
			{
				List<Vector3> backVertices = drawer.GetVertices( depth ); 
			
				if( cornerRadius > 0 )
				{
					backVertices.Insert(0, new Vector3(centroid.x, centroid.y, depth));
					AddFace( Face.MakeFanFace( backVertices, offset, backFill, false) );
					offset += edgeLength + 1;

				} else {
					AddFace( Face.MakeFace( backVertices, offset, backFill, true) );
					offset += edgeLength;
				}
			}

			if( depth > 0 )
			{
				ShapeFill sideFill = GetFill( ShapeFillSide.Side );
				if( sideFill.isVisible )
				{
					List<Vector3> edgeVertices = new List<Vector3>();
					edgeVertices.AddRange( drawer.GetVertices( 0 ) );
					edgeVertices.AddRange( drawer.GetVertices( depth ) );
					Face edge = Face.MakeEdgeFace( edgeVertices, edgeLength, offset, sideFill,  false);
					if( cornerRadius == 0 ) edge.FacetQuads( false );
					AddFace( edge );
				}
			}
		}

		float GetCornerOffset( float angle )
		{
			return cornerRadius / Mathf.Sin( angle/2 * Mathf.Deg2Rad );
		}

		public void OnDrawGizmos()
		{
			//Gizmos.matrix = transform.localToWorldMatrix;
			//drawer.OnDrawGizmos();
		}
		
		public Vector3 cornerHandlePosition
		{
			get{ return _cornerHandlePosition; }
		}
		

		public Vector3 firstVertex
		{
			get { return new Vector3( -0.5f * baseLength, -0.5f * height, 0 ); }
		}
		
		public Vector3 secondVertex
		{
			get{ return new Vector3( shear * 0.5f * baseLength, 0.5f * height, 0 ); }
		}
		
		public Vector3 thirdVertex
		{
			get{ return new Vector3( 0.5f * baseLength, -0.5f * height, 0 ); }
		}
		
		public float firstLeg
		{
			get{ return Vector3.Distance( firstVertex, secondVertex ); }
		}
		
		public float secondLeg
		{
			get{ return Vector3.Distance( thirdVertex, secondVertex ); }
		}
		
		public float thirdLeg
		{
			get{ return Vector3.Distance( firstVertex, thirdVertex ); }
		}
		
		public float shortestLeg
		{
			get{ return Mathf.Min( Mathf.Min( firstLeg, secondLeg ), thirdLeg ); }
		}
		
		public override string[] serializedPropertyNames
		{
			get{
				return new string[]{ "_shear", "_cornerRadius" };
			}
		}
		
		public float baseLength
		{
			get{ return size.x; }
			set
			{
				if( size.x == value ) return;
				size.Set( value, size.y );

				Draw();
			}
		}
		public float height
		{
			get{ return size.y; }
			set
			{
				if( size.y == value ) return;
				size.Set( size.x, value );

				Draw();
			}
		}
		
		public float shear
		{
			get{ return _shear; }
			set
			{
				if( _shear == value ) return;
				_shear = value;

				Draw();
			}
		}

		public float cornerRadius
		{
			get{ return _cornerRadius; }
			set
			{
				if( _cornerRadius != value )
				{
					_cornerRadius = Mathf.Clamp( value, 0, baseLength/2);
					Draw();
				}
			}
		}

	}
}