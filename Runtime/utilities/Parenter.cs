using UnityEngine;
using System.Collections;

public class Parenter : MonoBehaviour
{
    public static void MakeParent( Transform parent, Transform child, string name )
    {
        child.parent = parent;
        child.gameObject.name = name;
    }

    public static T InstantiateAndParent<T>( T obj, Transform parent, string name ) where T:UnityEngine.Component
    {
        return InstantiateAndParent( obj, obj.gameObject.transform.position, Quaternion.identity, parent, name );
    }
    public static T InstantiateAndParent<T>( T obj, Vector3 pos, Quaternion rot, Transform parent, string name ) where T:UnityEngine.Component
    {
        GameObject goToInstantiate = obj.gameObject;

        if ( obj.gameObject == null )
        {
            Debug.Log( "Not a GameObject." );
            return null;
        }

        GameObject newObject = (GameObject)Instantiate( goToInstantiate, pos, rot );
        newObject.name = name;
        newObject.transform.parent = parent;

        return newObject.GetComponent<T>();
    }
}
