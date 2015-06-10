using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UnityDesigner
{
	[CustomEditor (typeof (Star))]
	[CanEditMultipleObjects]
	public class StarEditor : ShapeEditor {

		[MenuItem ("Designer/Create/Star", false, 4)]
	    [ToolbarItem("1_Create", "star", "Star", 0, 3)]
	    static void CreateStar () {
	    	CreateShape<Star>();
	    }



	    public override void OnEditorGUI()
	    {

			Star star = target as Star;
			//LabelVertices();

			Handles.matrix = star.transform.localToWorldMatrix;
			Handles.color = ShapeGUI.handleColor;
			

			
			float angle = 360 / (float)(star.points * 2) * Mathf.Deg2Rad;

			float s = Mathf.Sin(angle);
			float c = Mathf.Cos(angle);

			Vector2 maxRadius = new Vector2(s * star.size.x * 0.5f, c * star.size.y * 0.5f);
			Vector2 innerRadiusHandle = new Vector2(s * star.size.x * 0.5f, c * star.size.y * 0.5f) * star.innerRadius;


			ShapeGUI.NameNextHandle("InnerRadius");
			innerRadiusHandle = ShapeGUI.VectorHandle( innerRadiusHandle );

			Vector3 normal = Vector3.Cross( innerRadiusHandle, Vector3.up ).normalized;

			float ratio = innerRadiusHandle.magnitude / maxRadius.magnitude;
			if( normal.z < 0 ) ratio = 0;
			float innerRadius = Mathf.Min(1, ratio) ;

			if( GUI.changed && star.innerRadius != innerRadius )
			{
				RegisterUndo();
				star.innerRadius = innerRadius;
			}

			if( star.drawer.points.Count > 0 )
			{

				DepthHandle( 
					star.drawer.points[0],
					star.drawer.points.ToArray()
				);

			}

			if( GUI.changed )
			{
				star.Draw();
			}

			DrawOutline( star );
		}
	}
}