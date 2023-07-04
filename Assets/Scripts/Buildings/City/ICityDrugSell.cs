namespace City
{
    public interface ICityDrugSell
    {
        void Sell(float weightSell, in string contractBuyers, in string typeFabricDrug, in ICityControlSell IcityControlSell);
    }
}
