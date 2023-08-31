using UnityEngine;
using Sirenix.OdinInspector;
using Resources;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFarmConfig", menuName = "Config/Buildings/Farm/Create New", order = 50)]
    public sealed class ConfigBuildingFarmEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons]
        private TypeProductionResources.TypeResource _typeProductionResource;
        public TypeProductionResources.TypeResource typeProductionResource => _typeProductionResource;

        [SerializeField, BoxGroup("Parameters"), MinValue(1)]
        private ushort _productionPerformanceStart = 1;
        public ushort productionStartPerformance => _productionPerformanceStart;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint _localCapacityProduction;
        public uint localCapacityProduction => _localCapacityProduction;
    }
}
