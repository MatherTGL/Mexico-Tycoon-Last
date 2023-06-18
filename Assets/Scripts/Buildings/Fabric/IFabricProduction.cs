namespace Fabric
{
    public interface IFabricProduction
    {
        void ProductionProduct(in float productivityKgPerDay, in float maxCapacityStock, ref float productInStock);
    }
}
