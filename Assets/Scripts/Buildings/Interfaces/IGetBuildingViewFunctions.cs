using static Building.City.Business.CityBusiness;
using static Config.Building.ConfigBuildingFarmEditor;
using static Resources.TypeProductionResources;

namespace Building
{
    public interface IGetBuildingViewFunctions
    {
        IBuilding GetBuilding();

        void BuyBuilding();

        void SellBuilding();

        void SetActivateBuilding();

        void SetDeactivateBuilding();

        void SetNewProductionResource(TypeResource typeResource);

        void ChangeFarmType(in TypeFarm typeFarm);

        void BuyBusiness(in TypeBusiness typeBusiness);

        void SellBusiness(in ushort indexBusiness);
    }
}
