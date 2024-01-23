namespace Building.City.Deliveries
{
    public interface IDeliveriesCurrentCosts
    {
        uint[] currentCostsSellResources { get; }

        uint[] currentCostBuyResources { get; }
    }
}
