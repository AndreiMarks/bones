using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnityDesigner
{
	[ExecuteInEditMode]
	[AddComponentMenu("Shapes/Circle")]
	public class Circle : Shape {

		
		[SerializeField]
		[Range(0,1)]
		[Space(10)]
		float _innerRadius = 0; 
		
		[SerializeField]
		float _fromAngle = 0;
		
		[SerializeField]
		float _toAngle = 360;
		
		[SerializeField]
		int _segments = 24;


		enum CircleGeometryType
		{
			Circle,
			Wedge,
			Ring,
			Segment
		}

		void CheckSegments()
		{		
			float r = Mathf.Min( size.x, size.y ) / 2;
			float arcAngle = Mathf.Abs( fromAngle - toAngle );	
			segments = GetSegmentsForRadius( r, arcAngle );
		}

		public override void DrawOutline()
		{
			CheckSegments();

			drawer.Clear();

			switch( geometryType )
			{
				case CircleGeometryType.Circle:
					DrawOutlineCircle();
				break;

				case CircleGeometryType.Wedge:
					DrawOutlineWedge();
				break;

				case CircleGeometryType.Ring:
					DrawOutlineRing();
				break;

				case CircleGeometryType.Segment:
					DrawOutlineSegment();
				break;
			}
			
		}


		void DrawOutlineCircle()
		{
			float angle = 0;
			float angleStep = 360 / (float)(segments) * Mathf.Deg2Rad;
			
			for(int i = 0; i < segments; i++ )
			{
				float s = Mathf.Sin(angle);
				float c = Mathf.Cos(angle);

				Vector2 point = new Vector2(s * size.x * 0.5f, c * size.y * 0.5f);

				drawer.Add( point );
				
				angle += angleStep;
			}

			//drawer.points.RemoveAt( drawer.points.Count - 1 );
		}

		void DrawOutlineWedge()
		{
			DrawOutlineSegment();
		}

		void DrawOutlineRing()
		{
			DrawOutlineSegment();
		}

		void DrawOutlineSegment()
		{
			float angle = fromAngle * Mathf.Deg2Rad ;
			float angleStep = (toAngle - fromAngle) / (float)(segments) * Mathf.Deg2Rad;
			
			
			for(int i = 0; i <= segments; i++ )
			{
				float s = Mathf.Sin(angle);
				float c = Mathf.Cos(angle);

				Vector2 inner = new Vector2(s * innerRadius * size.x * 0.5f, c * innerRadius * size.y * 0.5f);
				Vector2 outer = new Vector2(s * size.x * 0.5f, c * size.y* 0.5f);

				drawer.Insert( drawer.numPoints / 2,  inner );
				drawer.Insert( i, outer );
				
				angle += angleStep;
			}
		}








		public override void BuildFaces()
		{

			switch( geometryType )
			{
				case CircleGeometryType.Circle:
					BuildFacesCircle();
				break;

				case CircleGeometryType.Wedge:
					BuildFacesWedge();
				break;

				case CircleGeometryType.Ring:
					BuildFacesRing();
				break;

				case CircleGeometryType.Segment:
					BuildFacesSegment();
				break;
			}

		}

		void BuildFacesCircle()
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
					edgeVertices.AddRange( drawer.GetVertices( 0) );

					Face face = Face.MakeEdgeFace( edgeVertices, edgeLength, offset, sideFill, true );
					AddFace( face );
				}
			}
		}

		void BuildFacesWedge()
		{
			BuildFacesSegment();
		}

		void BuildFacesRing()
		{
			BuildFacesSegment();
		}

		void BuildFacesSegment()
		{

			List<Vector3> frontVertices = drawer.GetVertices(0);
			int edgeLength = drawer.numPoints;	
			int offset = 0;

			ShapeFill frontFill = GetFill( ShapeFillSide.Front );
			if( frontFill.isVisible )
			{
				AddFace( Face.MakeFace( frontVertices, 0, frontFill, false) );
				offset += edgeLength;
			}

			if( depth > 0 )
			{	
				List<Vector3> backVertices = drawer.GetVertices(depth);

				ShapeFill backFill = GetFill( ShapeFillSide.Back );
				if( backFill.isVisible )
				{
					AddFace( Face.MakeFace( backVertices, offset, backFill, true) );
					offset += edgeLength;
				}

				ShapeFill sideFill = GetFill( ShapeFillSide.Side );
				if( sideFill.isVisible )
				{
					List<Vector3> edgeVertices = new List<Vector3>();
					edgeVertices.AddRange( frontVertices );
					edgeVertices.AddRange( backVertices );

					AddFace( Face.MakeEdgeFace( edgeVertices, edgeLength, offset, sideFill, false) );
				}
			}
		}



		CircleGeometryType geometryType
		{
			get
			{
				bool isHollow = (innerRadius > 0);
				if( (fromAngle - toAngle) % 360 == 0)
				{
					return isHollow ? CircleGeometryType.Ring : CircleGeometryType.Circle;
				} else {
					return isHollow ? CircleGeometryType.Segment : CircleGeometryType.Wedge;
				}
				
			}
		}




		public override string[] serializedPropertyNames
		{
			get{
				return new string[]{ "_innerRadius", "_fromAngle", "_toAngle" };
			}
		}

		/**
		* Sets the radius of the circle. Affects size.
		* @see size
		*/
		public float radius
		{
			get{ return size.x / 2; }
			set
			{
				if( size.x / 2 == value ) return;
				float side = Mathf.Max( value * 2, 0);
				size = new Vector2( side, side );
			}

		}

		/**
		* Inner radius of circle, 0 to 1
		*/
		public float innerRadius
		{
			get{ return _innerRadius; }
			set
			{
				if( _innerRadius == value ) return;
				_innerRadius = Mathf.Clamp( value, 0, 1 );
				Draw();
			}
		}

		public float innerRadiusAbsolute
		{
			get{ return _innerRadius * radius; }
			set
			{
				if( _innerRadius * radius == value ) return;
				innerRadius = value / radius;
			}
		}
		
		public float fromAngle
		{
			get{ return _fromAngle; }
			set
			{
				if( _fromAngle == value ) return;
				_fromAngle = value;
				Draw();
			}
		}
		
		public float toAngle
		{
			get{ return _toAngle; }
			set
			{
				if( _toAngle == value ) return;
				_toAngle = value;
				Draw();
			}
		}
		
		public int segments
		{
			get{ return _segments; }
			set
			{
				if( _segments == value ) return;
				_segments = Mathf.Max(3, value);
			}
		}

	}
}