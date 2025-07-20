using Data;
using static Data.Player.DataPlayer;

namespace Building.Additional
{
    //TODO
    public sealed class BuildingMonitorEnergy : IBuildingMonitorEnergy
    {
        void IBuildingMonitorEnergy.CalculateConsumption(in IEnergyConsumption IenergyConsumption)
            => DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(20, SpendAndCheckMoneyState.Spend, Data.Player.MoneyTypes.Clean);
    }
}
