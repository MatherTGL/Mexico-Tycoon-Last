using Config.Employees;
using Hire.Employee;

namespace Building.Hire
{
    public interface IPossibleEmployees
    {
        ConfigPossibleEmployeesInShopEditor config { get; }

        AbstractEmployee[] possibleEmployeesInShop { get; }


        void UpdateOffers();
    }
}