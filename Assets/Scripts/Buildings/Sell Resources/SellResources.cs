using Building.City.Deliveries;
using DebugCustomSystem;
using Resources;
using System.Linq;

namespace Building.Additional
{
    public sealed class SellResources : ISellResources
    {
        private readonly IContract _Icontract;

        private IIndividualDeliveries _IindividualDeliveries;

        private double _salesProfit = 0;


        public SellResources(in IContract Ideliveries)
            => _Icontract = Ideliveries;

        void ISellResources.Sell(in IBuilding building)
        {
            if (_Icontract == null)
                throw new System.Exception("IContract == null");

            for (int i = _Icontract.l_deliveriesType.Count - 1; i >= 0; i--)
            {
                foreach (var drug in building.amountResources.Keys.ToArray())
                {
                    if (_Icontract.l_deliveriesType[i].typeDeliveries == DeliveriesControl.TypeDeliveries.Individual)
                        IndividualSell(building, i, drug);

                    _salesProfit += building.amountResources[drug] * _Icontract.GetResourceCosts(drug);
                    DebugSystem.Log($"Sales profit in city: {this} / ${_salesProfit} / Contract cost: {_Icontract.GetResourceCosts(drug)}",
                        DebugSystem.SelectedColor.Red, tag: "Deliveries");
                    building.amountResources[drug] = 0;
                }
            }

            building.amountResources[TypeProductionResources.TypeResource.DirtyMoney] += _salesProfit;
            _salesProfit = 0;

            DebugSystem.Log($"DirtyMoney in city ({this}): {building.amountResources[TypeProductionResources.TypeResource.DirtyMoney]}", 
                DebugSystem.SelectedColor.Blue, tag: "City");
        }

        private void IndividualSell(in IBuilding building, in int indexDeliveries, 
                                    in TypeProductionResources.TypeResource drug)
        {
            if (drug == TypeProductionResources.TypeResource.DirtyMoney)
                return;

            _IindividualDeliveries = _Icontract.l_deliveriesType[indexDeliveries] as IIndividualDeliveries;

            if (_IindividualDeliveries.GetResourceBeingSent() != drug || _IindividualDeliveries.IsContractIsFinalized())
                return;

            if (building.amountResources[drug] >= _IindividualDeliveries.GetDailyAllowanceKg())
            {
                _salesProfit += _IindividualDeliveries.GetDailyAllowanceKg() * _Icontract.GetResourceCosts(drug);
                building.amountResources[drug] -= _IindividualDeliveries.GetDailyAllowanceKg();
            }
        }
    }
}
