using Events.Buildings.PlantsDiseases.Epidemic;
using UnityEngine;

namespace Events.Buildings.Plants
{
    public sealed class PlantDiseasesEpidemic : IPlantDiseases
    {
        private ConfigEventPlantDiseasesEpidemic _config;


        public PlantDiseasesEpidemic(in ConfigEventPlantDiseasesEpidemic config)
        {
            _config = config;
        }

        void IPlantDiseases.Update(in IUsesBuildingsEvents buildingsEvents)
        {
            Debug.Log("huila");

            if (buildingsEvents.configBuildingsEvents.activePossibleEvents.Count > 0)
                Debug.Log("lox");
        }
    }
}
