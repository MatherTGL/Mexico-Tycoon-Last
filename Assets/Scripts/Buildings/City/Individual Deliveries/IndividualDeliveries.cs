using Config.Building.Deliveries;
using DebugCustomSystem;
using Resources;
using UnityEngine;
using static Building.City.Deliveries.DeliveriesControl;

namespace Building.City.Deliveries
{
    public sealed class IndividualDeliveries : IDeliveriesType, IIndividualDeliveries
    {
        private TypeDeliveries _typeDeliveries = TypeDeliveries.Individual;
        TypeDeliveries IDeliveriesType.typeDeliveries => _typeDeliveries;

        private readonly ConfigContractsEditor _config;

        private DataIndividualDeliveries _contractData;

        private bool _isSigned = false;


        public IndividualDeliveries(in ConfigContractsEditor configDeliveries, in DataIndividualDeliveries contractData)
        {
            _config = configDeliveries;
            _contractData = contractData;
        }

        //! refactoring
        double IDeliveriesType.GetResourceCost(in TypeProductionResources.TypeResource drug)
        {
            //var totalCost = _contractData.costPerKg * _randomPercentageCost / 100;
            DebugSystem.Log($"totalCost sell res in object: {this} / Cost: {_contractData.costPerKg} / drug: {_contractData.resource} / {drug}", 
                DebugSystem.SelectedColor.Red, tag: "Deliveries");
            return _contractData.costPerKg;
        }

        void IDeliveriesType.UpdateTime()
        {
            if (_isSigned == false)
                return;

            _contractData.remainingContractTime--;
            Debug.Log($"Contract period: {_contractData.remainingContractTime}");
        }

        bool IIndividualDeliveries.IsContractIsFinalized()
        {
            if (_contractData.remainingContractTime <= 0)
                return true;

            return false;
        }

        void IDeliveriesType.UpdateContract(in DataIndividualDeliveries contractData)
        {
            Debug.Log($"Contract Data before: {_contractData.resource}");
            _contractData = contractData;

            Debug.Log($"Contract Data after: {_contractData.resource}");
            Debug.Log($"Contract period: {_contractData.remainingContractTime}");
            Debug.Log("Individual Contract is updated");
        }

        TypeProductionResources.TypeResource IIndividualDeliveries.GetResourceBeingSent() => _contractData.resource;

        double IIndividualDeliveries.GetDailyAllowanceKg() => _contractData.dailyAllowanceKg;

        void IIndividualDeliveries.SignedContract() => _isSigned = true;

        bool IIndividualDeliveries.GetIsSignedContract() => _isSigned;
    }
}