using Building.Hire;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public interface IObjectsExpensesImplementation
    {
        IHiring Ihiring { get; set; }


        double GetTotalExpenses();

        void ChangeExpenses(in double addNumber, in AreaExpenditureType typeExpenses, in bool isAdd);
    }
}
