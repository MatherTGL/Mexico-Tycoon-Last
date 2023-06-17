namespace Fabric
{
    public interface IFabricProduction
    {
        void UpdateProductionParameters(float productQuality,
                                     float productivityKgPerDay,
                                     float productInStock,
                                     float maxCapacityStock);
        void ProductionProduct();
        float GetProductInStock();
    }
}
