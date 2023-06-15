using UnityEngine;


public sealed class InputControl : MonoBehaviour
{
    private float _axisHorizontalMove;
    public float axisHorizontalMove => _axisHorizontalMove;

    private float _axisVerticalMove;
    public float axisVerticalMove => _axisVerticalMove;

    private float _axisMouseScrollWheel;
    public float axisMouseScrollWheel => _axisMouseScrollWheel;



    private void Update()
    {
        AxisMovement();
        AxisMouse();
    }

    private void AxisMovement()
    {
        _axisHorizontalMove = Input.GetAxisRaw("Horizontal");
        _axisVerticalMove = Input.GetAxisRaw("Vertical");
    }

    private void AxisMouse()
    {
        _axisMouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
    }
}
