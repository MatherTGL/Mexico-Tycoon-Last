namespace Upgrade.Buildings.Fabric
{
    public interface IUpgradableFabric
    {
        float productQualityLocalMax { get; set; }
        float maxCapacityStock { get; set; }
        float productivityKgPerDay { get; set; }
        float currentFreeProductionKgPerDay { get; set; }
    }
}
