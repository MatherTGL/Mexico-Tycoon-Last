using UnityEngine;
using Building;

namespace Events.Buildings
{
    [RequireComponent(typeof(BuildingControl))]
    public sealed class EventEditorBuildingsControl : MonoBehaviour, IEventEditorBuildings
    {
        private IBuilding _Ibuilding;


        void IEventEditorBuildings.Init(in IBuilding Ibuilding)
        {
            _Ibuilding = Ibuilding;
        }
    }
}
