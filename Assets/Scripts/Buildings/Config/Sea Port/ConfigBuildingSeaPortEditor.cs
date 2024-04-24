using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Resources;
using System.Linq;
using SerializableDictionary.Scripts;
using static Config.Employees.ConfigEmployeeEditor;
using Config.Building.Events;
using static Resources.TypeProductionResources;
using System.Collections.Generic;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingSeaPortConfig", menuName = "Config/Buildings/SeaPort/Create New", order = 50)]
    public sealed class ConfigBuildingSeaPortEditor : ScriptableObject
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private ConfigBuildingsEventsEditor _configEvents;
        public ConfigBuildingsEventsEditor configEvents => _configEvents;

        [SerializeField, BoxGroup("Parameters/Storage")]
        private SerializableDictionary<TypeResource, uint> d_localCapacityProduction = new();
        public Dictionary<TypeResource, uint> localCapacityProduction => d_localCapacityProduction.Dictionary;

        [SerializeField, BoxGroup("Parameters/Employees")]
        [Tooltip("Required employees for the operation of the building, as well as their number")]
        private SerializableDictionary<TypeEmployee, byte> d_requiredEmployees = new();
        public SerializableDictionary<TypeEmployee, byte> requiredEmployees => d_requiredEmployees;

        [SerializeField, MinValue(10), BoxGroup("Parameters")]
        private double _costPurchase = 100_000;
        public double costPurchase => _costPurchase;
    }
}
