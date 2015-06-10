using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityDesigner
{
	[InitializeOnLoad]
	public class ContextEditor  {

		static ContextEditor()
		{
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.onSceneGUIDelegate += OnScene;
		}


		static void OnScene(SceneView SceneView)
		{	
			 
			Camera sceneCamera = SceneView.lastActiveSceneView.camera;
			if( sceneCamera == null ) return;

			if( !SelectionEditor.hasSelection ) return;

			Bounds selectionBounds = SelectionEditor.GetSelectedBounds();
			Vector3 topRight = new Vector3( selectionBounds.max.x, selectionBounds.max.y, selectionBounds.min.z );

			Vector2 point = sceneCamera.WorldToScreenPoint( topRight );

			point.y = sceneCamera.pixelHeight - point.y;

			Handles.BeginGUI();

			
	 
	 	 	if(GUI.Button( new Rect( point.x + 16, point.y, 32, 32 ), ShapeIcons.GetIcon("shape_properties")) )
			{
				ShapePropertyWindow.Init();
			}

			Handles.EndGUI(); 

		}

	}

}