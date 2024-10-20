using UnityEngine;
using Sirenix.OdinInspector;
using Config.Player;
using TimeControl;
using Boot;
using static Boot.Bootstrap;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(InputControl), typeof(TimeDateControl))]
    internal sealed class PlayerControlMovement : MonoBehaviour, IBoot
    {
        [SerializeField, Required, BoxGroup("Parameters/Configs")]
        [HideLabel, Title("Player Control Move", HorizontalLine = false)]
        private ConfigPlayerControlMoveEditor _configPlayerControlMove;

        private InputControl _inputControl;

        private Rigidbody _rigidbody;

        private Transform _transform;

        private Vector3 _directionMoveCamera;

        private float _currentSpeed;

        private float _distanceZoomSpeedMove;

        private float _direcionMoveX, _directionMoveY, _directionMoveZ;


        private PlayerControlMovement() { }

        void IBoot.InitAwake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _inputControl = GetComponent<InputControl>();
        }

        void IBoot.InitStart() => DontDestroyOnLoad(gameObject);

        private void Update()
        {
            CalculateDirection();
            PlayerTransformClamp();
            MovementPlayer();
        }

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
            _directionMoveCamera = new Vector3(_direcionMoveX, _directionMoveY, _directionMoveZ) * Time.fixedDeltaTime;
            _rigidbody.AddForce(_directionMoveCamera, ForceMode.Force);
        }

        private void CalculateDirection()
        {
            if (Input.GetKey(_inputControl.keycodeRightMouseButton))
            {
                _distanceZoomSpeedMove = _transform.position.z * _configPlayerControlMove.speedMoveMouse;

                _direcionMoveX = _distanceZoomSpeedMove * _inputControl.axisMouseX;
                _directionMoveY = _distanceZoomSpeedMove * _inputControl.axisMouseY;
            }
            else
            {
                if (Input.GetKey(_inputControl.keycodeFastMove))
                    _currentSpeed = _configPlayerControlMove.speedMoveFast;
                else
                    _currentSpeed = _configPlayerControlMove.speedMove;

                _distanceZoomSpeedMove = _transform.position.z * _currentSpeed;

                _direcionMoveX = _distanceZoomSpeedMove * _inputControl.axisHorizontalMove;
                _directionMoveY = _distanceZoomSpeedMove * _inputControl.axisVerticalMove;
            }
            var distanceZoomSpeedZoom = _transform.position.z * _configPlayerControlMove.speedZoom;
            _directionMoveZ = distanceZoomSpeedZoom * _inputControl.axisMouseScrollWheel;
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);
    }
}