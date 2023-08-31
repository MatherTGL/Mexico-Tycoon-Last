using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingCityConfig", menuName = "Config/Buildings/City/Create New", order = 50)]
    public sealed class ConfigBuildingCityEditor : ScriptableObject
    {
        [SerializeField, MinValue(-1.0f), MaxValue(0.0f)]
        private float _populationChangeStepPercentMin;
        public float populationChangeStepPercentMin => _populationChangeStepPercentMin;

        [SerializeField, MinValue(0.0f), MaxValue(1.0f)]
        private float _populationChangeStepPercentMax;
        public float populationChangeStepPercentMax => _populationChangeStepPercentMax;

        [SerializeField]
        private uint _populationStartMin;
        public uint populationStartMin => _populationStartMin;

        [SerializeField, MinValue("@_populationStartMin")]
        private uint _populationStartMax;
        public uint populationStartMax => _populationStartMax;
    }
}
