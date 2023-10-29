using System.Collections.Generic;
using Building.Hire;
using Config.Expenses;
using Expense.Areas;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        private ConfigExpensesManagementEditor _configExpensesManagement;

        private IHiring _Ihiring;
        IHiring IObjectsExpensesImplementation.Ihiring { get => _Ihiring; set => _Ihiring = value; }

        private Dictionary<AreaExpenditureType, AbstractAreasExpenditure> d_IareasExpenditure = new();


        public ExpensesBuildings(in ConfigExpensesManagementEditor config)
        {
            _configExpensesManagement = config;
        }

        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in AreaExpenditureType typeExpenses,
                                                           in bool isAdd)
        {
            if (d_IareasExpenditure.ContainsKey(typeExpenses) == false)
            {
                AbstractAreasExpenditure areasExpenditure;

                if (typeExpenses is AreaExpenditureType.Water)
                    areasExpenditure = new ExpensesOnWater(_configExpensesManagement);
                else if (typeExpenses is AreaExpenditureType.Security)
                    areasExpenditure = new ExpensesOnSecurity(_configExpensesManagement);
                else if (typeExpenses is AreaExpenditureType.Production)
                    areasExpenditure = new ExpensesOnProduction(_configExpensesManagement);
                else
                    areasExpenditure = new ExpensesOnEmployees(_Ihiring);

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
