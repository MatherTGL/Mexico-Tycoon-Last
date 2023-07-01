using Upgrade.Buildings.Fabric;


namespace Upgrade.Buildings
{
    public sealed class UpgradeBuildingsFactory : IUpgradeBuildingsFabric
    {
        private IUpgradableFabric _IupgradableFabric;
        private UpgradeControl _upgradeControl;


        public UpgradeBuildingsFactory(IUpgradableFabric IupgradableFabric, in UpgradeControl upgradeControl)
        {
            _IupgradableFabric = IupgradableFabric;
            _upgradeControl = upgradeControl;
        }

        public void UpgradeProductQuality()
        {
            if (_upgradeControl.isEqually)
                _IupgradableFabric.productQualityLocalMax = _upgradeControl.amount;
            else
                _IupgradableFabric.productQualityLocalMax += _upgradeControl.amount;
        }

        public void UpgradeMaxCapacityStock()
        {
            if (_upgradeControl.isEqually)
                _IupgradableFabric.maxCapacityStock = _upgradeControl.amount;
            else
                _IupgradableFabric.maxCapacityStock += _upgradeControl.amount;
        }

        public void ProductivityKgPerDay()
        {
            _IupgradableFabric.productivityKgPerDay += _upgradeControl.amount;
            _IupgradableFabric.currentFreeProductionKgPerDay += _upgradeControl.amount;
        }
    }
}
