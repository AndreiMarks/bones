using UnityEngine;
using System.Collections;

public static class TransformExtensions 
{
    public static T InstantiateChild<T>( this Transform trans, T obj ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, trans.position, trans.rotation, trans, obj.name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, Vector3 pos ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, pos, trans.rotation, trans, obj.name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, string name ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, trans.position, trans.rotation, trans, name );
    }

    public static T InstantiateChild<T>( this Transform trans, T obj, Vector3 pos, Quaternion rot, string name ) where T:UnityEngine.Component
    {
        return Parenter.InstantiateAndParent( obj, pos, rot, trans, name );
    }

}


