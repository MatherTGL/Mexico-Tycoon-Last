using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Country.Inflation
{
    [CreateAssetMenu(fileName = "DefaultInflationConfig", menuName = "Config/Country/Inflation/Create New", order = 50)]
    public sealed class ConfigInflationEditor : ScriptableObject
    {
        [SerializeField, MinValue(-4.0f), MaxValue(8.0f)]
        private float _startedInflation = 0.01f;
        public float startedInflation => _startedInflation;

        [SerializeField, MinValue(0.01f)]
        private float _percentageInflationMin = 0.01f;
        public float percentageInflationMin => _percentageInflationMin;

        [SerializeField, MinValue(1.0f), MaxValue(8.0f)]
        private float _percentageInflationMax = 1.0f;
        public float percentageInflationMax => _percentageInflationMax;

        [SerializeField, MaxValue(-0.01f)]
        private float _percentageDeflationMin = -0.01f;
        public float percentageDeflationMin => _percentageDeflationMin;

        [SerializeField, MinValue(-4.0f), MaxValue(-1.0f)]
        private float _percentageDeflationMax = -1.0f;
        public float percentageDeflationMax => _percentageDeflationMax;
    }
}
