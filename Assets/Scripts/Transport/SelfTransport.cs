using System;
using System.Collections.Generic;
using Resources;
using UnityEngine;

namespace Transport
{
    public sealed class SelfTransport : IDisposable
    {
        private ITransportInteractRoute _ItransportInteractRoute;

        private readonly TypeTransport _typeTransport;
        public TypeTransport typeTransport => _typeTransport;

        private readonly GameObject _someObject;

        private Dictionary<byte, bool[]> d_loadAndUnloadStates = new Dictionary<byte, bool[]>();

        private TypeProductionResources.TypeResource _typeCurrentTransportResource;

        private float _productLoad;

        private readonly float _speed;

        private byte _indexCurrentRoutePoint;

        private bool _isFirstPosition = true;

        private bool _isWaitingReception = false;


        public SelfTransport(in TypeTransport typeTransport,
                             in ITransportInteractRoute routeTransportControl,
                             in GameObject objectTransport)
        {
            _typeTransport = typeTransport;
            _ItransportInteractRoute = routeTransportControl;
            _someObject = objectTransport;

            _ItransportInteractRoute.onLateUpdateAction += MovementTransport;
            InitDictionaryStates();

            _speed = (_typeTransport.speed * (1.0f - _ItransportInteractRoute.impactOfObstaclesOnSpeed) / 100)
                         * Time.fixedDeltaTime;
        }

        private void InitDictionaryStates()
        {
            for (byte i = 0; i != 2; i++)
                d_loadAndUnloadStates.Add(i, new bool[2]);
        }

        private void CheckPosition()
        {
            if (_isFirstPosition) FirstPosition();
            else LastPosition();

            void FirstPosition()
            {
                if (_someObject.transform.position == _ItransportInteractRoute.routePoints[_indexCurrentRoutePoint])
                {
                    if (_indexCurrentRoutePoint < _ItransportInteractRoute.routePoints.Length - 1)
                        _indexCurrentRoutePoint++;
                    else if (_indexCurrentRoutePoint == _ItransportInteractRoute.routePoints.Length - 1)
                        SendRequestsAndCheckWaitingCar(indexReception: 1, isFirstPosition: false);
                }
            }

            void LastPosition()
            {
                if (_someObject.transform.position == _ItransportInteractRoute.routePoints[_indexCurrentRoutePoint])
                {
                    if (_indexCurrentRoutePoint > 0)
                        _indexCurrentRoutePoint--;
                    else if (_indexCurrentRoutePoint == 0)
                        SendRequestsAndCheckWaitingCar(indexReception: 0, isFirstPosition: true);
                }
            }

            void SendRequestsAndCheckWaitingCar(in byte indexReception, in bool isFirstPosition)
            {
                RequestLoad(indexStateLoad: 0, indexReception: indexReception);
                RequestUnload(indexStateLoad: 1, indexReception: indexReception);

                if (WaitLoadOrUnload()) _isFirstPosition = isFirstPosition;
            }
        }

        private void Move(in byte indexPositionRoute)
        {
            _someObject.transform.position = Vector3.MoveTowards(_someObject.transform.position,
                                                    _ItransportInteractRoute.routePoints[indexPositionRoute],
                                                    _speed);
        }

        private void RequestLoad(in byte indexStateLoad, in byte indexReception)
        {
            if (d_loadAndUnloadStates[indexReception][indexStateLoad])
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

        public void Dispose()
        {
            _ItransportInteractRoute.onLateUpdateAction -= MovementTransport;
            GC.SuppressFinalize(this);
        }

        public void MovementTransport()
        {
            CheckPosition();
            Move(_indexCurrentRoutePoint);
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
