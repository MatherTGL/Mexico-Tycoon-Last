using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Resources;
using System.Linq;
using SerializableDictionary.Scripts;
using static Config.Employees.ConfigEmployeeEditor;
using Config.Building.Events;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingSeaPortConfig", menuName = "Config/Buildings/SeaPort/Create New", order = 50)]
    public sealed class ConfigBuildingSeaPortEditor : ScriptableObject
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private ConfigBuildingsEventsEditor _configEvents;
        public ConfigBuildingsEventsEditor configEvents => _configEvents;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint[] _localCapacityProduction;
        public uint[] localCapacityProduction => _localCapacityProduction;

        [SerializeField, BoxGroup("Parameters/Employees")]
        [Tooltip("Required employees for the operation of the building, as well as their number")]
        private SerializableDictionary<TypeEmployee, byte> d_requiredEmployees = new();
        public SerializableDictionary<TypeEmployee, byte> requiredEmployees => d_requiredEmployees;

        [SerializeField, MinValue(10), BoxGroup("Parameters")]
        private double _costPurchase = 100_000;
        public double costPurchase => _costPurchase;

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
