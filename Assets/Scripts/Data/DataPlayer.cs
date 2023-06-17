using Config.Data.Player;
using UnityEngine;


namespace Data.Player
{
    public class DataPlayer : IDataPlayer
    {
        //TODO закинуть в ридонли конфиг для удобства 
        //? через конструктор
        private static double _money; 
        private static ushort _researchPoints;


        public void SetDataConfig(in ConfigDataPlayer configDataPlayer)
        {
            _money = configDataPlayer.startPlayerMoney;
            _researchPoints = configDataPlayer.startPlayerResearchPoints;
            Debug.Log($"{_money} / {_researchPoints}");
        }

        void IDataPlayer.AddPlayerMoney(in double amount) => _money += amount;

        void IDataPlayer.AddPlayerResearchPoints(in ushort amount) => _researchPoints += amount;

        bool IDataPlayer.CheckAndSpendingPlayerMoney(in double amount, in bool isSpending)
        {
            if (amount < _money)
            {
                if (isSpending)
                {
                    _money -= amount;
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        bool IDataPlayer.CheckAndSpendingPlayerResearchPoints(in ushort amount, in bool isSpending)
        {
            throw new System.NotImplementedException();
        }

        double IDataPlayer.GetPlayerMoney() => _money;

        ushort IDataPlayer.GetPlayerResearchPoints() => _researchPoints;
    }
}
