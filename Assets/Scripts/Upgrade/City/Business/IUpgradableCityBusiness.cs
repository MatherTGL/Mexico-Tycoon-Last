namespace City.Business
{
    public interface IUpgradableCityBusiness
    {
        byte maxBuildingSlots { get; set; }
        IBuisinessBuilding[] IbusinessBuilding { get; }
        byte CalculationNumberSlots();
    }
}
