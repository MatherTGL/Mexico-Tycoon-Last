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
using Building.Fabric;
using Building.City;
using Building.Aerodrome;
using Building.Stock;
using Building.SeaPort;
using Building.City.Business;
using Climate;
using Expense;
using static Boot.Bootstrap;
using Config.Expenses;
using System;
using static Building.BuildingEnumType;

namespace Building
{
    [RequireComponent(typeof(TransportReception))]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class BuildingControl : MonoBehaviour, IBoot, IBuildingRequestForTransport
    {
        [ShowInInspector, ReadOnly]
        private IBuilding _Ibuilding;

        private ISpending _Ispending;

        private IEnergyConsumption _IenergyConsumption;

        private IBuildingJobStatus _IbuildingJobStatus;

        private IBuildingPurchased _IbuildingPurchased;
        IBuildingPurchased IBuildingRequestForTransport.IbuildingPurchased => _IbuildingPurchased;

        private ICityBusiness _IcityBusiness;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel]
        private ConfigExpensesManagementEditor _configExpenses;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel, PropertySpace(0, 5), DisableInPlayMode]
        private ScriptableObject _configSO; //TODO: make general config

        private TimeDateControl _timeDateControl;

        private WaitForSeconds _coroutineTimeStep;

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeBuilding _typeBuilding;


        private BuildingControl() { }

        void IBoot.InitAwake()
        {
            _timeDateControl = FindObjectOfType<TimeDateControl>();
            _coroutineTimeStep = new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay());

            if (_configSO != null)
                CreateInstance();

            _Ispending = _Ibuilding as ISpending;
            _IcityBusiness = _Ibuilding as ICityBusiness;
            _IbuildingPurchased = _Ibuilding as IBuildingPurchased;

            ConnectExpensesManagementControl();
            CreateDictionaryTypeDrugs();
            StartCoroutine(ConstantUpdating());
        }

        private void ConnectExpensesManagementControl()
        {
            if (_Ibuilding is IUsesExpensesManagement expensesManagement)
            {
                IUsesExpensesManagement IusesExpensesManagement = expensesManagement;
                IExpensesManagement IexpensesManagement = FindObjectOfType<ExpenseManagementControl>();

                if (IexpensesManagement != null && _configExpenses != null)
                    IusesExpensesManagement.LoadExpensesManagement(IexpensesManagement, _configExpenses);
                else
                    throw new NullReferenceException();
            }
        }

        private void CreateInstance()
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

        private void CreateDictionaryTypeDrugs()
        {
            if (_Ibuilding is not BuildingBorder)
                _Ibuilding.InitDictionaries();
        }

        private void ChangeOwnerState(in bool isBuy)
        {
            if (isBuy) _IbuildingPurchased?.Buy();
            else _IbuildingPurchased?.Sell();
        }

        private void ChangeJobStatusBuilding(in bool isState)
        {
            _IbuildingJobStatus = _Ibuilding as IBuildingJobStatus;
            _IbuildingJobStatus?.ChangeJobStatus(isState);
        }

        private void ChangeFarmType(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            IChangedFarmType IchangedType = _Ibuilding as IChangedFarmType;
            IchangedType?.ChangeType(typeFarm);
        }

        private void UpdateSpendingBuildings()
        {
            if (_IbuildingJobStatus != null && _IbuildingJobStatus.isWorked)
                _Ispending?.Spending();
        }

        private void MonitorEnergy()
        {
            _IenergyConsumption ??= _Ibuilding as IEnergyConsumption;
            _IbuildingJobStatus ??= _Ibuilding as IBuildingJobStatus;

            if (_IbuildingJobStatus != null && _IbuildingJobStatus.isWorked)
                _IenergyConsumption.MonitorEnergy(_IenergyConsumption);
        }

        private IEnumerator ConstantUpdating()
        {
            while (true)
            {
                _Ibuilding.ConstantUpdatingInfo();
                UpdateSpendingBuildings();
                MonitorEnergy();
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

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.LotsOf);
        }

        public void SetClimateZone(in IClimateZone IclimateZone)
        {
            IUsesClimateInfo IusesClimateInfo = _Ibuilding as IUsesClimateInfo;
            IusesClimateInfo?.SetClimateZone(IclimateZone);
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
        private void ChangeFarmTypeEditor(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            ChangeFarmType(typeFarm);
        }

        #endregion

        #region City-Business

        [Button("Buy"), BoxGroup("Editor Control | City Business"), DisableInEditorMode]
        private void BuyBusiness(in CityBusiness.TypeBusiness typeBusiness)
        {
            _IcityBusiness.BuyBusiness(typeBusiness);
        }

        [Button("Sell"), BoxGroup("Editor Control | City Business"), DisableInEditorMode]
        private void SellBusiness(in ushort indexBusiness)
        {
            _IcityBusiness.SellBusiness(indexBusiness);
        }

        #endregion
#endif
    }
}
