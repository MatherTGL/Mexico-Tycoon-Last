using Config.Employees;
using Hire.Employee;

namespace Building.Hire
{
    public sealed class PossibleEmployeesInShop : IPossibleEmployees
    {
        private ConfigPossibleEmployeesInShopEditor _config;
        ConfigPossibleEmployeesInShopEditor IPossibleEmployees.config => _config;

        private readonly AbstractEmployee[] a_possibleEmployeesInShop = new AbstractEmployee[10];
        AbstractEmployee[] IPossibleEmployees.possibleEmployeesInShop => a_possibleEmployeesInShop;


        public PossibleEmployeesInShop(in ConfigPossibleEmployeesInShopEditor config)
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