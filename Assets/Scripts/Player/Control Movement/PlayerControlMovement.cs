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
    public sealed class PlayerControlMovement : MonoBehaviour, IBoot
    {
#if UNITY_EDITOR
        [SerializeField, ToggleLeft, BoxGroup("Parameters"), Title("Edit Parameters")]
        private bool _isEditParametersEditor;
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


        public void InitAwake() => DontDestroyOnLoad(gameObject);

        private void Update() => PlayerTransformClamp();

        private void FixedUpdate() => ControlPlayer();

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

        private void ControlPlayer()
        {
            float direcionMoveX;
            float directionMoveY;
            float directionMoveZ;

            if (Input.GetMouseButton(1))
            {
                direcionMoveX = _configPlayerControlMove.speedMoveMouse * _inputControl.axisMouseX;
                directionMoveY = _configPlayerControlMove.speedMoveMouse * _inputControl.axisMouseY;
                //todo сделать отдельные переменные отвечающие за скорость при нажатии на пкм
            }
            else
            {
                direcionMoveX = _configPlayerControlMove.speedMove * _inputControl.axisHorizontalMove;
                directionMoveY = _configPlayerControlMove.speedMove * _inputControl.axisVerticalMove;
            }
            directionMoveZ = _configPlayerControlMove.speedZoom * _inputControl.axisMouseScrollWheel;

            Vector3 directionMoveCamera = new Vector3(direcionMoveX, directionMoveY, directionMoveZ);
            _rigidbody.AddForce(directionMoveCamera, ForceMode.Impulse);
        }
    }
}