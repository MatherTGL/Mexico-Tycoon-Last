using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events.Transport;
using Resources;
using Transport.Breakdowns;
using Transport.Fuel;
using UnityEngine;

namespace Transport
{
    //TODO: refactoring
    public sealed class Transportation : ITransportation, IDisposable
    {
        private readonly IEventTransportation _IeventTransportation;

        private ITransportInteractRoute _ItransportInteractRoute;
        ITransportInteractRoute ITransportation.ItransportInteractRoute => _ItransportInteractRoute;

        private readonly TransportationFuel _transportationFuel;
        TransportationFuel ITransportation.transportationFuel => _transportationFuel;

        private readonly TransportationBreakdowns _transportationBreakdowns;
        TransportationBreakdowns ITransportation.transportationBreakdowns => _transportationBreakdowns;

        private readonly TransportationMovement _transportationMovement;

        private TypeTransport _typeTransport;
        public TypeTransport typeTransport => _typeTransport;

        private TypeTransport _futureConfigTypeTransport;

        private readonly Dictionary<byte, bool[]> d_loadAndUnloadStates = new()
        {
            {0, new bool[2]}, {1, new bool[2]}
        };

        private TypeProductionResources.TypeResource _typeCurrentTransportResource;

        private float _productLoad;

        private bool _isWaitingReception;

        private bool _isRequestTransportationRepairIsMade;

        private bool _isTransportationAwaiting;

        private volatile bool _isWait;


        public Transportation(in TypeTransport typeTransport,
                              in ITransportInteractRoute routeTransportControl,
                              in GameObject objectTransport)
        {
            _typeTransport = typeTransport;
            _ItransportInteractRoute = routeTransportControl;

            _transportationFuel = new(_typeTransport);
            _transportationBreakdowns = new(_typeTransport);
            _transportationMovement = new(this, objectTransport);
            _IeventTransportation = new EventEditorTransportation();

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _transportationMovement.isInStartedPosition += SendRequestFromPosition;
            _ItransportInteractRoute.fixedUpdate += _transportationMovement.MovementTransport;
            _ItransportInteractRoute.updatedTimeStep += UpdatedTimeStep;
        }

        private void UpdatedTimeStep()
        {
            if (_isTransportationAwaiting)
                return;

            _transportationFuel.FuelConsumption();
            _transportationMovement.ChangeSpeed();
            _transportationBreakdowns.DamageVehicles();
            _IeventTransportation.Update();
        }

        private void SendRequestFromPosition(bool isStartedPosition)
        {
            if (isStartedPosition)
                AsyncSendRequestsAndCheckWaitingCar(indexReception: 0, isFirstPosition: true);
            else
                AsyncSendRequestsAndCheckWaitingCar(indexReception: 1, isFirstPosition: false);
        }

        private async ValueTask AsyncSendRequestsAndCheckWaitingCar(byte indexReception, bool isFirstPosition)
        {
            if (_isWait)
                return;

            await Task.Run(async () =>
            {
                if (d_loadAndUnloadStates[indexReception][0] || d_loadAndUnloadStates[indexReception][^1] && _productLoad > 0)
                {
                    await AsyncDelayLoadAndUnload(indexReception);
                    _isTransportationAwaiting = IsWaitLoadOrUnload(indexReception);
                }
            });

            if (IsWaitLoadOrUnload(indexReception) == false)
                _transportationMovement.isFirstPosition = isFirstPosition;
        }

        private async ValueTask AsyncDelayLoadAndUnload(byte indexReception)
        {
            await Task.Run(async () =>
            {
                _isWait = true;
                await Task.Delay(_typeTransport.timeLoadAndUnloadInSeconds * 1_000);

                RequestLoad(indexStateLoad: 0, indexReception: indexReception);
                RequestUnload(indexStateLoad: 1, indexReception: indexReception);

                _isWait = false;
            });
        }

        private void RequestLoad(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad] && _productLoad == 0)
                _productLoad = _ItransportInteractRoute.GetPointsReception()[indexReception]
                        .GetRequestConnectionToLoadRes(_typeTransport.capacity, _typeCurrentTransportResource);
        }

        private void RequestUnload(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad])
            {
                if (_ItransportInteractRoute.GetPointsReception()[indexReception]
                    .IsRequestConnectionToUnloadRes(_productLoad, _typeCurrentTransportResource) == false)
                    return;

                _productLoad = 0;
            }
        }

        private bool IsWaitLoadOrUnload(in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][d_loadAndUnloadStates.Count - 1])
            {
                if (_isWaitingReception && _productLoad < _typeTransport.capacity ||
                    _isWaitingReception && _productLoad >= _typeTransport.capacity || !_isWaitingReception)
                {
                    return false;
                }
            }
            else if (d_loadAndUnloadStates[indexReception][d_loadAndUnloadStates.Count - 1] == false)
                return false;

            return true;
        }

        public void Dispose()
        {
            _ItransportInteractRoute.fixedUpdate -= _transportationMovement.MovementTransport;
            GC.SuppressFinalize(this);
        }

        public void SetTypeTransportingResource(in TypeProductionResources.TypeResource typeResource)
            => _typeCurrentTransportResource = typeResource;

        public void ChangeRoute(in ITransportInteractRoute routeTransportControl)
        {
            if (routeTransportControl != null)
            {
                _ItransportInteractRoute = routeTransportControl;
                _transportationMovement.ChangeRoute();
            }
        }

        public void SendVehicleForRepair()
            => _transportationBreakdowns.Repair();

        public void SendRequestReplaceTypeTransport(in TypeTransport typeTransport)
        {
            if (_typeTransport != typeTransport && !_isRequestTransportationRepairIsMade)
            {
                _futureConfigTypeTransport = typeTransport;
                _transportationMovement.isInStartedPosition += ReplaceTypeTransport;
                _isRequestTransportationRepairIsMade = true;
            }
        }

        private void ReplaceTypeTransport(bool isStartedPosition)
        {
            if (isStartedPosition)
            {
                _typeTransport = _futureConfigTypeTransport;
                _transportationMovement.isInStartedPosition -= ReplaceTypeTransport;
                _isRequestTransportationRepairIsMade = false;
            }
        }

        public void ChangeLoadUnloadStates(in byte indexReception, in byte indexLoadOrUnload, in bool isState)
        {
            if (d_loadAndUnloadStates.ContainsKey(indexReception))
                d_loadAndUnloadStates[indexReception][indexLoadOrUnload] = isState;
        }

        public void ChangeStateWaiting(in bool isState)
            => _isWaitingReception = isState;

        bool ITransportation.IsWait()
        {
            if (_isWait || _isTransportationAwaiting)
                return true;
            else
                return false;
        }
    }
}
