using UnityEngine;
using System.Collections;

namespace ApplicationLogic
{
	[RequireComponent( typeof(ApplicationBase) )]
	public abstract class ApplicationStateLoop : MonoBehaviour {

		public ApplicationState state;

		ApplicationBase _application;

		// methods executed from base
		void OnSetupState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Setup();			
		}

		void OnTeardownState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Teardown();
		}

		void OnPauseState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Pause();
		}

		void OnResumeState( ApplicationState state )
		{
			if( enabled && this.state == state )
				Resume();
		}

		// Set up
		void Setup() {
			if( application.debugStateChanges )
				Debug.Log( "Setup Application State " + this ); 

			OnSetup();
		}

		void Update()
		{
			if( isCurrentState )
				OnUpdate();
		}

		// Teardown
		void Teardown()
		{
			if( application.debugStateChanges )
				Debug.Log( "Teardown Application State " + this ); 

			OnTeardown();
		}

		void Pause()
		{

			if( application.debugStateChanges )
				Debug.Log( "Pause Application State " + this ); 
			OnPause();
		}

		void Resume()
		{
			if( application.debugStateChanges )
				Debug.Log( "Resume Application State " + this );
			OnResume(); 	
		}

		public abstract void OnSetup();		
		public abstract void OnTeardown();


		public virtual void OnPause(){}
		public virtual void OnResume(){}
		public virtual void OnUpdate(){}

		protected ApplicationBase application
		{
			get
			{
				if( _application == null )
					_application = GetComponent<ApplicationBase>();

				return _application;
			}

		}

		protected bool isPaused
		{
			get
			{
				return application.isPaused;
			}
		}
		
		protected bool isCurrentState
		{
			get
			{
				return application.IsCurrentState( this.state );
			}
		}
	}
}