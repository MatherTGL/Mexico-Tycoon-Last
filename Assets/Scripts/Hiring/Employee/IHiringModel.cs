using System;
using System.Collections.Generic;
using Hire.Employee;
using static Config.Employees.ConfigEmployeeEditor;

namespace Building.Hire
{
    public interface IHiringModel
    {
        Lazy<Dictionary<TypeEmployee, double>> d_employeeExpenses { get; set; }


        void ConstantUpdatingInfo();

        void AsyncHire(byte indexEmployee, IPossibleEmployees IpossibleEmployees);

        void Firing(in byte indexEmployee, in IPossibleEmployees IpossibleEmployees);

        Dictionary<TypeEmployee, List<AbstractEmployee>> GetAllEmployees();
    }
}
