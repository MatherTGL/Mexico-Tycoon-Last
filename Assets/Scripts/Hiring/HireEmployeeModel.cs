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

        private Lazy<Dictionary<TypeEmployee, double>> d_employeeExpenses = new();
        Lazy<Dictionary<TypeEmployee, double>> IHiring.d_employeeExpenses
        {
            get => d_employeeExpenses; set => d_employeeExpenses = value;
        }


        public HireEmployeeModel()
        {
            for (byte i = 0; i < l_possibleEmployeesInShop.Length; i++)
                l_possibleEmployeesInShop[i] = new Employee();

            //? CalculateAllExpenses();
        }

        private void Expenses()
        {
            foreach (var typeEmployeeExpenses in d_employeeExpenses.Value.Keys)
            {
                Debug.Log(d_employeeExpenses.Value[typeEmployeeExpenses]);
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(d_employeeExpenses.Value[typeEmployeeExpenses],
                                                                    SpendAndCheckMoneyState.Spend);
            }
        }

        // private void CalculateAllExpenses()
        // {
        //     _currentExpenses = 0;

        //     foreach (var employee in d_employees.Value.Keys)
        //         for (byte i = 0; i < d_employees.Value[employee].Count; i++)
        //             _currentExpenses += d_employees.Value[employee][i].paymentCostPerDay;
        // }

        public void ConstantUpdatingInfo() => Expenses();

        public void Hire(in byte indexEmployee)
        {
            var typeEmployee = l_possibleEmployeesInShop[indexEmployee].typeEmployee;
            Debug.Log($"Type Employee: {typeEmployee}");

            if (d_employees.Value.ContainsKey(typeEmployee) == false)
            {
                d_employees.Value.Add(typeEmployee, new List<AbstractEmployee>());
                d_employeeExpenses.Value.Add(typeEmployee, 0);
            }

            var hiredEmployee = l_possibleEmployeesInShop[indexEmployee].Clone();
            d_employees.Value[typeEmployee].Add(hiredEmployee);
            d_employeeExpenses.Value[typeEmployee] += hiredEmployee.paymentCostPerDay;
            Debug.Log($"CurrentExpenses: {d_employeeExpenses.Value[typeEmployee]}");
        }

        public void Firing(in byte indexEmployee)
        {
            var typeEmployee = l_possibleEmployeesInShop[indexEmployee].typeEmployee;

            if (d_employees.Value.ContainsKey(typeEmployee) && d_employees.Value[typeEmployee].IsNotEmpty(indexEmployee))
            {
                d_employeeExpenses.Value[typeEmployee] -= d_employees.Value[typeEmployee][indexEmployee].paymentCostPerDay;
                Debug.Log($"CurrentExpenses: {d_employeeExpenses.Value[typeEmployee]}");
                d_employees.Value[typeEmployee].RemoveAt(indexEmployee);
            }
        }
    }
}
