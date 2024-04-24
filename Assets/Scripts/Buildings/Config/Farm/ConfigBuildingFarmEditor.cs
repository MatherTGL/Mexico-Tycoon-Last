using UnityEngine;
using Sirenix.OdinInspector;
using Resources;
using System.Collections.Generic;
using System;
using System.Linq;
using Config.Country.Climate;
using Config.Building.Events;
using static Config.Employees.ConfigEmployeeEditor;
using SerializableDictionary.Scripts;
using static Resources.TypeProductionResources;
using static Config.Country.Climate.ConfigClimateZoneEditor;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFarmConfig", menuName = "Config/Buildings/Farm/Create New", order = 50)]
    public sealed class ConfigBuildingFarmEditor : ScriptableObject
    {
        private const int _defaultCapacityProductionValue = 10;

        [SerializeField, Required, BoxGroup("Parameters/Configs")]
        private ConfigBuildingsEventsEditor _configBuildingsEvents;
        public ConfigBuildingsEventsEditor configBuildingsEvents => _configBuildingsEvents;

        [SerializeField, BoxGroup("Parameters/Production"), HideLabel]
        private SerializableDictionary<TypeResource, SerializableDictionary<TypeResource, int>> d_requiredRawMaterials = new();
        public Dictionary<TypeResource, SerializableDictionary<TypeResource, int>> requiredRawMaterials => d_requiredRawMaterials.Dictionary;

        [SerializeField, BoxGroup("Parameters/Production"), EnumToggleButtons]
        private SerializableDictionary<TypeResource, ushort> d_typeProductionResource = new();
        public Dictionary<TypeResource, ushort> productionResources => d_typeProductionResource.Dictionary;

        public enum TypeFarm { Terrestrial, Underground }

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

        [SerializeField, BoxGroup("Parameters/Production"), MinValue(1)]
        private float _harvestRipeningTime;
        public float harvestRipeningTime => _harvestRipeningTime;

        [SerializeField, BoxGroup("Parameters/Other"), MinValue(0)]
        private double _costPurchase = 50_000;
        public double costPurchase => _costPurchase;
    }
}
