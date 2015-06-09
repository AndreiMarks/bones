
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityDesigner
{

	public class Square : Shape {

		[SerializeField]
		[Space(10)]
		private float _cornerRadius;



		public override void DrawOutline()
		{

			

			Vector2 position = new Vector2(0.5f * size.x, 0.5f * size.y - cornerRadius);
			Vector2 direction = Vector2.up;

			drawer.Clear();
			drawer.Start( position, direction );
			

			int segments = GetSegmentsForRadius( cornerRadius, 90 );

			bool horizontal = true;

			if( !hasCorners )
			{
			//	drawer.Clear();
				drawer.Add( new Vector2( 0.5f * size.x,  -0.5f * size.y ) );
				drawer.Add( new Vector2( -0.5f * size.x, -0.5f * size.y ) );
				drawer.Add( new Vector2( -0.5f * size.x,  0.5f * size.y ) );
			//	drawer.Add( new Vector2(  0.5f * size.x,  0.5f * size.y ) );
			} else {

				float cr = cornerRadius / Mathf.Sqrt(2);

				for(int i = 0; i < 4; i++)
				{
					
					drawer.Bevel( 90, cr, segments );

					float distance = 0;
					if( horizontal )
					{
						distance = size.x - 2 * cornerRadius;
					} else {
						distance = size.y - 2 * cornerRadius;
					}

					if( distance > 0 ) drawer.Move( distance );

					horizontal = !horizontal;
				}

				drawer.points.RemoveAt( drawer.points.Count - 1 );

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

				if( hasCorners )
				{
					frontVertices.Insert(0, Vector3.zero);
					AddFace( Face.MakeFanFace( frontVertices, 0, frontFill, false) );
					offset += edgeLength + 1;
				} else {	
					AddFace( Face.MakeFace( frontVertices, 0, frontFill,  false) );
					offset += edgeLength;
				}
			}


			if( depth > 0 )
			{

				ShapeFill backFill = GetFill( ShapeFillSide.Back );
				if( backFill.isVisible )
				{		

					List<Vector3> backVertices = drawer.GetVertices( depth ); 
				
					if( hasCorners )
					{
						backVertices.Insert(0, new Vector3(0,0,depth));
						AddFace( Face.MakeFanFace( backVertices, offset, backFill,  true) );
						offset += edgeLength + 1;
					} else {
						AddFace( Face.MakeFace( backVertices, offset, backFill,  true) );
						offset += edgeLength;
					}

				}

				ShapeFill sideFill = GetFill( ShapeFillSide.Side );
				if( sideFill.isVisible )
				{		
					List<Vector3> edgeVertices = new List<Vector3>();
					edgeVertices.AddRange( drawer.GetVertices( hasCorners ? 0 : depth ) );
					edgeVertices.AddRange( drawer.GetVertices( hasCorners ? depth : 0) );

					Face face = Face.MakeEdgeFace( edgeVertices, edgeLength, offset, sideFill, true );
					if( !hasCorners ) face.FacetQuads( true );
					AddFace( face );
				}
			}



			/* 

			if( depth > 0 )
			{
				
				AddFace( MakeFace(depth, edgeLength, true) );
				
			} */
		}

		public override string[] serializedPropertyNames
		{
			get{
				return new string[]{ "_cornerRadius" };
			}
		}

		bool hasCorners
		{
			get
			{
				return cornerRadius != 0;
			}
		}


		public float cornerRadius
		{
			get{ return _cornerRadius; }
			set
			{
				if( _cornerRadius == value ) return;
				float maxRadius = Mathf.Min( size.x, size.y ) / 2;
				_cornerRadius = Mathf.Clamp( value, 0, maxRadius );

				Draw();
			}
		}
	}
}