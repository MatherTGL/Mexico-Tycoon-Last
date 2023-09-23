namespace Building.Additional
{
    public interface IBuildingMonitorEnergy
    {
        void CalculateConsumption(in IEnergyConsumption IenergyConsumption);
    }
}
