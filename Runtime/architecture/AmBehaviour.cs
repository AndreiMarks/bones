using UnityEngine;
using System.Collections;

public class AmBehaviour : MonoBehaviour 
{
    // Controllers
    protected StageController _stage            { get { return StageController.Instance; } }

    // Settings
    protected InputSettings _inputSettings      { get { return InputSettings.Instance; } }

    // Components
    Transform _t;
    public Transform t 
    {
        get 
        { 
            if ( _t == null )
            {
                _t = this.transform;
            }

            return _t;
        }
    }

    Rigidbody _rb;
    public Rigidbody rb 
    {
        get 
        { 
            if ( _rb == null )
            {
                _rb = this.GetComponent<Rigidbody>();
            }

            return _rb;
        }
    }
}
