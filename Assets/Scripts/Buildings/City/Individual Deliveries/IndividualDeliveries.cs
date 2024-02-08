using Config.Building.Deliveries;
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

        double IDeliveriesType.GetResourceCost(in TypeProductionResources.TypeResource drug)
            => _contractData.costPerKg;

        void IDeliveriesType.UpdateTime()
        {
            if (_isSigned == false)
                return;

            _contractData.remainingContractTime--;
            Debug.Log($"contractData.remainingContractTime: {_contractData.remainingContractTime} / isSigned: {_isSigned}");
        }

        bool IIndividualDeliveries.IsContractIsFinalized()
        {
            if (_contractData.remainingContractTime <= 0)
                return true;

            return false;
        }

        void IDeliveriesType.UpdateContract(in DataIndividualDeliveries contractData)
            => _contractData = contractData;

        TypeProductionResources.TypeResource IIndividualDeliveries.GetResourceBeingSent() 
            => _contractData.resource;

        double IIndividualDeliveries.GetDailyAllowanceKg() => _contractData.dailyAllowanceKg;

        void IIndividualDeliveries.SignedContract() => _isSigned = true;

        bool IIndividualDeliveries.IsSignedContract() => _isSigned;
    }
}