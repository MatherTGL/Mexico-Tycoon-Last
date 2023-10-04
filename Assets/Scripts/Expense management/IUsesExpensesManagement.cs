using Config.Expenses;

namespace Expense
{
    public interface IUsesExpensesManagement
    {
        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; set; }


        void LoadExpensesManagement(in IExpensesManagement IexpensesManagement, 
                                    in ConfigExpensesManagementEditor configExpenses)
        {
            IobjectsExpensesImplementation = IexpensesManagement.Registration(
                this, ExpenseManagementControl.Type.Building, configExpenses);
        }
    }
}
