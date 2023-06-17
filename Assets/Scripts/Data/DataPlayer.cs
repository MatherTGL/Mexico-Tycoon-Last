using UnityEngine;


namespace Data.Player
{
    public class DataPlayer : IDataPlayer
    {
        //TODO закинуть в ридонли конфиг для удобства 
        //? через конструктор
        private static double _money = 50000; 
        private static ushort _researchPoints;


        void IDataPlayer.AddPlayerMoney(double amount) => _money += amount;

        void IDataPlayer.AddPlayerResearchPoints(ushort amount) => _researchPoints += amount;

        bool IDataPlayer.CheckAndSpendingPlayerMoney(double amount, bool isSpending)
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

        bool IDataPlayer.CheckAndSpendingPlayerResearchPoints(ushort amount, bool isSpending)
        {
            throw new System.NotImplementedException();
        }

        double IDataPlayer.GetPlayerMoney() => _money;

        ushort IDataPlayer.GetPlayerResearchPoints() => _researchPoints;
    }
}
