using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplicationBase : MonoBehaviour
{
    public ApplicationState initialState = ApplicationState.LOBBY;
    public bool debugStateChanges = false;

    bool _isPaused = false;
    ApplicationState _currentState = ApplicationState.NULL;

    public void Start()
    {
        ChangeState( initialState );
    }

    public void PauseCurrentState()
    {
        if(isPaused)
            return;

        _isPaused = true;
        SendMessage( "OnPauseState", currentState );	
    }

    public void ResumeCurrentState()
    {
        if(!isPaused)
            return;

        _isPaused = false;
        SendMessage( "OnResumeState", currentState );	
    }

    public void ChangeState( ApplicationState newState )
    {
        if( IsCurrentState(newState) )
            return;

        SendMessage( "OnTeardownState", currentState );	

        _currentState = newState;

        SendMessage( "OnSetupState", currentState );	
    }

    public bool isPaused
    {
        get
        {
            return _isPaused;
        }			
    }

    public bool IsCurrentState( ApplicationState state )
    {
        return currentState == state;
    }

    public ApplicationState currentState
    {
        get
        {
            return _currentState;
        }
    }
}

