namespace Building.City.Business
{
    public interface ICityBusiness
    {
        void BuyBusiness(in CityBusiness.TypeBusiness typeBusiness);
        void SellBusiness(in ushort indexBusiness);
    }
}
