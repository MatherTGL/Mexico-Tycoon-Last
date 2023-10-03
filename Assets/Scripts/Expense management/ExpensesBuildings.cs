using System.Collections.Generic;
using Expense.Areas;
using static Expense.ExpenseManagementControl;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        public enum TypeExpenses { General, Water, Security }

        private TypeExpenses _typeExpenses;

        private Dictionary<TypeExpenses, IAreasExpenditure> d_IareasExpenditure = new();

        private double _allExpenses;


        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses,
                                                           in AddOrReduceNumber addOrReduceNumber)
        {
            _typeExpenses = typeExpenses;

            if (d_IareasExpenditure.ContainsKey(_typeExpenses) == false)
            {
                IAreasExpenditure areasExpenditure;

                if (_typeExpenses is TypeExpenses.Water)
                    areasExpenditure = new ExpensesOnWater();
                else if (_typeExpenses is TypeExpenses.Security)
                    areasExpenditure = new ExpensesOnSecurity();
                else
                    return;

                d_IareasExpenditure.Add(_typeExpenses, areasExpenditure);
            }

            d_IareasExpenditure[_typeExpenses].ChangeExpenses(addNumber, addOrReduceNumber);
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
