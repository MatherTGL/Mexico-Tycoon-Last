namespace Upgrade.Buildings.Fabric
{
    public interface IUpgradableFabric
    {
        float productQuality { get; set; }
        float maxCapacityStock { get; set; }
        float productivityKgPerDay { get; set; }
        float currentFreeProductionKgPerDay { get; set; }
    }
}
