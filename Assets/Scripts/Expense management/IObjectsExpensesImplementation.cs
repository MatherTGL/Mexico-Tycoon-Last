using static Expense.ExpenseManagementControl;
using static Expense.ExpensesBuildings;

namespace Expense
{
    public interface IObjectsExpensesImplementation
    {
        double GetTotalExpenses();

        void ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses, in AddOrReduceNumber addOrReduceNumber);
    }
}
