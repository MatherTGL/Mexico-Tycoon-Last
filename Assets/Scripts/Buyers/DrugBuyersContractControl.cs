using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using Fabric;
using UnityEditor;

internal sealed class DrugBuyersContractControl : MonoBehaviour, ICityDrugBuyers
{
    [ShowInInspector, ReadOnly]
    private Dictionary<string, ContractBuyerInfo> d_contractBuyers = new Dictionary<string, ContractBuyerInfo>();
    Dictionary<string, ContractBuyerInfo> ICityDrugBuyers.d_contractBuyers => d_contractBuyers;

    private Dictionary<string, DrugParametersRandom> d_allParametersDrugs = new Dictionary<string, DrugParametersRandom>();

    [SerializeField, BoxGroup("Parameters")]
    private DrugParametersRandom[] _setDrugsParametersRandom;

    [SerializeField, BoxGroup("Parameters"), Title("Name Adding Contract Buyer", horizontalLine: false), HideLabel]
    private string _nameAddContractBuyer;


#if UNITY_EDITOR
    [Button("Add New Contract")]
    private void AddNewContractBuyer()
    {
        //! хардкод
        if (d_contractBuyers.ContainsKey(_nameAddContractBuyer))
        {
            d_contractBuyers[_nameAddContractBuyer].isCooperation = true;
            var lengthTypesProductionResources = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

            for (int i = 0; i < lengthTypesProductionResources.Length; i++)
            {
                d_contractBuyers[_nameAddContractBuyer].l_drugName.Add(lengthTypesProductionResources[i]);

                uint currentAddingDrugCost = (uint)UnityEngine.Random.Range(_setDrugsParametersRandom[i].GetMinCostDrugParameter(),
                                                                            _setDrugsParametersRandom[i].GetMaxCostDrugParameter());

                Debug.Log(currentAddingDrugCost);
                d_contractBuyers[_nameAddContractBuyer].d_drugCost.Add(lengthTypesProductionResources[i], currentAddingDrugCost);

                d_contractBuyers[_nameAddContractBuyer].d_drugDemand.Add(lengthTypesProductionResources[i], UnityEngine.Random.Range(10, 30));

                d_contractBuyers[_nameAddContractBuyer].d_drugIncreasedDemand.Add(lengthTypesProductionResources[i],
                                                                                  UnityEngine.Random.Range(0.1f, 0.5f)); //!
            }
        }
    }
#endif

    private void Awake()
    {
        var lengthTypesProductionResources = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

        for (int i = 0; i < lengthTypesProductionResources.Length; i++)
            d_allParametersDrugs.Add(lengthTypesProductionResources[i].ToString(), _setDrugsParametersRandom[i]);
    }
}

public sealed class ContractBuyerInfo
{
    public bool isCooperation;
    public List<string> l_drugName = new List<string>();
    public Dictionary<string, uint> d_drugCost = new Dictionary<string, uint>();
    public Dictionary<string, float> d_drugDemand = new Dictionary<string, float>();
    public Dictionary<string, float> d_drugIncreasedDemand = new Dictionary<string, float>();
}
