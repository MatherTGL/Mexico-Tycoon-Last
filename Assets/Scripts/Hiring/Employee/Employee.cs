using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config.Employees;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Config.Employees.ConfigEmployeeEditor;
using Random = UnityEngine.Random;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        public Employee(in AbstractEmployee[] possibleEmployeesInShop)
            => AsyncLoadRandomConfig(possibleEmployeesInShop);

        private Employee(in AbstractEmployee employee)
        {
            config = employee.config;
            LoadAndRandomizeData();
        }

        private async void AsyncLoadRandomConfig(AbstractEmployee[] possibleEmployeesInShop = null)
        {
            var loadHandle = Addressables.LoadAssetsAsync<ConfigEmployeeEditor>(new List<string> { "Employee" },
                conf => { }, Addressables.MergeMode.None);

            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                config = loadHandle.Result[Random.Range(0, loadHandle.Result.Count)];
            else
                throw new Exception("employee config error: {exception}");

            LoadAndRandomizeData(possibleEmployeesInShop);
        }

        private void LoadAndRandomizeData(AbstractEmployee[] possibleEmployeesInShop = null)
        {
            type = config.typeEmployee;
            rating = Random.Range(config.minRating, config.maxRating);

            paymentCostPerDay = config.minPaymentPerDay
                + (config.minPaymentPerDay * Random.Range(0, config.maxDeviationFromBasePay) * rating / 100);

            foreach (var employee in config.productionEfficiencyDictionary.Dictionary.Keys)
                efficiencyDictionary.TryAdd(employee, config.productionEfficiencyDictionary.Dictionary[employee]);

            if (possibleEmployeesInShop != null)
                Regenerate(possibleEmployeesInShop);
        }

        private async void Regenerate(AbstractEmployee[] possibleEmployeesInShop)
        {
            await Task.Run(() =>
            {
                for (byte i = 0; i < possibleEmployeesInShop.Length; i++)
                {
                    int identicalTypes = possibleEmployeesInShop.Select(item => possibleEmployeesInShop[i].type)
                                                                .Count();

                    do { possibleEmployeesInShop[i].UpdateOffer(possibleEmployeesInShop); }
                    while (identicalTypes <= possibleEmployeesInShop.Length / Enum.GetNames(typeof(TypeEmployee)).Length);
                }
            });
        }

        public sealed override AbstractEmployee Clone()
            => new Employee(this);

        public sealed override void UpdateOffer(AbstractEmployee[] possibleEmployeesInShop = null)
            => AsyncLoadRandomConfig(possibleEmployeesInShop);

        public override void UpdateState()
        {
            Debug.Log("State updated");
        }
    }
}
