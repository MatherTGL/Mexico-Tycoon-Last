using Config.Building.Business;
using Data;

namespace Business
{
    public sealed class CarWashBusiness : IBusiness
    {
        bool IBusiness.BuyBusiness(in ConfigCityBusinessEditor config)
        {
            if (DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(config.costPurchase, true))
                return true;
            else
                return false;
        }

        void IBusiness.SellBusiness()
        {
            throw new System.NotImplementedException();
        }

        void IBusiness.ToLaunderMoney(in double amountDirtyMoney, in ConfigCityBusinessEditor config)
        {
            var clearedMoney = amountDirtyMoney * config.percentageMoneyCleared;
            DataControl.IdataPlayer.AddPlayerMoney(clearedMoney);
        }
    }
}
