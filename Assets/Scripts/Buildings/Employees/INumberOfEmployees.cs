using System.Collections.Generic;
using Hire.Employee;
using static Config.Employees.ConfigEmployeeEditor;

namespace Building.Additional
{
    public interface INumberOfEmployees
    {
        bool IsThereAreEnoughEmployees(in Dictionary<TypeEmployee, byte> requiredEmployees,
                                       in Dictionary<TypeEmployee, List<AbstractEmployee>> employees);
    }
}