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

        public enum TypeExpenses : byte { Production, Water, Security }

        private Dictionary<TypeExpenses, IAreasExpenditure> d_IareasExpenditure = new();


        public ExpensesBuildings(in ConfigExpensesManagementEditor config)
        {
            _configExpensesManagement = config;
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
                    areasExpenditure = new ExpensesOnProduction(_configExpensesManagement);

                d_IareasExpenditure.Add(typeExpenses, areasExpenditure);
            }

            d_IareasExpenditure[typeExpenses].ChangeExpenses(addNumber, addOrReduceNumber);
        }

        double IObjectsExpensesImplementation.GetTotalExpenses()
        {
            double totalExpenses = 0;
            foreach (var item in d_IareasExpenditure.Values)
                totalExpenses += item.expenses;

            Debug.Log(totalExpenses);
            return totalExpenses;
        }
    }
}
