using System.Linq;
using UnityEngine;

namespace Events.Transport
{
    public sealed class PoliceRaidTransportEvent : ITransportEvents
    {
        private ConfigTransportPoliceRaidEventEditor _config;


        public PoliceRaidTransportEvent()
        {
            try
            {
                _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigTransportPoliceRaidEventEditor>().First();
            }
            catch (System.Exception ex) { throw new System.Exception($"{ex}"); }
        }

        void ITransportEvents.CheckConditionsAreMet()
        {
            Debug.Log("ITransportEvents.CheckConditionsAreMet()");
        }
    }
}