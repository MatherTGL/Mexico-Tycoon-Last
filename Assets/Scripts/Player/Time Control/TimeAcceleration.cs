using Config.Time;
using UnityEngine;

namespace TimeControl.Acceleration
{
    public sealed class TimeAcceleration
    {
        private readonly ConfigTimeControlEditor _configTimeControlEditor;

        private readonly InputControl _inputControl;


        public TimeAcceleration(in ConfigTimeControlEditor configTimeControl,
                                in InputControl inputControl)
        {
            _configTimeControlEditor = configTimeControl;
            _inputControl = inputControl;
        }

        private void ChangeState(ref bool pauseState, ref float currentAcceleration,
            in ConfigTimeControlEditor.AccelerationTime typeAcceleration, in bool isUseInvertState = false)
        {
            if (isUseInvertState)
                pauseState = !pauseState;
            else
                pauseState = false;

            currentAcceleration = (float)typeAcceleration;
        }

        public void AccelerationCheck(ref float currentAcceleration, ref bool pauseState)
        {
            if (Input.GetKeyDown(_inputControl.keycodeTimePause))
                ChangeState(ref pauseState, ref currentAcceleration, ConfigTimeControlEditor.AccelerationTime.X1, true);

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeDefault))
                ChangeState(ref pauseState, ref currentAcceleration, ConfigTimeControlEditor.AccelerationTime.X1);

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeTwo))
                ChangeState(ref pauseState, ref currentAcceleration, ConfigTimeControlEditor.AccelerationTime.X2);

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeThree))
                ChangeState(ref pauseState, ref currentAcceleration, ConfigTimeControlEditor.AccelerationTime.X4);
        }
    }
}
