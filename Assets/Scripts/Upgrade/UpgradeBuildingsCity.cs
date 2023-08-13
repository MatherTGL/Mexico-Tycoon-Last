using City.Business;
using UnityEngine;


namespace Upgrade.Buildings
{
    public sealed class UpgradeBuildingsCity : IUpgradeCityBusiness
    {
        private IUpgradableCityBusiness _IupgradableCityBusiness;
        private UpgradeControl _upgradeControl;


        public UpgradeBuildingsCity(IUpgradableCityBusiness IupgradableCityBusiness)
        {
            _IupgradableCityBusiness = IupgradableCityBusiness;
        }

        public void UpgradeBuildingSlots()
        {
            if (Application.isPlaying)
            {
                if (_IupgradableCityBusiness.buildingSlots < _IupgradableCityBusiness.maxBuildingSlots)
                    _IupgradableCityBusiness.buildingSlots++;
            }
            else { Debug.Log("Изменения доступны только в play mode"); }
        }

        public void UpgradeBusinessMaxNumberVisitors(in byte indexBusiness)
        {
            _IupgradableCityBusiness.IbusinessBuilding[indexBusiness].UpgradeMaxNumberVisitors();
        }
    }
}
