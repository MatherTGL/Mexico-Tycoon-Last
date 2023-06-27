using Config.Data.Player;
using UnityEngine;


namespace Data.Player
{
    public sealed class DataPlayer : IDataPlayer
    {
        private static double _money;
        private static ushort _researchPoints;


        public void SetDataConfig(in ConfigDataPlayer configDataPlayer)
        {
            _money = configDataPlayer.startPlayerMoney;
            _researchPoints = configDataPlayer.startPlayerResearchPoints;
        }

        void IDataPlayer.AddPlayerMoney(in double amountMoney)
        {
            _money += amountMoney;
            Debug.Log(_money);
        }

        void IDataPlayer.AddPlayerResearchPoints(in ushort amountResearchPoints) => _researchPoints += amountResearchPoints;

        bool IDataPlayer.CheckAndSpendingPlayerMoney(in double neededSum, in bool isSpending)
        {
            if (neededSum < _money)
            {
                if (isSpending)
                {
                    SpendingMoney(neededSum);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        private void SpendingMoney(double neededSum)
        {
            _money -= neededSum;
        }

        bool IDataPlayer.CheckAndSpendingPlayerResearchPoints(in ushort amount, in bool isSpending)
        {
            throw new System.NotImplementedException();
        }

        double IDataPlayer.GetPlayerMoney() => _money;

        ushort IDataPlayer.GetPlayerResearchPoints() => _researchPoints;
    }
}
