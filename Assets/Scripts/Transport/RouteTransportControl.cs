using UnityEngine;
using Sirenix.OdinInspector;
using Route.Builder;
using System;
using DebugCustomSystem;
using Transport.Reception;
using Resources;
using Building.Additional;
using TimeControl;
using System.Collections;
using Obstacle;

namespace Transport
{
    [RequireComponent(typeof(CreatorCurveRoad))]
    internal sealed class RouteTransportControl : MonoBehaviour, ITransportInteractRoute, IReRouteTransportation
    {
        [ShowInInspector]
        private ICreatorCurveRoad _IcreatorCurveRoad;

        [ShowInInspector, Required]
        private IReRouteTransportation _IreRouteTransportation;

        private TransportationDataStorage _transportationDataStorage = new TransportationDataStorage();

        private TimeDateControl _timeDateControl;

        [SerializeField]
        private TypeTransport[] _allTypesTransport;

        private WaitForSeconds _coroutineTimeStep;

        [SerializeField, ReadOnly]
        private Vector3[] _routePoints;
        public Vector3[] routePoints => _routePoints;

        private event Action _onLateUpdateAction;
        event Action ITransportInteractRoute.onLateUpdateAction
        {
            add { _onLateUpdateAction += value; }
            remove { _onLateUpdateAction -= value; }
        }

        private float _impactOfObstaclesOnSpeed;
        float ITransportInteractRoute.impactOfObstaclesOnSpeed => _impactOfObstaclesOnSpeed;


        private RouteTransportControl() { }

        private void OnEnable()
        {
            FindAndCreateComponents();
            StartCoroutine(CalculateExenses());

            void FindAndCreateComponents()
            {
                _IcreatorCurveRoad ??= GetComponent<ICreatorCurveRoad>();
                _timeDateControl ??= FindObjectOfType<TimeDateControl>();
                _coroutineTimeStep ??= new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay());
            }
        }

        private void OnDisable() => StopAllCoroutines();

        private void LateUpdate() => _onLateUpdateAction?.Invoke();

        private void MaintenanceExenses()
        {
            double finalMaintenanceExenses = 0;

            for (ushort i = 0; i < _transportationDataStorage.l_purchasedTransportData.Count; i++)
            {
                finalMaintenanceExenses += _transportationDataStorage.l_purchasedTransportData[i]
                                           .typeTransport.maintenanceExpenses;
            }

            SpendingToObjects.SendNewExpense(finalMaintenanceExenses);
        }

        private IEnumerator CalculateExenses()
        {
            while (true)
            {
                MaintenanceExenses();
                yield return _coroutineTimeStep;
            }
        }

        private void AddTransportDataStorage(in TransportationDataStorage allTransportation,
                                             in ushort index)
        {
            _transportationDataStorage.AddObject
            (
                allTransportation.l_purchasedTransportSprite[index],
                allTransportation.l_purchasedTransportData[index]
            );
        }

        private bool CheckRulesBuyingTransport(in byte indexTypeTransport)
        {
            if (_allTypesTransport[indexTypeTransport].type == _IcreatorCurveRoad.typeRoute)
                return true;
            else
                return false;
        }

        private void OnTriggerEnter2D(Collider2D collisionObject)
        {
            if (collisionObject.GetComponent(typeof(IObstacle)))
            {
                IObstacle obstacle = (IObstacle)collisionObject.GetComponent(typeof(IObstacle));
                _impactOfObstaclesOnSpeed += obstacle.config.percentageImpactSpeed;
            }
        }

        ITransportReception[] ITransportInteractRoute.GetPointsReception()
        {
            return _IcreatorCurveRoad.GetPointsConnectionRoute();
        }

        ushort[] IReRouteTransportation.SendTransportTransferRequest(
            in TransportationDataStorage allTransportation)
        {
            ushort[] foundObjectsForRerouting = new ushort[allTransportation.l_transportTransferStatus.Count];

            for (ushort i = 0; i < allTransportation.l_transportTransferStatus.Count; i++)
            {
                if (allTransportation.l_transportTransferStatus[i] == true)
                {
                    foundObjectsForRerouting[i] = i;
                    AddTransportDataStorage(allTransportation, i);
                }
            }
            return foundObjectsForRerouting;
        }


#if UNITY_EDITOR
        [SerializeField]
        private TypeProductionResources.TypeResource _typeTransportingResource;

        [SerializeField]
        private byte _indexTypeTransport, _indexTransportInList;


        [Button("Buy Transport"), HorizontalGroup("Hor")]
        private void BuyTransport()
        {
            if (CheckRulesBuyingTransport(_indexTypeTransport) == false)
                return;

            if (_routePoints[0] == Vector3.zero)
                _routePoints = _IcreatorCurveRoad.GetRoutePoints();

            GameObject newTransport = Instantiate(
                _allTypesTransport[_indexTypeTransport].prefab,
                _IcreatorCurveRoad.GetRouteMainPoint(),
                Quaternion.identity
            );

            SelfTransport selfTransportObject = new(
                _allTypesTransport[_indexTypeTransport], this, newTransport);

            _transportationDataStorage.AddObject(newTransport, selfTransportObject);
            UpdateTypeTransportingResource(_transportationDataStorage.l_purchasedTransportData.Count - 1);
        }

        [Button("Sell Transport"), HorizontalGroup("Hor")]
        private void SellTransport()
        {
            try
            {
                Destroy(_transportationDataStorage.DestroyTransport(_indexTransportInList).sprite);
                _transportationDataStorage.DestroyTransport(_indexTransportInList).data.Dispose();
                _transportationDataStorage.RemoveObjectFromList(_indexTransportInList);
            }
            catch (Exception exception)
            {
                DebugSystem.Log(exception, DebugSystem.SelectedColor.Red, "Exception", "Произошла ошибка: ");
            }
        }

        [Button("Load|Unload States")]
        private void ChangeLoadUnloadStates(in byte indexReception, in byte indexTypeState, in bool isState)
        {
            _transportationDataStorage.l_purchasedTransportData[_indexTransportInList]
                .ChangeLoadUnloadStates(indexReception, indexTypeState, isState);
        }

        [Button("Set Cat Waiting Mode")]
        private void SetCarWaitingMode(in bool isState)
        {
            _transportationDataStorage.l_purchasedTransportData[_indexTransportInList]
                .ChangeStateWaiting(isState);
        }

        [Button("Set New Type"), Tooltip("Set new type transporting resource on car")]
        private void UpdateTypeTransportingResource(in int index)
        {
            _transportationDataStorage.l_purchasedTransportData[index]
                .SetTypeTransportingResource(_typeTransportingResource);
        }

        [Button("Re-route Transportation"), Tooltip("Re-route the selected vehicle")]
        private void ReRouteTransportation()
        {
            ushort[] indexesForCleanup = _IreRouteTransportation?.SendTransportTransferRequest(_transportationDataStorage);

            for (ushort i = 0; i < _transportationDataStorage.l_purchasedTransportData.Count; i++)
            {
                _transportationDataStorage.l_purchasedTransportData[i]
                                .ChangeRoute((ITransportInteractRoute)_IreRouteTransportation);
            }

            _transportationDataStorage.RemoveObjectsFromList(indexesForCleanup);
        }

        [Button("Set Transfer Status")]
        private void SetNewStatusTransferTransportation(in ushort index, in bool isStatus)
        {
            _transportationDataStorage.SetTransferStatus(index, isStatus);
        }
#endif
    }
}