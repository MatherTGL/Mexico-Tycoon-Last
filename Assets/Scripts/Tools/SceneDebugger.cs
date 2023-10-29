using UnityEngine;
using Sirenix.OdinInspector;

namespace Tools.Debug
{
    public sealed class SceneDebugger : MonoBehaviour
    {
        private enum TypeObjectDraw : byte { Wire, NonWire }

        [SerializeField, EnumToggleButtons, BoxGroup("Parameters"), HideLabel]
        private TypeObjectDraw _typeObjectDraw;

        [SerializeField, InlineButton("SetDefaultCollider", SdfIconType.Box)]
        [BoxGroup("Parameters/Collider"), HideLabel]
        private Collider _collider;

        [SerializeField, EnableIf("@_isUseCustomPosition")]
        [BoxGroup("Parameters/Position")]
        private Vector3 _customPosition;

        [SerializeField, ReadOnly, BoxGroup("Parameters/Position")]
        private Vector3 _currentDrawPosition;

        [SerializeField, ColorPalette, BoxGroup("Parameters/Color"), HideLabel]
        private Color _drawColor;

        [SerializeField, BoxGroup("Parameters/States"), ToggleLeft]
        private bool _isUseCustomPosition;

        [SerializeField, BoxGroup("Parameters/States"), ToggleLeft]
        private bool _drawOnlyIfSelected;


        private void OnValidate()
        {
            if (_isUseCustomPosition == false)
                _currentDrawPosition = transform.position;
            else
                _currentDrawPosition = _customPosition;
        }

        private void OnDrawGizmos()
        {
            if (_drawOnlyIfSelected == false)
                Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (_drawOnlyIfSelected)
                Draw();
        }

        private void Draw()
        {
            Gizmos.color = _drawColor;

            if (_typeObjectDraw is TypeObjectDraw.Wire)
                Gizmos.DrawWireCube(_currentDrawPosition, _collider.bounds.size);
            else
                Gizmos.DrawCube(_currentDrawPosition, _collider.bounds.size);
        }

        private void SetDefaultCollider()
        {
            _collider = GetComponent<Collider>();
        }
    }
}
