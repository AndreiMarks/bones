using UnityEngine;
using UnityEditor;
using System.Collections;


namespace UnityDesigner
{

	[CustomEditor (typeof (Circle))]
	[CanEditMultipleObjects]
	public class CircleEditor : ShapeEditor {

	    [MenuItem ("Designer/Create/Circle", false, 1)]
	    [ToolbarItem("1_Create", "circle", "Circle", 0, 1)]
	    static void CreateCircle () {
	    	CreateShape<Circle>();
	    }

	    public override void OnInspectorGUI()
	    {
	    	base.OnInspectorGUI();

	    	Circle circle = target as Circle;
	    	circle.radius = EditorGUILayout.FloatField( "Radius", circle.radius );
	    }

		public override void OnEditorGUI()
	    {
			Circle circle = target as Circle;

			//LabelVertices();
			
			Handles.matrix = circle.transform.localToWorldMatrix;
			Handles.color = Color.red;
			

			

			
			ShapeGUI.NameNextHandle("ToAngle");
			float toAngle = ShapeGUI.SnapAngleHandle( circle.toAngle, circle.size, ShapeGUI.IsCurrentHandle("ToAngle") );
			
			if( toAngle <= circle.fromAngle ) toAngle += 360;

			if( circle.toAngle != toAngle )
			{
				RegisterUndo();
				circle.toAngle = toAngle;
			}

			ShapeGUI.NameNextHandle("FromAngle");
			float fromAngle = ShapeGUI.SnapAngleHandle( circle.fromAngle, circle.size, ShapeGUI.IsCurrentHandle("FromAngle") );
			
			if( circle.fromAngle != fromAngle )
			{
				RegisterUndo();
				circle.fromAngle = fromAngle;
			}

			
			float irAngle = Mathf.Lerp( circle.fromAngle, circle.toAngle, .5f);
			Vector3 radius = GetPointForAngle( irAngle, circle.size );
			if( radius.magnitude != 0 )
			{
				Vector3 irHandle = radius * circle.innerRadius;

				ShapeGUI.NameNextHandle("InnerRadius");
				irHandle = ShapeGUI.VectorHandle( irHandle );
				
				
				
				float innerRadius = irHandle.magnitude / radius.magnitude;
				if( Vector3.Dot( irHandle, radius ) < 0 ) innerRadius = 0;

				if( circle.innerRadius != innerRadius )
				{
					RegisterUndo();
					circle.innerRadius = innerRadius;
				}

			}


			float depthAngle = Mathf.Lerp( circle.fromAngle, circle.toAngle, .5f);
			Vector2 depthPoint = GetPointForAngle( depthAngle, circle.size );
			float[] angles = new float[]{ depthAngle, circle.fromAngle, circle.toAngle };
			Vector2[] depthLines = new Vector2[6];
			for(int i = 0; i  < angles.Length; i++ )
			{
				depthLines[i * 2 + 0] = GetPointForAngle( angles[i], circle.size );
				depthLines[i * 2 + 1] = circle.innerRadius * GetPointForAngle( angles[i], circle.size );
			}

			DepthHandle( depthPoint, depthLines );
			
			if( GUI.changed )
			{
				circle.Draw();
			}
			
			DrawOutline( circle );

		}

	    Vector2 GetPointForAngle( float angle, Vector2 size )
	    {
	        angle *= Mathf.Deg2Rad;
	        return new Vector2( Mathf.Sin(angle) * size.x * 0.5f, Mathf.Cos(angle) * size.y * 0.5f);
	    }
		

	}

}