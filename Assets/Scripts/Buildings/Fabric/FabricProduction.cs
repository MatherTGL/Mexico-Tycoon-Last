namespace Fabric
{
    public sealed class FabricProduction : IFabricProduction
    {
        void IFabricProduction.ProductionProduct(in float currentFreeProductionKgPerDay, in float maxCapacityStock, ref float productInStock)
        {
            if (productInStock < maxCapacityStock)
            {
                productInStock += currentFreeProductionKgPerDay;
            }
        }
    }
}
