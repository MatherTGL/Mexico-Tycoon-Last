using Config.Expenses;

namespace Expense
{
    public interface IExpensesManagement
    {
        IObjectsExpensesImplementation Registration(in IUsesExpensesManagement IusesExpensesManagement,
                                                    in ExpenseManagementControl.Type type,
                                                    in ConfigExpensesManagementEditor configExpenses);
    }
}
