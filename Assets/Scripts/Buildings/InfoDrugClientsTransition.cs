using System.Collections.Generic;


public sealed class InfoDrugClientsTransition
{
    public Dictionary<string, float> d_typeDrugAndAmountTransition = new Dictionary<string, float>();
    public Dictionary<IPluggableingRoad, IPluggableingRoad[]> d_allClientObjects = new Dictionary<IPluggableingRoad, IPluggableingRoad[]>();
}