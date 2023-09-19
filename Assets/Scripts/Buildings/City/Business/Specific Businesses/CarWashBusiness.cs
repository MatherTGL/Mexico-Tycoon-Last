using Building.City.Business;
using Data;
using UnityEngine;

namespace Business
{
    public sealed class CarWashBusiness : CityBusiness, IBusiness
    {
        void IBusiness.ToLaunderMoney(in double amountDirtyMoney)
        {
            _clearedMoney = amountDirtyMoney * _percentageMoneyCleared;
            DataControl.IdataPlayer.AddPlayerMoney(_clearedMoney);
            Debug.Log($"Money clear {_clearedMoney}");
        }
    }
}
