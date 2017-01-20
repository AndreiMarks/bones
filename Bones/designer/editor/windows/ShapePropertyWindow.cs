using UnityEngine;
using UnityEditor; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UnityDesigner
{

	public class ShapePropertyWindow : EditorWindow
	{  

		Vector2 scrollPos;
		GUIStyle foldoutStyle;

		SerializedObjects<Shape> serializedShape;

		Vector2 scrollPosition = Vector2.zero;
		string lastSelectionHash = "";


		Material material;
	    MaterialEditor editor;
	    ShapeFillSide currentFillSide;

		[MenuItem ("Designer/Properties")]  
		//[ShapeToolbarItem( "5_Properties", "shape_properties", "Properties", 0, 1 )]
		public static void Init ()
		{
			// Get existing open shapeWindow or if none, make a new one:
		 	ShapePropertyWindow shapeWindow = (ShapePropertyWindow)EditorWindow.GetWindow (typeof (ShapePropertyWindow));
			shapeWindow.title = "Properties";    
			shapeWindow.autoRepaintOnSceneChange = true;
			shapeWindow.Show();
		}

		void OnGUI ()
		{
			scrollPosition = GUILayout.BeginScrollView( scrollPosition );
			
			foldoutStyle = new GUIStyle("IN Title");
			foldoutStyle.fixedWidth = position.width;
			
			UpdateSelection();

			if( serializedShape == null || serializedShape.objects.Count == 0 )
			{
				GUI_NoShape();
			} else {

				EditorGUI.BeginChangeCheck();

				GUI_General();
				GUI_Shapes();
				GUI_Transform();
				GUI_Style();

				if( EditorGUI.EndChangeCheck() )
				{
					List<Shape> shapes = serializedShape.targetObjects;
					foreach( Shape shape in shapes )
					{
						shape.Draw();
					}
				}

			}
					
			GUILayout.EndScrollView();
		}

		void OnSelectionChange()
		{
			Repaint();
		}

		void OnEnable()
		{
			lastSelectionHash = "";
		}

		void UpdateSelection()
		{

			string currentSelectionHash = SelectionEditor.GetSelectionHash();

			if( lastSelectionHash != currentSelectionHash )
			{
				//split shapes into group s based on type
				Shape[] shapes = SelectionEditor.GetSelectedShapes();
				serializedShape = new SerializedObjects<Shape>( shapes );
				lastSelectionHash = currentSelectionHash;

			}
			
		}


		void GUI_General()
		{
			if( serializedShape == null ) return;
			
			if( !Foldout( "General", new GUIContent(" General", ShapeIcons.GetIcon("fill_all") ) ) ) return;
					
			EditorGUI.indentLevel++;
			serializedShape.PropertyField("_fidelity", SerializedPropertyType.Float);
			serializedShape.PropertyField("_size" , SerializedPropertyType.Vector2);
			serializedShape.PropertyField("_depth", SerializedPropertyType.Float);
		
			EditorGUI.indentLevel--;
		}



		void GUI_Shapes()
		{
			if( serializedShape == null ) return;
			
			var serializedObjects = serializedShape.objects;
			foreach( SerializedObject serializedObject in serializedObjects )
			{
				string type = serializedObject.targetObject.GetType().ToString();
				if( !Foldout( type, new GUIContent(" " + type, ShapeIcons.GetIcon( type.ToLower() ) ) ) )
				{
					continue;
				}

				EditorGUI.indentLevel++;
					
					serializedObject.Update();
					var serializedPropertyNames = (serializedObject.targetObject as Shape).serializedPropertyNames;

					SerializedProperty[] properties = new SerializedProperty[ serializedPropertyNames.Length ];
					for(int i = 0; i < serializedPropertyNames.Length; i++ )
					{
						SerializedProperty prop = serializedObject.FindProperty( serializedPropertyNames[i] );
						properties[i] = prop;
					}

					EditorGUI.BeginChangeCheck();

					foreach( SerializedProperty prop in properties )
					{
						EditorGUILayout.PropertyField( prop );
					}
					
					if( EditorGUI.EndChangeCheck() )
					{
						serializedObject.ApplyModifiedProperties();
					}


				EditorGUI.indentLevel--;
			}
		}

		void GUI_Transform()
		{
			if( Selection.activeTransform == null ) return;
			if( !Foldout("Transform", new GUIContent(" Transform", EditorGUIUtility.FindTexture("Transform Icon") )) ) return;
			
			EditorGUI.indentLevel++;

				Transform t = Selection.activeTransform;

				// Replicate the standard transform inspector gui
				t.localPosition = EditorGUILayout.Vector3Field("Position", t.localPosition);
				t.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
				t.localScale = EditorGUILayout.Vector3Field("Scale", t.localScale);


			EditorGUI.indentLevel--; 
		}

		void GUI_NoShape()
		{
			EditorGUILayout.HelpBox( "No Shapes Selected", MessageType.Info );
		}	



		void GUI_Style()
		{
			if( serializedShape == null ) return;
			if( !Foldout( "Style", new GUIContent(" Style", ShapeIcons.GetIcon("style") ) ) ) return;

	        Shape shape = serializedShape.targetObjects[0];
	        ShapeFill fill = shape.GetFillByType( currentFillSide );
	       	
	    	var mat = ShapeMaterial.RequestMaterial( fill );
	    	if( material != mat )
	    	{
	    		editor = null;
	    		material = mat;
	    	}

	        if( editor == null )
	        {
	        	editor = Editor.CreateEditor(material) as MaterialEditor;
	        }


		   //PREVIEW
		   	EditorGUILayout.Space();    
			float size = Mathf.Min( position.width, 200 );
	        editor.OnPreviewGUI( GUILayoutUtility.GetRect(size, size), "flow background" );
	       


	        //SIDES
	        EditorGUI.BeginChangeCheck();
	        
	        shape.useMultiFaceFill = EditorGUILayout.Toggle(new GUIContent("Use Multi-Face Fill", "This lets you set different materials and colors for the front, sides, and back of your shape"), shape.useMultiFaceFill );

	        EditorGUILayout.BeginVertical("box");



	        if( shape.useMultiFaceFill )
	        {
	        	if( currentFillSide == ShapeFillSide.All ) currentFillSide = ShapeFillSide.Front;
		       	int i = GUILayout.Toolbar( (int)currentFillSide - 1, new GUIContent[]{ 
					new GUIContent("Front", ShapeIcons.GetIcon("fill_front")),
		        	new GUIContent("Side", ShapeIcons.GetIcon("fill_side")),
		        	new GUIContent("Back", ShapeIcons.GetIcon("fill_back")),
		        }, "toolbarbutton" );

				currentFillSide = (ShapeFillSide)i + 1;
		    } else {
		    	currentFillSide = ShapeFillSide.All;
		    }


	        //PROPERTIES

		    bool useLabels = position.width > EditorGUIUtility.labelWidth ;
		    
		    
		    string title = "Show " + currentFillSide;

		    if( currentFillSide != ShapeFillSide.All ) fill.isVisible = EditorGUILayout.Toggle(title, fill.isVisible);

	        fill.isLit = EditorGUILayout.Toggle("Accept Lighting", fill.isLit);
	        fill.blendMode = (ShapeBlendMode)EditorGUILayout.EnumPopup( useLabels ? new GUIContent("Blend Mode") : GUIContent.none, fill.blendMode );
	        fill.fillType = (ShapeFillType)EditorGUILayout.EnumPopup( useLabels ? new GUIContent("Fill Type") : GUIContent.none, fill.fillType );

	        string currentFillName = currentFillSide.ToString().ToLower() + "Fill";
	        SerializedObject obj = new SerializedObject(shape);
			SerializedProperty fillProp = obj.FindProperty( currentFillName );

			

			string[] properties = new string[0];
	        switch( fill.fillType )
	        {
	        	case ShapeFillType.Solid:
	        		properties = new string[]{ "color" };
	        	break;

	        	case ShapeFillType.Gradient:
					properties = new string[]{ "gradient", "gradientType", "gradientAngle" };
	        	break;

	        	case ShapeFillType.Pattern:
	        		properties = new string[]{ "color", "secondaryColor" };
	        	break;

	        	case ShapeFillType.Texture:
	        		properties = new string[]{ "color", "texture" };
	        	break;
	        }
	        
	        foreach( string s in properties )
	        {
	        	SerializedProperty prop = fillProp.FindPropertyRelative( s );
			    EditorGUILayout.PropertyField(prop, useLabels ? null : GUIContent.none );
	        }


	        EditorGUILayout.EndVertical();

	      	

	        if( EditorGUI.EndChangeCheck() )
	        {
	        	obj.ApplyModifiedProperties();
	        }


		}

		bool Foldout( string name, GUIContent label )
		{
			bool isOpen = EditorPrefs.GetBool(name, true);
			bool result = EditorGUILayout.Foldout( isOpen, label, foldoutStyle ); 

			if( isOpen != result )
			{
				EditorPrefs.SetBool(name, result);	
			}

			return result;
			

		}



		

	}


}