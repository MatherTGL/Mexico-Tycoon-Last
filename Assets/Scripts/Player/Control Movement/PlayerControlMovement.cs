using UnityEngine;
using Sirenix.OdinInspector;
using Config.Player;


namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputControl))]
    public sealed class PlayerControlMovement : MonoBehaviour
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private ConfigPlayerControlMoveEditor _configPlayerControlMove;

        [SerializeField, Required, BoxGroup("Parameters")]
        private InputControl _inputControl;

        [SerializeField, Required, BoxGroup("Parameters")]
        private Rigidbody _rigidbody;


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
            float direcionMoveX = _configPlayerControlMove.speedMove * _inputControl.axisHorizontalMove;
            float directionMoveY = _configPlayerControlMove.speedMove * _inputControl.axisVerticalMove;
            float directionMoveZ = _configPlayerControlMove.speedZoom * _inputControl.axisMouseScrollWheel;

            Vector3 directionMoveCamera = new Vector3(direcionMoveX, directionMoveY, directionMoveZ);
            _rigidbody.AddForce(directionMoveCamera, ForceMode.Impulse);
        }
    }
}