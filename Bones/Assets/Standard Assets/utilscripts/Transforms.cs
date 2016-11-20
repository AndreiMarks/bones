using UnityEngine;
using System.Collections;

public static class TransformExtensions 
{
    /// <summary>
    /// Destroy all the children of a transform.
    /// </summary>
    /// <param name="transform"></param>
    public static void DestroyChildren(this Transform transform) {
        for(int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static T InstantiateChild<T>( this Transform trans, T obj ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, trans.position, trans.rotation, trans, obj.name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, Vector3 pos ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, pos, trans.rotation, trans, obj.name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, Vector3 pos, Quaternion rot ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, pos, rot, trans, obj.name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, string name ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, trans.position, trans.rotation, trans, name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, Vector3 pos, Quaternion rot, string name ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, pos, rot, trans, name );
    }

    /// <summary>
    /// Destroys all children.
    /// </summary>
    public static void DestroyAllChildren(this Transform transform) {
        foreach (Transform child in transform) {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }
}

