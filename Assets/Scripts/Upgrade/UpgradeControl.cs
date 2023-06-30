using UnityEngine;
using Upgrade.Buildings;
using Sirenix.OdinInspector;
using Boot;
using Fabric;
using Upgrade.Buildings.Fabric;


namespace Upgrade
{
    public sealed class UpgradeControl : MonoBehaviour, IBoot
    {
        private IUpgradeBuildingsFabric _IupgradeBuildingsFabric;

        private IUpgradableFabric _IupgradableFabric;

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
        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric && _typeUpgrade == TypeUpgrade.ProductQuality")]
        private void BtnUpgradeProductQualityFabric()
        {
            _IupgradeBuildingsFabric.UpgradeProductQuality();
        }

        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric && _typeUpgrade == TypeUpgrade.MaxCapacityStock")]
        private void BtnUpgradeMaxCapacityStock()
        {
            _IupgradeBuildingsFabric.UpgradeMaxCapacityStock();
        }

        [Button("Upgrade"), BoxGroup("Parameters/Control Upgrade")]
        [ShowIf("@_typeUpgradableObject == TypeUpgradableObject.Fabric && _typeUpgrade == TypeUpgrade.ProductivityKgPerDay")]
        private void BtnUpgradedProductivityKgPerDay()
        {
            _IupgradeBuildingsFabric.ProductivityKgPerDay();
        }
#endif


        public void InitAwake()
        {
            if (_typeUpgradableObject == TypeUpgradableObject.Fabric)
            {
                _IupgradableFabric = GetComponent<FabricControl>();
                _IupgradeBuildingsFabric = new UpgradeBuildingsFactory(_IupgradableFabric, this);
            }
        }
    }
}
