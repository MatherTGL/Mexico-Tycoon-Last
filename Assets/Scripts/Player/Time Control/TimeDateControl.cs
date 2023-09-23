using UnityEngine;
using Sirenix.OdinInspector;
using Config.Time;
using Boot;
using TimeControl.Acceleration;


namespace TimeControl
{
    internal sealed class TimeDateControl : MonoBehaviour, IBoot
    {
        private static TimeAcceleration _timeAcceleration;

        [SerializeField, BoxGroup("Parameters"), Required]
        private ConfigTimeControlEditor _configTimeControl;

        [SerializeField, BoxGroup("Parameters"), Required]
        private InputControl _inputControl;

        private float _currentTimeOneDay = 5;

        private bool _isPaused = false;


        private TimeDateControl() { }

        void IBoot.InitAwake()
        {
            _timeAcceleration = new(_configTimeControl, _inputControl);
        }

        (Bootstrap.TypeLoadObject typeLoad, bool isSingle) IBoot.GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.SuperImportant, isSingle: true);
        }

        private void Update()
        {
            _timeAcceleration?.AccelerationCheck(ref _currentTimeOneDay, ref _isPaused);
        }

        public float GetCurrentTimeOneDay(bool isUseCoroutine = false)
        {
            float accelerationDefault = (float)ConfigTimeControlEditor.AccelerationTime.X1;
            if (isUseCoroutine)
                return accelerationDefault / _currentTimeOneDay;
            else
                return _currentTimeOneDay;
        }

        public bool GetStatePaused() => _isPaused;
    }
}
