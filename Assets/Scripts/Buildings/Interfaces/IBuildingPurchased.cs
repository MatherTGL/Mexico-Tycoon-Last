using Country;
using Data;
using DebugCustomSystem;
using Events.Buildings;
using UnityEngine;
using static Data.Player.DataPlayer;

namespace Building.Additional
{
    public interface IBuildingPurchased : IUsesCountryInfo, IUsesWeather, IUsesBuildingsEvents
    {
        bool isBuyed { get; protected set; }

        protected double costPurchase { get; set; }


        void Buy()
        {
            if (!isBuyed && DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(costPurchase, SpendAndCheckMoneyState.Spend))
                isBuyed = true;
        }

        void Sell()
        {
            if (isBuyed)
                isBuyed = false;
        }

        void UpdateCostPurchased()
        {
            if (!isBuyed)
            {
                costPurchase += costPurchase * IcountryBuildings.IcountryInflation.GetTotalInflation() / 100;
                DebugSystem.Log($"Building {this} cost purchase: {costPurchase}", DebugSystem.SelectedColor.Green, tag: "Building");
            }
        }
    }
}
