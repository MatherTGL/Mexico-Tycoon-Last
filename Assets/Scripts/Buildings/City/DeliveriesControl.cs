using DebugCustomSystem;
using Resources;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Building.City.Deliveries
{
    public sealed class DeliveriesControl : MonoBehaviour, IDeliveries, IDeliveriesCurrentCosts, IContract
    {
        private readonly List<IDeliveriesType> l_deliveriesType = new();
        List<IDeliveriesType> IDeliveries.l_deliveriesType => l_deliveriesType;
        List<IDeliveriesType> IContract.l_deliveriesType => l_deliveriesType;

        private uint[] _currentCostsSellResources;
        uint[] IDeliveriesCurrentCosts.currentCostsSellResources => _currentCostsSellResources;

        private uint[] _currentCostBuyResources;
        uint[] IDeliveriesCurrentCosts.currentCostBuyResources => _currentCostBuyResources;

        public enum TypeDeliveries : byte { General, Individual }


        private DeliveriesControl() { }

        void IContract.Init(in CostResourcesConfig costResourcesConfig)
        {
            _currentCostsSellResources = costResourcesConfig.GetCostsSellResources();
            _currentCostBuyResources = costResourcesConfig.GetCostsBuyResources();

            l_deliveriesType.Add(new GeneralDeliveries(this));
        }

        [Button("Sign a Contract")]
        private void SignContract(int indexContract)
        {
            if (l_deliveriesType.IsNotEmpty(indexContract) == false)
                return;

            IIndividualDeliveries individualDeliveries = l_deliveriesType[indexContract] as IIndividualDeliveries;

            if (individualDeliveries == null || individualDeliveries.IsSignedContract())
                return;

            individualDeliveries.SignedContract();
        }

#if UNITY_EDITOR
        [Button("Get Contracts")]
        private void GetAllContracts()
        {
            for (byte i = 0; i < l_deliveriesType.Count; i++)
            {
                if (l_deliveriesType[i].typeDeliveries is TypeDeliveries.Individual)
                {
                    var contract = l_deliveriesType[i] as IIndividualDeliveries;
                    DebugSystem.Log($"Object: {this} Index: {i} Contract Type: {l_deliveriesType[i].typeDeliveries} / Res: {contract.GetResourceBeingSent()} / Cost: {l_deliveriesType[i].GetResourceCost(contract.GetResourceBeingSent())}",
                        DebugSystem.SelectedColor.Green, tag: "Deliveries");
                }
            }
        }
#endif

        void IDeliveries.AddNewContract(in IDeliveriesType deliveriesType)
        {
            if (deliveriesType != null)
                l_deliveriesType.Add(deliveriesType);
        }

        double IContract.GetResourceCosts(in TypeProductionResources.TypeResource typeResource)
            => l_deliveriesType[^1].GetResourceCost(typeResource);

        void IDeliveries.UpdateContract(in DataIndividualDeliveries dataIndividualDeliveries, in byte index)
            => l_deliveriesType[index].UpdateContract(dataIndividualDeliveries);
    }
}
