using Resources;

namespace Building.City.Deliveries
{
    public interface ILocalMarket
    {
        void Init(in CostResourcesConfig costResourcesConfig, in IBuilding building);

        double GetCurrentCostSellDrug(in TypeProductionResources.TypeResource typeResource);

        double GetCurrentCostBuyDrug(in TypeProductionResources.TypeResource typeResource);
    }
}
