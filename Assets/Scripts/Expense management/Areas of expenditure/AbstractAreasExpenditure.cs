using Building.Hire;
using UnityEngine;
using static Config.Employees.ConfigEmployeeEditor;

namespace Expense.Areas
{
    public abstract class AbstractAreasExpenditure
    {
        protected IHiring _Ihiring;

        protected double _baseExpenses;

        protected double _expenses;
        public double expenses => _expenses;

        protected int _percentageQuality;


        public virtual void ChangeExpenses(in double expenses, in bool isAdd)
        {
            if (isAdd == true)
                _expenses += expenses;
            else if ((_expenses - expenses) > 0)
                _expenses -= expenses;

            _baseExpenses = _expenses;
            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!
        }

        public virtual void ChangeSeasonExpenses(in double expenses)
            => _expenses = _baseExpenses + expenses;

        public void InitHiring(in IHiring Ihiring)
        {
            _Ihiring = Ihiring;
        }

        public virtual void ChangeEmployeesExpenses(in double expenses, in bool isAdd, in TypeEmployee typeEmployee)
        {
            Debug.Log($"AAE typeEmployee: {typeEmployee}");
            if (_Ihiring == null || _Ihiring.d_employeeExpenses.Value.ContainsKey(typeEmployee) == false)
                return;

            if (_Ihiring.d_employeeExpenses.Value[typeEmployee] <= 0)
                return;

            if (isAdd == true)
                _Ihiring.d_employeeExpenses.Value[typeEmployee] += expenses;
            else if ((_Ihiring.d_employeeExpenses.Value[typeEmployee] - expenses) > 0)
                _Ihiring.d_employeeExpenses.Value[typeEmployee] -= expenses;
            Debug.Log($"AAE expenses Employee: {typeEmployee} / {_Ihiring.d_employeeExpenses.Value[typeEmployee]}");

            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!hardcode
            Debug.Log($"{_expenses} / {_Ihiring.d_employeeExpenses.Value[typeEmployee]}");
        }
    }
}
