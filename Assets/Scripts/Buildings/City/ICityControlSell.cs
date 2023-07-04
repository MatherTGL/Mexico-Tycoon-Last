using System.Collections.Generic;


namespace City
{
    public interface ICityControlSell
    {
        ICityDrugBuyers _IcityDrugBuyers { get; }
        Dictionary<string, float> d_amountDrugsInCity { get; }
        Dictionary<string, float> d_weightToSellDrugs { get; }
        float buyersDrugsClampDemandMin { get; }
        float buyersDrugsClampDemandMax { get; }
    }
}
