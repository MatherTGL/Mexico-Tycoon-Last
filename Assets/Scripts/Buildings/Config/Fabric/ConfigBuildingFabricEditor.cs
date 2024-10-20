using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Config.Building.Events;
using SerializableDictionary.Scripts;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;
using Building.Additional.Crop;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFabricConfig", menuName = "Config/Buildings/Fabric/Create New", order = 50)]
    public sealed class ConfigBuildingFabricEditor : ScriptableObject
    {
        [SerializeField, Required, BoxGroup("Configs")]
        private ConfigBuildingsEventsEditor _configBuildingsEvents;
        public ConfigBuildingsEventsEditor configBuildingsEvents => _configBuildingsEvents;

        [SerializeField, Required, BoxGroup("Configs")]
        private ConfigCropSpoilage _configCropSpoilage;
        public ConfigCropSpoilage configCropSpoilage => _configCropSpoilage;

        [SerializeField, BoxGroup("Raw Materials"), HideLabel]
        private SerializableDictionary<TypeResource, SerializableDictionary<TypeResource, int>> d_requiredRawMaterials = new();
        public Dictionary<TypeResource, SerializableDictionary<TypeResource, int>> requiredRawMaterials => d_requiredRawMaterials.Dictionary;

        [SerializeField, BoxGroup("Parameters/Production"), EnumToggleButtons]
        private SerializableDictionary<TypeResource, ushort> d_typeProductionResource;
        public Dictionary<TypeResource, ushort> productionResources => d_typeProductionResource.Dictionary;

        [SerializeField, BoxGroup("Parameters/Storage")]
        private SerializableDictionary<TypeResource, uint> d_localCapacityProduction = new();
        public Dictionary<TypeResource, uint> localCapacityProduction => d_localCapacityProduction.Dictionary;

        [SerializeField, BoxGroup("Parameters/Employees")]
        [Tooltip("Required employees for the operation of the building, as well as their number")]
        private SerializableDictionary<TypeEmployee, byte> d_requiredEmployees = new();
        public Dictionary<TypeEmployee, byte> requiredEmployees => d_requiredEmployees.Dictionary;

        [SerializeField, BoxGroup("Parameters/Production")]
        private SerializableDictionary<TypeResource, float> d_harvestRipeningTime = new();
        public Dictionary<TypeResource, float> harvestRipeningTime => d_harvestRipeningTime.Dictionary;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        private double _costPurchase = 50_000;
        public double costPurchase => _costPurchase;
    }
}
