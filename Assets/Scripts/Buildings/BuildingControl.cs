using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Building.City;
using Config.Building;
using Building.Farm;
using Building.Stock;
using Transport.Reception;
using Building.Fabric;
using Resources;
using Building.Additional;
using Building.Border;
using Building.Aerodrome;


namespace Building
{
    [RequireComponent(typeof(Transport.Reception.TransportReception))]
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class BuildingControl : MonoBehaviour, IBoot, IBuildingRequestForTransport
    {
        private IBuilding _Ibuilding;

        private IBuildingJobStatus _IbuildingJobStatus;

        private IBuildingPurchased _IbuildingPurchased;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel, PropertySpace(0, 5)]
        private ScriptableObject _configSO;

        private TimeDateControl _timeDateControl;

        private WaitForSeconds _coroutineTimeStep;

        public enum TypeBuilding : byte
        {
            City, Fabric, Farm, Aerodrome, SeaPort, Stock, Border
        }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeBuilding _typeBuilding;


        private BuildingControl() { }

        public void InitAwake()
        {
            Find();

            void Find()
            {
                _timeDateControl = FindObjectOfType<TimeDateControl>();
                Create();
            }

            void Create()
            {
                _coroutineTimeStep = new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay());

                if (_configSO is not null)
                {
                    if (_typeBuilding is TypeBuilding.City)
                        _Ibuilding = new BuildingCity((ConfigBuildingCityEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Farm)
                        _Ibuilding = new BuildingFarm((ConfigBuildingFarmEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Fabric)
                        _Ibuilding = new BuildingFabric((ConfigBuildingFabricEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Stock)
                        _Ibuilding = new BuildingStock((ConfigBuildingStockEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Border)
                        _Ibuilding = new BuildingBorder((ConfigBuildingBorderEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Aerodrome)
                        _Ibuilding = new BuildingAerodrome((ConfigBuildingAerodromeEditor)_configSO);
                }

                Invoke();
            }

            void Invoke() => StartCoroutine(ConstantUpdating());
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SuperImportant, false);
        }

        float IBuildingRequestForTransport.RequestGetResource(in float transportCapacity,
            in TypeProductionResources.TypeResource typeResource)
        {
            CheckIncomingDrugType(typeResource);
            return _Ibuilding.GetResources(transportCapacity, typeResource);
        }

        bool IBuildingRequestForTransport.RequestUnloadResource(in float quantityResource,
            in TypeProductionResources.TypeResource typeResource)
        {
            CheckIncomingDrugType(typeResource);
            return _Ibuilding.SetResources(quantityResource, typeResource);
        }

        private void CheckIncomingDrugType(in TypeProductionResources.TypeResource typeResource)
        {
            if (_Ibuilding.d_amountResources.ContainsKey(typeResource) == false)
                _Ibuilding.d_amountResources.Add(typeResource, 0);
        }

        private void ChangeOwnerState(in bool isBuy)
        {
            if (_Ibuilding is IBuildingPurchased)
            {
                _IbuildingPurchased = (IBuildingPurchased)_Ibuilding;

                if (isBuy) _IbuildingPurchased.Buy();
                else _IbuildingPurchased.Sell();
            }
        }

        private void ChangeJobStatusBuilding(in bool isState)
        {
            if (_Ibuilding is IBuildingJobStatus)
            {
                _IbuildingJobStatus = (IBuildingJobStatus)_Ibuilding;
                _IbuildingJobStatus.ChangeJobStatus(isState);
            }
        }

        private IEnumerator ConstantUpdating()
        {
            while (true)
            {
                _Ibuilding.ConstantUpdatingInfo();
                yield return _coroutineTimeStep;
            }
        }


#if UNITY_EDITOR
        [Button("Buy Building"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor")]
        [DisableInEditorMode]
        private void BuyBuilding() => ChangeOwnerState(true);

        [Button("Sell Building"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor")]
        [DisableInEditorMode]
        private void SellBuilding() => ChangeOwnerState(false);

        [Button("Activate"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor2")]
        [DisableInEditorMode]
        private void SetActivateBuilding() => ChangeJobStatusBuilding(true);

        [Button("Deactivate"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor2")]
        [DisableInEditorMode]
        private void SetDeactivateBuilding() => ChangeJobStatusBuilding(false);
#endif
    }
}
