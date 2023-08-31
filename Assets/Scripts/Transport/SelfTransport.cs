using System;
using DebugCustomSystem;
using UnityEngine;


namespace Transport
{
    public sealed class SelfTransport : IDisposable
    {
        private ITransportInteractRoute _ItransportInteractRoute;

        private TypeTransport _typeTransport;

        private GameObject _objectTransport;

        private float _productLoad;

        private bool _isFirstPosition = true;

        private bool[] _loadAndUnloadStates = new bool[4];

        private byte _indexCurrentRoutePoint;


        public SelfTransport(in TypeTransport typeTransport,
                             in ITransportInteractRoute routeTransportControl,
                             in GameObject objectTransport)
        {
            _typeTransport = typeTransport;
            _ItransportInteractRoute = routeTransportControl;
            _objectTransport = objectTransport;

            _ItransportInteractRoute.onLateUpdateAction += MovementTransport;
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

        private void CheckPosition()
        {
            if (_isFirstPosition) FirstPosition();
            else LastPosition();

            void FirstPosition()
            {
                Debug.Log(_indexCurrentRoutePoint);
                if (_objectTransport.transform.position == _ItransportInteractRoute.routePoints[_indexCurrentRoutePoint])
                {
                    if (_indexCurrentRoutePoint < _ItransportInteractRoute.routePoints.Length - 1)
                        _indexCurrentRoutePoint++;
                    else if (_indexCurrentRoutePoint == _ItransportInteractRoute.routePoints.Length - 1)
                    {
                        Debug.Log("машина на месте и готова для погрузки FIRST-POSITION");
                        RequestLoad(indexStateLoad: 0, ref _isFirstPosition, indexReception: 0);
                        RequestUnload(indexStateLoad: 1, ref _isFirstPosition, indexReception: 0);
                        _isFirstPosition = false;
                    }
                    Debug.Log(_indexCurrentRoutePoint);
                }
            }

            void LastPosition()
            {
                Debug.Log(_indexCurrentRoutePoint);
                if (_objectTransport.transform.position == _ItransportInteractRoute.routePoints[_indexCurrentRoutePoint])
                {
                    Debug.Log(_indexCurrentRoutePoint);
                    if (_indexCurrentRoutePoint > 0) _indexCurrentRoutePoint--;
                    else if (_indexCurrentRoutePoint == 0)
                    {
                        Debug.Log("машина на месте и готова для погрузки SECOND-POSITION");
                        RequestLoad(indexStateLoad: 2, ref _isFirstPosition, indexReception: 1);
                        RequestUnload(indexStateLoad: 3, ref _isFirstPosition, indexReception: 1);
                        _isFirstPosition = true;
                    }
                }
            }
        }

        private void Move(in int indexPositionRoute)
        {
            _objectTransport.transform.position = Vector3.MoveTowards(_objectTransport.transform.position,
                                                    _ItransportInteractRoute.routePoints[indexPositionRoute],
                                                    _typeTransport.speed * Time.deltaTime);
        }

        private void RequestLoad(in byte indexStateLoad, ref bool isFirstPosition, in byte indexReception)
        {
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "RequestLoad()");
            if (_loadAndUnloadStates[indexStateLoad])
            {
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Запрос на погрузку проверяется");
                if (_ItransportInteractRoute.GetPointsReception()[indexReception].RequestConnectionToLoadRes(_typeTransport.capacity).confirmRequest)
                {
                    DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Запрос на погрузку принят");
                    _productLoad = _ItransportInteractRoute.GetPointsReception()[indexReception].RequestConnectionToLoadRes(_typeTransport.capacity).quantityPerLoad;
                }
            }
        }

        private void RequestUnload(in byte indexStateLoad, ref bool isFirstPosition, in byte indexReception)
        {
            if (_loadAndUnloadStates[indexStateLoad])
            {
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Запрос на разгрузку отправлен", "Transport");
                if (_ItransportInteractRoute.GetPointsReception()[indexReception].RequestConnectionToUnloadRes(_productLoad))
                {
                    DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Запрос на разгрузку принят", "Transport");
                    _productLoad = 0;
                    DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Машина разгружена и отправлена", "Transport");
                    DebugSystem.Log(_isFirstPosition, DebugSystem.SelectedColor.Green, "Состояние позиции", "Transport");
                }
            }
        }

#if UNITY_EDITOR
        public void ChangeLoadUnloadStates(in byte index, in bool isState)
        {
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"{_loadAndUnloadStates[index]}", "Transport", "Control");
            if (index <= _loadAndUnloadStates.Length - 1)
                _loadAndUnloadStates[index] = isState;
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"{_loadAndUnloadStates[index]}", "Transport", "Control");
        }
#endif
    }
}
