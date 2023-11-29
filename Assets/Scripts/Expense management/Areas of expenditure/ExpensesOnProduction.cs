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
    }
}