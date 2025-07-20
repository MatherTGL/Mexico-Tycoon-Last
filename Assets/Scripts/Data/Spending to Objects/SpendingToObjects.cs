using Data;
using static Data.Player.DataPlayer;

namespace Building.Additional
{
    public sealed class SpendingToObjects
    {
        public static void SendNewExpense(in double amount)
            => DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(amount, SpendAndCheckMoneyState.Spend, Data.Player.MoneyTypes.Clean);
    }
}
