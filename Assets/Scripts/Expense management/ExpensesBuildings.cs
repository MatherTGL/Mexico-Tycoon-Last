using System.Collections.Generic;
using Expense.Areas;
using UnityEngine;
using static Expense.ExpenseManagementControl;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        public enum TypeExpenses { General, Water, Security }

        private Dictionary<TypeExpenses, IAreasExpenditure> d_IareasExpenditure = new();

        private double _allExpenses;


        public ExpensesBuildings()
        {
            //* load default value to _maintenanceExpensesOnSecurity
            Debug.Log("Init ExpensesBuildings");
        }

        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses, 
                                                           in AddOrReduceNumber addOrReduceNumber)
        {
            //! refactoring
            if (typeExpenses == TypeExpenses.Water)
            {
                if (d_IareasExpenditure.ContainsKey(TypeExpenses.Water))
                {
                    d_IareasExpenditure[TypeExpenses.Water].ChangeExpenses(addNumber, addOrReduceNumber);
                }
                else
                {
                    d_IareasExpenditure.Add(TypeExpenses.Water, new ExpensesOnWater());
                }
                _allExpenses++;
            }
            //CheckAndAddMaintenanceExpenses(ref addNumber, )
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
