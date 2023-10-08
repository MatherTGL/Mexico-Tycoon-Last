using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public interface IObjectsExpensesImplementation
    {
        double GetTotalExpenses();

        void ChangeExpenses(in double addNumber, in AreaExpenditureType typeExpenses, in bool isAdd);
    }
}
