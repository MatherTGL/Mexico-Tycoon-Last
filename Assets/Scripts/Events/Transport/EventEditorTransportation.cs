using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config.Building.Events;
using Transport;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Events.Transport
{
    public sealed class EventEditorTransportation : IEventTransportation
    {
        private ConfigActiveTransportEventsEditor _config;

        private readonly List<ITransportEvents> l_allTransportationEvents = new();


        //TODO поменять загрузку конфига
        public EventEditorTransportation(in IEventsInfo IeventsInfo)
            => AsyncLoadConfig(IeventsInfo);

        private async void AsyncLoadConfig(IEventsInfo IeventsInfo)
        {
            var loadHandle = Addressables.LoadAssetAsync<ConfigActiveTransportEventsEditor>("TransportEvents");
            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                _config = loadHandle.Result;
            else
                throw new Exception("AsyncOperationStatus.Failed and config not loaded");

            InitEvents(IeventsInfo);
        }

        private void InitEvents(in IEventsInfo IeventsInfo)
        {
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