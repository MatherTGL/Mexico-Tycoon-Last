using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Time
{
    [CreateAssetMenu(fileName = "TimeControlDefault", menuName = "Config/Time/Create New", order = 50)]
    public sealed class ConfigTimeControlEditor : ScriptableObject
    {
        [BoxGroup("Parameters")]
        [FoldoutGroup("Parameters/TimeOneDay"), Title("Default Time One Day", horizontalLine: false), HideLabel]
        [Tooltip("Параметр стандартного значения для currentTimeOneDay"), SerializeField]
        private byte _defaultTimeOneDay = 1;
        public byte defaultTimeOneDay => _defaultTimeOneDay;

        [FoldoutGroup("Parameters/TimeOneDay"), Title("Acceleration Time One Day x2", horizontalLine: false), HideLabel]
        [Tooltip("Параметр ускоренного значения для currentTimeOneDay в 2 раза"), SerializeField]
        private byte _timeOneDayX2 = 2;
        public byte timeOneDayX2 => _timeOneDayX2;

        [FoldoutGroup("Parameters/TimeOneDay"), Title("Acceleration Time One Day x4", horizontalLine: false), HideLabel]
        [Tooltip("Параметр ускоренного значения для currentTimeOneDay в 4 раза"), SerializeField]
        private byte _timeOneDayX4 = 4;
        public byte timeOneDayX4 => _timeOneDayX4;
    }
}
