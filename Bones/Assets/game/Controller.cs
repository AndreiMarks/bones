using UnityEngine;
using System.Collections;

public class Controller<T> : AmBehaviour where T : AmBehaviour
{
    static Transform _parent;
    static T _instance;
	public static T Instance
	{
		get
        { 
            if ( _instance == null ) _instance = LoadInstanceFromPrefab();
			return _instance; 
		}
	}

    void Awake()
    {
        if ( _instance != null )
        {
            Debug.Log( "_instance already exists. Returning");
            return;
        }
        _instance = this as T;
    }

    static T LoadInstanceFromPrefab()
    {
        T sceneObject = (T) FindObjectOfType( typeof(T) );
        if( sceneObject != null ) return sceneObject;

        GameObject prefab = Resources.Load(resourcePath, typeof(GameObject)) as GameObject;
        if( prefab == null ) return null;
        
        GameObject go = Instantiate( prefab ) as GameObject;
        go.name = typeof(T).ToString();
        
        if( _parent == null )
        {
            GameObject parentGO = GameObject.Find("_Controllers");
            if ( parentGO == null )
            {
                parentGO = new GameObject( "_Controllers" );
            }
            _parent = parentGO.transform;
            go.transform.parent = _parent;
        }
        
        T instance = go.GetComponent<T>();
        return instance;
    }

    static string resourcePath { get { return "controllers/" + typeof(T); } }
}
