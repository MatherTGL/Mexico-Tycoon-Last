using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingStockConfig", menuName = "Config/Buildings/Stock/Create New", order = 50)]
    public sealed class ConfigBuildingStockEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private double _maintenanceExpenses = 10;
        public double maintenanceExpenses => _maintenanceExpenses;
    }
}
