using Data;
using static Data.Player.DataPlayer;

namespace Building.Additional
{
    public interface IBuildingPurchased
    {
        bool isBuyed { get; protected set; }

        double costPurchase { get; }


        void Buy()
        {
            if (!isBuyed && DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(costPurchase, SpendAndCheckMoneyState.Spend))
                isBuyed = true;
        }

        void Sell()
        {
            if (isBuyed)
                isBuyed = false;
        }
    }
}
