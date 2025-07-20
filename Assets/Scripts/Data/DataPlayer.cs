using System;
using Config.Data.Player;
using DebugCustomSystem;
using UnityEngine;

namespace Data.Player
{
    public sealed class DataPlayer : IDataPlayer
    {
        public enum SpendAndCheckMoneyState : byte { Check, Spend }

        private Action dataChanged;

        Action IDataPlayer.dataChanged { get => dataChanged; set => dataChanged = value; }

        private static double _cleanMoney;

        private static double _dirtMoney;

        private static ushort _researchPoints;


        void IDataPlayer.AddPlayerMoney(in double amountMoney, MoneyTypes moneyType)
        {
            if (moneyType == MoneyTypes.Clean)
                _cleanMoney += amountMoney;
            else
                _dirtMoney += amountMoney;

            dataChanged?.Invoke();
            DebugSystem.Log($"Haved amount money: {_cleanMoney}", DebugSystem.SelectedColor.Yellow, tag: "Player");
        }

        void IDataPlayer.AddPlayerResearchPoints(in ushort amountResearchPoints)
        {
            _researchPoints += amountResearchPoints;
            dataChanged?.Invoke();
        }

        bool IDataPlayer.CheckAndSpendingPlayerMoney(in double neededSum, in SpendAndCheckMoneyState state, MoneyTypes moneyType)
        {
            if (moneyType == MoneyTypes.Clean)
                return CalculateMoney(ref _cleanMoney, neededSum, state);
            else
                return CalculateMoney(ref _dirtMoney, neededSum, state);

            bool CalculateMoney(ref double money, in double neededSum, in SpendAndCheckMoneyState state)
            {
                if ((money - neededSum) > 0 && state == SpendAndCheckMoneyState.Spend)
                {
                    money -= neededSum;
                    dataChanged?.Invoke();
                    money = Math.Round(money);
                    DebugSystem.Log($"Haved amount money: {money}", DebugSystem.SelectedColor.Yellow, tag: "Player");
                    return true;
                }
                else return false;
            }
        }

        bool IDataPlayer.CheckAndSpendingPlayerResearchPoints(in ushort amount, in SpendAndCheckMoneyState state)
        {
            throw new System.NotImplementedException();
        }

        double IDataPlayer.GetPlayerMoney(MoneyTypes moneyType)
        {
            if (moneyType == MoneyTypes.Clean)
                return _cleanMoney;
            else
                return _dirtMoney;
        }

        ushort IDataPlayer.GetPlayerResearchPoints() => _researchPoints;

        void IDataPlayer.SetDataConfig(in ConfigDataPlayer configDataPlayer)
        {
            _cleanMoney = configDataPlayer.startPlayerMoney;
            _researchPoints = configDataPlayer.startPlayerResearchPoints;
        }
    }
}
