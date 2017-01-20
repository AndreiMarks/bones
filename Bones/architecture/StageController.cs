using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : Controller<StageController>
{
    public Transform stage;
    
    private Dictionary<string, Transform> __holders;
    private Dictionary<string, Transform> _holders
    {
        get
        {
            if ( __holders == null ) __holders = new Dictionary<string, Transform>();
            return __holders;
        }
    }

    public Transform GetHolder( string name )
    {
        if ( !_holders.ContainsKey( name ) )
        {
            CreateHolder( name );
        }

        return _holders[name];
    }

    private void CreateHolder( string name )
    {
        Transform newHolder = (new GameObject(name + "Holder" ) ).transform;
        newHolder.parent = stage;
        _holders.Add( name, newHolder );
    }
}


