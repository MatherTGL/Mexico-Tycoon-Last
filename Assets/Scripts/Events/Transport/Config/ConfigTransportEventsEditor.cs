using System.Collections.Generic;
using Events.Transport;
using UnityEngine;

namespace Config.Building.Events
{
    [CreateAssetMenu(fileName = "TransportationEventsConfig", menuName = "Config/Transport/Events/Create New", order = 50)]
    public sealed class ConfigActiveTransportEventsEditor : ScriptableObject
    {
        [SerializeField]
        private List<TransportationEventTypes> l_activeEvents = new();

        public List<TransportationEventTypes> activeEvents => l_activeEvents;
    }
}