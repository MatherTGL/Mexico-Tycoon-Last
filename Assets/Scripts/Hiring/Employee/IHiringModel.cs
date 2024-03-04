using System;
using System.Collections.Generic;
using Hire.Employee;
using static Config.Employees.ConfigEmployeeEditor;

namespace Building.Hire
{
    public interface IHiringModel
    {
        Lazy<Dictionary<TypeEmployee, double>> d_employeeExpenses { get; set; }

#if UNITY_EDITOR
        AbstractEmployee[] a_possibleEmployeesInShop { get; }
#endif

        void ConstantUpdatingInfo();

        void Hire(in byte indexEmployee);

        void Firing(in byte indexEmployee);

        Dictionary<TypeEmployee, List<AbstractEmployee>> GetAllEmployees();

        void UpdateAllEmployees();
    }
}
