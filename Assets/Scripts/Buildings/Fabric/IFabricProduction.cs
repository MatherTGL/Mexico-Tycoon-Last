namespace Fabric
{
    public interface IFabricProduction
    {
        void ProductionProduct(in float currentFreeProductionKgPerDay,
                               in float maxCapacityStock,
                               ref float productInStock);
    }
}
