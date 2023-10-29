using System;
using System.Collections.Generic;
using Data;
using Hire.Employee;
using UnityEngine;
using static Config.Employees.ConfigEmployeeEditor;
using static Data.Player.DataPlayer;

namespace Building.Hire
{
    public sealed class HireEmployeeModel : IHiring
    {
        private Lazy<Dictionary<TypeEmployee, List<AbstractEmployee>>> d_employees = new();

        private AbstractEmployee[] l_possibleEmployeesInShop = new AbstractEmployee[10];

        private double _currentExpenses;
        double IHiring.currentExpenses { get => _currentExpenses; set => _currentExpenses = value; }


        public HireEmployeeModel()
        {
            for (byte i = 0; i < l_possibleEmployeesInShop.Length; i++)
                l_possibleEmployeesInShop[i] = new Employee();
            CalculateAllExpenses();
        }

        private void Expenses()
        {
            Debug.Log(_currentExpenses);
            DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(_currentExpenses, SpendAndCheckMoneyState.Spend);
        }

        private void CalculateAllExpenses()
        {
            _currentExpenses = 0;

            foreach (var employee in d_employees.Value.Keys)
                for (byte i = 0; i < d_employees.Value[employee].Count; i++)
                    _currentExpenses += d_employees.Value[employee][i].paymentCostPerDay;
        }

        public void ConstantUpdatingInfo() => Expenses();

        public void Hire(in byte indexEmployee)
        {
            var typeEmployee = l_possibleEmployeesInShop[indexEmployee].typeEmployee;

            if (d_employees.Value.ContainsKey(typeEmployee) == false)
                d_employees.Value.Add(typeEmployee, new List<AbstractEmployee>());

            var hiredEmployee = l_possibleEmployeesInShop[indexEmployee].Clone();
            d_employees.Value[typeEmployee].Add(hiredEmployee);
            _currentExpenses += hiredEmployee.paymentCostPerDay;
            Debug.Log(hiredEmployee.paymentCostPerDay);
            Debug.Log(_currentExpenses);
        }

        public void Firing(in byte indexEmployee)
        {
            var typeEmployee = l_possibleEmployeesInShop[indexEmployee].typeEmployee;

            if (d_employees.Value.ContainsKey(typeEmployee))
            {
                if (d_employees.Value[typeEmployee].IsNotEmpty(indexEmployee))
                {
                    _currentExpenses -= d_employees.Value[typeEmployee][indexEmployee].paymentCostPerDay;
                    Debug.Log(_currentExpenses);
                    d_employees.Value[typeEmployee].RemoveAt(indexEmployee);
                }
            }
        }
    }
}
