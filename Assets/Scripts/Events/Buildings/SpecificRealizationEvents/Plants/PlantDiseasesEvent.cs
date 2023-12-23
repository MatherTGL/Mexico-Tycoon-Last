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
            IPlantDiseases plantDiseases = null;

            if (_config.isActiveEpidemic)
                plantDiseases = new PlantDiseasesEpidemic(_config.configEventPlantDiseasesEpidemic);

            if (plantDiseases != null)
                l_variousPlantEvents.Add(plantDiseases);
        }

        void IBuildingEvent.CheckConditionsAreMet(in IUsesBuildingsEvents buildingsEvents)
        {
            for (byte i = 0; i < l_variousPlantEvents.Count; i++)
                l_variousPlantEvents[i].Update(buildingsEvents);
        }
    }
}
