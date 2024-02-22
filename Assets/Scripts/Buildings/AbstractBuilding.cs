using System.Collections.Generic;
using Expense;
using static Resources.TypeProductionResources;

namespace Building
{
    public abstract class AbstractBuilding
    {
        protected IObjectsExpensesImplementation IobjectsExpensesImplementation;

        protected Dictionary<TypeResource, double> d_amountResources = new();

        protected Dictionary<TypeResource, uint> d_stockCapacity = new();

        protected bool isWorked;

        protected bool isBuyed;
    }
}
