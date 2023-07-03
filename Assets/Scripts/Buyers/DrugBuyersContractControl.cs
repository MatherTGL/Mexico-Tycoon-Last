using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Fabric;
using System;


internal sealed class DrugBuyersContractControl : MonoBehaviour, ICityDrugBuyers//, IBoot
{
    [ShowInInspector]
    private Dictionary<string, ContractBuyerInfo> d_contractBuyers = new Dictionary<string, ContractBuyerInfo>();
    Dictionary<string, ContractBuyerInfo> ICityDrugBuyers.d_contractBuyers => d_contractBuyers;

    [SerializeField, BoxGroup("Parameters"), Title("Name Adding Contract Buyer", horizontalLine: false), HideLabel]
    private string _nameAddContractBuyer;



#if UNITY_EDITOR
    [Button("Add New Contract")]
    private void AddNewContractBuyer()
    {
        if (d_contractBuyers.ContainsKey(_nameAddContractBuyer))
        {
            Debug.Log(d_contractBuyers[_nameAddContractBuyer]);
            d_contractBuyers[_nameAddContractBuyer].isCooperation = true;
            Debug.Log(d_contractBuyers[_nameAddContractBuyer].isCooperation);

            var lenghtTypesProductionResources = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

            for (int i = 0; i < lenghtTypesProductionResources.Length; i++)
            {
                d_contractBuyers[_nameAddContractBuyer].drugName.Add(lenghtTypesProductionResources[i]);
                d_contractBuyers[_nameAddContractBuyer].d_drugCost.Add(lenghtTypesProductionResources[i], ((uint)UnityEngine.Random.Range(15000, 25000)));
                d_contractBuyers[_nameAddContractBuyer].d_drugDemand.Add(lenghtTypesProductionResources[i], UnityEngine.Random.Range(10, 30));
                d_contractBuyers[_nameAddContractBuyer].d_drugIncreasedDemand.Add(lenghtTypesProductionResources[i], UnityEngine.Random.Range(0.1f, 0.5f));
            }
        }
    }
#endif
}

public class ContractBuyerInfo
{
    public bool isCooperation;
    public List<string> drugName = new List<string>();
    public Dictionary<string, uint> d_drugCost = new Dictionary<string, uint>();
    public Dictionary<string, float> d_drugDemand = new Dictionary<string, float>();
    public Dictionary<string, float> d_drugIncreasedDemand = new Dictionary<string, float>();
}
