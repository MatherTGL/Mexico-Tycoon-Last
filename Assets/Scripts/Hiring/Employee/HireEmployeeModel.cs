using System;
using System.Collections.Generic;
using Data;
using Hire.Employee;
using UnityEngine;
using static Config.Employees.ConfigEmployeeEditor;
using static Data.Player.DataPlayer;

namespace Building.Hire
{
    //TODO
    public sealed class HireEmployeeModel : IHiringModel
    {
        private readonly Lazy<Dictionary<TypeEmployee, List<AbstractEmployee>>> d_employees = new();

        private readonly AbstractEmployee[] a_possibleEmployeesInShop = new AbstractEmployee[10];

#if UNITY_EDITOR
        AbstractEmployee[] IHiringModel.a_possibleEmployeesInShop => a_possibleEmployeesInShop;
#endif

        private Lazy<Dictionary<TypeEmployee, double>> d_employeeExpenses = new();
        Lazy<Dictionary<TypeEmployee, double>> IHiringModel.d_employeeExpenses
        { get => d_employeeExpenses; set => d_employeeExpenses = value; }


        public HireEmployeeModel()
        {
            for (byte i = 0; i < a_possibleEmployeesInShop.Length; i++)
                a_possibleEmployeesInShop[i] = new Employee();

            //? CalculateAllExpenses();
        }

        private void Expenses()
        {
            foreach (var typeEmployeeExpenses in d_employeeExpenses.Value.Keys)
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(d_employeeExpenses.Value[typeEmployeeExpenses],
                                                                    SpendAndCheckMoneyState.Spend);
        }

        void IHiringModel.ConstantUpdatingInfo() => Expenses();

        void IHiringModel.Hire(in byte indexEmployee)
        {
            var typeEmployee = a_possibleEmployeesInShop[indexEmployee].typeEmployee;
            Debug.Log($"Type Employee: {typeEmployee}");

            if (d_employees.Value.ContainsKey(typeEmployee) == false)
            {
                d_employees.Value.Add(typeEmployee, new List<AbstractEmployee>());
                d_employeeExpenses.Value.Add(typeEmployee, 0);
            }

            var hiredEmployee = a_possibleEmployeesInShop[indexEmployee].Clone();
            d_employees.Value[typeEmployee].Add(hiredEmployee);
            d_employeeExpenses.Value[typeEmployee] += hiredEmployee.paymentCostPerDay;
            Debug.Log($"CurrentExpenses: {d_employeeExpenses.Value[typeEmployee]}");
        }

        void IHiringModel.Firing(in byte indexEmployee)
        {
            var typeEmployee = a_possibleEmployeesInShop[indexEmployee].typeEmployee;

            if (d_employees.Value.ContainsKey(typeEmployee) && d_employees.Value[typeEmployee].IsNotEmpty(indexEmployee))
            {
                d_employeeExpenses.Value[typeEmployee] -= d_employees.Value[typeEmployee][indexEmployee].paymentCostPerDay;
                Debug.Log($"CurrentExpenses: {d_employeeExpenses.Value[typeEmployee]}");
                d_employees.Value[typeEmployee].RemoveAt(indexEmployee);
            }
        }

        Dictionary<TypeEmployee, List<AbstractEmployee>> IHiringModel.GetAllEmployees() => d_employees.Value;

        void IHiringModel.UpdateAllEmployees()
        {
            for (byte i = 0; i < a_possibleEmployeesInShop.Length; i++)
                a_possibleEmployeesInShop[i].UpdateOffers();
        }
    }
}
