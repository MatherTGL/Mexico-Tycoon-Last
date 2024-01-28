namespace Building.Additional
{
    public interface IEnergyConsumption
    {
        IBuildingMonitorEnergy IbuildingMonitorEnergy { get; }


        void MonitorEnergy(in IEnergyConsumption IenergyConsumption)
        {
            IbuildingMonitorEnergy.CalculateConsumption(IenergyConsumption);
        }
    }
}
