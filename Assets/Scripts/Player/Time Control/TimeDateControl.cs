using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Config.Time;
using Boot;
using TimeControl.Acceleration;


namespace TimeControl
{
    public sealed class TimeDateControl : MonoBehaviour, IBoot
    {
        [SerializeField, BoxGroup("Parameters"), Required]
        private ConfigTimeControlEditor _configTimeControl;

        [SerializeField, BoxGroup("Parameters"), Required]
        private InputControl _inputControl;

        private static TimeAcceleration _timeAcceleration;

        private float _currentTimeOneDay = 1;

        private bool _isPaused = false;


        public void InitAwake()
        {
            _timeAcceleration = new TimeAcceleration(_configTimeControl, _inputControl);
            _isPaused = false;
            Debug.Log("Инициализация TimeDateControl успешна");
        }

        private void Update() => _timeAcceleration.AccelerationCheck(ref _currentTimeOneDay, ref _isPaused);

        public float GetCurrentTimeOneDay(bool isUseCoroutine = false)
        {
            if (isUseCoroutine)
                return _configTimeControl.defaultTimeOneDay / _currentTimeOneDay;
            else { return _currentTimeOneDay; }
        }

        public bool GetStatePaused() => _isPaused;
    }
}
