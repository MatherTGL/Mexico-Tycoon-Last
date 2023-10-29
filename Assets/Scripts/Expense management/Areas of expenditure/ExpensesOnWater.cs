using Config.Expenses;

namespace Expense.Areas
{
    public sealed class ExpensesOnWater : AbstractAreasExpenditure
    {
        public ExpensesOnWater(in ConfigExpensesManagementEditor config)
        {
            _expenses = config.expensesOnWater;
        }
    }
}
