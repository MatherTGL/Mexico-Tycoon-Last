using Sirenix.OdinInspector;
using UnityEngine;

namespace Events.Buildings.PlantsDiseases.Epidemic
{
    [CreateAssetMenu(fileName = "ConfigPlantDiseasesEpidemicEvent", menuName = "Config/Buildings/Events/Specific/PlantDiseases/Epidemic/Create New", order = 50)]
    public sealed class ConfigEventPlantDiseasesEpidemic : ScriptableObject
    {
        [SerializeField, MinValue(0.0f), MaxValue(1.0f)]
        private float _spawnChance;

        public float spawnChance => _spawnChance;
    }
}
