using System;
using System.Linq;
using Config.Employees;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        private const string _pathEmployeesConfigs = "Configs/Employees";


        public Employee() => LoadRandomConfig();

        private Employee(in AbstractEmployee employee)
        {
            config = employee.config;
            LoadAndRandomizeData();
        }

        private void LoadRandomConfig()
        {
            try
            {
                config = UnityEngine.Resources.LoadAll<ConfigEmployeeEditor>(_pathEmployeesConfigs)
                    .OrderBy(rand => Random.Range(int.MinValue, int.MaxValue))
                    .First();

                LoadAndRandomizeData();
            }
            catch (Exception exception)
            {
                Debug.Log($"employee config error: {exception}");
            }
        }

        //TODO: randomize data
        private void LoadAndRandomizeData()
        {
            typeEmployee = config.typeEmployee;
            paymentCostPerDay = config.paymentPerDay;
            rating = config.rating;

            foreach (var employee in config.productionEfficiencyDictionary.Dictionary.Keys)
                efficiencyDictionary.TryAdd(employee, config.productionEfficiencyDictionary.Dictionary[employee]);
        }

        public sealed override AbstractEmployee Clone() => new Employee(this);

        public sealed override void Update() => LoadRandomConfig();
    }
}
