using UnityEngine;

namespace Expense.Areas
{
    public abstract class AbstractAreasExpenditure
    {
        protected double _expenses;
        public double expenses => _expenses;

        protected int _percentageQuality;


        public virtual void ChangeExpenses(in double expenses, in bool isAdd)
        {
            //TODO: refactoring
            if (isAdd == true)
                _expenses += expenses;
            else if ((_expenses - expenses) > 0)
                _expenses -= expenses;

            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!
            Debug.Log($"{_expenses} / {_percentageQuality}");
        }
    }
}
