using System;
using Config.Data.Player;

namespace Data.Player
{
    public interface IDataPlayer
    {
        Action dataChanged { get; set; }


        void SetDataConfig(in ConfigDataPlayer configDataPlayer);

        void AddPlayerMoney(in double amountMoney, MoneyTypes moneyType);

        bool CheckAndSpendingPlayerMoney(in double amount, in DataPlayer.SpendAndCheckMoneyState state, MoneyTypes moneyType);

        double GetPlayerMoney(MoneyTypes moneyType);

        void AddPlayerResearchPoints(in ushort amount);

        bool CheckAndSpendingPlayerResearchPoints(in ushort amount, in DataPlayer.SpendAndCheckMoneyState state);

        ushort GetPlayerResearchPoints();

        void AddGlobalReputation(float amount);

        void ReduceReputation(float amount);

        float GetGlobalReputation();
    }
}
