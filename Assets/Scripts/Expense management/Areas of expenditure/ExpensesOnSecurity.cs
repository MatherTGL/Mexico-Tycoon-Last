using UnityEngine;
using static Expense.ExpenseManagementControl;

namespace Expense.Areas
{
    public sealed class ExpensesOnSecurity : IAreasExpenditure
    {
        private double _expenses;

        private int _percentageQuality;


        void IAreasExpenditure.ChangeExpenses(in double expenses, in AddOrReduceNumber addOrReduceType)
        {
            //TODO: refactoring and remove duplicate code
            _expenses += expenses;
            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!
            Debug.Log($"{_expenses} / {_percentageQuality}");
        }
    }
}
