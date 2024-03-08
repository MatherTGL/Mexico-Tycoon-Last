using Building.Hire;
using UnityEngine;
using static Config.Employees.ConfigEmployeeEditor;

namespace Expense.Areas
{
    public abstract class AbstractAreasExpenditure
    {
        protected IHiringModel _IhiringModel;

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
            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95);
        }

        public virtual void ChangeSeasonExpenses(in double expenses)
            => _expenses = _baseExpenses + expenses;

        public void InitHiring(in IHiringModel IhiringModel)
            => _IhiringModel = IhiringModel;

        public virtual void ChangeEmployeesExpenses(in double expenses, in bool isAdd, in TypeEmployee typeEmployee)
        {
            Debug.Log($"AAE typeEmployee: {typeEmployee}");
            if (_IhiringModel == null || _IhiringModel.d_employeeExpenses.Value.ContainsKey(typeEmployee) == false)
                return;

            if (_IhiringModel.d_employeeExpenses.Value[typeEmployee] <= 0)
                return;

            if (isAdd == true)
                _IhiringModel.d_employeeExpenses.Value[typeEmployee] += expenses;
            else if ((_IhiringModel.d_employeeExpenses.Value[typeEmployee] - expenses) > 0)
                _IhiringModel.d_employeeExpenses.Value[typeEmployee] -= expenses;
            Debug.Log($"AAE expenses Employee: {typeEmployee} / {_IhiringModel.d_employeeExpenses.Value[typeEmployee]}");

            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!hardcode
            Debug.Log($"{_expenses} / {_IhiringModel.d_employeeExpenses.Value[typeEmployee]}");
        }
    }
}
