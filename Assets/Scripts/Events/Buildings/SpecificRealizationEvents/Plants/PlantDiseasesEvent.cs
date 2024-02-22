using System.Collections.Generic;
using UnityEngine;

namespace Events.Buildings.Plants
{
    public sealed class PlantDiseasesEvent : MonoBehaviour, IUserEvent
    {
        private IUsesBuildingsEvents _IusesBuildingsEvents;

        private ConfigEventPlantDiseases _config;

        private readonly List<IPlantDiseases> l_variousPlantEvents = new();


        void IUserEvent.Init(in ScriptableObject config)
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

        void IUserEvent.CheckConditionsAreMet()
        {
            for (byte i = 0; i < l_variousPlantEvents.Count; i++)
                l_variousPlantEvents[i].Update(_IusesBuildingsEvents);
        }
    }
}
