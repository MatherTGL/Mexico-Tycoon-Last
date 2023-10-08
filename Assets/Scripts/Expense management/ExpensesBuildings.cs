using System.Collections.Generic;
using Config.Expenses;
using Expense.Areas;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        private ConfigExpensesManagementEditor _configExpensesManagement;

        private Dictionary<AreaExpenditureType, IAreasExpenditure> d_IareasExpenditure = new();


        public ExpensesBuildings(in ConfigExpensesManagementEditor config)
        {
            _configExpensesManagement = config;
        }

        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in AreaExpenditureType typeExpenses,
                                                           in bool isAdd)
        {
            if (d_IareasExpenditure.ContainsKey(typeExpenses) == false)
            {
                IAreasExpenditure areasExpenditure;

                if (typeExpenses is AreaExpenditureType.Water)
                    areasExpenditure = new ExpensesOnWater(_configExpensesManagement);
                else if (typeExpenses is AreaExpenditureType.Security)
                    areasExpenditure = new ExpensesOnSecurity(_configExpensesManagement);
                else
                    areasExpenditure = new ExpensesOnProduction(_configExpensesManagement);

                d_IareasExpenditure.Add(typeExpenses, areasExpenditure);
            }

            d_IareasExpenditure[typeExpenses].ChangeExpenses(addNumber, isAdd);
        }

        double IObjectsExpensesImplementation.GetTotalExpenses()
        {
            double totalExpenses = 0;
            foreach (var item in d_IareasExpenditure.Values)
                totalExpenses += item.expenses;

            return totalExpenses;
        }
    }
}
