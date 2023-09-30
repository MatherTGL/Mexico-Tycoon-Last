namespace Expense
{
    public interface IExpensesManagement
    {
        void Registration(in IUsesExpensesManagement IusesExpensesManagement,
                          in ExpenseManagementControl.Type type);
    }
}
