using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityDesigner
{

	public class AlignmentEditor : EditorWindow {

		static Rect rect;

		void OnGUI()
		{

			var options = new GUILayoutOption[]{ GUILayout.Height(25), GUILayout.Width(30)  };
			var labelStyle = new GUIStyle( EditorStyles.label );
			labelStyle.alignment = TextAnchor.MiddleRight;

			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			bool canAlign = shapes.Length >= 2;
			bool canDistribute = shapes.Length >= 3;

			EditorGUILayout.BeginHorizontal();

				GUI.enabled = true;
				GUILayout.Label("Align X:", labelStyle);

				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_left"), "Align Left"), EditorStyles.miniButtonLeft, options ) ) AlignLeft();
				
				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_center_horizontal"), "Align Center"), EditorStyles.miniButtonMid, options ) ) AlignCenterHorizontal();
				
				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_right"), "Align Right"), EditorStyles.miniButtonMid, options ) ) AlignRight();
				
				GUI.enabled = canDistribute;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("distribute_horizontal")), EditorStyles.miniButtonRight, options ) ) DistributeEditor.DistributeHorizontal();


			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

				GUI.enabled = true;
				GUILayout.Label("Align Y:", labelStyle);

				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_top"), "Align Top"), EditorStyles.miniButtonLeft, options ) ) AlignTop();
				
				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_center_vertical"), "Align Middle"), EditorStyles.miniButtonMid, options ) ) AlignCenterVertical();
				
				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_bottom"),  "Align Bottom"), EditorStyles.miniButtonMid, options ) ) AlignBottom();
				
				GUI.enabled = canDistribute;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("distribute_vertical")), EditorStyles.miniButtonRight, options ) ) DistributeEditor.DistributeVertical();

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

				GUI.enabled = true;
				GUILayout.Label("Align Z:", labelStyle);

				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_back"), "Align Back"), EditorStyles.miniButtonLeft, options ) ) AlignBack();
				
				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_center_depth"), "Align Halfway"), EditorStyles.miniButtonMid, options ) ) AlignCenterDepth();
				
				GUI.enabled = canAlign;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("align_front"), "Align Front"), EditorStyles.miniButtonMid, options ) ) AlignFront();
				
				GUI.enabled = canDistribute;
				if( GUILayout.Button( new GUIContent(ShapeIcons.GetIcon("distribute_depth")), EditorStyles.miniButtonRight, options ) ) DistributeEditor.DistributeDepth();


			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
		
		[ToolbarItem( "3_AlignmentPanel", "align_left", "Align", 2, 1 )]
		static void TogglePanel()
		{
			var win = EditorWindow.GetWindow<SceneView>();
			rect = DesignerToolbar.lastRect;
			rect.position += win.position.position + new Vector2(0,30);

			EditorApplication.delayCall += DelayedOpen;
		}

		static void DelayedOpen()
		{
			var window = ScriptableObject.CreateInstance<AlignmentEditor>();
			window.ShowAsDropDown( rect, new Vector2(200,100) );
		}
		



		//X AXIS
		[MenuItem("Designer/Alignment/Align Left", false, 1)]
		static void AlignLeft()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Left");
				shape.transform.position += Vector3.left * Mathf.Abs(shape.bounds.min.x - bounds.min.x);
			}
		}

		[MenuItem("Designer/Alignment/Align Center", false, 2)]
		static void AlignCenterHorizontal()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Center");
				shape.transform.position += Vector3.right * (bounds.center.x - shape.bounds.center.x);
			}
		}

		[MenuItem("Designer/Alignment/Align Right", false, 3)]
		static void AlignRight()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Right");
				shape.transform.position += Vector3.right * (bounds.max.x - shape.bounds.max.x);
			}
		}


		//Y AXIS
		[MenuItem("Designer/Alignment/Align Top", false, 20)]
		static void AlignTop()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Top");
				shape.transform.position += Vector3.up * Mathf.Abs(shape.bounds.max.y - bounds.max.y);
			}
		}

		[MenuItem("Designer/Alignment/Align Middle", false, 21)]
		static void AlignCenterVertical()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Middle");
				shape.transform.position += Vector3.up * (bounds.center.y - shape.bounds.center.y);
			}
		}

		[MenuItem("Designer/Alignment/Align Bottom", false, 22)]
		static void AlignBottom()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Bottom");
				shape.transform.position += Vector3.down * Mathf.Abs(shape.bounds.min.y - bounds.min.y);
			}
		}





		//Z AXIS
		[MenuItem("Designer/Alignment/Align Back", false, 40)]
		static void AlignBack()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Back");
				shape.transform.position += Vector3.back * Mathf.Abs(shape.bounds.min.z - bounds.min.z);
			}
		}

		[MenuItem("Designer/Alignment/Align Halfway", false, 41)]
		static void AlignCenterDepth()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Halfway");
				shape.transform.position += Vector3.forward * (bounds.center.z - shape.bounds.center.z);
			}
		}

		[MenuItem("Designer/Alignment/Align Front", false, 42)]
		static void AlignFront()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			Bounds bounds  = SelectionEditor.GetSelectedBounds();

			foreach( Shape shape in shapes )
			{
				Undo.RecordObject (shape.transform, "Align Front");
				shape.transform.position += Vector3.forward * (bounds.max.z - shape.bounds.max.z);
			}
		}
			
			


	}

}