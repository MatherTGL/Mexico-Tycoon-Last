using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using City;
using Boot;
using Fabric;
using System;

public sealed class DrugBuyersContractControl : MonoBehaviour, ICityDrugBuyers//, IBoot
{
    private IContractedCity _IcontractedCity;

    [ShowInInspector, BoxGroup("Parameters")]
    private Dictionary<string, bool> d_contractContactAndDrug = new Dictionary<string, bool>(); //! название клана и активно ли сотрудничество
    Dictionary<string, bool> ICityDrugBuyers.d_contractContactAndDrug => d_contractContactAndDrug;

    [ShowInInspector, BoxGroup("Parameters")]
    private Dictionary<string, float> d_contractDrugsCityCostSell = new Dictionary<string, float>(); //! название наркотика и стоимость за кг
    Dictionary<string, float> ICityDrugBuyers.d_contractDrugsCityCostSell => d_contractDrugsCityCostSell;

    [ShowInInspector, BoxGroup("Parameters")]
    private Dictionary<string, float> d_contractDrugsCityDemand = new Dictionary<string, float>(); //! название наркотика и спрос
    Dictionary<string, float> ICityDrugBuyers.d_contractDrugsCityDemand => d_contractDrugsCityDemand;

    [SerializeField, BoxGroup("Parameters"), Title("Name Adding Contract Buyer", horizontalLine: false), HideLabel]
    private string _nameAddContractBuyer;


#if UNITY_EDITOR
    [Button("Add New Contract")]
    private void AddNewContractBuyer()
    {
        if (d_contractContactAndDrug.ContainsKey(_nameAddContractBuyer))
        {
            d_contractContactAndDrug[_nameAddContractBuyer] = true;
        }
    }
#endif

    public void Awake()
    {
        var lenghtTypesProductionResources = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

        for (int i = 0; i < lenghtTypesProductionResources.Length; i++)
        {
            if (d_contractDrugsCityDemand.ContainsKey(lenghtTypesProductionResources[i]) is false)
                d_contractDrugsCityDemand.Add(lenghtTypesProductionResources[i], UnityEngine.Random.Range(10, 30)); //! хардкод
        }

    }
}