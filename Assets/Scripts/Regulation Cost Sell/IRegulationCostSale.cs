using Resources;

namespace Regulation
{
    public interface IRegulationCostSale
    {
        int Registration(in uint[] resourceCosts);

        uint[] GetResourceCosts(in int cellIndex);

        void SetResourcesCosts(in int cellIndex, in TypeProductionResources.TypeResource typeResource,
                               in uint newCost);
    }
}
