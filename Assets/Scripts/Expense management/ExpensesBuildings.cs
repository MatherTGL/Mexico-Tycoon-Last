using UnityEngine;

namespace Expense
{
    public sealed class ExpensesBuildings : IObjectsExpensesImplementation
    {
        private double _maintenanceExpensesOnSecurity;


        public ExpensesBuildings()
        {
            Debug.Log("Init ExpensesBuildings");
        }

        void IObjectsExpensesImplementation.ChangeMaintenance(in double addNumber)
        {
            _maintenanceExpensesOnSecurity += addNumber;
            Debug.Log(_maintenanceExpensesOnSecurity);
        }
    }
}
