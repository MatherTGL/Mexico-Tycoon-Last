using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Transport
{
    public sealed class TransportationMovement
    {
        private const float _fullSpeedPercentage = 1.0f;

        private readonly ITransportation _Itransportation;

        private readonly GameObject _someObject;

        private event Action<bool> _isInStartedPosition;
        public event Action<bool> isInStartedPosition
        {
            add => _isInStartedPosition += value;
            remove => _isInStartedPosition -= value;
        }

        private float _currentSpeed;

        private float _maxSpeed, _minSpeed;

        private byte _indexCurrentRoutePoint;

        private bool _isFirstPosition = true;
        public bool isFirstPosition { get => _isFirstPosition; set => _isFirstPosition = value; }


        public TransportationMovement(in ITransportation Itransportation, in GameObject someObject)
        {
            _Itransportation = Itransportation;
            _someObject = someObject;

            SetSpeed();
        }

        private void SetSpeed()
        {
            float availableSpeedAfterImpact = (_fullSpeedPercentage
                - _Itransportation.ItransportInteractRoute.impactOfObstaclesOnSpeed) / 100;

            _maxSpeed = _Itransportation.typeTransport.maxSpeed * availableSpeedAfterImpact * Time.fixedDeltaTime;
            _minSpeed = _maxSpeed * _Itransportation.typeTransport.minSpeedPercentageMaxSpeed;
        }

        private void CheckPosition()
        {
            var currentRoutePoint = _Itransportation.ItransportInteractRoute.routePoints[_indexCurrentRoutePoint];
            var lastRoutePoint = _Itransportation.ItransportInteractRoute.routePoints.Length - 1;

            if (_someObject.transform.position == currentRoutePoint)
            {
                if (_indexCurrentRoutePoint == 0)
                {
                    if (IsBuildingWorkingAndInvokeAction(true) == false)
                        return;
                }
                else if (_indexCurrentRoutePoint == lastRoutePoint)
                {
                    if (IsBuildingWorkingAndInvokeAction(false) == false)
                        return;
                }

                if (_isFirstPosition && _indexCurrentRoutePoint < lastRoutePoint)
                    _indexCurrentRoutePoint++;
                else if (_indexCurrentRoutePoint > 0)
                    _indexCurrentRoutePoint--;
            }
        }

        private void Move(in byte indexPositionRoute)
        {
            Vector3 currentPosition = _someObject.transform.position;
            Vector3 targetPosition = _Itransportation.ItransportInteractRoute.routePoints[indexPositionRoute];

            _someObject.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, _currentSpeed);
        }

        private bool IsBuildingWorkingAndInvokeAction(in bool isStartedPosition)
        {
            if (IsBuildingWork(isStartedPosition) == false)
                return false;

            _isInStartedPosition.Invoke(isStartedPosition);
            return true;
        }

        private bool IsBuildingWork(in bool isStartedPosition)
        {
            byte indexNextReception = Convert.ToByte(isStartedPosition);
            var jobStatus = _Itransportation.ItransportInteractRoute
                                            .GetPointsReception()[indexNextReception]
                                            .IbuildingRequest.IbuildingJobStatus;

            if (jobStatus == null)
                return true;
            else
                return jobStatus.isWorked;
        }

        public void ChangeSpeed()
            => _currentSpeed = Random.Range(_minSpeed, _maxSpeed);

        public void MovementTransport()
        {
            if (_Itransportation.transportationFuel.IsFuelAvailable() && _Itransportation.transportationBreakdowns.IsNotInRepair()
                    && _Itransportation.IsWait() == false)
            {
                CheckPosition();
                Move(_indexCurrentRoutePoint);
            }
        }

        public void ChangeRoute()
        {
            _indexCurrentRoutePoint = 0;
            _someObject.transform.position = _Itransportation.ItransportInteractRoute.routePoints[0];
        }
    }
}
