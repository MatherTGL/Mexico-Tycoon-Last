using Building.City.Business;
using Data;
using UnityEngine;

namespace Business
{
    public sealed class CarWashBusiness : CityBusiness, IBusiness
    {
        void IBusiness.ToLaunderMoney(in double amountDirtyMoney)
        {
            Debug.Log($"Enter to launder money method {amountDirtyMoney}");
            Debug.Log($"Enter to launder money method %: {_percentageMoneyCleared}");
            _clearedMoney = amountDirtyMoney * _percentageMoneyCleared;
            Debug.Log($"Cleared money {_clearedMoney}");
            DataControl.IdataPlayer.AddPlayerMoney(_clearedMoney);
            Debug.Log($"Money clear {_clearedMoney}");
        }
    }
}
