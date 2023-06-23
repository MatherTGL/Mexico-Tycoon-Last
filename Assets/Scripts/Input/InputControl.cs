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

    [SerializeField, BoxGroup("Parameters/Keycodes")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeNumberOne = KeyCode.Alpha1;
    public KeyCode keycodeNumberOne => _keycodeNumberOne;

    [SerializeField, BoxGroup("Parameters/Keycodes")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeNumberTwo = KeyCode.Alpha2;
    public KeyCode keycodeNumberTwo => _keycodeNumberTwo;

    [SerializeField, BoxGroup("Parameters/Keycodes")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeNumberThree = KeyCode.Alpha3;
    public KeyCode keycodeNumberThree => _keycodeNumberThree;

    [SerializeField, BoxGroup("Parameters/Keycodes")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeNumberFour = KeyCode.Alpha4;
    public KeyCode keycodeNumberFour => _keycodeNumberFour;

    [SerializeField, BoxGroup("Parameters/Keycodes")]
    [FoldoutGroup("Parameters/Keycodes/Special")]
    private KeyCode _keycodeSpace = KeyCode.Space;
    public KeyCode keycodeSpace => _keycodeSpace;

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
