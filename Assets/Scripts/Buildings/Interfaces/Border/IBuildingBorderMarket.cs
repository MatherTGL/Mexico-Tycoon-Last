using Resources;


namespace Building.Border
{
    public interface IBuildingBorderMarket
    {
        bool CheckResourceInSale(in TypeProductionResources.TypeResource typeResource);
    }
}
