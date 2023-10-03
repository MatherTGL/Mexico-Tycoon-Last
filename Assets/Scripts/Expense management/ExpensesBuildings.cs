using System.Collections.Generic;
using Expense.Areas;
using static Expense.ExpenseManagementControl;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        public enum TypeExpenses { General, Water, Security }

        private Dictionary<TypeExpenses, IAreasExpenditure> d_IareasExpenditure = new();

        private double _allExpenses;


        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses,
                                                           in AddOrReduceNumber addOrReduceNumber)
        {
            TypeExpenses typeExpensesEnum = typeExpenses;

            if (d_IareasExpenditure.ContainsKey(typeExpensesEnum) == false)
            {
                IAreasExpenditure areasExpenditure = null;

                if (typeExpensesEnum is TypeExpenses.Water)
                    areasExpenditure = new ExpensesOnWater();
                else
                    return;

                d_IareasExpenditure.Add(typeExpensesEnum, areasExpenditure);
            }

            d_IareasExpenditure[typeExpensesEnum].ChangeExpenses(addNumber, addOrReduceNumber);
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
