using UnityEngine;

namespace Events.Buildings
{
    public sealed class PlantDiseasesEvent : IBuildingEvent
    {
        private ConfigEventPlantDiseases _config;


        public PlantDiseasesEvent(in ConfigEventPlantDiseases config)
        {
            _config = config;
            Debug.Log("PlantDiseasesEvent created!");
        }
    }
}
