using UnityEngine;
using Sirenix.OdinInspector;


public sealed class InputControl : MonoBehaviour
{
    [SerializeField, BoxGroup("Parameters"), Title("Max Force Mouse Clamp", horizontalLine: false, subtitle: "Horizontal"), HideLabel]
    [HorizontalGroup("Parameters/Clamp")]
    private float _maxForceMouseClampHorizontal;

    [SerializeField, BoxGroup("Parameters"), Title("", horizontalLine: false, subtitle: "Vertical"), HideLabel]
    [HorizontalGroup("Parameters/Clamp")]
    private float _maxForceMouseClampVertical;


    private float _axisHorizontalMove;
    public float axisHorizontalMove => _axisHorizontalMove;

    private float _axisVerticalMove;
    public float axisVerticalMove => _axisVerticalMove;

    private float _axisMouseScrollWheel;
    public float axisMouseScrollWheel => _axisMouseScrollWheel;

    private float _axisMouseX;
    public float axisMouseX => _axisMouseX;

    private float _axisMouseY;
    public float axisMouseY => _axisMouseY;


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
        _axisMouseX = Mathf.Clamp(Input.GetAxis("Mouse X"), -_maxForceMouseClampHorizontal, _maxForceMouseClampHorizontal);
        _axisMouseY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -_maxForceMouseClampVertical, _maxForceMouseClampVertical);
    }
}
