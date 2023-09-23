using Data;


namespace Building.Additional
{
    public sealed class SpendingToObjects
    {
        public static void SendNewExpense(in double amount)
        {
            DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(amount, true);
        }
    }
}
