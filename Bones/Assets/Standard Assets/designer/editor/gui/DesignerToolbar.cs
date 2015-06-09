using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace UnityDesigner
{

	[InitializeOnLoad]
	public class DesignerToolbar {
		
		static List<ToolbarItem> items = new List<ToolbarItem>();
		public static Rect lastRect;
		
		static DesignerToolbar()
		{
			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where( x => x.GetName().Name == "Assembly-CSharp-Editor" ).Single();
			
			Type[] types = assembly.GetTypes();
			
			items.Clear();
			
			foreach( Type type in types )
			{
				MethodInfo[] methods = type.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static )
					.Where( x => x.IsDefined( typeof(ToolbarItemAttribute), false ) ).ToArray();
				
				foreach( MethodInfo method in methods )
				{
					items.Add( new ToolbarItem( method ) );
				}
			}
			
			items = items.OrderBy( x => x.group ).ThenBy( x => x.attribute.priority ).ToList();
		
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.onSceneGUIDelegate += OnScene;
		}
		
	    static void OnScene(SceneView sceneView) {
		
			
			Handles.BeginGUI();

			GUILayout.BeginArea( new Rect(5, 5, Screen.width-10, 45) );

			
			EditorGUILayout.BeginHorizontal();
			

			
			int numShapesSelected = 0;
			foreach( UnityEngine.Object obj in Selection.objects )
			{
				GameObject go = obj as GameObject;	
				if( go != null && go.GetComponent<Shape>() != null )
				{
					numShapesSelected++;
				}
			}
			
			
			bool first = true;
			for( int i = 0; i < items.Count; i++ )
			{
				ToolbarItem item = items[i];
				bool last = (i + 1 == items.Count) || ( item.group != items[i+1].group );
				
				GUIStyle style = "ButtonMid"; 
				
				if( first && last ) style = "Button";
				else if ( first ) style = "ButtonLeft";
				else if ( last ) style = "ButtonRight";
				

				GUI.enabled = numShapesSelected >= item.attribute.minimumSelected;
				lastRect = GUILayoutUtility.GetRect(30, 25);
				if( GUI.Button( lastRect, new GUIContent( item.icon, item.attribute.tooltip ), style ) )
				{
					item.Invoke();
				}
				
				if( last )
				{
					GUILayoutUtility.GetRect(5, 25);
				}

				first = last;
			}

			GUI.enabled = true;
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		
			//int numDelegates = SceneView.onSceneGUIDelegate.GetInvocationList().Length;
			//EditorGUILayout.LabelField( numDelegates + " delegates" );
		
			GUILayout.EndArea();
			Handles.EndGUI();
		}
		
		
	}


	class ToolbarItem
	{
		public MethodInfo method;
		public ToolbarItemAttribute attribute;
		
		private Texture2D _icon;
		
		public ToolbarItem( MethodInfo method )
		{
			this.method = method;
			
			attribute = (ToolbarItemAttribute)System.Attribute.GetCustomAttribute( method, typeof(ToolbarItemAttribute) );	
		}
		
		
		public Texture2D icon
		{
			get
			{
				if( _icon == null )
				{
					_icon = ShapeIcons.GetIcon( attribute.icon );
				}
				
				return _icon;
			}
		}
		
		public string group
		{
			get
			{
				return attribute.group;
			}
		}
		
		public void Invoke()
		{
			method.Invoke(null,null);
		}
		
		
		
		
	}




	public class ToolbarItemAttribute : Attribute {

		public string group;
		public string icon;
		public string tooltip;
		public int priority;
		public int minimumSelected;
		
		public ToolbarItemAttribute( string group, string icon, string tooltip, int minimumSelected, int priority )
		{
			this.group = group;
			this.icon = icon;
			this.tooltip = tooltip;
			this.priority = priority;
			this.minimumSelected = minimumSelected;
		}
		
	}

}