using UnityEngine;
using System.Collections;

public class CameraBackgroundColorChanger : MonoBehaviour 
{
    public Camera colorCamera;

    public ColorChangeType changeType; 
    public Color[] colors;
    public float changeSpeed;

    private float _currentValue;
    private ColorDeltaMovementType _colorDeltaMovementType = ColorDeltaMovementType.Ascending;
	
	void Update () 
    {
	    ChangeColor();
	}

    private void ChangeColor()
    {
        switch( changeType )
        {
            case( ColorChangeType.PingPong ):
                PingPongColor();
            break;
        }
    }

    private void PingPongColor()
    {
        switch( _colorDeltaMovementType )
        {
            case( ColorDeltaMovementType.Ascending ):
                _currentValue += Time.deltaTime * changeSpeed;
                if ( _currentValue > 1f ) 
                {
                    _colorDeltaMovementType = ColorDeltaMovementType.Descending;
                    _currentValue = 1f;
                }

            break;

            case( ColorDeltaMovementType.Descending ):

                _currentValue -= Time.deltaTime * changeSpeed;
                if ( _currentValue < 0f ) 
                {
                    _colorDeltaMovementType = ColorDeltaMovementType.Ascending;
                    _currentValue = 0f;
                }

            break;
        }

        Color currentColor = GetColorFromColorArrayByValue( _currentValue );
        colorCamera.backgroundColor = currentColor;
        
    }

    private Color GetColorFromColorArrayByValue( float val )
    {
        float totalValue = (float)( colors.Length - 1 );
        float relativeValue = val * totalValue;
        
        int floor = Mathf.FloorToInt( relativeValue );
        int ceiling = Mathf.CeilToInt( relativeValue );
        float internalVal = relativeValue - floor;
        
        return Color.Lerp( colors[floor], colors[ceiling], internalVal );
    }
}

public enum ColorChangeType
{
    PingPong
}

public enum ColorDeltaMovementType
{
    Ascending,
    Descending
}
