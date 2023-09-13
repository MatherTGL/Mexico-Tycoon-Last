using UnityEngine;


namespace Config.Time
{
    [CreateAssetMenu(fileName = "TimeControlDefault", menuName = "Config/Time/Create New", order = 50)]
    public sealed class ConfigTimeControlEditor : ScriptableObject
    {
        public enum AccelerationTime : byte
        {
            X1 = 1, X2 = 2, X4 = 4
        }
    }
}
