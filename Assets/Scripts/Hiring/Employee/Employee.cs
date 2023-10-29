using System;
using Config.Employees;
using UnityEngine;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        private const string _pathEmployeesConfigs = "Configs/Employees";


        public Employee()
        {
            try
            {
                var configs = UnityEngine.Resources.LoadAll<ConfigEmployeeEditor>(_pathEmployeesConfigs);

                foreach (var conf in configs)
                {
                    if (conf.typeEmployee == typeEmployee)
                    {
                        config = conf;
                        return;
                    }
                }

                LoadData();
            }
            catch (Exception exception)
            {
                Debug.Log($"employee config error: {exception}");
            }
        }

        private Employee(in AbstractEmployee employee)
        {
            config = employee.config;
            LoadData();
        }

        private void LoadData()
        {
            typeEmployee = config.typeEmployee;
            paymentCostPerDay = config.paymentPerDay;
            rating = config.rating;
            efficiency = config.efficiency;
        }

        public sealed override AbstractEmployee Clone() => new Employee(this);
    }
}
