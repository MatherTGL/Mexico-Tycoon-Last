using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Player
{
    [CreateAssetMenu(fileName = "PlayerControlDefaultConfig", menuName = "Config/Player/Control/Move/Create New", order = 50)]
    public sealed class ConfigPlayerControlMoveEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters")]
        private float _speedMove;
        public float speedMove => _speedMove;

        [SerializeField, BoxGroup("Parameters")]
        private float _speedMoveFast;
        public float speedMoveFast => _speedMoveFast;

        [SerializeField, BoxGroup("Parameters")]
        private float _speedZoom;
        public float speedZoom => _speedZoom;

        [SerializeField, BoxGroup("Parameters")]
        private float _minZoomCameraDistance;
        public float minZoomCameraDistance => _minZoomCameraDistance;

        [SerializeField, BoxGroup("Parameters")]
        private float _maxZoomCameraDistance;
        public float maxZoomCameraDistance => _maxZoomCameraDistance;

        [SerializeField, BoxGroup("Parameters")]
        private float _maxHorizontalDistanceCamera;
        public float maxHorizontalDistanceCamera => _maxHorizontalDistanceCamera;

        [SerializeField, BoxGroup("Parameters")]
        private float _maxVerticalDistanceCamera;
        public float maxVerticalDistanceCamera => _maxVerticalDistanceCamera; 
    }
}