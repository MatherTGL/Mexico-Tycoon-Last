using Config.Building.Deliveries;
using Resources;
using UnityEngine;
using static Building.City.Deliveries.Deliveries;

namespace Building.City.Deliveries
{
    public sealed class IndividualDeliveries : IDeliveriesType, IIndividualDeliveries
    {
        private TypeDeliveries _typeDeliveries = TypeDeliveries.Individual;
        TypeDeliveries IDeliveriesType.typeDeliveries => _typeDeliveries;

        private ConfigContractsEditor _config;

        private DataIndividualDeliveries _contractData;

        private int _randomPercentageCost;


        public IndividualDeliveries(in ConfigContractsEditor configDeliveries, in DataIndividualDeliveries contractData)
        {
            _config = configDeliveries;
            _contractData = contractData;
            _randomPercentageCost = Random.Range(_config.minPercentageOfMarketValue, _config.maxPercentageOfMarketValue);

            Debug.Log($"Contract period: {_contractData.remainingContractTime}");
            Debug.Log($"randomPercentageCost: {_randomPercentageCost}");
        }

        double IDeliveriesType.GetResourceCost(in TypeProductionResources.TypeResource _)
        {
            var totalCost = _contractData.costPerKg * _randomPercentageCost / 100;
            Debug.Log($"totalCost sell res: {totalCost}");

            return totalCost;
        }

        void IDeliveriesType.UpdateTime()
        {
            _contractData.remainingContractTime--;
            Debug.Log($"Contract period: {_contractData.remainingContractTime}");
        }

        bool IIndividualDeliveries.IsContractIsFinalized()
        {
            if (_contractData.remainingContractTime <= 0)
                return true;
            return false;
        }

        void IIndividualDeliveries.UpdateContract(in DataIndividualDeliveries contractData)
        {
            _contractData = contractData;
            _randomPercentageCost = Random.Range(_config.minPercentageOfMarketValue, _config.maxPercentageOfMarketValue);

            Debug.Log($"Contract period: {_contractData.remainingContractTime}");
            Debug.Log($"randomPercentageCost: {_randomPercentageCost}");
            Debug.Log("Individual Contract is updated");
        }

        TypeProductionResources.TypeResource IIndividualDeliveries.GetResourceBeingSent() => _contractData.resource;

        double IIndividualDeliveries.GetDailyAllowanceKg() => _contractData.dailyAllowanceKg;
    }
}