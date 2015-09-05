using UnityEngine;
using System.Collections;

public class Colors : MonoBehaviour {

    public static Color GetRandomColor()
    {
        return new Color( Random.value, Random.value, Random.value, 1f );
    }

    public static Color GetFullColor( Color color )
    {
        return GetColorWithAlpha( color, 1f );
    }

    public static Color GetClearColor( Color color )
    {
        return GetColorWithAlpha( color, 0f );
    }

    public static Color GetColorWithAlpha( Color color, float alpha )
    {
        return new Color( color.r, color.g, color.b, alpha );
    }
}
