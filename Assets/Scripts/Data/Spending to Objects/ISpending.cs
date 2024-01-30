using Expense;

namespace Building.Additional
{
    public interface ISpending
    {
        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; }


        double GetExpenses() => IobjectsExpensesImplementation.GetTotalExpenses();

        void Spending() => SpendingToObjects.SendNewExpense(GetExpenses());
    }
}
