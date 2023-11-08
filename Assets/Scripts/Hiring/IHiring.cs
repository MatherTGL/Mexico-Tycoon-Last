using System;
using System.Collections.Generic;
using static Config.Employees.ConfigEmployeeEditor;

namespace Building.Hire
{
    public interface IHiring
    {
        Lazy<Dictionary<TypeEmployee, double>> d_employeeExpenses { get; set; }


        void ConstantUpdatingInfo();

        void Hire(in byte indexEmployee);

        void Firing(in byte indexEmployee);
    }
}
