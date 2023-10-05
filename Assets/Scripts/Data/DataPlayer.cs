using Config.Data.Player;
using UnityEngine;

namespace Data.Player
{
    public sealed class DataPlayer : IDataPlayer
    {
        private static readonly DataPlayer _instance = new DataPlayer();
        public static DataPlayer GetInstance => _instance;

        public enum SpendAndCheckMoneyState { Check, Spend }

        private static double _money;
        private static ushort _researchPoints;


        private DataPlayer() { }

        void IDataPlayer.AddPlayerMoney(in double amountMoney)
        {
            _money += amountMoney;
            Debug.Log($"Player Current Amount Money: {_money}$");
        }

        void IDataPlayer.AddPlayerResearchPoints(in ushort amountResearchPoints)
        {
            _researchPoints += amountResearchPoints;
        }

        bool IDataPlayer.CheckAndSpendingPlayerMoney(in double neededSum, in SpendAndCheckMoneyState state)
        {
            if ((_money - neededSum) > 0 && state == SpendAndCheckMoneyState.Spend)
            {
                _money -= neededSum;
                Debug.Log(_money);
                return true;
            }
            else return false;
        }

        bool IDataPlayer.CheckAndSpendingPlayerResearchPoints(in ushort amount, in SpendAndCheckMoneyState state)
        {
            throw new System.NotImplementedException();
        }

        double IDataPlayer.GetPlayerMoney() => _money;

        ushort IDataPlayer.GetPlayerResearchPoints() => _researchPoints;

        public void SetDataConfig(in ConfigDataPlayer configDataPlayer)
        {
            _money = configDataPlayer.startPlayerMoney;
            _researchPoints = configDataPlayer.startPlayerResearchPoints;
        }
    }
}
