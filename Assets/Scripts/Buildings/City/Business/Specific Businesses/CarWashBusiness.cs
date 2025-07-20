using Config.Building.Business;
using Data;
using static Data.Player.DataPlayer;

namespace Business
{
    public sealed class CarWashBusiness : IBusiness
    {
        bool IBusiness.BuyBusiness(in ConfigCityBusinessEditor config)
        {
            if (DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(config.costPurchase,
                                                                    SpendAndCheckMoneyState.Spend,
                                                                    Data.Player.MoneyTypes.Clean))
                return true;
            else
                return false;
        }

        void IBusiness.MaintenanceConsumption(in ConfigCityBusinessEditor config)
        {
            DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(config.maintenanceCost,
                                                                SpendAndCheckMoneyState.Spend,
                                                                Data.Player.MoneyTypes.Clean);
        }

        void IBusiness.SellBusiness(in double costSell)
            => DataControl.IdataPlayer.AddPlayerMoney(costSell, Data.Player.MoneyTypes.Clean);

        void IBusiness.ToLaunderMoney(in double amountDirtyMoney, in ConfigCityBusinessEditor config)
        {
            double clearedMoney = amountDirtyMoney * config.percentageMoneyCleared;
            DataControl.IdataPlayer.AddPlayerMoney(clearedMoney, Data.Player.MoneyTypes.Clean);
        }
    }
}
