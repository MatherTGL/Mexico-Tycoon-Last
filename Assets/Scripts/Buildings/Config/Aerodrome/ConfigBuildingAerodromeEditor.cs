using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Resources;
using System.Linq;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingAerodromeConfig", menuName = "Config/Buildings/Aero/Create New", order = 50)]
    public sealed class ConfigBuildingAerodromeEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters"), MinValue(200)]
        private double _maintenanceExpenses = 200;
        public double maintenanceExpenses => _maintenanceExpenses;

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
