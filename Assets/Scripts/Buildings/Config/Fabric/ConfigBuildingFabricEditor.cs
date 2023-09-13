using UnityEngine;
using Sirenix.OdinInspector;
using Resources;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFabricConfig", menuName = "Config/Buildings/Fabric/Create New", order = 50)]
    public sealed class ConfigBuildingFabricEditor : ScriptableObject
    {
        [SerializeField]
        private TypeProductionResources.TypeResource _typeProductionResource;
        public TypeProductionResources.TypeResource typeProductionResource => _typeProductionResource;

        [SerializeField]
        private ushort _productionPerformance = 1;
        public ushort productionPerformance => _productionPerformance;

        [SerializeField]
        private ushort _productConversionStep = 5;
        public ushort productConversionStep => _productConversionStep;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private double _maintenanceExpenses = 10;
        public double maintenanceExpenses => _maintenanceExpenses;
    }
}
