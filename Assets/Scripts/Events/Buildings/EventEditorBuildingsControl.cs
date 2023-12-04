using UnityEngine;
using Building;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Events.Buildings
{
    [RequireComponent(typeof(BuildingControl))]
    public sealed class EventEditorBuildingsControl : MonoBehaviour, IEventEditorBuildings
    {
        private IUsesBuildingsEvents _IusesBuildingsEvents;

        [ShowInInspector, ReadOnly]
        private List<IBuildingEvent> l_allCreatedEvents = new();


        void IEventEditorBuildings.Init(in IUsesBuildingsEvents IusesBuildingsEvents)
        {
            _IusesBuildingsEvents = IusesBuildingsEvents;

            for (byte i = 0; i < _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count; i++)
            {
                var eventType = _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].typeEvent;
                var config = _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].config;


                CreateDependence(eventType, config);
            }
        }

        private void CreateDependence(in BuildingEventTypes buildingEventTypes, in ScriptableObject config)
        {
            if (buildingEventTypes is BuildingEventTypes.PlantDiseases)
                l_allCreatedEvents.Add(new PlantDiseasesEvent((ConfigEventPlantDiseases)config)); //? Set config
        }

        private void CheckCreatedDependencies()
        {
            for (byte i = 0; i < l_allCreatedEvents.Count; i++)
            {
                //? l_allCreatedEvents[i]. 
            }
        }

        // private IEnumerator CheckTerms()
        // {
        //     while (true)
        //     {
        //         for (byte i = 0; i < _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count; i++)
        //         {
        //             if (_IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].isUseAccessResourcesAmount)
        //                 ResourcesEvent(i);
        //         }
        //         yield return new WaitForSeconds(10); //!
        //     }
        // }

        // private bool TemporaryEvent()
        // {
        //     return true;
        // }

        // private bool InstantEvent()
        // {
        //     return true;
        // }

        // private void ResourcesEvent(in byte indexElement)
        // {
        //     for (int i = 0; i < _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count; i++)
        //     {
        //         TypeResource typeResource = _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.G;
        //         if (_IusesBuildingsEvents.amountResources[item.typeResources] >= _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[indexElement].typeResources[typeResource])
        //         {
        //             Debug.Log("Hui");
        //         }

        //     }
        // }
    }
}
