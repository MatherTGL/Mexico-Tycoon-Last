using Config.Expenses;
using UnityEngine;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public interface IUsesExpensesManagement
    {
        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; set; }


        void LoadExpensesManagement(in IExpensesManagement IexpensesManagement,
                                    in ConfigExpensesManagementEditor configExpenses)
        {
            IobjectsExpensesImplementation = IexpensesManagement.Registration(
                this, ExpensesTypeObject.Building, configExpenses);
        }
    }
}
