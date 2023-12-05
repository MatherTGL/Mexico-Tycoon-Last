using UnityEngine;
using Building;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using TimeControl;
using Events.Buildings.Plants;

namespace Events.Buildings
{
    [RequireComponent(typeof(BuildingControl))]
    public sealed class EventEditorBuildingsControl : MonoBehaviour, IEventEditorBuildings
    {
        private IUsesBuildingsEvents _IusesBuildingsEvents;

        [ShowInInspector, ReadOnly]
        private List<IBuildingEvent> l_allCreatedEvents = new();

        private WaitForSeconds _timeStep;


        void IEventEditorBuildings.Init(in IUsesBuildingsEvents IusesBuildingsEvents)
        {
            _IusesBuildingsEvents = IusesBuildingsEvents;
            _timeStep = new WaitForSeconds(FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay());

            for (byte i = 0; i < _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count; i++)
            {
                var eventType = _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].typeEvent;
                var config = _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].config;

                CreateDependence(eventType, config);
            }

            if (_IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count != 0)
                StartCoroutine(UpdateEvents());
        }

        private void CreateDependence(in BuildingEventTypes buildingEventTypes, in ScriptableObject config)
        {
            if (buildingEventTypes is BuildingEventTypes.PlantDiseases)
                l_allCreatedEvents.Add(new PlantDiseasesEvent((ConfigEventPlantDiseases)config));
        }

        private void UpdateAllEvents()
        {
            for (byte i = 0; i < l_allCreatedEvents.Count; i++)
                l_allCreatedEvents[i].CheckConditionsAreMet(_IusesBuildingsEvents);
        }

        private IEnumerator UpdateEvents()
        {
            while (true)
            {
                UpdateAllEvents();
                yield return _timeStep;
            }
        }
    }
}
