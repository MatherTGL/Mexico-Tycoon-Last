using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Player
{
    [CreateAssetMenu(fileName = "PlayerControlDefaultConfig", menuName = "Config/Player/Control/Move/Create New", order = 50)]
    public sealed class ConfigPlayerControlMoveEditor : ScriptableObject
    {
        [BoxGroup("Parameters")]
        [SerializeField, FoldoutGroup("Parameters/Speed/Move"), Title("Keyboard", HorizontalLine = false), HideLabel]
        [HorizontalGroup("Parameters/Speed/Move/HorizontalMove"), Tooltip("Чем выше значение, тем ниже скорость игрока")]
        private float _speedMove;
        public float speedMove => _speedMove;

        [SerializeField, FoldoutGroup("Parameters/Speed/Move"), Title("Mouse", horizontalLine: false)]
        [HorizontalGroup("Parameters/Speed/Move/HorizontalMove"), HideLabel]
        private float _speedMoveMouse;
        public float speedMoveMouse => _speedMoveMouse;

        [SerializeField, FoldoutGroup("Parameters/Speed/Move"), Title("Fast", HorizontalLine = false)]
        [HorizontalGroup("Parameters/Speed/Move/HorizontalMove"), HideLabel, MinValue("@_speedMove")]
        private float _speedMoveFast;
        public float speedMoveFast => _speedMoveFast;

        [SerializeField, BoxGroup("Parameters/Speed"), Title("Zoom", HorizontalLine = false), HideLabel]
        private float _speedZoom;
        public float speedZoom => _speedZoom;

        [BoxGroup("Parameters/Clamp Distance"), MinValue(0.01f)]
        [SerializeField, FoldoutGroup("Parameters/Clamp Distance/Zoom"), Tooltip("Минимальное расстояние от 0 координат по Z")]
        [Title("Min", HorizontalLine = false), HideLabel, HorizontalGroup("Parameters/Clamp Distance/Zoom/HorizontalZoom")]
        private float _minZoomCameraDistance;
        public float minZoomCameraDistance => _minZoomCameraDistance;

        [SerializeField, FoldoutGroup("Parameters/Clamp Distance/Zoom"), Tooltip("Максимальное расстояние от 0 координат по Z")]
        [Title("Max", HorizontalLine = false), HideLabel, HorizontalGroup("Parameters/Clamp Distance/Zoom/HorizontalZoom"), MinValue("@_minZoomCameraDistance")]
        private float _maxZoomCameraDistance;
        public float maxZoomCameraDistance => _maxZoomCameraDistance;

        [SerializeField, FoldoutGroup("Parameters/Clamp Distance/Transform")]
        [Title("Max Horizontal", HorizontalLine = false), HideLabel]
        [HorizontalGroup("Parameters/Clamp Distance/Transform/HorizontalTransform")]
        private float _maxHorizontalDistanceCamera;
        public float maxHorizontalDistanceCamera => _maxHorizontalDistanceCamera;

        [SerializeField, FoldoutGroup("Parameters/Clamp Distance/Transform")]
        [Title("Max Vertical", HorizontalLine = false), HideLabel]
        [HorizontalGroup("Parameters/Clamp Distance/Transform/HorizontalTransform")]
        private float _maxVerticalDistanceCamera;
        public float maxVerticalDistanceCamera => _maxVerticalDistanceCamera;
    }
}