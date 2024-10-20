using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Config.Building.Events;
using static Config.Employees.ConfigEmployeeEditor;
using SerializableDictionary.Scripts;
using static Resources.TypeProductionResources;
using static Config.Country.Climate.ConfigClimateZoneEditor;
using Building.Additional.Crop;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFarmConfig", menuName = "Config/Buildings/Farm/Create New", order = 50)]
    public sealed class ConfigBuildingFarmEditor : ScriptableObject
    {
        private const int _defaultCapacityProductionValue = 10;

        [SerializeField, Required, BoxGroup("Parameters/Configs")]
        private ConfigBuildingsEventsEditor _configBuildingsEvents;
        public ConfigBuildingsEventsEditor configBuildingsEvents => _configBuildingsEvents;

        [SerializeField, Required, BoxGroup("Configs")]
        private ConfigCropSpoilage _configCropSpoilage;
        public ConfigCropSpoilage configCropSpoilage => _configCropSpoilage;

        [SerializeField, BoxGroup("Parameters/Production"), HideLabel]
        private SerializableDictionary<TypeResource, SerializableDictionary<TypeResource, int>> d_requiredRawMaterials = new();
        public Dictionary<TypeResource, SerializableDictionary<TypeResource, int>> requiredRawMaterials => d_requiredRawMaterials.Dictionary;

        [SerializeField, BoxGroup("Parameters/Production"), EnumToggleButtons]
        private SerializableDictionary<TypeResource, ushort> d_typeProductionResource = new();
        public Dictionary<TypeResource, ushort> productionResources => d_typeProductionResource.Dictionary;

        public enum TypeFarm : byte { Terrestrial, Underground }

        [SerializeField, BoxGroup("Parameters"), EnumPaging]
        public TypeFarm typeFarm;

        [SerializeField, BoxGroup("Parameters/Seasons"), HideLabel]
        private List<TypeSeasons> l_growingSeasons = new();
        public List<TypeSeasons> growingSeasons => l_growingSeasons;

        [SerializeField, BoxGroup("Parameters/Storage")]
        private SerializableDictionary<TypeResource, uint> d_localCapacityProduction = new();
        public Dictionary<TypeResource, uint> localCapacityProduction => d_localCapacityProduction.Dictionary;

        [SerializeField, BoxGroup("Parameters/Employees")]
        [Tooltip("Required employees for the operation of the building, as well as their number")]
        private SerializableDictionary<TypeEmployee, byte> d_requiredEmployees = new();
        public Dictionary<TypeEmployee, byte> requiredEmployees => d_requiredEmployees.Dictionary;

        [SerializeField, BoxGroup("Parameters/Farm Type")]
        [Tooltip("Determines how long it takes to convert to a particular type")]
        private ushort _timeChangeFarmType = 10;
        public ushort timeChangeFarmType => _timeChangeFarmType;

        [SerializeField, BoxGroup("Parameters/Farm Type")]
        private double _typeChangeCost = 10_000;
        public double typeChangeCost => _typeChangeCost;

        [SerializeField, BoxGroup("Parameters/Production")]
        private SerializableDictionary<TypeResource, float> d_harvestRipeningTime = new();
        public Dictionary<TypeResource, float> harvestRipeningTime => d_harvestRipeningTime.Dictionary;

        [SerializeField, BoxGroup("Parameters/Other"), MinValue(0)]
        private double _costPurchase = 50_000;
        public double costPurchase => _costPurchase;
    }
}
