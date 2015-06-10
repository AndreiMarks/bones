using UnityEngine;
using UnityEditor;
using System.Collections;


namespace UnityDesigner
{
	[InitializeOnLoad]
	public class CameraEditor  {

		[MenuItem("Designer/Camera/Match Main Camera to Scene &1", false, 4)]
		[ToolbarItem( "5_Camera", "camera_maintoscene", "Match Main Camera To Scene", 0, 0 )]
		private static void DoMatchMainCameraToScene(){

	 		Camera sceneCamera = SceneView.lastActiveSceneView.camera;
			if( sceneCamera == null) return;

			Camera mainCamera = Camera.main;
			if( mainCamera == null ) return;

			Undo.RecordObject (mainCamera.transform, "Match Main Camera To Scene");
			mainCamera.transform.position = sceneCamera.transform.position;
			mainCamera.transform.rotation = sceneCamera.transform.rotation;

		}


		[MenuItem("Designer/Camera/Match Scene Camera to Main &2", false, 5)]
		[ToolbarItem( "5_Camera", "camera_scenetomain", "Match Scene To Main Camera", 0, 1 )]
		private static void DoMatchSceneCameraToMain(){

	 		Camera sceneCamera = SceneView.lastActiveSceneView.camera;
			if( sceneCamera == null) return;

			Camera mainCamera = Camera.main;
			if( mainCamera == null ) return;


			float size = SceneView.lastActiveSceneView.size;

			SceneView.lastActiveSceneView.LookAt( mainCamera.transform.position + mainCamera.transform.forward * size, mainCamera.transform.rotation );

		}
		
		
		[MenuItem("Designer/Camera/Match Scene Camera to Game Object &3", true)]
		private static bool ValidateMatchSceneCameraToSelection(){
		
			GameObject target = Selection.activeGameObject;
			if( target == null ) return false;
			
			return true;
		}
		

		[MenuItem("Designer/Camera/Match Scene Camera to Game Object &3", false, 6)]
		[ToolbarItem( "5_Camera", "camera_scenetogo", "Match Scene Camera to Game Object", 1, 2)]
		private static void DoMatchSceneCameraToSelection(){

			GameObject target = Selection.activeGameObject;

			float size = SceneView.lastActiveSceneView.size;
			
			SceneView.lastActiveSceneView.LookAt( target.transform.position + target.transform.forward * size, target.transform.rotation );
		}


		[MenuItem("Designer/Camera/Toggle Camera Frustums", false, 26)]
		[ToolbarItem( "5_Camera", "camera_toggle", "Toggle Camera Frustums", 0, 3)] 	
		static void ToggleDrawCamera()
		{
			bool isEnabled = EditorPrefs.GetBool("DesignerCameraFrustum", false);
			isEnabled = !isEnabled;

			EditorPrefs.SetBool("DesignerCameraFrustum", isEnabled);
			UpdateCameraFrustumEnabled();
		}

		

		static CameraEditor()
		{
			UpdateCameraFrustumEnabled();
		}

		static void UpdateCameraFrustumEnabled()
		{
			bool isEnabled = EditorPrefs.GetBool("DesignerCameraFrustum", false);

			if( isEnabled )
			{
				SceneView.onSceneGUIDelegate -= OnScene;
				SceneView.onSceneGUIDelegate += OnScene;
			} else {
				SceneView.onSceneGUIDelegate -= OnScene;
			}

			SceneView.RepaintAll();
		}


		static void OnScene(SceneView SceneView)
		{	
	    	Camera[] cameras  = Camera.allCameras;
			Camera currentlySelectedCamera = null;

	    	if(Selection.activeGameObject)
	    		currentlySelectedCamera = Selection.activeGameObject.GetComponent<Camera>();    	

	    	for( int i = 0; i < cameras.Length; i++)
	    	{
	    		Camera camera = cameras[i];
	    		if(camera != currentlySelectedCamera)
	    		{
	    			DrawCameraFrustrum( camera, true );    		
	    		}else{
	    			DrawCameraFrustrum( camera, false );    		
	    		}
	    	}
		}



		static void DrawCameraFrustrum( Camera camera , bool isSelected)
		{
			Handles.color = (isSelected == true) ? ( Color.grey ) : ( Color.white );

			float z =  camera.nearClipPlane;
			Vector3 v1a = camera.ViewportToWorldPoint(new Vector3(0f, 0f, z));
			Vector3 v2a = camera.ViewportToWorldPoint(new Vector3(0f, 1f, z));
			Vector3 v3a = camera.ViewportToWorldPoint(new Vector3(1f, 1f, z));
			Vector3 v4a = camera.ViewportToWorldPoint(new Vector3(1f, 0f, z));
			
			z = camera.farClipPlane;
			Vector3 v1b = camera.ViewportToWorldPoint(new Vector3(0f, 0f, z));
			Vector3 v2b = camera.ViewportToWorldPoint(new Vector3(0f, 1f, z));
			Vector3 v3b = camera.ViewportToWorldPoint(new Vector3(1f, 1f, z));
			Vector3 v4b = camera.ViewportToWorldPoint(new Vector3(1f, 0f, z));
			

			Handles.DrawLine(v1a, v1b);
			Handles.DrawLine(v2a, v2b);
			Handles.DrawLine(v3a, v3b);
			Handles.DrawLine(v4a, v4b);

			Handles.DrawLine(v1a, v2a);
			Handles.DrawLine(v1a, v4a);
			Handles.DrawLine(v3a, v2a);
			Handles.DrawLine(v3a, v4a);


			Handles.DrawLine(v1b, v2b);
			Handles.DrawLine(v1b, v4b);
			Handles.DrawLine(v3b, v2b);
			Handles.DrawLine(v3b, v4b);
		}

	}

}