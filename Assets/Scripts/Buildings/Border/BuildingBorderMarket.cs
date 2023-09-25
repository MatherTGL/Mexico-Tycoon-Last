using Data;
using Resources;
using System;
using Config.Building;

namespace Building.Border
{
    public sealed class BuildingBorderMarket : IBuildingBorderMarket
    {
        private uint[] _costBuyPerKg, _costSellPerKg;

        private bool[] _isSale;

        private byte _indexTypeDrug;


        public BuildingBorderMarket(in ConfigBuildingBorderEditor config) => InitArrays(config);

        private void InitArrays(in ConfigBuildingBorderEditor config)
        {
            int amountTypeDrugs = Enum.GetValues(typeof(TypeProductionResources.TypeResource)).Length;

            _isSale = new bool[amountTypeDrugs];
            _costBuyPerKg = new uint[amountTypeDrugs];
            _costSellPerKg = new uint[amountTypeDrugs];

            _costBuyPerKg = config.costResourcesConfig.GetCostsBuyResources();
            _costSellPerKg = config.costResourcesConfig.GetCostsSellResources();

            for (byte i = 0; i < _isSale.Length; i++)
                _isSale[i] = true;
        }

        bool IBuildingBorderMarket.CalculateBuyCost(in TypeProductionResources.TypeResource typeResource,
                                                    in float amount)
        {
            _indexTypeDrug = (byte)typeResource;
            double productPurchaseCost = amount * _costBuyPerKg[_indexTypeDrug];

            if (_isSale[_indexTypeDrug])
            {
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(productPurchaseCost, true);
                return true;
            }
            else { return false; }
        }

        void IBuildingBorderMarket.SellResources(in TypeProductionResources.TypeResource typeResource,
                                                 in float amount)
        {
            double salesProfit = amount * _costSellPerKg[(int)typeResource];
            DataControl.IdataPlayer.AddPlayerMoney(salesProfit);
        }
    }
}
