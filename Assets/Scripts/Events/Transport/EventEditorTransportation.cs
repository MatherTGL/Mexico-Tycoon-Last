using System.Collections.Generic;
using System.Linq;
using Config.Building.Events;
using Transport;
using UnityEngine;

namespace Events.Transport
{
    public sealed class EventEditorTransportation : IEventTransportation
    {
        private readonly ConfigActiveTransportEventsEditor _config;

        private readonly ITransportInteractRoute _ItransportInteractRoute;

        private readonly List<ITransportEvents> l_allTransportationEvents = new();


        public EventEditorTransportation(in ITransportInteractRoute ItransportInteractRoute)
        {
            _ItransportInteractRoute = ItransportInteractRoute;

            try
            {
                _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigActiveTransportEventsEditor>()[0];
            }
            catch (System.Exception ex) { throw new System.Exception($"{ex}"); }

            for (byte indexEvent = 0; indexEvent < _config.activeEvents.Count; indexEvent++)
            {
                if (_config.activeEvents[indexEvent] is TransportationEventTypes.PoliceRaid)
                {
                    if (l_allTransportationEvents.Any(item => item is PoliceRaidTransportEvent))
                        return;

                    l_allTransportationEvents.Add(new PoliceRaidTransportEvent());
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