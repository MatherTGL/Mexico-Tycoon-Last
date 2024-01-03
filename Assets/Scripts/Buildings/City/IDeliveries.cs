using Config.Building.Deliveries;
using Resources;
using System.Collections.Generic;

namespace Building.City.Deliveries
{
    public interface IDeliveries
    {
        List<IDeliveriesType> l_deliveriesType { get; }


        void Init(in ConfigDeliveriesEditor config, in CostResourcesConfig costResourcesConfig);

        double GetResourceCosts(in TypeProductionResources.TypeResource typeResource);
    }
}
