using Config.Expenses;

namespace Expense.Areas
{
    public sealed class ExpensesOnProduction : AbstractAreasExpenditure, IAreasExpenditure
    {
        double IAreasExpenditure.expenses { get => _expenses; set => _expenses = value; }

        int IAreasExpenditure.percentageQuality { get => _percentageQuality; set => _percentageQuality = value; }


        public ExpensesOnProduction(in ConfigExpensesManagementEditor config)
        {
            _expenses = config.expensesOnProduction;
        }
    }
}