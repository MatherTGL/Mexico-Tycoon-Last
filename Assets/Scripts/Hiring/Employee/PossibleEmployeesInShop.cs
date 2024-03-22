using Hire.Employee;
using UnityEngine;
using confPossibleEmployees = Config.Employees.ConfigPossibleEmployeesInShopEditor;

namespace Building.Hire
{
    public sealed class PossibleEmployeesInShop : IPossibleEmployees
    {
        private confPossibleEmployees _config;
        confPossibleEmployees IPossibleEmployees.config => _config;

        private readonly AbstractEmployee[] a_possibleEmployeesInShop = new AbstractEmployee[confPossibleEmployees.numberEmployeesOffered];
        AbstractEmployee[] IPossibleEmployees.possibleEmployeesInShop => a_possibleEmployeesInShop;


        public PossibleEmployeesInShop(in confPossibleEmployees config)
        {
            _config = config;

            for (byte i = 0; i < a_possibleEmployeesInShop.Length; i++)
                a_possibleEmployeesInShop[i] = new Employee(a_possibleEmployeesInShop);
        }

        void IPossibleEmployees.UpdateOffers()
        {
            for (byte i = 0; i < a_possibleEmployeesInShop.Length; i++)
                a_possibleEmployeesInShop[i].UpdateOffer();
        }
    }
}