using System;
using System.Collections.Generic;
using Resources;
using Transport.Breakdowns;
using Transport.Fuel;
using UnityEngine;

namespace Transport
{
    //TODO: refactoring
    public sealed class Transportation : ITransportation, IDisposable
    {
        private ITransportInteractRoute _ItransportInteractRoute;
        ITransportInteractRoute ITransportation.ItransportInteractRoute => _ItransportInteractRoute;

        private TransportationFuel _transportationFuel;
        TransportationFuel ITransportation.transportationFuel => _transportationFuel;

        private TransportationBreakdowns _transportationBreakdowns;
        TransportationBreakdowns ITransportation.transportationBreakdowns => _transportationBreakdowns;

        private TransportationMovement _transportationMovement;

        private TypeTransport _typeTransport;
        public TypeTransport typeTransport => _typeTransport;

        private TypeTransport _futureConfigTypeTransport;

        private Dictionary<byte, bool[]> d_loadAndUnloadStates = new Dictionary<byte, bool[]>();

        private TypeProductionResources.TypeResource _typeCurrentTransportResource;

        private float _productLoad;

        private bool _isWaitingReception;

        private bool _isRequestTransportationRepairIsMade;


        public Transportation(in TypeTransport typeTransport,
                              in ITransportInteractRoute routeTransportControl,
                              in GameObject objectTransport)
        {
            _typeTransport = typeTransport;
            _ItransportInteractRoute = routeTransportControl;

            _transportationFuel = new(_typeTransport);
            _transportationBreakdowns = new(_typeTransport);
            _transportationMovement = new(this, objectTransport);

            SubscribeToEvents();
            InitDictionaryStates();
        }

        private void InitDictionaryStates()
        {
            for (byte i = 0; i != 2; i++)
                d_loadAndUnloadStates.Add(i, new bool[2]);
        }

        private void SubscribeToEvents()
        {
            _transportationMovement.isInStartedPosition += SendRequestFromPosition;
            _ItransportInteractRoute.fixedUpdate += _transportationMovement.MovementTransport;
            _ItransportInteractRoute.updatedTimeStep += _transportationMovement.ChangeSpeed;
            _ItransportInteractRoute.updatedTimeStep += _transportationFuel.FuelConsumption;
            _ItransportInteractRoute.updatedTimeStep += _transportationBreakdowns.DamageVehicles;
        }

        private void SendRequestFromPosition(bool isStartedPosition)
        {
            if (isStartedPosition)
                SendRequestsAndCheckWaitingCar(indexReception: 0, isFirstPosition: true);
            else
                SendRequestsAndCheckWaitingCar(indexReception: 1, isFirstPosition: false);
        }

        private void SendRequestsAndCheckWaitingCar(in byte indexReception, in bool isFirstPosition)
        {
            RequestLoad(indexStateLoad: 0, indexReception: indexReception);
            RequestUnload(indexStateLoad: 1, indexReception: indexReception);

            if (WaitLoadOrUnload())
                _transportationMovement.isFirstPosition = isFirstPosition;
        }

        private void RequestLoad(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad] && _productLoad == 0)
            {
                _productLoad = _ItransportInteractRoute.GetPointsReception()[indexReception]
                                .RequestConnectionToLoadRes(_typeTransport.capacity, _typeCurrentTransportResource);
                Debug.Log($"Product load: {_productLoad}");
            }
        }

        private void RequestUnload(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad])
            {
                _ItransportInteractRoute.GetPointsReception()[indexReception]
                                .RequestConnectionToUnloadRes(_productLoad, _typeCurrentTransportResource);
                _productLoad = 0;
                Debug.Log($"Product load: {_productLoad}");
            }
        }

        private bool WaitLoadOrUnload()
        {
            if (_isWaitingReception && _productLoad >= _typeTransport.capacity || !_isWaitingReception)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            _ItransportInteractRoute.fixedUpdate -= _transportationMovement.MovementTransport;
            GC.SuppressFinalize(this);
        }

        public void SetTypeTransportingResource(in TypeProductionResources.TypeResource typeResource)
        {
            _typeCurrentTransportResource = typeResource;
        }

        public void ChangeRoute(in ITransportInteractRoute routeTransportControl)
        {
            if (routeTransportControl != null)
            {
                _ItransportInteractRoute = routeTransportControl;
                _transportationMovement.ChangeRoute();
            }
        }

        public void SendVehicleForRepair()
        {
            _transportationBreakdowns.Repair();
        }

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

        public void ChangeStateWaiting(in bool isState) => _isWaitingReception = isState;
    }
}
