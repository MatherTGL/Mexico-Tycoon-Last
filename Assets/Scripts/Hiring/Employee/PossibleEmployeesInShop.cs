using System;
using System.Linq;
using System.Threading.Tasks;
using Config.Employees;
using Hire.Employee;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using confPossibleEmployees = Config.Employees.ConfigPossibleEmployeesInShopEditor;
using Random = UnityEngine.Random;

namespace Building.Hire
{
    public sealed class PossibleEmployeesInShop : IPossibleEmployees
    {
        private confPossibleEmployees _config;
        confPossibleEmployees IPossibleEmployees.config => _config;

        private ConfigEmployeeEditor[] _employeeConfigs;

        private readonly AbstractEmployee[] a_possibleEmployeesInShop = new AbstractEmployee[confPossibleEmployees.numberEmployeesOffered];
        AbstractEmployee[] IPossibleEmployees.possibleEmployeesInShop => a_possibleEmployeesInShop;


        public PossibleEmployeesInShop(in confPossibleEmployees config)
        {
            _config = config;
            AsyncLoadConfigs();
        }

        private async ValueTask AsyncLoadConfigs()
        {
            var loadHandle = Addressables.LoadAssetsAsync<ConfigEmployeeEditor>("Employee", null);
            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _employeeConfigs = new ConfigEmployeeEditor[loadHandle.Result.Count];
                _employeeConfigs = loadHandle.Result.ToArray();
                CreateEmployees();
            }
            else
                throw new Exception("employee config error: {exception}"); ;

            Addressables.Release(loadHandle);
        }

        private void CreateEmployees()
        {
            for (byte i = 0; i < a_possibleEmployeesInShop.Length; i++)
                a_possibleEmployeesInShop[i] = new Employee(_employeeConfigs[Random.Range(0, _employeeConfigs.Length)]);
        }

        void IPossibleEmployees.UpdateOffers()
        {
            for (byte i = 0; i < a_possibleEmployeesInShop.Length; i++)
                a_possibleEmployeesInShop[i]?.UpdateOffer();
        }
    }
}