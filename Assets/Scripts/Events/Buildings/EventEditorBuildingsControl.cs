using UnityEngine;
using Building;
using System.Collections;
using static Resources.TypeProductionResources;

namespace Events.Buildings
{
    [RequireComponent(typeof(BuildingControl))]
    public sealed class EventEditorBuildingsControl : MonoBehaviour, IEventEditorBuildings
    {
        private IUsesBuildingsEvents _IusesBuildingsEvents;


        void IEventEditorBuildings.Init(in IUsesBuildingsEvents IusesBuildingsEvents)
        {
            _IusesBuildingsEvents = IusesBuildingsEvents;
            //? _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[0].isUseAccessResourcesAmount
            StartCoroutine(CheckTerms());
        }

        private IEnumerator CheckTerms()
        {
            while (true)
            {
                for (byte i = 0; i < _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents.Count; i++)
                {
                    if (_IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[i].isUseAccessResourcesAmount)
                        ResourcesEvent(i);
                }
                yield return new WaitForSeconds(10); //!
            }
        }

        private bool TemporaryEvent()
        {
            return true;
        }

        private bool InstantEvent()
        {
            return true;
        }

        private void ResourcesEvent(in byte indexElement)
        {
            foreach (var item in _IusesBuildingsEvents.configBuildingsEvents.activePossibleEvents[indexElement].typeResources)
            {
                TypeResource typeResource = item.Key;
                //? if (_IusesBuildingsEvents.amountResources[typeResource] )
            }
        }
    }
}
