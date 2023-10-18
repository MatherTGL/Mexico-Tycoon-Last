using System.Linq;
using Config.Employees;
using UnityEngine;

namespace Hire.Employee
{
    public sealed class Employee : AbstractEmployee
    {
        private const string _pathEmployeesConfigs = "Configs/Employees";


        public Employee()
        {
            try
            {
                config = UnityEngine.Resources.LoadAll<ConfigEmployeeEditor>(_pathEmployeesConfigs).Single();
                LoadData();

                Debug.Log(config);
                Debug.Log(this.config);
            }
            finally
            {
                Debug.Log("employee created");
            }
        }

        private Employee(in AbstractEmployee employee)
        {
            config = employee.config;
            LoadData();
        }

        private void LoadData()
        {
            typeEmployee = config.typeEmployee;
            paymentCostPerDay = config.paymentPerDay;
            rating = config.rating;
            efficiency = config.efficiency;
        }

        public sealed override AbstractEmployee Clone() => new Employee(this);
    }
}
