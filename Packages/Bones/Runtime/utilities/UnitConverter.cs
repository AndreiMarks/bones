using UnityEngine;
using System.Collections;

public static class UnitConverter 
{
    public static float WorldToPixel
    {
        get
        {
            return ( ( Screen.height * .5f ) / Camera.main.orthographicSize );
        }
    }
}
