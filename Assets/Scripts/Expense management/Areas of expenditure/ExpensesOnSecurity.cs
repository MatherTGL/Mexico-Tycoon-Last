using Config.Expenses;

namespace Expense.Areas
{
    public sealed class ExpensesOnSecurity : AbstractAreasExpenditure
    {
        public ExpensesOnSecurity(in ConfigExpensesManagementEditor config)
        {
            _expenses = config.expensesOnSecurity;
        }
    }
}
