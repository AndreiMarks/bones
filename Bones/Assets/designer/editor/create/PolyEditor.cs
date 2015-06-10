using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityDesigner
{
	[CustomEditor (typeof (Poly))]
	[CanEditMultipleObjects]
	public class PolyEditor : ShapeEditor {

	    [MenuItem ("Designer/Create/Poly", false, 3)]
	    [ToolbarItem("1_Create", "Poly", "Poly", 0, 2)]
	    static void CreatePoly () {
	        CreateShape<Poly>();
	    }

		public override void OnEditorGUI()
		{
			Poly poly = target as Poly;
			//LabelVertices();

			Handles.matrix = poly.transform.localToWorldMatrix;
			Handles.color = Color.red;
		
			if( poly.drawer.points.Count > 0 )
			{

				DepthHandle( 
					poly.drawer.points[0],
					poly.drawer.points.ToArray()
				);

			}

			if( GUI.changed )
			{
				poly.Draw();
			}

			DrawOutline( poly );
		}

		

	}

}