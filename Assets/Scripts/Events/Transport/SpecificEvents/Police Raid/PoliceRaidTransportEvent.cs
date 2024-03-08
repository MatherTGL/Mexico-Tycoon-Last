using Transport;
using UnityEngine;

namespace Events.Transport
{
    public sealed class PoliceRaidTransportEvent : ITransportEvents
    {
        private readonly ConfigTransportPoliceRaidEventEditor _config;

        private readonly IEventsInfo _IeventsInfo; //! make general link in parent class


        public PoliceRaidTransportEvent(in IEventsInfo IeventsInfo)
        {
            _IeventsInfo = IeventsInfo;

            try
            {
                _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigTransportPoliceRaidEventEditor>()[0];
            }
            catch (System.Exception ex) { throw new System.Exception($"{ex}"); }
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