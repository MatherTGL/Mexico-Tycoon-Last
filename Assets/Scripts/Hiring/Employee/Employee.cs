using System;
using Config.Employees;
using UnityEngine;
using Random = UnityEngine.Random;

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
                var randomNumberConfig = Random.Range(0, configs.Length);

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

        //TODO: randomize data
        private void LoadAndRandomizeData()
        {
            typeEmployee = config.typeEmployee;
            paymentCostPerDay = config.paymentPerDay;
            rating = config.rating;

            foreach (var employee in config.productionEfficiencyDictionary.Dictionary.Keys)
                efficiencyDictionary.Add(employee, config.productionEfficiencyDictionary.Dictionary[employee]);
        }

        public sealed override AbstractEmployee Clone() => new Employee(this);
    }
}
