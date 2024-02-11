using UnityEngine;
using Unity.VisualScripting;
using Resources;
using Building.Additional;
using System.Collections;
using TimeControl;
using Building.City.Market;
using static Resources.TypeProductionResources;

namespace Building.City.Deliveries
{
    public sealed class LocalMarketControl : MonoBehaviour, ILocalMarket
    {
        private IBuilding _Ibuilding;

        private IDeliveries _Ideliveries;

        private IContractsGenerator _IcontractsGenerator;

        private ISellResources _IsellResources;

        private IProductDemand _IproductDemand;
        IProductDemand ILocalMarket.IproductDemand => _IproductDemand;

        private WaitForSeconds _IsellWaitForSeconds;

        private uint[] _costSellingResources;

        private uint[] _resourcePurchaseCost;


        private LocalMarketControl() { }

        void ILocalMarket.Init(in CostResourcesConfig costResourcesConfig, in IBuilding building)
        {
            this.AddComponent<DeliveriesControl>();
            this.AddComponent<ContractsGenerator>();

            _costSellingResources = costResourcesConfig.GetCostsSellResources();
            _resourcePurchaseCost = costResourcesConfig.GetCostsBuyResources();

            _Ideliveries = GetComponent<IDeliveries>();
            _IcontractsGenerator = GetComponent<IContractsGenerator>();
            _Ibuilding = building;
            IContract contract = GetComponent<IContract>();

            _IsellResources = new SellResources(_Ideliveries as IContract);
            _IsellWaitForSeconds = new WaitForSeconds(FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay());
            _IproductDemand = new ProductDemand(_Ibuilding as IPotentialConsumers);

            contract.Init(costResourcesConfig);
            _IcontractsGenerator.Init(costResourcesConfig.configForContracts);

            StartCoroutine(TickUpdate());
        }

        private IEnumerator TickUpdate()
        {
            while (true)
            {
                yield return _IsellWaitForSeconds;
                _IsellResources.Sell(_Ibuilding);
            }
        }

        double ILocalMarket.GetCurrentCostSellDrug(in TypeResource typeResource)
            => _costSellingResources[(int)typeResource];

        double ILocalMarket.GetCurrentCostBuyDrug(in TypeResource typeResource)
            => _resourcePurchaseCost[(int)typeResource];
    }
}
