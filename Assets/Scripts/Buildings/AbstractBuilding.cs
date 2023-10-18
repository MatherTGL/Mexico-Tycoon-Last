using System.Collections.Generic;
using Expense;
using Resources;

namespace Building
{
    public abstract class AbstractBuilding
    {
        protected IObjectsExpensesImplementation IobjectsExpensesImplementation;

        protected Dictionary<TypeProductionResources.TypeResource, double> d_amountResources = new();

        protected Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new();

        protected bool isWorked;

        protected bool isBuyed;
    }
}
