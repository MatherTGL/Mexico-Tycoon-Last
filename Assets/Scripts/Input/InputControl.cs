using UnityEngine;
using Sirenix.OdinInspector;

public sealed class InputControl : MonoBehaviour
{
    [Title("Max Force Mouse Clamp", horizontalLine: false, subtitle: "Horizontal"), HideLabel]
    [HorizontalGroup("Parameters/Clamp"), SerializeField, BoxGroup("Parameters")]
    private float _maxForceMouseClampHorizontal;

    [SerializeField, BoxGroup("Parameters"), Title("", horizontalLine: false, subtitle: "Vertical"), HideLabel]
    [HorizontalGroup("Parameters/Clamp")]
    private float _maxForceMouseClampVertical;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Alpha 1")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeAccelerationTimeDefault = KeyCode.Alpha1;
    public KeyCode keycodeAccelerationTimeDefault => _keycodeAccelerationTimeDefault;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Alpha 2")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeAccelerationTimeTwo = KeyCode.Alpha2;
    public KeyCode keycodeAccelerationTimeTwo => _keycodeAccelerationTimeTwo;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Alpha 3")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeAccelerationTimeThree = KeyCode.Alpha3;
    public KeyCode keycodeAccelerationTimeThree => _keycodeAccelerationTimeThree;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Alpha 4")]
    [FoldoutGroup("Parameters/Keycodes/Alpha")]
    private KeyCode _keycodeAccelerationTimeFour = KeyCode.Alpha4;
    public KeyCode keycodeAccelerationTimeFour => _keycodeAccelerationTimeFour;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Space")]
    [FoldoutGroup("Parameters/Keycodes/Special")]
    private KeyCode _keycodeTimePause = KeyCode.Space;
    public KeyCode keycodeTimePause => _keycodeTimePause;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Left Control")]
    [FoldoutGroup("Parameters/Keycodes/Special")]
    private KeyCode _keycodeFastMove = KeyCode.LeftControl;
    public KeyCode keycodeFastMove => _keycodeFastMove;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Left Button")]
    [FoldoutGroup("Parameters/Keycodes/Mouse")]
    private KeyCode _keycodeLeftMouseButton = KeyCode.Mouse0;
    public KeyCode keycodeLeftMouseButton => _keycodeLeftMouseButton;

    [SerializeField, BoxGroup("Parameters/Keycodes"), LabelText("Right Button")]
    [FoldoutGroup("Parameters/Keycodes/Mouse")]
    private KeyCode _keycodeRightMouseButton = KeyCode.Mouse1;
    public KeyCode keycodeRightMouseButton => _keycodeRightMouseButton;

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
        _axisMouseScrollWheel = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel"), -5, 5); //!
        _axisMouseX = Mathf.Clamp(Input.GetAxis("Mouse X"), -_maxForceMouseClampHorizontal, _maxForceMouseClampHorizontal);
        _axisMouseY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -_maxForceMouseClampVertical, _maxForceMouseClampVertical);
    }
}
