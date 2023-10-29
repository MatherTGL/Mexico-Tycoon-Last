using Building.Hire;
using UnityEngine;

namespace Expense.Areas
{
    public sealed class ExpensesOnEmployees : AbstractAreasExpenditure
    {
        private IHiring _Ihiring;


        public ExpensesOnEmployees(in IHiring hiring)
        {
            _Ihiring = hiring;
        }

        public override void ChangeExpenses(in double expenses, in bool isAdd)
        {
            //TODO: Finalize the mechanics
            if (isAdd == true)
                _Ihiring.currentExpenses += expenses;
            else if ((_Ihiring.currentExpenses - expenses) > 0)
                _Ihiring.currentExpenses -= expenses;

            _percentageQuality = Mathf.Clamp((int)(_expenses / 4), 10, 95); //!hardcode
            Debug.Log($"{_expenses} / {_Ihiring.currentExpenses}");
            //base.ChangeExpenses(expenses, isAdd);
        }
    }
}
