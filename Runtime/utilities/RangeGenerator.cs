using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RangeGenerator : MonoBehaviour 
{
    public static List<int> GetListOfIntegers( int count )
    {
        return Enumerable.Range( 0, count ).ToList();
    }
}
