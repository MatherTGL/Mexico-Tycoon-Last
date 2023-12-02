using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Resources;
using System.Linq;
using Config.Building.Events;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingStockConfig", menuName = "Config/Buildings/Stock/Create New", order = 50)]
    public sealed class ConfigBuildingStockEditor : ScriptableObject
    {
        [SerializeField, Required, BoxGroup("Configs")]
        private ConfigBuildingsEventsEditor _configBuildingsEvents;
        public ConfigBuildingsEventsEditor configBuildingsEvents => _configBuildingsEvents;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint[] _localCapacityProduction;
        public uint[] localCapacityProduction => _localCapacityProduction;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        private double _costPurchase = 50_000;
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
