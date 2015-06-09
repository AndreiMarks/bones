using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityDesigner
{
	[CustomEditor(typeof(Square))]
	[CanEditMultipleObjects]
	public class SquareEditor : ShapeEditor {


		[MenuItem("Designer/Create/Square", false, 0)]
		[ToolbarItem("1_Create", "square", "Square", 0, 0)]
	    static void CreateSquare () 
	    {
	    	CreateShape<Square>();
	    }

		public override void OnEditorGUI()
	    {

			Square square = target as Square;
			//LabelVertices();

			Handles.matrix = square.transform.localToWorldMatrix;
			Handles.color = Color.red;
			
			ShapeGUI.NameNextHandle("CornerRadius");
			Vector3 cornerHandle = new Vector3( -0.5f * square.size.x + square.cornerRadius, 0.5f * square.size.y, 0 );
			cornerHandle = ShapeGUI.VectorHandle( cornerHandle );

			float cornerRadius = 0.5f * square.size.x + cornerHandle.x ;

			if( square.cornerRadius != cornerRadius )
			{
				RegisterUndo();
				square.cornerRadius = cornerRadius;
			}

			float rt2 = Mathf.Sqrt(2);
			float adj = (rt2 - 1 ) * cornerRadius / rt2;

			DepthHandle( 
				new Vector2( square.size.x * .5f - adj, square.size.y * .5f - adj),
				new Vector2( square.size.x * .5f - adj, square.size.y * .5f - adj),
				new Vector2( -square.size.x * .5f + adj, square.size.y * .5f - adj),
				new Vector2( -square.size.x * .5f + adj, -square.size.y * .5f + adj),
				new Vector2( square.size.x * .5f - adj, -square.size.y * .5f + adj)
			);

			if( GUI.changed )
			{
				square.Draw();
			}

			DrawOutline( square );
		}



	}
}