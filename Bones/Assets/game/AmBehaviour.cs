using UnityEngine;
using System.Collections;

public class AmBehaviour : MonoBehaviour 
{
    // Controllers
    protected CameraController _cameras         { get { return CameraController.Instance; } }
    protected GameplayController _gameplay      { get { return GameplayController.Instance; } }
    protected InputController _input            { get { return InputController.Instance; } }
    protected StageController _stage            { get { return StageController.Instance; } }

    // Settings
    protected AnimationSettings _animationSettings{ get { return AnimationSettings.Instance; } }
    protected CameraSettings _cameraSettings    { get { return CameraSettings.Instance; } }
    protected InputSettings _inputSettings      { get { return InputSettings.Instance; } }
    protected GameplaySettings _gameplaySettings{ get { return GameplaySettings.Instance; } }
    protected PhaseSettings _phaseSettings      { get { return PhaseSettings.Instance; } }
    protected PrefabSettings _prefabSettings    { get { return PrefabSettings.Instance; } }

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
                _rb = this.rigidbody;
            }

            return _rb;
        }
    }
}
