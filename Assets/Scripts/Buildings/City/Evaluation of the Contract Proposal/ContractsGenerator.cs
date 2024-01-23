using Config.Building.Deliveries;
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


        void IContractsGenerator.Init(in ConfigContractsEditor configContracts)
        {
            _waitForSeconds = new WaitForSeconds(FindAnyObjectByType<TimeDateControl>().GetCurrentTimeOneDay());
            _Ideliveries = GetComponent<IDeliveries>();
            _configContracts = configContracts;
            _IlocalMarket = GetComponent<ILocalMarket>();

            GenerateContracts();
            StartCoroutine(TimeUpdate());
        }

        private void GenerateContracts()
        {
            DataIndividualDeliveries individualContractData;

            for (byte i = 0; i < _configContracts.maxIndividualContracts; i++)
            {
                individualContractData = new DataIndividualDeliveries();
                GenerateRandomParameters(ref individualContractData);

                if (IsContractAlreadyExists(individualContractData))
                    GenerateRandomParameters(ref individualContractData);

                IDeliveriesType individualContract = new IndividualDeliveries(_configContracts, individualContractData);
                _Ideliveries.AddNewContract(individualContract);
            }
        }

        //! refactoring
        private void GenerateRandomParameters(ref DataIndividualDeliveries individualContractData)
        {
            int lengthDrugTypes = Enum.GetNames(typeof(TypeProductionResources.TypeResource)).Length;
            var drugType = UnityEngine.Random.Range(0, lengthDrugTypes);

            while (drugType is (int)TypeProductionResources.TypeResource.DirtyMoney)
                drugType = UnityEngine.Random.Range(0, lengthDrugTypes);

            var defaultCostPerKg = _IlocalMarket.GetCurrentCostSellDrug(individualContractData.resource);

            var fastRandom = new FastRandom();
            double costPerKg = fastRandom.Range(defaultCostPerKg * _configContracts.minPercentageOfMarketValue / 100,
                defaultCostPerKg * _configContracts.maxPercentageOfMarketValue / 100);

            individualContractData.resource = (TypeProductionResources.TypeResource)drugType;
            individualContractData.costPerKg = costPerKg;

            //! refactoring
            individualContractData.dailyAllowanceKg = fastRandom.Range(10, 100);
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
    }
}
