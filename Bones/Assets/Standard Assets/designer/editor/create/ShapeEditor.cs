using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityDesigner
{
	[CanEditMultipleObjects]
	public class ShapeEditor : Editor {

		List<string> serializedPropertyNames = new List<string>();

		SerializedProperty[] _properties;

		static SerializedObject _serializedShapes;

		public static void CreateShape<T> () where T : Shape{
			string name = typeof(T).Name;
	        GameObject go = new GameObject( name );
	        go.AddComponent<T>();
	        go.transform.position = SceneView.lastActiveSceneView.pivot;
	        
	        Selection.objects = new Object[]{go};
	        
	        Undo.RegisterCreatedObjectUndo (go, "Create " + name );
	    }

		void OnEnable()
		{
			Shape shape = target as Shape;
			serializedPropertyNames.Clear();
			serializedPropertyNames.AddRange( shape.sharedSerializedPropertyNames );
			serializedPropertyNames.AddRange( shape.serializedPropertyNames );
			
	        Undo.undoRedoPerformed += Redraw;

	        SceneView.onSceneGUIDelegate -= OnScene;
	        SceneView.onSceneGUIDelegate += OnScene;
	        
	        HideWireframe();
	    }

	    void OnDisable()
	    {
	        Undo.undoRedoPerformed -= Redraw;

	        SceneView.onSceneGUIDelegate -= OnScene;
	    }
	    
		void OnScene(SceneView sceneView)
	    {
	        if( target == null )
	        {
	            SceneView.onSceneGUIDelegate -= OnScene;
	            return;
	        }
	        
	    
			OnEditorGUI(); 
		}


		public virtual void OnEditorGUI() { }



		override public void OnInspectorGUI()
		{	

			
			serializedObject.Update();
			
			foreach( SerializedProperty prop in properties )
			{
				EditorGUILayout.PropertyField( prop, new GUIContent( ObjectNames.NicifyVariableName( prop.name ) ));
			}

	    	serializedObject.ApplyModifiedProperties();
	    	
	    	
	    	
	    	if( GUI.changed && ! Application.isPlaying)
	    	{
	    		foreach( Shape shape in serializedObject.targetObjects )
	    		{
	    			shape.Draw();
	    		}
	    	}

		}
		
		public virtual void OnSceneGUI()
		{
				
		}

		
		public void LabelVertices()
		{
			Vector3[] verts = ((Shape)target).GetComponent<MeshFilter>().sharedMesh.vertices;

	        for(int i = 0; i < verts.Length; i++)
	        {
	            Handles.Label( verts[i], i.ToString() );
	        }
		}


		public void Redraw()
		{
			Shape shape = target as Shape;
			if( shape == null ) return;
			shape.Draw();
		}

		public void RegisterUndo()
		{
			Undo.RecordObject ( target, "Edit " + target.GetType().Name );
		}

		public void DrawOutline( Shape shape )
		{
			Drawer drawer = shape.drawer;
			Color color = Handles.color;
			Handles.color = ShapeGUI.shapeColor;
	 
			for( float i = 0; i <= shape.depth; i += shape.depth )
			{
				List<Vector3> points = drawer.GetVertices(i);
			
				if( points.Count > 0 )
				{
					points.Add( points[0] );
					Handles.DrawPolyLine( points.ToArray() );
				}
				
				if( shape.depth == 0 ) break;
				
				Handles.color = ShapeGUI.IsCurrentHandle("Depth") ? Handles.selectedColor : ShapeGUI.depthColor;
			}

			Handles.color = color;
			
		}

		public void DepthHandle( Vector2 point, params Vector2[] depthLines )
		{
			
			Shape shape = target as Shape;

			Handles.color = ShapeGUI.depthColor;

			Vector3 depthPoint = new Vector3( point.x, point.y, shape.depth );
			ShapeGUI.NameNextHandle("Depth");
			Vector3 newDepthPoint = ShapeGUI.VectorHandle( depthPoint );
			if( depthPoint != newDepthPoint )
			{
				RegisterUndo();
				shape.depth = Mathf.Max( newDepthPoint.z, 0 ) ;
			}
		
			if( ShapeGUI.IsCurrentHandle("Depth") )
			{
				Handles.color = Handles.selectedColor;
			}

			
			foreach( Vector2 depthLine in depthLines )
			{
				Handles.DrawLine( new Vector3( depthLine.x, depthLine.y, shape.depth), (Vector3)depthLine );	
			}

			
			//Handles.DrawLine( depthPoint, new Vector3(0,0,shape.depth) );
		}


		public void HideWireframe()
		{
			Shape shape = target as Shape;
			if( shape == null ) return;
			MeshRenderer meshRenderer = shape.GetComponent<Renderer>() as MeshRenderer;
			if( meshRenderer == null ) return;


			EditorUtility.SetSelectedWireframeHidden( meshRenderer, true);
		}

		public void ShowWireframe()
		{
			Shape shape = target as Shape;
			if( shape == null ) return;
			MeshRenderer meshRenderer = shape.GetComponent<Renderer>() as MeshRenderer;
			if( meshRenderer == null ) return;

			EditorUtility.SetSelectedWireframeHidden( meshRenderer, false);
		}


		public SerializedProperty[] properties
		{
			get
			{
				if( _properties == null )
				{
					_properties = new SerializedProperty[ serializedPropertyNames.Count ];
					for(int i = 0; i < serializedPropertyNames.Count; i++ )
					{
						SerializedProperty prop = serializedObject.FindProperty( serializedPropertyNames[i] );
						_properties[i] = prop;
					}
				}
				
				return _properties;
			}
			
		}

	}





	[CustomPropertyDrawer( typeof( ShapeAnchorPoint ) )]
	public class ShapeAnchorPointDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) 
	    {

			
	    	position.xMin += 5;
			position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), label );
			
			ShapeAnchorPoint[] types = (ShapeAnchorPoint[])System.Enum.GetValues(typeof(ShapeAnchorPoint));

			
			Rect pos = new Rect( position.x, position.y, 15, 15 );
			

			ShapeAnchorPoint returnValue = (ShapeAnchorPoint)prop.enumValueIndex;
			bool changed = false;
			
	    	for( int y = 0; y < 3; y++ )
	    	{
	    		for( int x = 0; x < 3; x++ )
	    		{
	    			int i = 3 * y + x;
	    			bool isCurrentValue = types[i] == returnValue;
	    			
	    			if( prop.hasMultipleDifferentValues ) isCurrentValue = false;
	    			isCurrentValue = EditorGUI.Toggle(pos, isCurrentValue, EditorStyles.radioButton);
	    			
	    			if(isCurrentValue && types[i] != returnValue)
	    			{
	    				changed = true;
	    				returnValue = types[i];
	    			}
	    			pos.x += 15;
	    		}
	    		
	    		pos.x = position.x;
	    		pos.y += 15;
	    	}
	    	
	    	if(changed)
		    	prop.enumValueIndex = (int)returnValue;

	    }
	    
	    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return 50;
		}    
	    
	    
	}

}