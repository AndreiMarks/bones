using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{
    private float _currentSpeed;
    private int _direction;

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float rotateSpeed = _direction * _currentSpeed * Time.deltaTime;
        transform.Rotate( Vector3.forward * rotateSpeed );
    }

    public void SetDirection( int newDirection )
    {
        _direction = newDirection;
    }

    public void SetRandomDirection()
    {
        _direction = ( Random.value < .5f ) ? -1 : 1 ;
    }

    public void SetSpeed( float newSpeed )
    {
        _currentSpeed = newSpeed;
    }

    public void ToggleDirection()
    {
        _direction *= -1;
    }
}
