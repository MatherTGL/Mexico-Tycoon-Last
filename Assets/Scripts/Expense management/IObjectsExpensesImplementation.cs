using Building.Hire;
using static Config.Employees.ConfigEmployeeEditor;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public interface IObjectsExpensesImplementation
    {
        IHiringModel IhiringModel { get; set; }


        double GetTotalExpenses();

        void ChangeExpenses(in double addNumber, in AreaExpenditureType typeExpenses, in bool isAdd);

        void ChangeEmployeesExpenses(in double expenses, in bool isAdd, in TypeEmployee typeEmployee);

        void ChangeSeasonExpenses(in double expenses);
    }
}
