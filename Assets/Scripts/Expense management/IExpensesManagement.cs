using Config.Expenses;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public interface IExpensesManagement
    {
        IObjectsExpensesImplementation Registration(in IUsesExpensesManagement IusesExpensesManagement,
                                                    in ExpensesTypeObject type,
                                                    in ConfigExpensesManagementEditor configExpenses);
    }
}
