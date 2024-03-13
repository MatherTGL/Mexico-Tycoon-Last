using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Hire.Employee;
using UnityEngine;
using static Config.Employees.ConfigEmployeeEditor;
using static Data.Player.DataPlayer;

namespace Building.Hire
{
    public sealed class HireEmployeeModel : IHiringModel
    {
        private readonly Lazy<Dictionary<TypeEmployee, List<AbstractEmployee>>> d_employees = new();

        private Lazy<Dictionary<TypeEmployee, double>> d_employeeExpenses = new();
        Lazy<Dictionary<TypeEmployee, double>> IHiringModel.d_employeeExpenses
        { get => d_employeeExpenses; set => d_employeeExpenses = value; }


        public HireEmployeeModel() { }

        private void CalculateExpenses()
        {
            foreach (var typeEmployeeExpenses in d_employeeExpenses.Value.Keys)
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(d_employeeExpenses.Value[typeEmployeeExpenses],
                                                                    SpendAndCheckMoneyState.Spend);
        }

        void IHiringModel.ConstantUpdatingInfo() => CalculateExpenses();

        async void IHiringModel.AsyncHire(byte indexEmployee, IPossibleEmployees IpossibleEmployees)
        {
            var typeEmployee = IpossibleEmployees.possibleEmployeesInShop[indexEmployee].typeEmployee;

            if (d_employees.Value.ContainsKey(typeEmployee) == false)
            {
                d_employees.Value.Add(typeEmployee, new List<AbstractEmployee>());
                d_employeeExpenses.Value.Add(typeEmployee, 0);
            }

            var hiredEmployee = IpossibleEmployees.possibleEmployeesInShop[indexEmployee].Clone();
            d_employees.Value[typeEmployee].Add(hiredEmployee);
            d_employeeExpenses.Value[typeEmployee] += hiredEmployee.paymentCostPerDay;

            TypeEmployee tempType = IpossibleEmployees.possibleEmployeesInShop[indexEmployee].typeEmployee;
            Debug.Log($"tempType: {tempType}");

            await Task.Run(() =>
            {
                do
                {
                    IpossibleEmployees.possibleEmployeesInShop[indexEmployee].UpdateOffer(IpossibleEmployees.possibleEmployeesInShop);
                } while (IpossibleEmployees.possibleEmployeesInShop[indexEmployee].typeEmployee == tempType);
            });
            Debug.Log($"tempType: {tempType} / {IpossibleEmployees.possibleEmployeesInShop[indexEmployee].typeEmployee}");
        }

        void IHiringModel.Firing(in byte indexEmployee, in IPossibleEmployees IpossibleEmployees)
        {
            var typeEmployee = IpossibleEmployees.possibleEmployeesInShop[indexEmployee].typeEmployee;

            if (d_employees.Value.ContainsKey(typeEmployee) && d_employees.Value[typeEmployee].IsNotEmpty(indexEmployee))
            {
                d_employeeExpenses.Value[typeEmployee] -= d_employees.Value[typeEmployee][indexEmployee].paymentCostPerDay;
                Debug.Log($"CurrentExpenses: {d_employeeExpenses.Value[typeEmployee]}");
                d_employees.Value[typeEmployee].RemoveAt(indexEmployee);
            }
        }

        Dictionary<TypeEmployee, List<AbstractEmployee>> IHiringModel.GetAllEmployees()
            => d_employees.Value;
    }
}
