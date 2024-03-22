using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config.Employees;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;
using Random = UnityEngine.Random;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        public Employee(in AbstractEmployee[] possibleEmployeesInShop)
            => AsyncLoadRandomConfig(possibleEmployeesInShop);

        private Employee(in AbstractEmployee clone)
        {
            config = clone.config;
            type = clone.type;
            rating = clone.rating;
            currentEfficiency = clone.currentEfficiency;
            increaseEmployeeSalary.Value.Init(config, clone.paymentCostPerDay);

            foreach (var item in clone.efficiencyDictionary.Keys)
                efficiencyDictionary.TryAdd(item, clone.efficiencyDictionary[item]);
        }

        private async void AsyncLoadRandomConfig(AbstractEmployee[] possibleEmployeesInShop = null)
        {
            var loadHandle = Addressables.LoadAssetsAsync<ConfigEmployeeEditor>("Employee", conf => { }, Addressables.MergeMode.None);
            Debug.Log($"AsyncLoadRandomConfig 1");
            await loadHandle.Task;
            Debug.Log($"AsyncLoadRandomConfig 2");

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                config = loadHandle.Result[Random.Range(0, loadHandle.Result.Count)];
            else
                throw new Exception("employee config error: {exception}");
            Debug.Log($"AsyncLoadRandomConfig 4 {config}");
            LoadAndRandomizeData(possibleEmployeesInShop);
        }

        private void LoadAndRandomizeData(AbstractEmployee[] possibleEmployeesInShop)
        {
            type = config.typeEmployee;
            rating = Random.Range(config.minRating, config.maxRating);

            paymentCostPerDay = config.minPaymentPerDay
                + (config.minPaymentPerDay * Random.Range(0, config.maxDeviationFromBasePay) * rating / 100);

            // increaseEmployeeSalary.Value.Init(paymentCostPerDay, rating);

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

        public sealed override void UpdateState()
        {
            UpdateEfficiency();
            UpdateIncreaseEmployeeSalary();
        }

        private void UpdateIncreaseEmployeeSalary()
            => increaseEmployeeSalary.Value.Update();

        private void UpdateEfficiency()
        {
            if (efficiencyDictionary.Count <= 0)
                return;

            Debug.Log($"bbbb {efficiencyDictionary[TypeResource.CocaLeaves]}");

            Debug.Log($"UpdateState: {efficiencyDictionary.Count}");
            if (currentEfficiency > config.minEfficiency)
            {
                currentEfficiency -= config.rateDeclineEfficiency;
                Debug.Log($"efficiency{currentEfficiency} / {type}");

                for (TypeResource type = 0; (int)type < efficiencyDictionary.Count; type++)
                {
                    Debug.Log($"2 efficiencyDictionary[{type}]: {efficiencyDictionary[type]}");
                    Debug.Log($"xxx.porn {currentEfficiency} / {efficiencyDictionary[type] * Mathf.RoundToInt(currentEfficiency / 100)}");
                    efficiencyDictionary[type] = Mathf.RoundToInt(efficiencyDictionary[type] * currentEfficiency / 100);
                    Debug.Log($"efficiencyDictionary[{type}]: {efficiencyDictionary[type]}");
                }
            }
        }
    }
}
