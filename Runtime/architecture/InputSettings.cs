using UnityEngine;
using System;
using System.Collections;

public class InputSettings : Controller<InputSettings> 
{
    public static void DefaultAction( TouchInfo ti ) {}
    
    public static event Action<TouchInfo> OnTouchBegan = DefaultAction;
    public static event Action<TouchInfo> OnTouchEnded = DefaultAction;
    public static event Action<TouchInfo> OnTouchMoved = DefaultAction;

    public void ReportTouchBegan( TouchInfo touch )
    {
        OnTouchBegan( touch );
    }

    public void ReportTouchEnded( TouchInfo touch )
    {
        OnTouchEnded( touch );
    }

    public void ReportTouchMoved( TouchInfo touch )
    {
        OnTouchMoved( touch );
    }

    // Main Functions ==================================================
    public Vector3 GetScreenPosInWorldSpace( Vector2 pos, Camera cam = null )
    {
        if ( cam == null ) cam = Camera.main;
        Vector3 posToCheck = new Vector3( pos.x, pos.y, 10f );
        return cam.ScreenToWorldPoint( posToCheck );
    }

    public Vector3 GetCursorPosInWorldSpace( Camera cam = null )
    {
        return GetScreenPosInWorldSpace( Input.mousePosition, cam );
    }

    private bool ShouldUseKeyboard()
    {
        return  Application.isEditor || 
                ( Application.platform != RuntimePlatform.IPhonePlayer && 
                Application.platform != RuntimePlatform.Android ); 
    }
}
