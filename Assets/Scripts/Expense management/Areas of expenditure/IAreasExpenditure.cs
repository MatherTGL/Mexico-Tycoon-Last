using static Expense.ExpenseManagementControl;

namespace Expense.Areas
{
    public interface IAreasExpenditure
    {
        void ChangeExpenses(in double expenses, in AddOrReduceNumber addOrReduceType);
    }
}
