using UnityEngine;

namespace Building.Additional
{
    public interface IEnergyConsumption
    {
        IBuildingMonitorEnergy IbuildingMonitorEnergy { get; }


        void MonitorEnergy(in IEnergyConsumption IenergyConsumption)
        {
            IbuildingMonitorEnergy.CalculateConsumption(IenergyConsumption);
            Debug.Log("Monitor energy");
        }
    }
}
