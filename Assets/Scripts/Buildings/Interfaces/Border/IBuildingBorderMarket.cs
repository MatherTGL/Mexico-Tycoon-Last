using Resources;


namespace Building.Border
{
    public interface IBuildingBorderMarket
    {
        bool CalculateBuyCost(in TypeProductionResources.TypeResource typeResource, in float amount);
        void SellResources(in TypeProductionResources.TypeResource typeResource, in float amount);
    }
}
