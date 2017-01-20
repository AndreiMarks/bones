using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Mono;
using System.Reflection;
using System.IO;
using System;

public class SettingsWindow : EditorWindow {

	public static string projectAssetPath = "Assets/resources/settings/";
	private Editor settingsEditor;
	private Vector2 scrollPos = Vector2.zero;
	private Type[] _settingsTypes;
	private bool _shouldRefresh = false;
	private bool _wasPlaying = false;
	
	private Dictionary<Type, Editor> _editors = new Dictionary<Type, Editor>();

	[MenuItem ("Window/Settings %g")]
    static void Init()
    {
        SettingsWindow window = (SettingsWindow)EditorWindow.GetWindow (typeof (SettingsWindow));
        window.title = "Settings";
        window.Show();
    }
	void OnInspectorUpdate()
    {
		Repaint();
    } 
    
    void Update()
    {
    	if(EditorApplication.isCompiling)
    	{
    		_shouldRefresh = true;
		} else if( _shouldRefresh )
		{
			RefreshList();
			_shouldRefresh = false;
		}
		
		if( EditorApplication.isPlaying && !_wasPlaying)
		{
			SetModeAll( SettingsViewMode.Scene );
			_wasPlaying = true;
		}
		
		if( !EditorApplication.isPlaying && _wasPlaying)
		{
			SetModeAll( SettingsViewMode.Prefab );
			_wasPlaying = false;
		}
    }        
    Color prefabColor = new Color(0, .67f, 1f, 1);
	Color prefabColorDim = new Color(.5f, .35f, 0, 1);
	
	void OnGUI ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos );
		
