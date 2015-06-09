using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class Settings<T> : MonoBehaviour where T : MonoBehaviour {

	public static string projectAssetPath = "Assets/resources/settings/";

	public static void DefaultAction<U>(U param){}
	public static void DefaultAction(){}
	
	static Transform _settingsParent;
	static T _instance;
	static bool isQuitting;
	
    EventInfo[] events;
	List<Delegate> delegates = new List<Delegate>();

	public static T Instance
	{
		get{ 
			if(_instance == null) _instance = LoadInstanceFromPrefab();
			return _instance; 
		}
	}
		
	public static bool Subscribe( Component com )
	{
		if( Instance != null && !isQuitting)
		{
			(Instance as Settings<T>).SubscribeComponent( com );
			return true;
		} else {
			return false;
		}
	}
	
	public static bool Unsubscribe( Component com )
	{
		if( Instance != null && !isQuitting)
		{
			(Instance as Settings<T>).UnsubscribeComponent( com );
			return true;
		} else {
			return false;
		}
	}
	
	public static T LoadInstanceFromPrefab()
	{
		if( Application.isPlaying && !isQuitting) 
		{
			//Check scene
			T inScene = FindObjectOfType( typeof(T) ) as T;
			if( inScene != null ) return inScene;

			GameObject prefab = Resources.Load(resourcePath, typeof(GameObject)) as GameObject;
			if( prefab == null )
			{
				Debug.LogError("No prefab found to instantiate " + typeof(T) + ". Click 'Create " + typeof(T)+ " prefab' in the Settings window to make one."  );
				return null;
			}
			
			GameObject go = Instantiate( prefab ) as GameObject;
			go.name = typeof(T).ToString();
			
			if( _settingsParent == null )
			{
	            GameObject parentGO = GameObject.Find("_Settings");
    	        if ( parentGO == null )
        	    {
            	    parentGO = new GameObject( "_Settings" );
                	DontDestroyOnLoad( parentGO );
	            }
    	        _settingsParent = parentGO.transform;
        	    go.transform.parent = _settingsParent;
        	}
			
			T instance = go.GetComponent<T>();
			if( instance == null ) 
				Debug.LogError("Settings prefab doesn't contain " + typeof(T) + " component");
		
			DontDestroyOnLoad(go);
			return instance;
			
		} else {
			#if UNITY_EDITOR
				string path = projectAssetPath + typeof(T).Name + ".prefab";
				var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T) );
				return asset as T;
			#else		
				return null;
			#endif
		}
	}
	
	
	static string resourcePath
	{
		get
		{
			return "settings/" + typeof(T);
		}
	}

	
	//------------------------------------------------------------------------------------------------
	
	
	void Awake()
	{
		_instance = this as T;
	}
	
	void OnApplicationQuit()
    {
    	isQuitting = true;
		DestroyInstance();
    }
    
    protected void FireEvent( Action e )
    {
    	foreach( Action action in e.GetInvocationList() )
    	{
    		Debug.Log( action.Target );
    		action();
    	}
    }
    
    protected void FireEvent<V>( Action<V> e, V param )
    {
    	foreach( Action<V> action in e.GetInvocationList() )
    	{
    		if( action.Target == null )
    		{
  //  			Debug.Log( "NULL: " + action.Method );
    		} else {
//	    		Debug.Log( action.Target + ": " + action.Method );
	    		action( param );
	    	}
    	}
    }
	
	void CacheEvents()
	{
		if( events != null ) return;	
		
		Type myType = GetType();
		events = myType.GetEvents( BindingFlags.Public  | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
	}
	
	MethodInfo GetEventHandlerMethod( Component com, EventInfo e )
	{
		MethodInfo method = com.GetType().GetMethod( e.Name , BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
		return method;
	}
		
	Delegate GetDelegate( Component com, EventInfo e )
	{
		MethodInfo method = GetEventHandlerMethod( com, e );
		if( method == null ) return null;
		
		Delegate handler = Delegate.CreateDelegate(e.EventHandlerType, com, method);
		return handler;
	}

	bool HasDelegate( Component com, EventInfo e )
	{
		foreach( Delegate d in delegates )
		{
			if( d.Target == com )
			{
				MethodInfo method = GetEventHandlerMethod( com, e );	
				if( d.Method == method )
				{
					return true;
				}
			}
		}
		
		return false;
	}
	
	void SubscribeComponent( Component com )
	{
		CacheEvents();
		
		foreach( EventInfo e in events )
		{
			if( HasDelegate( com, e ) ) continue;
			
			Delegate handler = GetDelegate( com, e );
			if( handler != null )
			{
				e.AddEventHandler(this, handler);
				delegates.Add( handler );
			}
		}
	}
	
	void UnsubscribeComponent( Component com )
	{
		CacheEvents();

		for( int i = delegates.Count - 1; i >= 0; i-- )
		{
			Delegate d = delegates[i];
			
			if( d.Target != com ) continue;

			bool shouldRemove = false;
			foreach( EventInfo e in events )
			{
				MethodInfo method = GetEventHandlerMethod( com, e );	
				if( d.Method == method )
				{
					e.RemoveEventHandler( this, d );	
					shouldRemove = true;
				}
			}
			
			if( shouldRemove )
			{
				delegates.Remove( d );
			}
		}
		

	}
		
	void DestroyInstance()
    {
        if (_instance != null) DestroyImmediate(_instance.gameObject);    
    }

	
}