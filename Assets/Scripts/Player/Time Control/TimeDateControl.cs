using UnityEngine;
using Boot;
using TimeControl.Acceleration;
using static Boot.Bootstrap;

namespace TimeControl
{
    internal sealed class TimeDateControl : MonoBehaviour, IBoot
    {
        private static TimeAcceleration _timeAcceleration;

        private InputControl _inputControl;

        private float _currentTimeOneDay = 5;

        private float _accelerationDefault;

        private bool _isPaused = false;


        private TimeDateControl() { }

        void IBoot.InitAwake()
        {
            _inputControl = GetComponent<InputControl>();
            _timeAcceleration = new(_inputControl);
        }

        void IBoot.InitStart()
            => _accelerationDefault = (float)AccelerationTime.X1 / _currentTimeOneDay;

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (typeLoad: TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);

        private void Update()
            => _timeAcceleration?.AccelerationCheck(ref _currentTimeOneDay, ref _isPaused);

        public float GetCurrentTimeOneDay(bool isUseCoroutine = false)
        {
            if (isUseCoroutine)
                return _accelerationDefault;
            else
                return _currentTimeOneDay;
        }

        public bool IsStatePaused() => _isPaused;
    }
}
