using Config.Time;
using UnityEngine;


namespace TimeControl.Acceleration
{
    public sealed class TimeAcceleration
    {
        private ConfigTimeControlEditor _configTimeControlEditor;
        private InputControl _inputControl;


        public TimeAcceleration(in ConfigTimeControlEditor configTimeControl, in InputControl inputControl)
        {
            _configTimeControlEditor = configTimeControl;
            _inputControl = inputControl;
        }

        public void AccelerationCheck(ref float currentAcceleration, ref bool pauseState)
        {
            if (Input.GetKeyDown(_inputControl.keycodeTimePause))
            {
                currentAcceleration = _configTimeControlEditor.defaultTimeOneDay;
                pauseState = !pauseState;
            }

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeDefault))
            {
                currentAcceleration = _configTimeControlEditor.defaultTimeOneDay;
                pauseState = false;
            }

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeTwo))
            {
                currentAcceleration = _configTimeControlEditor.timeOneDayX2;
                pauseState = false;
            }

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeThree))
            {
                currentAcceleration = _configTimeControlEditor.timeOneDayX4;
                pauseState = false;
            }
        }
    }
}
