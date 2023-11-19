using UnityEngine;
using static Data.Player.DataPlayer;

namespace Bank
{
    public sealed class BankModel
    {
        private double _affordableCredit;

        private double _currentDebt;

        private float _loanInterest;


        public BankModel(in BankControl bankControl)
        {
            _affordableCredit = bankControl.configBank.affordableCredit;
            _loanInterest = bankControl.configBank.loanInterest;

            bankControl.updated += AccrueInterestDebt;
        }

        public void TakeLoan(in float percentage)
        {
            if ((_affordableCredit * percentage / 100) <= _affordableCredit)
            {
                double loan = _affordableCredit * percentage / 100;
                _affordableCredit -= loan;
                _currentDebt += loan;
                Debug.Log($"Current Debt: {_currentDebt} / Affordable Credit: {_affordableCredit}");
                Data.DataControl.IdataPlayer.AddPlayerMoney(loan);
            }
        }

        public void LoanRepayment(in float percentage)
        {
            if (Data.DataControl.IdataPlayer.GetPlayerMoney() >= (_currentDebt * percentage / 100))
            {
                double repayment = _currentDebt * percentage / 100;
                Debug.Log(repayment);
                _currentDebt -= repayment;
                _affordableCredit += repayment;
                Debug.Log($"AF: {_affordableCredit} / CD: {_currentDebt}");
                Data.DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(repayment, SpendAndCheckMoneyState.Spend);
            }
        }

        private void AccrueInterestDebt()
        {
            if (_currentDebt > 0)
                _currentDebt += _currentDebt * _loanInterest / 100;
            Debug.Log(_currentDebt);
        }
    }
}
