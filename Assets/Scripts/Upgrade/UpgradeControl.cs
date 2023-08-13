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

        private IUpgradeCityBusiness _IupgradeCityBusiness;

        private enum TypeUpgradableObject
        {
            Fabric, City
        }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeUpgradableObject _typeUpgradableObject;


        public void InitAwake()
        {
            if (_typeUpgradableObject == TypeUpgradableObject.Fabric)
                _IupgradeBuildingsFabric = new UpgradeBuildingsFactory(_IupgradableFabric = GetComponent<FabricControl>(), this);
            else if (_typeUpgradableObject == TypeUpgradableObject.City)
                _IupgradeCityBusiness = new UpgradeBuildingsCity(_IupgradableCityBusiness = GetComponent<CityBusinessControl>());
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.SimpleImportant, isSingle: false);
        }


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
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric")]
        private float _amount;
        public float amount => _amount;

        [SerializeField, BoxGroup("Parameters"), Title("Equally or Add (false = Equally)")]
        [DisableIf("@_typeUpgrade == TypeUpgrade.ProductivityKgPerDay"), PropertySpace(10, 10), HideLabel]
        [HorizontalGroup("Parameters/Hor", marginLeft: 10), ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric")]
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

        [SerializeField, BoxGroup("Parameters/Control Upgrade City")]
        private byte _indexBusiness;

        [Button("Building Slots"), BoxGroup("Parameters/Control Upgrade City")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.City"), HorizontalGroup("Parameters/Control Upgrade City/Hor")]
        private void BtnUpgradeBuildingSlots()
        {
            _IupgradeCityBusiness.UpgradeBuildingSlots();
        }

        [Button("Max Number Visitors"), BoxGroup("Parameters/Control Upgrade City")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.City"), HorizontalGroup("Parameters/Control Upgrade City/Hor")]
        private void BtnUpgradeBusinessMaxNumberVisitors()
        {
            _IupgradeCityBusiness.UpgradeBusinessMaxNumberVisitors(_indexBusiness);
        }

        #endregion

#endif
    }
}
