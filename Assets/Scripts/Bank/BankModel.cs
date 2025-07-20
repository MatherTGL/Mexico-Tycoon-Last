using Config.Bank;
using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Data.Player.DataPlayer;

namespace Bank
{
    public sealed class BankModel
    {
        private readonly Dictionary<ConfigBankEditor, double> d_affordableCredit = new();

        private readonly Dictionary<ConfigBankEditor, double> d_currentDebt = new();

        private readonly Dictionary<ConfigBankEditor, float> d_loanInterest = new();

        private readonly Dictionary<ConfigBankEditor, double> d_deposits = new();


        public BankModel(in BankControl bankControl)
        {
            for (byte i = 0; i < bankControl.configBanks.Length; i++)
            {
                d_affordableCredit.Add(bankControl.configBanks[i], bankControl.configBanks[i].affordableCredit);
                d_loanInterest.Add(bankControl.configBanks[i], bankControl.configBanks[i].loanInterest);
                d_currentDebt.Add(bankControl.configBanks[i], 0);
                d_deposits.Add(bankControl.configBanks[i], 0);
            }

            bankControl.updated += AccrueInterestDebt;
            bankControl.updated += AccrualOfInterestOnTheDeposit;
        }

        public void TakeLoan(in float percentage, in ConfigBankEditor bank)
        {
            if ((d_affordableCredit[bank] * percentage / 100) <= d_affordableCredit[bank])
            {
                double loan = d_affordableCredit[bank] * percentage / 100;
                d_affordableCredit[bank] -= loan;
                d_currentDebt[bank] += loan;
                //!Debug.Log($"Current Debt: {d_currentDebt[bank]} / Affordable Credit: {d_affordableCredit[bank]}");
                DataControl.IdataPlayer.AddPlayerMoney(loan, Data.Player.MoneyTypes.Clean);
            }
        }

        public void LoanRepayment(in float percentage, in ConfigBankEditor bank)
        {
            if (DataControl.IdataPlayer.GetPlayerMoney(Data.Player.MoneyTypes.Clean) >= (d_currentDebt[bank] * percentage / 100))
            {
                double repayment = d_currentDebt[bank] * percentage / 100;
                //!Debug.Log(repayment);
                d_currentDebt[bank] -= repayment;
                d_affordableCredit[bank] += repayment;
                //!Debug.Log($"AF: {d_affordableCredit[bank]} / CD: {d_currentDebt[bank]}");
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(repayment, SpendAndCheckMoneyState.Spend, Data.Player.MoneyTypes.Clean);
            }
        }

        public void PutOnDeposit(in double sum, in ConfigBankEditor bank)
        {
            if ((d_deposits[bank] + sum) <= bank.maxDepositSum)
            {
                d_deposits[bank] += sum;
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(sum, SpendAndCheckMoneyState.Spend, Data.Player.MoneyTypes.Clean);
                Debug.Log($"Current depo in this bank: {d_deposits[bank]}");
            }
            else
            {
                Debug.Log($"Max deposit in this bank: {d_deposits[bank]}");
            }
        }

        public void WithdrawMoneyFromTheDeposit(in double sum, in ConfigBankEditor bank)
        {
            if (d_deposits[bank] < sum)
                return;

            d_deposits[bank] -= sum;
            DataControl.IdataPlayer.AddPlayerMoney(sum, Data.Player.MoneyTypes.Clean);
            Debug.Log($"Current depo in this bank: {d_deposits[bank]}");
        }

        public double GetMoneyOnDeposit(in ConfigBankEditor bank) => d_deposits[bank];

        private void AccrueInterestDebt()
        {
            foreach (var bank in d_affordableCredit.Keys)
            {
                if (d_currentDebt[bank] > 0)
                    d_currentDebt[bank] += d_currentDebt[bank] * d_loanInterest[bank] / 100;
                //!DebugSystem.Log($"Current debt in bank: {d_currentDebt[bank]}", DebugSystem.SelectedColor.Green, tag: "Bank");
            }
        }

        private void AccrualOfInterestOnTheDeposit()
        {
            foreach (var bank in d_deposits.Keys.ToList())
                d_deposits[bank] += d_deposits[bank] * bank.interestOnTheDeposit;
        }
    }
}
