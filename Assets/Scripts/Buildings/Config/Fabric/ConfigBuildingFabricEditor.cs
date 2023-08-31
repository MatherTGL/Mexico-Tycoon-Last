using UnityEngine;
using Sirenix.OdinInspector;
using Resources;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFabricConfig", menuName = "Config/Buildings/Fabric/Create New", order = 50)]
    public sealed class ConfigBuildingFabricEditor : ScriptableObject
    {
        private TypeProductionResources.TypeResource _typeProductionResource;
        public TypeProductionResources.TypeResource typeProductionResource => _typeProductionResource;
    }
}
