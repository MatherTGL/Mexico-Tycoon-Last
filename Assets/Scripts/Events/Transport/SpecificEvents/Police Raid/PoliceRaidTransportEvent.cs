using UnityEngine;

namespace Events.Transport
{
    public sealed class PoliceRaidTransportEvent : ITransportEvents
    {
        private readonly ConfigTransportPoliceRaidEventEditor _config;


        public PoliceRaidTransportEvent()
        {
            try
            {
                _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigTransportPoliceRaidEventEditor>()[0];
            }
            catch (System.Exception ex) { throw new System.Exception($"{ex}"); }
        }

        void ITransportEvents.CheckConditionsAreMet()
        {
            //? WHAT
            Debug.Log("ITransportEvents.CheckConditionsAreMet()");
        }
    }
}