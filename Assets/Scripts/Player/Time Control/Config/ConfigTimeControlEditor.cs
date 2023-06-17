using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Time
{
    [CreateAssetMenu(fileName = "TimeControlDefault", menuName = "Config/Time/Create New", order = 50)]
    public sealed class ConfigTimeControlEditor : ScriptableObject
    {
        [BoxGroup("Parameters")]
        [SerializeField, FoldoutGroup("Parameters/TimeOneDay"), Title("Current Time One Day", horizontalLine:false), HideLabel]
        [Tooltip("Сколько будет проходить в секундах для смены дня на следующий"), MinValue(1)]
        private byte _currentTimeOneDay = 1;
        public byte currentTimeOneDay => _currentTimeOneDay;

        [SerializeField, FoldoutGroup("Parameters/TimeOneDay"), Title("Default Time One Day", horizontalLine:false), HideLabel]
        [Tooltip("Параметр стандартного значения для currentTimeOneDay")]
        private byte _defaultTimeOneDay = 1;
        public byte defaultTimeOneDay => _defaultTimeOneDay;
    }
}
