using UnityEngine;

namespace Building.Additional
{
    public interface ISpending
    {
        double maintenanceExpenses { get; }


        void Spending()
        {
            SpendingToObjects.SendNewExpense(maintenanceExpenses);
            Debug.Log("spending");
        }
    }
}
