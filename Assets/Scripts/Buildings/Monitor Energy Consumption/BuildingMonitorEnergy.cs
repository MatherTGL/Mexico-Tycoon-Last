using Data;

namespace Building.Additional
{
    public sealed class BuildingMonitorEnergy : IBuildingMonitorEnergy
    {
        void IBuildingMonitorEnergy.CalculateConsumption(in IEnergyConsumption IenergyConsumption)
        {
            DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(20, true);
        }
    }
}
