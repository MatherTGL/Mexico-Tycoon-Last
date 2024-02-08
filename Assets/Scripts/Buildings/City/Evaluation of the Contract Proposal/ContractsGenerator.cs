using Config.Building.Deliveries;
using Mono.Cecil;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeControl;
using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building.City.Deliveries
{
    public sealed class ContractsGenerator : MonoBehaviour, IContractsGenerator
    {
        private IDeliveries _Ideliveries;

        private ILocalMarket _IlocalMarket;

        private ConfigContractsEditor _configContracts;

        private WaitForSeconds _mainUpdateTime;

        private WaitForSeconds _contractRenewalTime;

        private readonly Dictionary<TypeResource, float> d_percentageUsersForIndividualContracts = new();


        void IContractsGenerator.Init(in ConfigContractsEditor configContracts)
        {
            _mainUpdateTime = new WaitForSeconds(FindAnyObjectByType<TimeDateControl>().GetCurrentTimeOneDay());
            _contractRenewalTime = new WaitForSeconds(configContracts.contractRenewalTime);

            _Ideliveries = GetComponent<IDeliveries>();
            _configContracts = configContracts;
            _IlocalMarket = GetComponent<ILocalMarket>();

            foreach (TypeResource resource in Enum.GetValues(typeof(TypeResource)))
                d_percentageUsersForIndividualContracts.TryAdd(resource, 0);

            GenerateContracts();

            StartCoroutine(TimeUpdate());
            StartCoroutine(UpdateContracts());
        }

        private void GenerateContracts()
        {
            DataIndividualDeliveries individualContractData;

            for (byte i = 0; i < _configContracts.maxIndividualContracts; i++)
            {
                individualContractData = new DataIndividualDeliveries();
                GenerateRandomParameters(ref individualContractData);

                IDeliveriesType individualContract = new IndividualDeliveries(_configContracts, individualContractData);
                _Ideliveries.AddNewContract(individualContract);
            }
        }

        //! refactoring
        private void GenerateRandomParameters(ref DataIndividualDeliveries individualContractData)
        {
            int lengthDrugTypes = Enum.GetNames(typeof(TypeResource)).Length;
            int drugType;

            do { drugType = UnityEngine.Random.Range(0, lengthDrugTypes); }
            while (drugType is (int)TypeResource.DirtyMoney);

            individualContractData.resource = (TypeResource)drugType;
            
            if (IsContractAlreadyExists(individualContractData))
                GenerateRandomParameters(ref individualContractData);

            double defaultCostPerKg = _IlocalMarket.GetCurrentCostSellDrug(individualContractData.resource);
            var fastRandom = new FastRandom();

            double costPerKg = defaultCostPerKg + (fastRandom.Range(defaultCostPerKg * _configContracts.minPercentageOfMarketValue / 100,
                defaultCostPerKg * _configContracts.maxPercentageOfMarketValue / 100));

            individualContractData.remainingContractTime = _configContracts.remainingContractTime;
            individualContractData.costPerKg = costPerKg;
            individualContractData.dailyAllowanceKg = _IlocalMarket.IproductDemand.FormAndGet(individualContractData.resource,
                FormAndGetPercentageUsers(individualContractData.resource));
        }

        private bool IsContractAlreadyExists(in DataIndividualDeliveries individualContractData)
        {
            for (byte j = 0; j < _Ideliveries.l_deliveriesType.Count; j++)
                if (_Ideliveries.l_deliveriesType[j] is IIndividualDeliveries contract)
                    if (contract.GetResourceBeingSent() == individualContractData.resource)
                        return true;

            return false;
        }

        private float FormAndGetPercentageUsers(in TypeResource typeResource)
        {
            d_percentageUsersForIndividualContracts[typeResource] = UnityEngine.Random.Range(_configContracts.percentageUsers.Dictionary[typeResource][0],
                                                                                             _configContracts.percentageUsers.Dictionary[typeResource][1]) / 100;
            return d_percentageUsersForIndividualContracts[typeResource];
        }

        private IEnumerator TimeUpdate()
        {
            while (true)
            {
                yield return _mainUpdateTime;

                //! refactoring
                for (byte i = 0; i < _Ideliveries.l_deliveriesType.Count; i++)
                {
                    if (_Ideliveries.l_deliveriesType[i] is IIndividualDeliveries)
                        continue;

                    if (_Ideliveries.l_deliveriesType[i] is IIndividualDeliveries contract && 
                        contract.IsContractIsFinalized() == false)
                    {
                        _Ideliveries.l_deliveriesType[i].UpdateTime();
                    }
                }
            }
        }

        //! refactoring
        private IEnumerator UpdateContracts()
        {
            while (true)
            {
                yield return _contractRenewalTime;

                for (byte index = 0; index < _Ideliveries.l_deliveriesType.Count; index++)
                    UpdateContract(index);
            }
        }

        private void UpdateContract(in byte index)
        {
            if (_Ideliveries.l_deliveriesType[index].typeDeliveries is DeliveriesControl.TypeDeliveries.General)
                return;

            DataIndividualDeliveries individualDeliveries = new();
            GenerateRandomParameters(ref individualDeliveries);
            _Ideliveries.UpdateContract(individualDeliveries, index);
        }
    }
}
