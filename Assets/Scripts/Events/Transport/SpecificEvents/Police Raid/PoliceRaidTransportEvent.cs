using System;
using Transport;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Events.Transport
{
    public sealed class PoliceRaidTransportEvent : ITransportEvents
    {
        private ConfigTransportPoliceRaidEventEditor _config;

        private readonly IEventsInfo _IeventsInfo; //? make general link in parent class


        public PoliceRaidTransportEvent(in IEventsInfo IeventsInfo)
        {
            _IeventsInfo = IeventsInfo;
            AsyncLoadConfig();
        }

        private async void AsyncLoadConfig()
        {
            var loadHandle = Addressables.LoadAssetAsync<ConfigTransportPoliceRaidEventEditor>("TransportEventPoliceRaid");
            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                _config = loadHandle.Result;
            else
                throw new Exception("AsyncOperationStatus.Failed and config not loaded");
        }

        void ITransportEvents.CheckConditionsAreMet()
        {
            if (_IeventsInfo.isCargoPackaging)
                Debug.Log("USE BLYAT PACKAGING SUKA NAXOY");
            //? WHAT
            Debug.Log("ITransportEvents.CheckConditionsAreMet()");
        }
    }
}