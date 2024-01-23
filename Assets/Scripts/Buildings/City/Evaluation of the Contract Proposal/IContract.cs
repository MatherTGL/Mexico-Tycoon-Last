using Resources;
using System.Collections.Generic;

namespace Building.City.Deliveries
{
    public interface IContract
    {
        List<IDeliveriesType> l_deliveriesType { get; }


        void Init(in CostResourcesConfig costResourcesConfig);

        double GetResourceCosts(in TypeProductionResources.TypeResource typeResource);
    }
}
