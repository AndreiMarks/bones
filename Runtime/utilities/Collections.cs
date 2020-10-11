using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Collections 
{
    /// <summary>
    /// Get a random element from a list
    /// </summary>
    /// <returns>A random element.</returns>
    public static T RandomElement<T>( this IList<T> collection )
    {
        if ( collection.Count > 0 )
            return collection[UnityEngine.Random.Range( 0, collection.Count )];

        return default( T );
    }
}
