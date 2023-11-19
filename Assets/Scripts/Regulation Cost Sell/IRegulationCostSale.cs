namespace Regulation
{
    public interface IRegulationCostSale
    {
        int Registration(in uint[] resourceCosts);

        uint[] GetResourceCosts(in int cellIndex);
    }
}
