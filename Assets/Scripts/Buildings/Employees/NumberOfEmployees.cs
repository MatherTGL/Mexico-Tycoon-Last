using System.Collections.Generic;
using Config.Employees;
using Hire.Employee;

namespace Building.Additional
{
    public sealed class NumberOfEmployees : INumberOfEmployees
    {
        bool INumberOfEmployees.IsThereAreEnoughEmployees(in Dictionary<ConfigEmployeeEditor.TypeEmployee, byte> requiredEmployees,
            in Dictionary<ConfigEmployeeEditor.TypeEmployee, List<AbstractEmployee>> employees)
        {
            foreach (var employee in requiredEmployees.Keys)
                if (employees.ContainsKey(employee) == false ||
                    employees[employee].Count < requiredEmployees[employee])
                    return false;

            return true;
        }
    }
}