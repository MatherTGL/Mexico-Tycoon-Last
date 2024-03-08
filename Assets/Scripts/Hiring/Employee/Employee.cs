using System;
using System.Collections.Generic;
using System.Linq;
using Config.Employees;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        public Employee() => AsyncLoadRandomConfig();

        private Employee(in AbstractEmployee employee)
        {
            config = employee.config;
            LoadAndRandomizeData();
        }

        private async void AsyncLoadRandomConfig()
        {
            var loadHandle = Addressables.LoadAssetsAsync<ConfigEmployeeEditor>(new List<string> { "Employee" },
                conf => { }, Addressables.MergeMode.None);

            await loadHandle.Task;

            if (loadHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                config = loadHandle.Result.OrderBy(rand => Random.Range(int.MinValue, int.MaxValue)).First();
            else
                throw new Exception("employee config error: {exception}");

            LoadAndRandomizeData();
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

        public sealed override void UpdateOffers() => AsyncLoadRandomConfig();
    }
}
