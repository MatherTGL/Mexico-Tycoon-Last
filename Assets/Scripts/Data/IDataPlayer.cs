using Config.Data.Player;


namespace Data.Player
{
    public interface IDataPlayer
    {
        void SetDataConfig(in ConfigDataPlayer configDataPlayer);

        bool CheckAndSpendingPlayerMoney(in double amount, in bool isSpending);

        void AddPlayerMoney(in double amount);

        double GetPlayerMoney();

        bool CheckAndSpendingPlayerResearchPoints(in ushort amount, in bool isSpending);

        void AddPlayerResearchPoints(in ushort amount);

        ushort GetPlayerResearchPoints();
    }
}
