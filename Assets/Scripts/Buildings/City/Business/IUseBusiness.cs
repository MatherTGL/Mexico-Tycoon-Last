using System;

namespace Building.City.Business
{
    public interface IUseBusiness
    {
        event Action updatedTimeStep;


        void BuyBusiness(in CityBusiness.TypeBusiness typeBusiness);

        void SellBusiness(in ushort indexBusiness);
    }
}
