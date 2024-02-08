using static Resources.TypeProductionResources;

namespace Building.City.Market
{
    public interface IProductDemand
    {
        double FormAndGet(in TypeResource typeResource, in float percentageUsers);
    }
}
