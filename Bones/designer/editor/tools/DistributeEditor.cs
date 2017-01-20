using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

namespace UnityDesigner
{
	public class DistributeEditor : Editor {

		//[ShapeToolbarItem( "4_Distribute", "distribute_horizontal", "Distribute Horizontally", 3, 1 )]
		public static void DistributeHorizontal()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes().OrderBy( shape => shape.transform.position.x ).ToArray();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			float totalWidth = 0;
			foreach( Shape shape in shapes )
			{
				totalWidth += shape.bounds.size.x;
			}

			float difference = bounds.size.x - totalWidth;
			float gap = difference / (shapes.Length - 1);

			float x = bounds.min.x;
			foreach( Shape shape in shapes )
			{
				Undo.RecordObject( shape.transform,  "Distribute Shapes Horizontally");

				shape.transform.position += Vector3.left * (shape.bounds.min.x - x);

				x += shape.bounds.size.x + gap;
			}

		}

		//[ShapeToolbarItem( "4_Distribute", "distribute_vertical", "Distribute Vertically", 3, 1 )]
		public static void DistributeVertical()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes().OrderBy( shape => shape.transform.position.y ).ToArray();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			float totalHeight = 0;
			foreach( Shape shape in shapes )
			{
				totalHeight += shape.bounds.size.y;
			}

			float difference = bounds.size.y - totalHeight;
			float gap = difference / (shapes.Length - 1);

			float y = bounds.min.y;
			foreach( Shape shape in shapes )
			{
				Undo.RecordObject( shape.transform,  "Distribute Shapes Vertically");

				shape.transform.position += Vector3.down * (shape.bounds.min.y - y);

				y += shape.bounds.size.y + gap;
			}
		}

		//[ShapeToolbarItem( "4_Distribute", "distribute_depth", "Distribute By Depth", 3, 1 )]
		public static void DistributeDepth()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes().OrderBy( shape => shape.transform.position.z ).ToArray();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			float totalDepth = 0;
			foreach( Shape shape in shapes )
			{
				totalDepth += shape.bounds.size.z;
			}

			float difference = bounds.size.z - totalDepth;
			float gap = difference / (shapes.Length - 1);

			float z = bounds.min.z;
			foreach( Shape shape in shapes )
			{
				Undo.RecordObject( shape.transform,  "Distribute Shapes By Depth");

				shape.transform.position += Vector3.back * (shape.bounds.min.z - z);

				z += shape.bounds.size.z + gap;
			}
		}
	}
}