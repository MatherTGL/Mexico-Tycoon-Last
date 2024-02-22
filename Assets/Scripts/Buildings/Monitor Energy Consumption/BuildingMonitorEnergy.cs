using Data;
using static Data.Player.DataPlayer;

namespace Building.Additional
{
    //TODO: https://yougile.com/team/bf00efa6ea26/#chat:c206ec9ecde1
    public sealed class BuildingMonitorEnergy : IBuildingMonitorEnergy
    {
        void IBuildingMonitorEnergy.CalculateConsumption(in IEnergyConsumption IenergyConsumption)
            => DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(20, SpendAndCheckMoneyState.Spend);
    }
}
