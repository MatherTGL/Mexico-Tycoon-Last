using UnityEngine;

namespace Expense.Areas
{
    public interface IAreasExpenditure
    {
        double expenses { get; protected set; }

        int percentageQuality { get; protected set; }


        void ChangeExpenses(in double expenses, in bool isAdd)
        {
            //TODO: refactoring
            if (isAdd == true)
                this.expenses += expenses;
            else if ((this.expenses - expenses) > 0)
                this.expenses -= expenses;

            percentageQuality = Mathf.Clamp((int)(this.expenses / 4), 10, 95); //!
            Debug.Log($"{this.expenses} / {percentageQuality}");
        }
    }
}
