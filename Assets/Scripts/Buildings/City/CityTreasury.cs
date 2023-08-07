using UnityEngine;


namespace City
{
    public sealed class CityTreasury
    {
        private double _amountDirtyMoneyTreasury;


        public void AddMoneyToTreasury(double amountDirtyMoney)
        {
            _amountDirtyMoneyTreasury += amountDirtyMoney;
            Debug.Log($"Amount dirty money treasury: {_amountDirtyMoneyTreasury}");
        }

        public void SubtractMoneyFromTreasury(double amountDirtyMoney)
        {
            _amountDirtyMoneyTreasury -= amountDirtyMoney;
            Debug.Log($"Amount dirty money treasury: {_amountDirtyMoneyTreasury}");
        }

        public double LaunderMoney(float percentageMoneyCleared, in double maxAmountMoneyLaundered)
        {
            var amountClearedMoney = Mathf.Clamp((float)(_amountDirtyMoneyTreasury * percentageMoneyCleared), 0.0f, (float)maxAmountMoneyLaundered);

            if (_amountDirtyMoneyTreasury > amountClearedMoney)
            {
                _amountDirtyMoneyTreasury -= amountClearedMoney;
                Debug.Log($"Amount cleared money{amountClearedMoney}");
            }
            return amountClearedMoney;
        }
    }
}
