using UnityEngine;
using Sirenix.OdinInspector;
using Config.Player;
using TimeControl;
using Boot;
using static Boot.Bootstrap;

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

        [SerializeField, Required, BoxGroup("Parameters/Components"), EnableIf("_isEditParametersEditor")]
        [HideLabel, Title("Transform", HorizontalLine = false)]
        private Transform _transform;

        private Vector3 _directionMoveCamera;

        private float _currentSpeed;

        private float _distanceZoomSpeedMove;

        private float _direcionMoveX, _directionMoveY, _directionMoveZ;


        private PlayerControlMovement() { }

        void IBoot.InitAwake() => DontDestroyOnLoad(gameObject);

        private void LateUpdate() => PlayerTransformClamp();

        private void FixedUpdate() => MovementPlayer();

        private void PlayerTransformClamp()
        {
            float clampPositionX = Mathf.Clamp(_transform.position.x,
                                               -_configPlayerControlMove.maxHorizontalDistanceCamera,
                                               _configPlayerControlMove.maxHorizontalDistanceCamera);

            float clampPositionY = Mathf.Clamp(_transform.position.y,
                                               -_configPlayerControlMove.maxVerticalDistanceCamera,
                                               _configPlayerControlMove.maxVerticalDistanceCamera);

            float clampPositionZ = Mathf.Clamp(_transform.position.z,
                                               _configPlayerControlMove.minZoomCameraDistance,
                                               _configPlayerControlMove.maxZoomCameraDistance);

            _transform.position = new Vector3(clampPositionX, clampPositionY, clampPositionZ);
        }

        private void MovementPlayer()
        {
            GetDirections();

            _directionMoveCamera = new Vector3(_direcionMoveX, _directionMoveY, _directionMoveZ);
            _rigidbody.AddForce(_directionMoveCamera, ForceMode.Impulse);
        }

        private void GetDirections()
        {
            if (Input.GetKey(_inputControl.keycodeRightMouseButton))
            {
                _distanceZoomSpeedMove = _transform.position.z / _configPlayerControlMove.speedMoveMouse;

                _direcionMoveX = _distanceZoomSpeedMove * _inputControl.axisMouseX;
                _directionMoveY = _distanceZoomSpeedMove * _inputControl.axisMouseY;
            }
            else
            {
                if (Input.GetKey(_inputControl.keycodeFastMove))
                    _currentSpeed = _configPlayerControlMove.speedMoveFast;
                else
                    _currentSpeed = _configPlayerControlMove.speedMove;

                _distanceZoomSpeedMove = _transform.position.z / _currentSpeed;

                _direcionMoveX = _distanceZoomSpeedMove * _inputControl.axisHorizontalMove;
                _directionMoveY = _distanceZoomSpeedMove * _inputControl.axisVerticalMove;
            }
            var distanceZoomSpeedZoom = _transform.position.z / _configPlayerControlMove.speedZoom;
            _directionMoveZ = distanceZoomSpeedZoom * _inputControl.axisMouseScrollWheel;
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);
        }
    }
}