using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingAerodromeConfig", menuName = "Config/Buildings/Aero/Create New", order = 50)]
    public sealed class ConfigBuildingAerodromeEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters"), MinValue(200)]
        private double _maintenanceExpenses = 200;
        public double maintenanceExpenses => _maintenanceExpenses;
    }
}
