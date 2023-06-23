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
            Debug.Log("Инициализация Time Acceleration успешна");
        }

        public void AccelerationCheck(ref float currentAcceleration, ref bool pauseState)
        {
            if (Input.GetKeyDown(_inputControl.keycodeSpace))
            {
                currentAcceleration = _configTimeControlEditor.defaultTimeOneDay;
                pauseState = true;
                Debug.Log("Set acceleration pause");
            }

            if (Input.GetKeyDown(_inputControl.keycodeNumberOne))
            {
                currentAcceleration = _configTimeControlEditor.defaultTimeOneDay;
                pauseState = false;
                Debug.Log("Set acceleration default");
            }

            if (Input.GetKeyDown(_inputControl.keycodeNumberTwo))
            {
                currentAcceleration = _configTimeControlEditor.timeOneDayX2;
                pauseState = false;
                Debug.Log("Set acceleration x2");
            }

            if (Input.GetKeyDown(_inputControl.keycodeNumberThree))
            {
                currentAcceleration = _configTimeControlEditor.timeOneDayX4;
                pauseState = false;
                Debug.Log("Set acceleration x4");
            }


            Debug.Log("Time Acceleration Run");
            Debug.Log(currentAcceleration);
        }
    }
}
