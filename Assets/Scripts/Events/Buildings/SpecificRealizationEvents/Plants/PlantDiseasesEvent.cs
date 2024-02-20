using System.Collections.Generic;
using UnityEngine;

namespace Events.Buildings.Plants
{
    public sealed class PlantDiseasesEvent : MonoBehaviour, IBuildingEvent
    {
        private IUsesBuildingsEvents _IusesBuildingsEvents;

        private ConfigEventPlantDiseases _config;

        private readonly List<IPlantDiseases> l_variousPlantEvents = new();


        void IBuildingEvent.Init(in ScriptableObject config)
        {
            _config = config as ConfigEventPlantDiseases;
            _IusesBuildingsEvents = GetComponent<IUsesBuildingsEvents>();

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

        void IBuildingEvent.CheckConditionsAreMet()
        {
            for (byte i = 0; i < l_variousPlantEvents.Count; i++)
                l_variousPlantEvents[i].Update(_IusesBuildingsEvents);
        }
    }
}
