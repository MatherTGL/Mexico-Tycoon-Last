using System.Collections.Generic;
using Building.Hire;
using Config.Expenses;
using Expense.Areas;
using static Config.Employees.ConfigEmployeeEditor;
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
            CreateAreasExpenditure();
        }

        private void CreateAreasExpenditure()
        {
            d_IareasExpenditure.TryAdd(AreaExpenditureType.Production, new ExpensesOnProduction(_configExpensesManagement));
            d_IareasExpenditure.TryAdd(AreaExpenditureType.Water, new ExpensesOnWater(_configExpensesManagement));
            d_IareasExpenditure.TryAdd(AreaExpenditureType.Security, new ExpensesOnSecurity(_configExpensesManagement));
            d_IareasExpenditure.TryAdd(AreaExpenditureType.Employees, new ExpensesOnEmployees());

            d_IareasExpenditure[AreaExpenditureType.Employees].InitHiring(_Ihiring);
        }

        void IObjectsExpensesImplementation.ChangeExpenses(in double addNumber, in AreaExpenditureType typeExpenses,
                                                           in bool isAdd)
        {
            d_IareasExpenditure[typeExpenses].ChangeExpenses(addNumber, isAdd);
        }

        void IObjectsExpensesImplementation.ChangeSeasonExpenses(in double expenses)
        {
            d_IareasExpenditure[AreaExpenditureType.Production].ChangeSeasonExpenses(expenses);
        }

        void IObjectsExpensesImplementation.ChangeEmployeesExpenses(in double expenses, in bool isAdd, in TypeEmployee typeEmployee)
        {
            d_IareasExpenditure[AreaExpenditureType.Employees].ChangeEmployeesExpenses(expenses, isAdd, typeEmployee);
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
