using UnityEngine;
using Upgrade.Buildings;
using Sirenix.OdinInspector;
using Boot;
using Fabric;
using Upgrade.Buildings.Fabric;
using City.Business;
using City;

namespace Upgrade
{
    public sealed class UpgradeControl : MonoBehaviour, IBoot
    {
        private IUpgradeBuildingsFabric _IupgradeBuildingsFabric;

        private IUpgradableFabric _IupgradableFabric;

        private IUpgradableCityBusiness _IupgradableCityBusiness;

        //private IUpgradeBuildingsCity

        private enum TypeUpgradableObject
        {
            Fabric, City
        }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeUpgradableObject _typeUpgradableObject;


#if UNITY_EDITOR
        private enum TypeUpgrade
        {
            ProductQuality, MaxCapacityStock, ProductivityKgPerDay
        }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, ShowIf("_typeUpgradableObject", TypeUpgradableObject.Fabric)]
        [HideLabel]
        private TypeUpgrade _typeUpgrade;
#endif


        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f), Title("amount"), HideLabel]
        [PropertySpace(10, 10), HorizontalGroup("Parameters/Hor", marginRight: 10)]
        private float _amount;
        public float amount => _amount;

        [SerializeField, BoxGroup("Parameters"), Title("Equally or Add (false = Equally)")]
        [DisableIf("@_typeUpgrade == TypeUpgrade.ProductivityKgPerDay"), PropertySpace(10, 10), HideLabel]
        [HorizontalGroup("Parameters/Hor", marginLeft: 10)]
        private bool _isEqually;
        public bool isEqually => _isEqually;


#if UNITY_EDITOR
        #region Fabric
        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade Fabric")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric && _typeUpgrade == TypeUpgrade.ProductQuality")]
        private void BtnUpgradeProductQualityFabric()
        {
            _IupgradeBuildingsFabric.UpgradeProductQuality();
        }

        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade Fabric")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric && _typeUpgrade == TypeUpgrade.MaxCapacityStock")]
        private void BtnUpgradeMaxCapacityStock()
        {
            _IupgradeBuildingsFabric.UpgradeMaxCapacityStock();
        }

        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade Fabric")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric && _typeUpgrade == TypeUpgrade.ProductivityKgPerDay")]
        private void BtnUpgradedProductivityKgPerDay()
        {
            _IupgradeBuildingsFabric.ProductivityKgPerDay();
        }
        #endregion


        #region City

        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade City")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.City")]
        private void BtnUpgradeBuildingSlots()
        {
            _IupgradableCityBusiness.UpgradeBuildingSlots();
        }

        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade City")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.City")]
        private void BtnUpgradeBusinessMaxNumberVisitors()
        {
            _IupgradableCityBusiness.UpgradeBusinessMaxNumberVisitors();
        }

        #endregion

#endif


        public void InitAwake()
        {
            if (_typeUpgradableObject == TypeUpgradableObject.Fabric)
            {
                _IupgradableFabric = GetComponent<FabricControl>();
                _IupgradeBuildingsFabric = new UpgradeBuildingsFactory(_IupgradableFabric, this);
            }
            else if (_typeUpgradableObject == TypeUpgradableObject.City)
            {
                _IupgradableCityBusiness = GetComponent<CityBusinessControl>();
            }
        }
    }
}
