using Config.Expenses;

namespace Expense.Areas
{
    public sealed class ExpensesOnSecurity : AbstractAreasExpenditure, IAreasExpenditure
    {
        double IAreasExpenditure.expenses { get => _expenses; set => _expenses = value; }

        int IAreasExpenditure.percentageQuality { get => _percentageQuality; set => _percentageQuality = value; }


        public ExpensesOnSecurity(in ConfigExpensesManagementEditor config)
        {
            _expenses = config.expensesOnSecurity;
        }
    }
}
