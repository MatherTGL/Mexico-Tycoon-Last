using Building.City.Market;
using Resources;
using static Resources.TypeProductionResources;

namespace Building.City.Deliveries
{
    public interface ILocalMarket
    {
        IProductDemand IproductDemand { get; }


        void Init(in CostResourcesConfig costResourcesConfig, in IBuilding building);

        double GetCurrentCostSellDrug(in TypeResource typeResource);

        double GetCurrentCostBuyDrug(in TypeResource typeResource);
    }
}
