using Config.Expenses;

namespace Expense.Areas
{
    public sealed class ExpensesOnProduction : AbstractAreasExpenditure
    {
        public ExpensesOnProduction(in ConfigExpensesManagementEditor config)
        {
            _expenses = config.expensesOnProduction;
            _baseExpenses = config.expensesOnProduction;
        }

        // public sealed override void ChangeExpenses(in double expenses, in bool isAdd)
        // {
        //     _expenses = _baseExpenses + expenses;

        //     _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!
        //     Debug.Log($"{_expenses} / {_percentageQuality}");
        // }
    }
}