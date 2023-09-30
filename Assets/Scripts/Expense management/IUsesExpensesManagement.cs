namespace Expense
{
    public interface IUsesExpensesManagement
    {
        void LoadExpensesManagement(in IExpensesManagement IexpensesManagement);

        void SetMaintenanceExpensesOnSecurity(in double number);
    }
}
