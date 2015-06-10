using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityDesigner
{

	public class GroupingEditor {

		[MenuItem("Designer/Grouping/Group %g", false, 3)]
		static void Group()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			GameObject go = new GameObject("Group");
			Undo.RegisterCreatedObjectUndo (go, "Group Shapes");
			go.transform.position = bounds.center; 
			

			foreach( Shape shape in shapes )
			{
				Undo.SetTransformParent( shape.transform, go.transform, "Group Shapes" );
			}
		}
	}
}