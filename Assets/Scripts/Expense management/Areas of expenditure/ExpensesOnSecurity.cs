using Config.Expenses;

namespace Expense.Areas
{
    public sealed class ExpensesOnSecurity : IAreasExpenditure
    {
        private double _expenses;
        double IAreasExpenditure.expenses { get => _expenses; set => _expenses = value; }

        private int _percentageQuality;
        int IAreasExpenditure.percentageQuality { get => _percentageQuality; set => _percentageQuality = value; }


        public ExpensesOnSecurity(in ConfigExpensesManagementEditor config)
        {
            _expenses = config.expensesOnSecurity;
        }
    }
}
