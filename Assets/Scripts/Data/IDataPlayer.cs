namespace Data.Player
{
    public interface IDataPlayer
    {
        bool CheckAndSpendingPlayerMoney(double amount, bool isSpending);
        void AddPlayerMoney(double amount);
        double GetPlayerMoney();
        bool CheckAndSpendingPlayerResearchPoints(ushort amount, bool isSpending);
        void AddPlayerResearchPoints(ushort amount);
        ushort GetPlayerResearchPoints();
    }
}
