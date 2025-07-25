using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Building.Farm;
using Transport.Reception;
using Building.Additional;
using Building.Border;
using Building.Fabric;
using Building.City;
using Building.Aerodrome;
using Building.Stock;
using Building.SeaPort;
using Building.City.Business;
using Expense;
using Config.Expenses;
using System;
using Building.Hire;
using Hire;
using static Boot.Bootstrap;
using static Building.BuildingEnumType;
using Country;
using Country.Climate.Weather;
using Unity.VisualScripting;
using Events.Buildings;
using Building.City.Deliveries;
using static Resources.TypeProductionResources;
using static Config.Building.ConfigBuildingFarmEditor;
using static Building.City.Business.CityBusiness;
using Building.Additional.Production;

namespace Building
{
    [RequireComponent(typeof(TransportReception), typeof(BoxCollider))]
    public sealed class BuildingControl : MonoBehaviour, IBoot, IBuildingRequestForTransport, ICountryAreaFindSceneObjects
    {
        private IBuilding _Ibuilding;

        private ISpending _Ispending;

        private IProductionBuilding _IproductionBuilding;

        private IEnergyConsumption _IenergyConsumption;

        private IBuildingJobStatus _IbuildingJobStatus;
        IBuildingJobStatus IBuildingRequestForTransport.IbuildingJobStatus => _IbuildingJobStatus;

        private IBuildingPurchased _IbuildingPurchased;
        IBuildingPurchased IBuildingRequestForTransport.IbuildingPurchased => _IbuildingPurchased;

        private IUseBusiness _IcityBusiness;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel]
        private ConfigExpensesManagementEditor _configExpenses;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel, PropertySpace(0, 5), DisableInPlayMode]
        private ScriptableObject _configSO;

        private WaitForSeconds _coroutineTimeStep;

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeBuilding _typeBuilding;


        private BuildingControl() { }

        [Obsolete]
        void IBoot.InitAwake()
        {
            _coroutineTimeStep = new WaitForSeconds(FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay());

            if (_configSO != null)
                CreateInstance();

            _Ispending = _Ibuilding as ISpending;
            _IcityBusiness = _Ibuilding as IUseBusiness;
            _IbuildingPurchased = _Ibuilding as IBuildingPurchased;

            ConnectEventEditor();
            ConnectExpensesManagementControl();
            CreateDictionaryTypeDrugs();
        }

        void IBoot.InitStart() => StartCoroutine(ConstantUpdating());

        private void ConnectExpensesManagementControl()
        {
            if (_Ibuilding is IUsesExpensesManagement expensesManagement)
            {
                IUsesExpensesManagement IusesExpensesManagement = expensesManagement;
                IExpensesManagement IexpensesManagement = FindObjectOfType<ExpenseManagementControl>();

                if (IexpensesManagement != null & _configExpenses != null)
                    IusesExpensesManagement.LoadExpensesManagement(IexpensesManagement, _configExpenses);
                else
                    throw new NullReferenceException();

                ConnectHiring();
            }
        }

        private void ConnectHiring()
        {
            if (_Ibuilding is IUsesHiring)
            {
                gameObject.AddComponent<HireEmployeeControl>().Init();
                _Ispending.IobjectsExpensesImplementation.IhiringModel = GetComponent<HireEmployeeControl>().IhiringModel;
            }
        }

        private void ConnectEventEditor()
        {
            if (_IbuildingPurchased != null)
            {
                this.AddComponent<EventBuildingsEditorControl>();
                GetComponent<IEventEditorBuildings>().Init(_IbuildingPurchased);
            }
        }

        private void CreateInstance()
        {
            if (_typeBuilding is TypeBuilding.City)
            {
                this.AddComponent<LocalMarketControl>();
                _Ibuilding = new BuildingCity(_configSO, GetComponent<ILocalMarket>());
            }
            else if (_typeBuilding is TypeBuilding.Farm)
                _Ibuilding = new BuildingFarm(_configSO);
            else if (_typeBuilding is TypeBuilding.Fabric)
                _Ibuilding = new BuildingFabric(_configSO);
            else if (_typeBuilding is TypeBuilding.Stock)
            {
                _Ibuilding = new BuildingStock(_configSO);
                this.AddComponent<WarehouseCleaning>().Init((ICleaningResources)_Ibuilding);
            }
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

        private void ChangeFarmType(in TypeFarm typeFarm)
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

        private void ChangeProductionResource(in TypeResource typeResource)
        {
            if (_Ibuilding is IProductionBuilding)
            {
                _IproductionBuilding ??= _Ibuilding as IProductionBuilding;
                _IproductionBuilding.SetNewProductionResource(typeResource);
            }
        }

        private void CalculateBuildingCostPurchased()
            => _IbuildingPurchased?.UpdateCostPurchased();

        private IEnumerator ConstantUpdating()
        {
            while (true)
            {
                _Ibuilding.ConstantUpdatingInfo();
                UpdateSpendingBuildings();
                MonitorEnergy();
                CalculateBuildingCostPurchased();
                yield return _coroutineTimeStep;
            }
        }

        float IBuildingRequestForTransport.RequestGetResource(in float capacity, in TypeResource typeResource)
            => _Ibuilding.GetResources(capacity, typeResource);

        bool IBuildingRequestForTransport.RequestUnloadResource(in float quantity, in TypeResource typeResource)
            => _Ibuilding.IsSetResources(quantity, typeResource);

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.LotsOf);

        void ICountryAreaFindSceneObjects.SetCountry(in ICountryBuildings IcountryBuildings)
        {
            IUsesCountryInfo IusesCountryInfo = _Ibuilding as IUsesCountryInfo;
            IusesCountryInfo?.SetCountry(IcountryBuildings);
        }

        void ICountryAreaFindSceneObjects.ActivateWeatherEvent(in IWeatherZone IweatherZone)
            => _IbuildingPurchased?.ActivateWeatherEvent(IweatherZone);

        void ICountryAreaFindSceneObjects.DeactiveWeatherEvent(in IWeatherZone IweatherZone)
            => _IbuildingPurchased?.DeactiveWeatherEvent(IweatherZone);

        //TODO: move all to view class and make mvc

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

        #region production
        [Button("Change Production Resource"), BoxGroup("Editor Control"), DisableInEditorMode]
        private void SetNewProductionResource(TypeResource typeResource) => ChangeProductionResource(typeResource);
        #endregion

        #region Farm

        [Button("Change Farm Type"), BoxGroup("Editor Control | Farm"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == BuildingEnumType.TypeBuilding.Farm")]
        private void ChangeFarmTypeEditor(in TypeFarm typeFarm)
            => ChangeFarmType(typeFarm);

        #endregion

        #region City-Business

        [Button("Buy"), BoxGroup("Editor Control | City Business"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == BuildingEnumType.TypeBuilding.City")]
        private void BuyBusiness(in TypeBusiness typeBusiness)
            => _IcityBusiness.BuyBusiness(typeBusiness);

        [Button("Sell"), BoxGroup("Editor Control | City Business"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == BuildingEnumType.TypeBuilding.City")]
        private void SellBusiness(in ushort indexBusiness)
            => _IcityBusiness.SellBusiness(indexBusiness);
        #endregion
#endif
    }
}
