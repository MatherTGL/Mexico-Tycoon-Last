using Config.Bank;
using DebugCustomSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using static Data.Player.DataPlayer;

namespace Bank
{
    public sealed class BankModel
    {
        private readonly Dictionary<ConfigBankEditor, double> d_affordableCredit = new();

        private readonly Dictionary<ConfigBankEditor, double> d_currentDebt = new();

        private readonly Dictionary<ConfigBankEditor, float> d_loanInterest = new();


        public BankModel(in BankControl bankControl)
        {
            for (byte i = 0; i < bankControl.configBanks.Length; i++)
            {
                d_affordableCredit.Add(bankControl.configBanks[i], bankControl.configBanks[i].affordableCredit);
                d_loanInterest.Add(bankControl.configBanks[i], bankControl.configBanks[i].loanInterest);
                d_currentDebt.Add(bankControl.configBanks[i], 0);
            }

            bankControl.updated += AccrueInterestDebt;
        }

        public void TakeLoan(in float percentage, in ConfigBankEditor bank)
        {
            if ((d_affordableCredit[bank] * percentage / 100) <= d_affordableCredit[bank])
            {
                double loan = d_affordableCredit[bank] * percentage / 100;
                d_affordableCredit[bank] -= loan;
                d_currentDebt[bank] += loan;
                Debug.Log($"Current Debt: {d_currentDebt[bank]} / Affordable Credit: {d_affordableCredit[bank]}");
                Data.DataControl.IdataPlayer.AddPlayerMoney(loan);
            }
        }

        public void LoanRepayment(in float percentage, in ConfigBankEditor bank)
        {
            if (Data.DataControl.IdataPlayer.GetPlayerMoney() >= (d_currentDebt[bank] * percentage / 100))
            {
                double repayment = d_currentDebt[bank] * percentage / 100;
                Debug.Log(repayment);
                d_currentDebt[bank] -= repayment;
                d_affordableCredit[bank] += repayment;
                Debug.Log($"AF: {d_affordableCredit[bank]} / CD: {d_currentDebt[bank]}");
                Data.DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(repayment, SpendAndCheckMoneyState.Spend);
            }
        }

        private void AccrueInterestDebt()
        {
            foreach (var bank in d_affordableCredit.Keys)
            {
                if (d_currentDebt[bank] > 0)
                    d_currentDebt[bank] += d_currentDebt[bank] * d_loanInterest[bank] / 100;
                DebugSystem.Log($"Current debt in bank: {d_currentDebt[bank]}", DebugSystem.SelectedColor.Green, tag: "Bank");
            }
        }
    }
}
