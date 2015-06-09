using UnityEngine;
using System.Collections;

namespace ApplicationLogic
{

	[RequireComponent( typeof(ApplicationBase) )]
	public class ApplicationStateDebugMenu : MonoBehaviour
	{
		public KeyCode displayKey = KeyCode.Space;
		public KeyCode pauseKey = KeyCode.Backspace;

		ApplicationBase application; 
		bool isDisplayed = false;

		public void Start()
		{
			application = GetComponent<ApplicationBase>();
		}

		public void Update()
		{
			if(Input.GetKeyDown( displayKey ))
				isDisplayed = (isDisplayed == true) ? ( false ) : ( true );

			if(Input.GetKeyDown( pauseKey ))
				application.PauseCurrentState();
		}

		void OnGUI()
		{
			if( application.isPaused )
			{
				Rect rectangle = new Rect(Screen.width * 0.5f - 60.0f,Screen.height * 0.5f - 15.0f,120.0f, 30.0f);
				if(GUI.Button( rectangle , "Resume"))
					application.ResumeCurrentState();
				return;
			}

			if(!isDisplayed)
				return;

			ApplicationState[] states = System.Enum.GetValues( typeof(ApplicationState) ) as ApplicationState[];
			foreach( ApplicationState state in states )
			{
				GUI.enabled = !application.IsCurrentState( state );

				if( GUILayout.Button( state.ToString() ) )
				{
					application.ChangeState( state );
				}

				GUI.enabled = true;
			}
		}
	}

}