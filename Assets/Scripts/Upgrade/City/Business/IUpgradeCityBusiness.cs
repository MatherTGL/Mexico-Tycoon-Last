using UnityEngine;


public interface IUpgradeCityBusiness
{
    void UpgradeBuildingSlots();
    void UpgradeBusinessMaxNumberVisitors(in byte indexBusiness);
}