using static Expense.ExpenseManagementControl;
using static Expense.ExpensesBuildings;

namespace Expense
{
    public interface IObjectsExpensesImplementation
    {
        void SetAllExpensesInStart(in double number);

        double GetAllExpenses();

        void ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses, in AddOrReduceNumber addOrReduceNumber);
    }
}
