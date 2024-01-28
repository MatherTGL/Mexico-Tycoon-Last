using Config.Building.Deliveries;
using DebugCustomSystem;
using Resources;
using System;
using System.Collections;
using TimeControl;
using UnityEngine;

namespace Building.City.Deliveries
{
    public sealed class ContractsGenerator : MonoBehaviour, IContractsGenerator
    {
        private IDeliveries _Ideliveries;

        private ILocalMarket _IlocalMarket;

        private ConfigContractsEditor _configContracts;

        private WaitForSeconds _waitForSeconds;

        private WaitForSeconds _contractRenewalTime;


        void IContractsGenerator.Init(in ConfigContractsEditor configContracts)
        {
            _waitForSeconds = new WaitForSeconds(FindAnyObjectByType<TimeDateControl>().GetCurrentTimeOneDay());
            _contractRenewalTime = new WaitForSeconds(configContracts.contractRenewalTime);

            _Ideliveries = GetComponent<IDeliveries>();
            _configContracts = configContracts;
            _IlocalMarket = GetComponent<ILocalMarket>();

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
            int lengthDrugTypes = Enum.GetNames(typeof(TypeProductionResources.TypeResource)).Length;
            int drugType;

            do { drugType = UnityEngine.Random.Range(0, lengthDrugTypes); }
            while (drugType is (int)TypeProductionResources.TypeResource.DirtyMoney);

            individualContractData.resource = (TypeProductionResources.TypeResource)drugType;
            
            if (IsContractAlreadyExists(individualContractData))
                GenerateRandomParameters(ref individualContractData);

            double defaultCostPerKg = _IlocalMarket.GetCurrentCostSellDrug(individualContractData.resource);
            var fastRandom = new FastRandom();

            double costPerKg = defaultCostPerKg + (fastRandom.Range(defaultCostPerKg * _configContracts.minPercentageOfMarketValue / 100,
                defaultCostPerKg * _configContracts.maxPercentageOfMarketValue / 100));

            DebugSystem.Log($"CostPerKg in contract: {costPerKg} / DefaultCost: {defaultCostPerKg}", 
                DebugSystem.SelectedColor.Green, tag: "Contracts");

            individualContractData.remainingContractTime = _configContracts.remainingContractTime;
            individualContractData.costPerKg = costPerKg;
            individualContractData.dailyAllowanceKg = fastRandom.Range(10, 100); //! refactoring
        }

        private bool IsContractAlreadyExists(DataIndividualDeliveries individualContractData)
        {
            for (byte j = 0; j < _Ideliveries.l_deliveriesType.Count; j++)
                if (_Ideliveries.l_deliveriesType[j] is IIndividualDeliveries contract)
                    if (contract.GetResourceBeingSent() == individualContractData.resource)
                        return true;

            return false;
        }

        private IEnumerator TimeUpdate()
        {
            while (true)
            {
                //! refactoring
                for (byte i = 0; i < _Ideliveries.l_deliveriesType.Count; i++)
                {
                    if (_Ideliveries.l_deliveriesType[i] is IIndividualDeliveries contract && 
                        contract.IsContractIsFinalized() == false || _Ideliveries.l_deliveriesType[i] is not IIndividualDeliveries)
                    {
                        _Ideliveries.l_deliveriesType[i].UpdateTime();
                    }
                }

                yield return _waitForSeconds;
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
