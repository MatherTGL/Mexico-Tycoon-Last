using UnityEngine;
using Sirenix.OdinInspector;
using Resources;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingBorderConfig", menuName = "Config/Buildings/Border/Create New", order = 50)]
    public sealed class ConfigBuildingBorderEditor : ScriptableObject
    {
        [SerializeField, Required]
        private CostResourcesConfig _costResourcesConfig;
        public CostResourcesConfig costResourcesConfig => _costResourcesConfig;
    }
}