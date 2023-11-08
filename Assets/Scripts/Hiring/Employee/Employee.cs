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

                var randomNumberConfig = UnityEngine.Random.Range(0, configs.Length);
                config = configs[randomNumberConfig];

                LoadAndRandomizeData();
            }
            catch (Exception exception)
            {
                Debug.Log($"employee config error: {exception}");
            }
        }

        private Employee(in AbstractEmployee employee)
        {
            config = employee.config;
            LoadAndRandomizeData();
        }

        private void LoadAndRandomizeData()
        {
            //TODO: randomize data
            typeEmployee = config.typeEmployee;
            paymentCostPerDay = config.paymentPerDay;
            rating = config.rating;
            efficiency = config.efficiency;
        }

        public sealed override AbstractEmployee Clone() => new Employee(this);
    }
}
