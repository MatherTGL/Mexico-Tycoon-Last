using System.Collections.Generic;
using System.Linq;
using Config.Building.Events;
using Transport;
using UnityEngine;

namespace Events.Transport
{
    public sealed class EventEditorTransportation : IEventTransportation
    {
        private const byte numberAttemptsLoadConfig = 3;

        private readonly ConfigActiveTransportEventsEditor _config;

        private readonly List<ITransportEvents> l_allTransportationEvents = new();


        public EventEditorTransportation(in IEventsInfo IeventsInfo)
        {
            for (byte i = 0; i < numberAttemptsLoadConfig; i++)
            {
                if (_config == null)
                    _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigActiveTransportEventsEditor>()[0];
            }

            for (byte indexEvent = 0; indexEvent < _config.activeEvents.Count; indexEvent++)
            {
                if (_config.activeEvents[indexEvent] is TransportationEventTypes.PoliceRaid)
                {
                    if (l_allTransportationEvents.Any(item => item is not PoliceRaidTransportEvent))
                        l_allTransportationEvents.Add(new PoliceRaidTransportEvent(IeventsInfo));
                }
            }
        }

        void IEventTransportation.Update()
        {
            for (byte i = 0; i < l_allTransportationEvents.Count; i++)
                l_allTransportationEvents[i].CheckConditionsAreMet();

            Debug.Log("IEventTransportation.Update()");
        }
    }
}