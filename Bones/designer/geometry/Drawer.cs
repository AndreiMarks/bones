using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityDesigner
{
	[System.Serializable]
	public class Drawer {

		
		public List<Vector2> points = new List<Vector2>();

		public Vector2 direction;
		public Vector2 position;

		public void Start( Vector3 position, Vector3 direction)
		{
			this.direction = direction.normalized;
			this.position = position;

			points.Add( this.position );
		}

		public void Move( float distance )
		{
			position += distance * direction;
			points.Add( this.position );
		}

		public void Add( Vector2 point )
		{
			points.Add( point );
		}

		public void Insert( int index, Vector2 point )
		{
			points.Insert(index, point );
		}

		public void Bevel( float angle, float radius, float steps )
		{


			if( radius == 0 )
			{
				Rotate(angle);
				return;
			}

			Vector2 endPointDirection = direction.Rotate(angle/2);
			Vector2 endPoint = endPointDirection * radius * 2 + position;

			Vector2 centerPoint = endPointDirection * radius + position - new Vector2( -endPointDirection.y, endPointDirection.x ) * radius / Mathf.Tan( (180 - angle / 2) * Mathf.Deg2Rad );		
		
			Vector2 offset = position - centerPoint;


			for( int i = 1; i < steps; i++ )
			{
				offset = offset.Rotate( angle/steps );
				Add( centerPoint + offset );
			}

			

			Add( endPoint );
			position = endPoint;
			Rotate( angle );
		}

		public void Rotate( float angle )
		{
			direction = direction.Rotate(angle);
		}

		public void Skew( Vector2 skew )
		{
			
			for( int i = 0; i < points.Count; i++ )
			{
				Vector2 point = points[i];
				Vector2 skewFactor = new Vector2( skew.x * point.y, skew.y * point.x );
				points[i] = point + skewFactor;
			}
		}


		public void Clear()
		{
			points.Clear();
		}

		float Chord(float angle)
		{
			return 2 * Mathf.Sin( angle / 2 );
		}

		public List<Vector3> GetVertices( float height )
		{
			List<Vector3> vertices = new List<Vector3>();
			for( int i = 0; i < points.Count; i++ )
			{
				vertices.Add( new Vector3( points[i].x, points[i].y, height ) );
			}
			return vertices;
		}

		public void OnDrawGizmos()
		{
			if( points.Count == 0 ) return;

			Vector3 position = points[0];

			foreach( Vector3 point in points )
			{
				Gizmos.DrawLine( position, point );
				//Gizmos.DrawWireSphere( point, 0.01f );

				position = point;
			}
		}

		public int numPoints
		{
			get
			{
				return points.Count;
			}
		} 


	}



	public static class Vector2Extensions
	{
		public static Vector2 Rotate( this Vector2 vector, float angle )
		{
			var radians = Mathf.Deg2Rad * angle;

			var ca = Mathf.Cos(radians);
	        var sa = Mathf.Sin(radians);

			return new Vector2( ca * vector.x - sa * vector.y, sa * vector.x + ca * vector.y );
		}
	}
}