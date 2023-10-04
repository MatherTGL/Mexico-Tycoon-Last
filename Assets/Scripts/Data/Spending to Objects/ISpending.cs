using Expense;

namespace Building.Additional
{
    public interface ISpending
    {
        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; }


        double GetExpenses()
        {
            return IobjectsExpensesImplementation.GetAllExpenses();
        }

        void Spending()
        {
            SpendingToObjects.SendNewExpense(GetExpenses());
        }
    }
}
