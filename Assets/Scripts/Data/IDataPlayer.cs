using System;
using Config.Data.Player;

namespace Data.Player
{
    public interface IDataPlayer
    {
        Action dataChanged { get; set; }


        void SetDataConfig(in ConfigDataPlayer configDataPlayer);

        bool CheckAndSpendingPlayerMoney(in double amount, in DataPlayer.SpendAndCheckMoneyState state, MoneyTypes moneyType);

        void AddPlayerMoney(in double amountMoney, MoneyTypes moneyType);

        double GetPlayerMoney(MoneyTypes moneyType);

        bool CheckAndSpendingPlayerResearchPoints(in ushort amount, in DataPlayer.SpendAndCheckMoneyState state);

        void AddPlayerResearchPoints(in ushort amount);

        ushort GetPlayerResearchPoints();
    }
}
