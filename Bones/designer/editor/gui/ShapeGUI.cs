using UnityEngine;
using UnityEditor;
using System.Collections;

public class ShapeGUI {


	public static Color shapeColor = Handles.zAxisColor; //new Color(.16f, .51f, .90f, 1);
	public static Color handleColor = Handles.xAxisColor; //new Color(.58f, .51f, .90f, 1);
	public static Color depthColor = Handles.yAxisColor; //new Color(.16f, .90f, .16f, 1);

	public static float vectorSnapThreshold = 0.05f;

	private static Texture2D _handleTexture;
	public static Texture2D handleTexture
	{
		get
		{
			if( _handleTexture == null )
			{
				_handleTexture = ShapeIcons.GetIcon("shape_handle");
			}
			
			return _handleTexture;
		}
	}
	
	private static GUIStyle _handleTextureStyle;
	public static GUIStyle handleTextureStyle
	{
		get
		{
			if( _handleTextureStyle == null )
			{
				_handleTextureStyle = new GUIStyle( GUI.skin.label );
				_handleTextureStyle.contentOffset = new Vector2(-handleTexture.width/2 ,-handleTexture.height/2);
			}
			
			return _handleTextureStyle;
		}
	}

	public static float GetHandleSize( Vector3 position ) 
	{ 
		return HandleUtility.GetHandleSize( position ) / 8;
	}
	
	public static void NameNextHandle( string name )
	{
		GUI.SetNextControlName( name );
	}
	
	public static bool IsCurrentHandle( string name )
	{
		return GUI.GetNameOfFocusedControl() == name;
	}
	


	public static bool Snap( float value, float snapValue, out float snappedValue )
    {
    	float snapThreshold = 5;
    	snappedValue = Mathf.Round( value / snapValue ) * snapValue;
    	if( Mathf.Abs( snappedValue - value ) <= snapThreshold ) return true;
    	else return false;
    }
	
	public static Vector3 GetPointOnRadius( float angle, Vector3 size )
	{
	    float angleRad = Mathf.Deg2Rad * angle;
		return new Vector3( Mathf.Sin( angleRad ) * size.x* 0.5f,  Mathf.Cos( angleRad ) * size.y* 0.5f, 0 );
	}
	
	
	public static Vector3 VectorHandle( Vector3 vector )
    {
  
		vector = Handles.FreeMoveHandle( vector, Quaternion.identity, GetHandleSize(vector), Vector3.zero, Handles.SphereCap );
				
		Handles.Label( vector, new GUIContent( handleTexture ), handleTextureStyle );
		
    	return vector;
    	
    }

    public static void DrawSnapLine( Vector3 start, Vector3 end )
    {
    	Color c = Handles.color;
		Handles.color = Color.yellow;
		Handles.DrawLine( start, end );
		Handles.color = c;	
    }

    public static Vector3 SnapVectorHandle( Vector3 vector, bool showSnap, params VectorSnapPoint[] snapPoints )
    {
    	vector = VectorHandle( vector );

    	if( Event.current.shift )
    	{

	    	foreach( VectorSnapPoint snapPoint in snapPoints )
	    	{
	    		float dist = Vector3.Distance( vector, snapPoint.snapPosition );
		    	if( dist < vectorSnapThreshold )
		    	{
		    		vector = snapPoint.snapPosition;

		    		if( showSnap )
		    		{
						DrawSnapLine( snapPoint.snapPosition, snapPoint.snapLineEnd );
		    		}
		    		break;
		    	}
	    	}
	    }
	    	

    	return vector;
    }
    
    
    public static float RadiusHandle( float angle, Vector3 size )
    {
    	Vector3 vector = VectorHandle( GetPointOnRadius( angle, size ) );
    	return vector.magnitude;
    }
    
    public static float AngleHandle( float angle, Vector3 size )
    {
		Vector3 vector = VectorHandle( GetPointOnRadius( angle, size ) );
    	return 180 + Mathf.Rad2Deg * Mathf.Atan2( -vector.x / (size.x* 0.5f), -vector.y / (size.y* 0.5f) );
    }
    
    public static float SnapAngleHandle( float angle, Vector3 size, bool showSnap )
    {
    	if( size.x == 0 || size.y == 0 ) return angle;

    	float fromAngle = AngleHandle( angle, size );
		float snappedAngle = fromAngle;
		bool snap = Snap( fromAngle, 45, out snappedAngle );
		
		float result = snap ? snappedAngle : fromAngle;
		

		if( showSnap && snap )
		{
			DrawSnapLine( GetPointOnRadius( result, size ), Vector3.zero );
		}
		
		return result;
    }



}



public class VectorSnapPoint
{
	public Vector3 snapPosition;
	public Vector3 snapLineEnd;

	public VectorSnapPoint(Vector3 snapPosition, Vector3 snapLineEnd)
	{
		this.snapPosition = snapPosition;
		this.snapLineEnd = snapLineEnd;
	}
}
