using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace UnityDesigner
{

	public class ReflectionEditor : Editor {

		[ToolbarItem( "2_Reflect", "flip_horizontal", "Flip Horizontally", 1, 2 )]
		static void FlipHorizontal()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();

			foreach (Shape shape in shapes)
			{
				Vector3 scale = shape.transform.localScale;
				scale.x = -scale.x;
				shape.transform.localScale = scale;
			}
		}
		
		
		[ToolbarItem( "2_Reflect", "flip_vertical", "Flip Vertically", 1, 3 )]
		static void FlipVertical()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();

			foreach (Shape shape in shapes)
			{
				Vector3 scale = shape.transform.localScale;
				scale.y = -scale.y;
				shape.transform.localScale = scale;
			}
				
		}



	}

}