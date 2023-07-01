using System.Collections.Generic;


public interface ICityDrugBuyers
{
    Dictionary<string, bool> d_contractContactAndDrug { get; }
    Dictionary<string, float> d_contractDrugsCityCostSell { get; }
    Dictionary<string, float> d_contractDrugsCityDemand { get; }
}