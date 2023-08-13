using UnityEngine;


namespace City.Business
{
    public interface IUpgradableCityBusiness
    {
        byte buildingSlots { get; set; }
        byte maxBuildingSlots { get; set; }
        IBuisinessBuilding[] IbusinessBuilding { get; }
    }
}
