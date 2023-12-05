using System.Collections.Generic;
using UnityEngine;

namespace Events.Buildings.Plants
{
    public sealed class PlantDiseasesEvent : IBuildingEvent
    {
        private ConfigEventPlantDiseases _config;

        private List<IPlantDiseases> l_variousPlantEvents = new();


        public PlantDiseasesEvent(in ConfigEventPlantDiseases config)
        {
            _config = config;
            CreateAllPlantDiseasesEvents();
        }

        private void CreateAllPlantDiseasesEvents()
        {
            Debug.Log(_config);
            Debug.Log(_config.configEventPlantDiseasesEpidemic);
            l_variousPlantEvents.Add(new PlantDiseasesEpidemic(_config.configEventPlantDiseasesEpidemic));
        }

        void IBuildingEvent.CheckConditionsAreMet(in IUsesBuildingsEvents buildingsEvents)
        {
            for (byte i = 0; i < l_variousPlantEvents.Count; i++)
                l_variousPlantEvents[i].Update(buildingsEvents);
        }
    }
}
