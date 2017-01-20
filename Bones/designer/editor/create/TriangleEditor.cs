using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityDesigner
{
	[CustomEditor(typeof(Triangle))]
	[CanEditMultipleObjects]
	public class TriangleEditor : ShapeEditor {


		[MenuItem("Designer/Create/Triangle", false, 2)]
		[ToolbarItem("1_Create", "triangle", "Triangle", 0, 1)]
	    static void CreateTriangle () 
	    {
	    	CreateShape<Triangle>();
	    }

		public override void OnSceneGUI()
	    {
			Triangle triangle = target as Triangle;

			Vector3 firstVertex = triangle.firstVertex;
			Vector3 secondVertex = triangle.secondVertex;
			Vector3 thirdVertex = triangle.thirdVertex;

			Handles.matrix = triangle.transform.localToWorldMatrix;
			Handles.color = ShapeGUI.handleColor;
			
	//		LabelVertices();
			
			ShapeGUI.NameNextHandle("Shear");
			Vector3 shearHandle = secondVertex;
			
			shearHandle = ShapeGUI.SnapVectorHandle( shearHandle, ShapeGUI.IsCurrentHandle("Shear"),
				new VectorSnapPoint( new Vector3( 0, shearHandle.y, 0 ), new Vector3( 0, firstVertex.y, 0 ) ),
				new VectorSnapPoint( new Vector3( firstVertex.x, shearHandle.y, 0 ), new Vector3( firstVertex.x, firstVertex.y, 0 ) ),
				new VectorSnapPoint( new Vector3( thirdVertex.x, shearHandle.y, 0 ), new Vector3( thirdVertex.x, firstVertex.y, 0 ) )
			);

			if( triangle.baseLength > 0 )
			{
				float shear = Mathf.Clamp( shearHandle.x / (0.5f * triangle.baseLength ), -1, 1 );
				if( triangle.shear != shear )
				{
					RegisterUndo();
					triangle.shear = shear;
				}
			}

			ShapeGUI.NameNextHandle("CornerRadius");
			Vector3 cornerHandle = triangle.cornerHandlePosition;
			cornerHandle = ShapeGUI.VectorHandle( cornerHandle );
			
			Vector2 firstDirection = (secondVertex - firstVertex).normalized;
			float   firstAngle = Vector2.Angle( firstDirection, Vector2.right );
			float	firstOffset = Mathf.Max( cornerHandle.x - firstVertex.x, 0 );

			float cornerRadius = Mathf.Min( triangle.shortestLeg / 4, firstOffset * Mathf.Sin( firstAngle/2 * Mathf.Deg2Rad ) );
			if( triangle.cornerRadius != cornerRadius )
			{
				RegisterUndo();
				triangle.cornerRadius = cornerRadius;
			}
			if( triangle.cornerRadius > 0 )
			{
				Handles.color = Color.yellow;
				Handles.DrawDottedLine( firstVertex, secondVertex, 5 );
				Handles.DrawDottedLine( secondVertex, thirdVertex, 5 );
				Handles.DrawDottedLine( firstVertex, thirdVertex, 5 );
			}
			
			Handles.color = ShapeGUI.shapeColor;
			
			Vector2 firstMidpoint = Vector3.Lerp( triangle.firstVertex, triangle.secondVertex, 0.5f );
			Vector2 secondMidpoint = Vector3.Lerp( triangle.thirdVertex, triangle.secondVertex, 0.5f );
			Vector2 thirdMidpoint = Vector3.Lerp( triangle.firstVertex, triangle.thirdVertex, 0.5f );

			DepthHandle( firstMidpoint,
				firstMidpoint,
				secondMidpoint,
				thirdMidpoint
			);
		
			
			if( GUI.changed )
			{
				triangle.Draw();
			}

			DrawOutline( triangle );
			
		}


	}
}