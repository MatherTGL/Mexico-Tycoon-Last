using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Config.Building;
using Building.Farm;
using Transport.Reception;
using Resources;
using Building.Additional;
using Building.Border;
using System;
using Building.Fabric;
using Building.City;
using Building.Aerodrome;
using Building.Stock;
using Building.SeaPort;
using System.Linq;

namespace Building
{
    [RequireComponent(typeof(Transport.Reception.TransportReception))]
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class BuildingControl : MonoBehaviour, IBoot, IBuildingRequestForTransport
    {
        [ShowInInspector, ReadOnly]
        private IBuilding _Ibuilding;

        private ISpending _Ispending;

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

        void IBoot.InitAwake()
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

                if (_configSO != null)
                {
                    if (_typeBuilding is TypeBuilding.City)
                        _Ibuilding = new BuildingCity(_configSO);
                    else if (_typeBuilding is TypeBuilding.Farm)
                        _Ibuilding = new BuildingFarm(_configSO);
                    else if (_typeBuilding is TypeBuilding.Fabric)
                        _Ibuilding = new BuildingFabric(_configSO);
                    else if (_typeBuilding is TypeBuilding.Stock)
                        _Ibuilding = new BuildingStock(_configSO);
                    else if (_typeBuilding is TypeBuilding.Border)
                        _Ibuilding = new BuildingBorder(_configSO);
                    else if (_typeBuilding is TypeBuilding.Aerodrome)
                        _Ibuilding = new BuildingAerodrome(_configSO);
                    else if (_typeBuilding is TypeBuilding.SeaPort)
                        _Ibuilding = new BuildingSeaPort(_configSO);
                }

                if (_Ibuilding is ISpending)
                    _Ispending = (ISpending)_Ibuilding;

                Invoke();
            }

            void Invoke()
            {
                CreateDictionaryTypeDrugs();
                StartCoroutine(ConstantUpdating());
            }
        }

        private void CreateDictionaryTypeDrugs()
        {
            if (_Ibuilding is not BuildingBorder)
            {
                foreach (TypeProductionResources.TypeResource typeDrug
                        in Enum.GetValues(typeof(TypeProductionResources.TypeResource)))
                {
                    if (_Ibuilding.amountResources.ContainsKey(typeDrug) == false)
                        _Ibuilding.amountResources.Add(typeDrug, 0);
                }

                _Ibuilding.InitDictionaryStockCapacity();
            }
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

        private void ChangeFarmType(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            if (_Ibuilding is IChangedFarmType)
            {
                IChangedFarmType IchangedType = (IChangedFarmType)_Ibuilding;
                IchangedType.ChangeType(typeFarm);
            }
        }

        private void UpdateSpendingBuildings()
        {
            if (_IbuildingJobStatus != null && _IbuildingJobStatus.isWorked)
                _Ispending?.Spending();
        }

        private IEnumerator ConstantUpdating()
        {
            while (true)
            {
                _Ibuilding.ConstantUpdatingInfo();
                UpdateSpendingBuildings();
                yield return _coroutineTimeStep;
            }
        }

        float IBuildingRequestForTransport.RequestGetResource(in float transportCapacity,
            in TypeProductionResources.TypeResource typeResource)
        {
            return _Ibuilding.GetResources(transportCapacity, typeResource);
        }

        bool IBuildingRequestForTransport.RequestUnloadResource(in float quantityResource,
            in TypeProductionResources.TypeResource typeResource)
        {
            return _Ibuilding.SetResources(quantityResource, typeResource);
        }

        (Bootstrap.TypeLoadObject typeLoad, bool isSingle) IBoot.GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SuperImportant, false);
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

        #region Farm

        [Button("Change Farm Type"), BoxGroup("Editor Control | Farm"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == TypeBuilding.Farm")]
        private void ChangeFarmTypeEditor(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            ChangeFarmType(typeFarm);
        }

        #endregion
#endif
    }
}