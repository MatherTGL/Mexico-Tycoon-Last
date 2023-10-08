using System;
using System.Collections.Generic;
using Resources;
using Transport.Breakdowns;
using Transport.Fuel;
using UnityEngine;

namespace Transport
{
    public sealed class Transportation : IDisposable
    {
        private const float _fullSpeedPercentage = 1.0f;

        private ITransportInteractRoute _ItransportInteractRoute;

        private TransportationFuel _transportationFuel;

        private TransportationBreakdowns _transportationBreakdowns;

        private readonly TypeTransport _typeTransport;
        public TypeTransport typeTransport => _typeTransport;

        private readonly GameObject _someObject;

        private Dictionary<byte, bool[]> d_loadAndUnloadStates = new Dictionary<byte, bool[]>();

        private TypeProductionResources.TypeResource _typeCurrentTransportResource;

        private float _productLoad;

        private float _maxSpeed, _minSpeed;

        private float _currentSpeed;

        private byte _indexCurrentRoutePoint;

        private bool _isFirstPosition = true;

        private bool _isWaitingReception;


        public Transportation(in TypeTransport typeTransport,
                             in ITransportInteractRoute routeTransportControl,
                             in GameObject objectTransport)
        {
            _typeTransport = typeTransport;
            _ItransportInteractRoute = routeTransportControl;
            _someObject = objectTransport;

            _transportationFuel = new TransportationFuel(_typeTransport);
            _transportationBreakdowns = new TransportationBreakdowns(_typeTransport);

            SubscribeToEvents();
            InitDictionaryStates();
            SetAdditionalCharacteristics();
        }

        private void InitDictionaryStates()
        {
            for (byte i = 0; i != 2; i++)
                d_loadAndUnloadStates.Add(i, new bool[2]);
        }

        private void SetAdditionalCharacteristics()
        {
            float availableSpeedAfterImpact = (_fullSpeedPercentage
                - _ItransportInteractRoute.impactOfObstaclesOnSpeed) / 100;

            _maxSpeed = _typeTransport.maxSpeed * availableSpeedAfterImpact * Time.fixedDeltaTime;
            _minSpeed = _maxSpeed * _typeTransport.minSpeedPercentageMaxSpeed;
        }

        private void SubscribeToEvents()
        {
            _ItransportInteractRoute.lateUpdated += MovementTransport;
            _ItransportInteractRoute.updatedTimeStep += ChangeSpeed;
            _ItransportInteractRoute.updatedTimeStep += _transportationFuel.FuelConsumption;
            _ItransportInteractRoute.updatedTimeStep += _transportationBreakdowns.DamageVehicles;
        }

        private void CheckPosition()
        {
            //TODO: refactoring
            if (_someObject.transform.position == _ItransportInteractRoute.routePoints[_indexCurrentRoutePoint])
            {
                if (_indexCurrentRoutePoint == 0)
                    SendRequestsAndCheckWaitingCar(indexReception: 0, isFirstPosition: true);
                else if (_indexCurrentRoutePoint == _ItransportInteractRoute.routePoints.Length - 1)
                    SendRequestsAndCheckWaitingCar(indexReception: 1, isFirstPosition: false);

                if (_isFirstPosition && _indexCurrentRoutePoint < _ItransportInteractRoute.routePoints.Length - 1)
                    _indexCurrentRoutePoint++;
                else if (_indexCurrentRoutePoint > 0)
                    _indexCurrentRoutePoint--;
            }

            void SendRequestsAndCheckWaitingCar(in byte indexReception, in bool isFirstPosition)
            {
                RequestLoad(indexStateLoad: 0, indexReception: indexReception);
                RequestUnload(indexStateLoad: 1, indexReception: indexReception);

                if (WaitLoadOrUnload())
                    _isFirstPosition = isFirstPosition;
            }
        }

        private void Move(in byte indexPositionRoute)
        {
            _someObject.transform.position = Vector3.MoveTowards(_someObject.transform.position,
                                                    _ItransportInteractRoute.routePoints[indexPositionRoute],
                                                    _currentSpeed);
        }

        private void RequestLoad(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad] && _productLoad == 0)
            {
                _productLoad = _ItransportInteractRoute.GetPointsReception()[indexReception]
                                .RequestConnectionToLoadRes(_typeTransport.capacity, _typeCurrentTransportResource);
            }
        }

        private void RequestUnload(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad])
            {
                _ItransportInteractRoute.GetPointsReception()[indexReception]
                                .RequestConnectionToUnloadRes(_productLoad, _typeCurrentTransportResource);
                _productLoad = 0;
            }
        }

        private bool WaitLoadOrUnload()
        {
            if (_isWaitingReception && _productLoad >= _typeTransport.capacity || !_isWaitingReception)
                return true;
            else
                return false;
        }

        private void MovementTransport()
        {
            if (_transportationFuel.IsFuelAvailable() && _transportationBreakdowns.IsNotInRepair())
            {
                CheckPosition();
                Move(_indexCurrentRoutePoint);
            }
        }

        private void ChangeSpeed()
        {
            _currentSpeed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
        }

        public void Dispose()
        {
            _ItransportInteractRoute.lateUpdated -= MovementTransport;
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
                _indexCurrentRoutePoint = 0;
                _someObject.transform.position = _ItransportInteractRoute.routePoints[0];
            }
        }

        public void SendVehicleForRepair()
        {
            _transportationBreakdowns.Repair();
        }


#if UNITY_EDITOR
        public void ChangeLoadUnloadStates(in byte indexReception, in byte indexLoadOrUnload, in bool isState)
        {
            if (d_loadAndUnloadStates.ContainsKey(indexReception))
                d_loadAndUnloadStates[indexReception][indexLoadOrUnload] = isState;
        }

        public void ChangeStateWaiting(in bool isState) => _isWaitingReception = isState;
#endif
    }
}
