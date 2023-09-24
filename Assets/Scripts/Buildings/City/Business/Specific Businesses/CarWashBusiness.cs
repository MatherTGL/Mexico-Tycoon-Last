using Data;

namespace Business
{
    public sealed class CarWashBusiness : IBusiness
    {
        void IBusiness.ToLaunderMoney(in double amountDirtyMoney, in float percentageMoneyCleared)
        {
            var clearedMoney = amountDirtyMoney * percentageMoneyCleared;
            DataControl.IdataPlayer.AddPlayerMoney(clearedMoney);
        }
    }
}
