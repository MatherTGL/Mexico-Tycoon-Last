using Expense;

namespace Building.Additional
{
    public interface ISpending
    {
        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; }


        double GetExpenses()
        {
            return IobjectsExpensesImplementation.GetTotalExpenses();
        }

        void Spending()
        {
            SpendingToObjects.SendNewExpense(GetExpenses());
        }
    }
}
