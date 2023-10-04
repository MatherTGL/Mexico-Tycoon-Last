using System.Collections.Generic;
using Expense.Areas;
using static Expense.ExpenseManagementControl;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        public enum TypeExpenses : byte { General, Water, Security }

        private Dictionary<TypeExpenses, IAreasExpenditure> d_IareasExpenditure = new();

        private double _allExpenses;


        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses,
                                                           in AddOrReduceNumber addOrReduceNumber)
        {
            if (d_IareasExpenditure.ContainsKey(typeExpenses) == false)
            {
                IAreasExpenditure areasExpenditure;

                if (typeExpenses is TypeExpenses.Water)
                    areasExpenditure = new ExpensesOnWater();
                else if (typeExpenses is TypeExpenses.Security)
                    areasExpenditure = new ExpensesOnSecurity();
                else
                    return;

                d_IareasExpenditure.Add(typeExpenses, areasExpenditure);
            }

            d_IareasExpenditure[typeExpenses].ChangeExpenses(addNumber, addOrReduceNumber);
            _allExpenses += addNumber;
        }

        void IObjectsExpensesImplementation.SetAllExpensesInStart(in double generalExpensesInConfig)
        {
            _allExpenses = generalExpensesInConfig;
        }

        double IObjectsExpensesImplementation.GetAllExpenses()
        {
            return _allExpenses;
        }
    }
}
