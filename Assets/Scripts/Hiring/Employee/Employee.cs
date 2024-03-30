using Config.Employees;
using UnityEngine;
using static Resources.TypeProductionResources;
using Random = UnityEngine.Random;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        public Employee(in ConfigEmployeeEditor config)
        {
            this.config = config;
            LoadAndRandomizeData();
        }

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

        private void LoadAndRandomizeData()
        {
            type = config.typeEmployee;
            rating = Random.Range(config.minRating, config.maxRating);

            paymentCostPerDay = config.minPaymentPerDay
                + (config.minPaymentPerDay * Random.Range(0, config.maxDeviationFromBasePay) * rating / 100);

            foreach (var employee in config.productionEfficiencyDictionary.Dictionary.Keys)
                efficiencyDictionary.TryAdd(employee, config.productionEfficiencyDictionary.Dictionary[employee]);

            // if (possibleEmployeesInShop != null)
            //     AsyncRegenerate(possibleEmployeesInShop);
        }

        // private async void AsyncRegenerate(AbstractEmployee[] possibleEmployeesInShop)
        // {
        //     await Task.Run(() =>
        //     {
        //         for (byte i = 0; i < possibleEmployeesInShop.Length; i++)
        //         {
        //             Debug.Log($"L: {possibleEmployeesInShop.Length} / {i}");
        //             int identicalTypes = possibleEmployeesInShop.Select(item => possibleEmployeesInShop[i].type)
        //                                                         .Count();

        //             do { possibleEmployeesInShop[i].UpdateOffer(possibleEmployeesInShop); }
        //             while (identicalTypes <= possibleEmployeesInShop.Length / Enum.GetNames(typeof(TypeEmployee)).Length);
        //         }
        //     });
        // }

        public sealed override AbstractEmployee Clone()
            => new Employee(this);

        //TODO complete
        public sealed override void UpdateOffer()
        {
            //=> AsyncLoadRandomConfig(possibleEmployeesInShop);
        }

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
