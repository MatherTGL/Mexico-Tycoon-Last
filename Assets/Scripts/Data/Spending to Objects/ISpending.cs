namespace Building.Additional
{
    public interface ISpending
    {
        double maintenanceExpenses { get; }


        void Spending()
        {
            SpendingToObjects.SendNewExpense(maintenanceExpenses);
        }
    }
}
