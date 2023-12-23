using Events.Buildings.PlantsDiseases.Epidemic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Events.Buildings
{
    [CreateAssetMenu(fileName = "ConfigPlantDiseasesEvent", menuName = "Config/Buildings/Events/Specific/PlantDiseases/Create New", order = 50)]
    public sealed class ConfigEventPlantDiseases : ScriptableObject
    {
        [SerializeField, Required]
        private ConfigEventPlantDiseasesEpidemic _configPlantDiseasesEpidemic;
        public ConfigEventPlantDiseasesEpidemic configEventPlantDiseasesEpidemic => _configPlantDiseasesEpidemic;

        [SerializeField, ToggleLeft]
        private bool _isActiveEpidemic;
        public bool isActiveEpidemic => _isActiveEpidemic;
    }
}
