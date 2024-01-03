using UnityEngine;
using Sirenix.OdinInspector;
using Resources;
using System;
using System.Linq;
using Config.Building.Deliveries;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingCityConfig", menuName = "Config/Buildings/City/Create New", order = 50)]
    public sealed class ConfigBuildingCityEditor : ScriptableObject
    {
        [SerializeField, Required]
        private CostResourcesConfig _costResourcesConfig;
        public CostResourcesConfig costResourcesConfig => _costResourcesConfig;

        [SerializeField, Required]
        private ConfigDeliveriesEditor _configDeliveriesEditor;
        public ConfigDeliveriesEditor configDeliveries => _configDeliveriesEditor;

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

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint[] _localCapacityProduction;
        public uint[] localCapacityProduction => _localCapacityProduction;


#if UNITY_EDITOR
        [Button("Update Info Capacity")]
        private void UpdateInfoCapacity()
        {
            int count = Enum.GetNames(typeof(TypeProductionResources.TypeResource)).ToArray().Length;
            if (_localCapacityProduction.Length < count)
                _localCapacityProduction = new uint[count];
        }
#endif
    }
}