//		prefabColor = EditorGUILayout.ColorField( prefabColor );
		prefabColorDim = prefabColor;
		prefabColorDim.a = .5f;
		
		EditorGUILayout.BeginVertical();
		
		GUIStyle style = new GUIStyle( EditorStyles.foldout );
		
		style.active = EditorStyles.boldLabel.active;
		style.font = EditorStyles.boldLabel.font;
		style.fontSize = EditorStyles.boldLabel.fontSize;
		style.fontStyle = EditorStyles.boldLabel.fontStyle;
		
		if( settingsTypes.Length == 0 )
		{
			EditorGUILayout.HelpBox("No Settings in this project. Create a class that extends Settings to get started, like so:", MessageType.Info, true);
			EditorGUILayout.HelpBox("public class MySettings : Settings<MySettings>", MessageType.None, true);
		}
		
		foreach(Type type in settingsTypes)
		{			
			GUI.enabled = true;
	
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
			bool isOpen = EditorPrefs.GetBool( "Foldout" + type.Name );
			isOpen = EditorGUILayout.Foldout( isOpen, Humanize( type.Name ), style );
			EditorPrefs.SetBool( "Foldout" + type.Name, isOpen );
			
			if( !Application.isPlaying ) EditorGUI.indentLevel++;
			
			
			if( isOpen )
			{
				

				if( Application.isPlaying )
				{
				
					Rect rect = EditorGUILayout.GetControlRect();
					rect.width /=2;
					GUI.color = GetMode( type ) == SettingsViewMode.Scene ? Color.white : Color.grey;
					GUI.enabled = Application.isPlaying;
					if( GUI.Button( rect, "Scene", "TL tab left" ) )
					{
						SetMode( type, SettingsViewMode.Scene );
						ClearEditor( type );
					}
					
				
					
					
					rect.x += rect.width;
					GUI.color = GetMode( type ) == SettingsViewMode.Prefab ? prefabColor : prefabColorDim;
					GUI.enabled = true;
					if( GUI.Button( rect, "Prefab", "TL tab right" ) )
					{
						SetMode( type, SettingsViewMode.Prefab );
						ClearEditor( type );
					}
				
					GUI.enabled = true;

					GUI.color = GetMode( type ) == SettingsViewMode.Prefab ? prefabColor : Color.white;

					EditorGUILayout.BeginVertical( "box" );
					
					EditorGUILayout.Space();
					
				} else {
					EditorGUILayout.BeginVertical( );
				}
				
				string path = projectAssetPath + type.Name + ".prefab";	
				var instance = AssetDatabase.LoadAssetAtPath(path, type);
				bool hasPrefab = instance != null;
				
				if( Application.isPlaying && GetMode( type ) == SettingsViewMode.Scene )
				{
					Component com = FindObjectOfType( type ) as Component; 
					instance = com;
				}

				if( Application.isPlaying )
				{
					GUI.enabled = ( GetMode(type) == SettingsViewMode.Scene && instance != null);
					EditorGUILayout.BeginHorizontal();
					
					if( GUILayout.Button("Select", EditorStyles.miniButtonLeft) )
					{
						Component com = FindObjectOfType( type ) as Component;
						if( com != null )
						{
							GameObject obj = com.gameObject;
							Selection.objects = new GameObject[]{ obj };
						}
						
						
					}
					
					if( GUILayout.Button("Revert", EditorStyles.miniButtonMid) )
					{
						SaveValues( AssetDatabase.LoadAssetAtPath(path, type), instance );
					}
					
					if( GUILayout.Button("Apply", EditorStyles.miniButtonRight) )
					{
						SaveValues( instance, AssetDatabase.LoadAssetAtPath(path, type) );
					}
					
						
					EditorGUILayout.EndHorizontal();
					GUI.enabled = true;
					EditorGUILayout.Space();
				} 
				
				
				GUI.color = Color.white;
	
				
				
				if( instance == null )
				{
					if( GetMode(type) == SettingsViewMode.Scene && hasPrefab )
					{
						GUI.enabled = false;
						GUILayout.Button(type.Name + " not referenced in scene.");
						GUI.enabled = true;	 
					} else {
						if( GUILayout.Button("Create " + type.Name + " prefab") )
						{
							CreatePrefab( type );
						}
					}
					
					ClearEditor( type );
					
				} else if ( instance != null ) {
					
					Editor editor = GetEditor( type, instance );
					editor.OnInspectorGUI();

				}
				

				EditorGUILayout.EndVertical();		
				
			}
			
			
			if( !Application.isPlaying ) EditorGUI.indentLevel--;
		}
		
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}
	
	
	void SaveValues( UnityEngine.Object from, UnityEngine.Object to )
	{
		SerializedObject source = new SerializedObject( from );
		SerializedObject destination = new SerializedObject( to );
				
		
		SerializedProperty property = source.GetIterator();
		property.NextVisible(true);
		while( property.NextVisible(false) )
		{
			destination.CopyFromSerializedProperty( property );
		}
		
		destination.ApplyModifiedProperties();	
	}
	
	void CreatePrefab(Type type)
	{
		CheckPath();
		string path = projectAssetPath + type.Name + ".prefab";
		UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab( path );		
		GameObject go = new GameObject( type.Name );
		UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent( go, "Assets/Standard Assets/settings/editor/SettingsWindow.cs (242,3)", type.Name );
		PrefabUtility.ReplacePrefab( go, prefab, ReplacePrefabOptions.ConnectToPrefab );
		GameObject.DestroyImmediate( go );
	}
	
	void ClearEditor(Type type)
	{
		Editor editor = null;
		if( _editors.TryGetValue( type, out editor ) )
		{
			DestroyImmediate( editor );
			_editors.Remove( type );
		}
	}
	
	Editor GetEditor( Type type, UnityEngine.Object instance )
	{
		Editor editor = null;
		
		if( !_editors.TryGetValue( type, out editor ) )
		{
			editor = Editor.CreateEditor( instance );
			_editors[type] = editor;
		}
		
		return editor;
	}
	
	SettingsViewMode GetMode(Type type)
	{
	 	return (SettingsViewMode)EditorPrefs.GetInt( "ViewMode" + type.Name );
	}
	
	void SetMode( Type type, SettingsViewMode mode )
	{
		EditorPrefs.SetInt( "ViewMode" + type.Name, (int)mode );
	}
	
	
	void SetModeAll( SettingsViewMode mode )
	{
		foreach(Type type in settingsTypes)
		{
			SetMode( type, mode );
		}
		
		_editors.Clear();
	}
	
	void CheckPath()
	{
		if(!Directory.Exists( Application.dataPath + "/resources" ) )
		{
			AssetDatabase.CreateFolder("Assets", "resources");
		}
		
		if(!Directory.Exists( Application.dataPath + "/resources/settings" ) )
		{
			AssetDatabase.CreateFolder("Assets/resources", "settings");
		}
	}
	
	void RefreshList()
	{
		_settingsTypes = GetAllSubTypes( typeof(Settings<>) );
	}
	
	
	public Type[] settingsTypes
	{
		get
		{
			if( _settingsTypes == null)
			{
				RefreshList();
			}
			
			return _settingsTypes;
		}
	}
	
	string Humanize( string name )
	{
		if( name.IndexOf("Settings") >= 0 ) return name.Replace("Settings", "") + "s";
		else return name;
	}
	
	static Type[] GetAllSubTypes(Type aBaseClass)
	{
		List<Type> result = new List<Type>();
		Assembly[] AS = System.AppDomain.CurrentDomain.GetAssemblies();
		foreach (var A in AS)
		{
			Type[] types = A.GetTypes();
			foreach (Type T in types)
			{
				if (aBaseClass != T && IsSubclassOfRawGeneric(aBaseClass, T))
					result.Add(T);
			}
		}
		return result.ToArray();
	}
	
	static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
		while (toCheck != null && toCheck != typeof(object)) {
			var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
			if (generic == cur) {
				return true;
			}
			toCheck = toCheck.BaseType;
		}
		return false;
	}
}

enum SettingsViewMode
{
	Prefab,
	Scene
}