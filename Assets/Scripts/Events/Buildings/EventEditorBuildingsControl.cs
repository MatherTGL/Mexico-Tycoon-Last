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
    public sealed class EventBuildingsEditorControl : MonoBehaviour, IEventEditorBuildings
    {
        [ShowInInspector, ReadOnly]
        private readonly List<IUserEvent> l_allCreatedEvents = new();

        private WaitForSeconds _timeStep;

        [System.Obsolete]
        void IEventEditorBuildings.Init(in IUsesBuildingsEvents IusesBuildingsEvents)
        {
            _timeStep = new WaitForSeconds(FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay());

            for (byte i = 0; i < IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count; i++)
            {
                var eventType = IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].typeEvent;
                var config = IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].config;

                CreateDependence(eventType, config);
            }

            if (IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count != 0)
                StartCoroutine(UpdateEvents());
        }

        private void CreateDependence(in BuildingEventTypes buildingEventTypes, in ScriptableObject config)
        {
            if (buildingEventTypes is BuildingEventTypes.PlantDiseases)
                gameObject.AddComponent<PlantDiseasesEvent>();

            l_allCreatedEvents.AddRange(GetComponents<IUserEvent>());

            for (byte i = 0; i < l_allCreatedEvents.Count; i++)
                l_allCreatedEvents[i].Init(config);
        }

        private IEnumerator UpdateEvents()
        {
            while (true)
            {
                yield return _timeStep;

                for (byte i = 0; i < l_allCreatedEvents.Count; i++)
                    l_allCreatedEvents[i].CheckConditionsAreMet();
            }
        }
    }
}
