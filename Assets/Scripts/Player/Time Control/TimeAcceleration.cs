using Config.Time;
using UnityEngine;
using static Config.Time.ConfigTimeControlEditor;

namespace TimeControl.Acceleration
{
    public sealed class TimeAcceleration
    {
        private readonly InputControl _inputControl;


        public TimeAcceleration(in InputControl inputControl)
            => _inputControl = inputControl;

        private void ChangeState(ref bool pauseState, ref float currentAcceleration,
            in AccelerationTime typeAcceleration, in bool isUseInvertState = false)
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
                ChangeState(ref pauseState, ref currentAcceleration, AccelerationTime.X1, true);

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeDefault))
                ChangeState(ref pauseState, ref currentAcceleration, AccelerationTime.X1);

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeTwo))
                ChangeState(ref pauseState, ref currentAcceleration, AccelerationTime.X2);

            if (Input.GetKeyDown(_inputControl.keycodeAccelerationTimeThree))
                ChangeState(ref pauseState, ref currentAcceleration, AccelerationTime.X4);
        }
    }
}
