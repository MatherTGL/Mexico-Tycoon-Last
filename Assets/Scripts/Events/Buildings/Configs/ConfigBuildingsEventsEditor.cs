using UnityEngine;
using Sirenix.OdinInspector;
using Events.Buildings;
using System.Collections.Generic;

namespace Config.Building.Events
{
    [CreateAssetMenu(fileName = "BuildingsEventsConfig", menuName = "Config/Buildings/Events/Create New", order = 50)]
    public sealed class ConfigBuildingsEventsEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Events"), LabelText("Active Events")]
        private List<BuildingEventStructure> l_activePossibleEvents = new();
        public List<BuildingEventStructure> activePossibleEvents => l_activePossibleEvents;
    }
}
