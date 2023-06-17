using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Config.Time;


namespace TimeControl
{
    public sealed class TimeDateControl : MonoBehaviour
    {
        [SerializeField, BoxGroup("Parameters"), Required]
        private ConfigTimeControlEditor _configTimeControl;


        public byte GetCurrentTimeOneDay()
        {
            return _configTimeControl.currentTimeOneDay;
        }
    }
}
