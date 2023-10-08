using System.Collections.Generic;
using Expense;
using Resources;

namespace Building
{
    public abstract class AbstractBuilding
    {
        protected IObjectsExpensesImplementation _IobjectsExpensesImplementation { get; set; }

        protected Dictionary<TypeProductionResources.TypeResource, double> d_amountResources { get; set; } = new();

        protected Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity { get; set; } = new();

        protected bool _isWorked { get; set; }

        protected bool _isBuyed { get; set; }
    }
}
