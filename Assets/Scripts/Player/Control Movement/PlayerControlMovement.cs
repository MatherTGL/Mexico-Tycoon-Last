using UnityEngine;
using Sirenix.OdinInspector;
using Config.Player;
using TimeControl;
using Boot;


namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputControl))]
    [RequireComponent(typeof(TimeDateControl))]
    internal sealed class PlayerControlMovement : MonoBehaviour, IBoot
    {
#if UNITY_EDITOR
        [ShowInInspector, ToggleLeft, BoxGroup("Parameters")]
        [Title("Edit Parameters", horizontalLine: false), HideLabel]
        private readonly bool _isEditParametersEditor;
#endif

        [SerializeField, Required, BoxGroup("Parameters/Configs"), EnableIf("_isEditParametersEditor")]
        [HideLabel, Title("Player Control Move", HorizontalLine = false)]
        private ConfigPlayerControlMoveEditor _configPlayerControlMove;

        [SerializeField, Required, BoxGroup("Parameters/Components"), EnableIf("_isEditParametersEditor")]
        [HideLabel, Title("Input Control", HorizontalLine = false)]
        private InputControl _inputControl;

        [SerializeField, Required, BoxGroup("Parameters/Components"), EnableIf("_isEditParametersEditor")]
        [HideLabel, Title("Rigidbody", HorizontalLine = false)]
        private Rigidbody _rigidbody;

        [SerializeField, BoxGroup("Parameters/Readonly"), ReadOnly]
        private float _currentSpeed;

        [SerializeField, BoxGroup("Parameters/Readonly"), ReadOnly]
        private float _distanceZoomSpeedMove;

        private float _direcionMoveX;
        private float _directionMoveY;
        private float _directionMoveZ;


        public void InitAwake() => DontDestroyOnLoad(gameObject);

        private void Update() => PlayerTransformClamp();

        private void FixedUpdate() => MovementPlayer();

        private void PlayerTransformClamp()
        {
            float clampPositionX = Mathf.Clamp(transform.position.x,
                                               -_configPlayerControlMove.maxHorizontalDistanceCamera,
                                               _configPlayerControlMove.maxHorizontalDistanceCamera);

            float clampPositionY = Mathf.Clamp(transform.position.y,
                                               -_configPlayerControlMove.maxVerticalDistanceCamera,
                                               _configPlayerControlMove.maxVerticalDistanceCamera);

            float clampPositionZ = Mathf.Clamp(transform.position.z,
                                               _configPlayerControlMove.minZoomCameraDistance,
                                               _configPlayerControlMove.maxZoomCameraDistance);

            transform.position = new Vector3(clampPositionX, clampPositionY, clampPositionZ);
        }

        private void MovementPlayer()
        {
            GetDirections();

            Vector3 directionMoveCamera = new Vector3(_direcionMoveX, _directionMoveY, _directionMoveZ);
            _rigidbody.AddForce(directionMoveCamera, ForceMode.Impulse);
        }

        private void GetDirections()
        {
            if (Input.GetKey(_inputControl.keycodeRightMouseButton))
            {
                _distanceZoomSpeedMove = transform.position.z / _configPlayerControlMove.speedMoveMouse;

                _direcionMoveX = _distanceZoomSpeedMove * _inputControl.axisMouseX;
                _directionMoveY = _distanceZoomSpeedMove * _inputControl.axisMouseY;
            }
            else
            {
                if (Input.GetKey(_inputControl.keycodeLeftCtrl)) //? переименовать в действие, а не клавишу
                    _currentSpeed = _configPlayerControlMove.speedMoveFast;
                else
                    _currentSpeed = _configPlayerControlMove.speedMove;

                _distanceZoomSpeedMove = transform.position.z / _currentSpeed;

                _direcionMoveX = _distanceZoomSpeedMove * _inputControl.axisHorizontalMove;
                _directionMoveY = _distanceZoomSpeedMove * _inputControl.axisVerticalMove;
            }

            _directionMoveZ = _configPlayerControlMove.speedZoom * _inputControl.axisMouseScrollWheel;
        }
    }
}