using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityDesigner
{
	public class Face
	{
		public List<Vector3> vertices;
		public List<int> triangles;	
		public ShapeFill fill;
		public Shape shape;


		public Face( List<Vector3> vertices, List<int> triangles, ShapeFill fill )
		{
			this.vertices = vertices;
			this.triangles = triangles;
			this.fill = fill;
		}
		
		public static List<int> TriangleStripToTriangles( int[] indices, bool reverseWind = false )
		{
			List<int> triangles = new List<int>();

			for( int i = 0; i < indices.Length - 2; i++ )
			{ 
				if( reverseWind )
				{
					triangles.Add( indices[i] );
					triangles.Add( indices[i + 2] );
					triangles.Add( indices[i + 1] );
				} else {
					triangles.Add( indices[i] );
					triangles.Add( indices[i + 1] );
					triangles.Add( indices[i + 2] );
				}

				reverseWind = !reverseWind;
			}

			return triangles;
		}
		


		public static List<int> FanToTriangles( int[] indices, bool reverseWind = false )
		{
			List<int> triangles = new List<int>();

			for( int i = 1; i < indices.Length - 1; i++ )
			{
				if( reverseWind )
				{
					triangles.Add( indices[i] );
					triangles.Add( indices[i+1]);
					triangles.Add( indices[0] );
				} else {
					triangles.Add( indices[i+1]);
					triangles.Add( indices[i] );
					triangles.Add( indices[0] );
				}			
			}

			return triangles;
		}

		
		public static Face MakeFace( List<Vector3> vertices, int offset, ShapeFill fill, bool reverseWind )
		{
			List<int> indices = new List<int>();

			for(int i = 0; i < vertices.Count/2; i++)
			{
				indices.Add( (vertices.Count - 1) - i + offset);
				indices.Add( i + offset );
			}

			List<int> triangles = TriangleStripToTriangles( indices.ToArray(), reverseWind );
			return new Face( vertices, triangles, fill  );
		}

		public static Face MakeFanFace( List<Vector3> vertices, int offset, ShapeFill fill, bool reverseWind )
		{
			List<int> indices = new List<int>();

			for(int i = 0; i < vertices.Count; i++)
			{
				indices.Add( i + offset);
			}

			indices.Add( 1 + offset );	

			List<int> triangles = FanToTriangles( indices.ToArray(), reverseWind );
			return new Face( vertices, triangles,  fill  );
		}


		public static Face MakeEdgeFace( List<Vector3> vertices, int edgeLength, int offset, ShapeFill fill, bool reverseWind = false )
		{
			List<int> indices = new List<int>();

			for( int i = 0; i < edgeLength; i++ )
			{
				indices.Add( i + offset );
				indices.Add( i + edgeLength + offset );
			}

			indices.Add( offset );
			indices.Add( edgeLength + offset );

			List<int> triangles = TriangleStripToTriangles( indices.ToArray(), reverseWind );
			return new Face( vertices, triangles, fill );
		}
		
		
		
		
		
		

		public List<Vector3> GetVertices()
		{
			return vertices;
		}

		public List<int> GetTriangles()
		{
			return triangles;
		}

		public List<Color> GetColors( Vector2 size )
		{
			List<Color> colors = new List<Color>();

			for( int i = 0; i < vertices.Count; i++ )
			{
				Vector3 v = vertices[i];
				Color color = Color.magenta;

				if( fill.fillType == ShapeFillType.Gradient )
				{
					color = fill.GetGradientColor( v, size );
				} else {
					color = fill.color;
				}

				colors.Add( color );	
			}
			return colors; 
		}


		public List<Vector2> GetUV( Vector2 size )
		{
			List<Vector2> uvs = new List<Vector2>();

			for( int i = 0; i < vertices.Count; i++ )
			{
				Vector3 v = vertices[i];
				uvs.Add( new Vector2( (v.x + size.x/2) / size.x, (v.y + size.y/2) / size.y ) );
			} 

			return uvs;
		}
		
		public void FacetQuads( bool reverseWind )
		{


			int offset = triangles[0];
			
			for( int i = 0; i < triangles.Count - 2; i += 6 )
			{
				if( reverseWind )
				{
					DupeVertex(i, offset);
					DupeVertex(i+2, offset);
					triangles[i+3] = triangles[i+2];
				} else {
					DupeVertex(i, offset);
					DupeVertex(i+1, offset);
					triangles[i+3] = triangles[i+1];
				}
			}
			
		}
		
		void DupeVertex( int index, int offset )
		{
			int adjIndex = triangles[index] - offset;
			if( adjIndex >= vertices.Count ) return;
			vertices.Add( vertices[ adjIndex ] );
			triangles[index] = vertices.Count - 1 + offset;
		}
		

	}

}