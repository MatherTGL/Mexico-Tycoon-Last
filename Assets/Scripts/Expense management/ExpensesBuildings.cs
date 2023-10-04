using System.Collections.Generic;
using Config.Expenses;
using Expense.Areas;
using UnityEngine;
using static Expense.ExpenseManagementControl;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        private ConfigExpensesManagementEditor _configExpensesManagement;

        public enum TypeExpenses : byte { General, Water, Security }

        private Dictionary<TypeExpenses, IAreasExpenditure> d_IareasExpenditure = new();

        private double _allExpenses;


        public ExpensesBuildings(in ConfigExpensesManagementEditor config)
        {
            _configExpensesManagement = config;
            _allExpenses = config.allExpenses;
        }

        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in TypeExpenses typeExpenses,
                                                           in AddOrReduceNumber addOrReduceNumber)
        {
            if (d_IareasExpenditure.ContainsKey(typeExpenses) == false)
            {
                IAreasExpenditure areasExpenditure;

                if (typeExpenses is TypeExpenses.Water)
                    areasExpenditure = new ExpensesOnWater(_configExpensesManagement);
                else if (typeExpenses is TypeExpenses.Security)
                    areasExpenditure = new ExpensesOnSecurity(_configExpensesManagement);
                else
                    return;

                d_IareasExpenditure.Add(typeExpenses, areasExpenditure);
            }

            if ((_allExpenses + addNumber) > 0)
            {
                d_IareasExpenditure[typeExpenses].ChangeExpenses(addNumber, addOrReduceNumber);
                _allExpenses += addNumber;;
            }
        }

        double IObjectsExpensesImplementation.GetAllExpenses()
        {
            return _allExpenses;
        }
    }
}
