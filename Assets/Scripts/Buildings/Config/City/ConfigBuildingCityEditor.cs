using UnityEngine;
using Sirenix.OdinInspector;
using Resources;
using Config.Building.Deliveries;
using SerializableDictionary.Scripts;
using static Resources.TypeProductionResources;
using System.Collections.Generic;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingCityConfig", menuName = "Config/Buildings/City/Create New", order = 50)]
    public sealed class ConfigBuildingCityEditor : ScriptableObject
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private CostResourcesConfig _costResourcesConfig;
        public CostResourcesConfig costResourcesConfig => _costResourcesConfig;

        [SerializeField, Required, BoxGroup("Parameters")]
        private ConfigContractsEditor _configDeliveriesEditor;
        public ConfigContractsEditor configDeliveries => _configDeliveriesEditor;

        [SerializeField, BoxGroup("Parameters"), MinValue(-1.0f), MaxValue(0.0f)]
        private float _populationChangeStepPercentMin;
        public float populationChangeStepPercentMin => _populationChangeStepPercentMin;

        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f), MaxValue(1.0f)]
        private float _populationChangeStepPercentMax;
        public float populationChangeStepPercentMax => _populationChangeStepPercentMax;

        [SerializeField, BoxGroup("Parameters")]
        private uint _populationStartMin;
        public uint populationStartMin => _populationStartMin;

        [SerializeField, BoxGroup("Parameters"), MinValue("@_populationStartMin")]
        private uint _populationStartMax;
        public uint populationStartMax => _populationStartMax;

        [SerializeField, BoxGroup("Parameters/Storage")]
        private SerializableDictionary<TypeResource, uint> d_localCapacityProduction = new();
        public Dictionary<TypeResource, uint> localCapacityProduction => d_localCapacityProduction.Dictionary;
    }
}
