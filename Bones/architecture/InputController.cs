using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InputController : Controller<InputController>
{
    private List<TouchInfo> _currentTouches = new List<TouchInfo>();

    void Update()
    {
        HandleTouches();
    }

    // Main Functions ==================================================
    private TouchInfo GetTouchById( int id )
    {
        for( int i = 0; i < _currentTouches.Count; i++ )
        {
            Debug.Log( _currentTouches[i].id );
            if ( _currentTouches[i].id == id ) return _currentTouches[i];
        }

        return null;
    }

    private void HandleTouches()
    {
        TouchInfo newTouch = null;

        if ( EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() ) return;
        
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            newTouch = new TouchInfo( Input.mousePosition );
            _currentTouches.Add( newTouch );

            _inputSettings.ReportTouchBegan( newTouch );
        } else if ( Input.GetMouseButtonUp( 0 ) ) {

            newTouch = GetTouchById( 0 );
            newTouch.Update( Input.mousePosition );

            _inputSettings.ReportTouchEnded( newTouch );
            _currentTouches.Remove( newTouch );

        } else if ( Input.GetMouseButton( 0 ) ) {
            newTouch = GetTouchById( 0 );
            newTouch.Update( Input.mousePosition );

            _inputSettings.ReportTouchMoved( newTouch );
        }

        for( int i = 0; i < Input.touches.Length; i++ )
        {
            Touch t = Input.touches[i];
            int id = t.fingerId;

            if ( t.phase == TouchPhase.Began )
            {
                newTouch = new TouchInfo( t );
                _currentTouches.Add( newTouch );

                _inputSettings.ReportTouchBegan( newTouch );
            } else if ( t.phase == TouchPhase.Ended ) {
                newTouch = GetTouchById( id );
                newTouch.Update( t );

                _inputSettings.ReportTouchEnded( newTouch );
                _currentTouches.Remove( newTouch );
            } else if ( t.phase == TouchPhase.Moved ) {
                newTouch = GetTouchById( id );
                newTouch.Update( t );

                _inputSettings.ReportTouchMoved( newTouch );
            }
        }
    }
}

public class TouchInfo
{
    public Touch touch;
    public Vector2 position;

    public TouchInfo( Touch t )
    {
        touch = t;
        position = touch.position;
    }

    public TouchInfo( Vector2 position )
    {
        Debug.Log( touch.fingerId );
        this.position = position;
    }

    public Vector3 GetWorldPoint( Camera cam, float camDistance = 10f )
    {
        Vector3 point = new Vector3( position.x, position.y, camDistance );
        return cam.ScreenToWorldPoint( point );
    }

    public void Update( Vector2 screenPosition )
    {
        position = screenPosition;
    }

    public void Update( Touch t )
    {
        touch = t;
        position = t.position;
    }
    
    public int id { get { return touch.fingerId; } }//( touch == null ) ? -1 : touch.fingerId; } }
}
