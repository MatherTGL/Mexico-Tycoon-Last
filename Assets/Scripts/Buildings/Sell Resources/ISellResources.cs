using Resources;
using System.Collections.Generic;

namespace Building.Additional
{
    public interface ISellResources
    {
        void Sell(ref Dictionary<TypeProductionResources.TypeResource, double> amountResources);
    }
}
