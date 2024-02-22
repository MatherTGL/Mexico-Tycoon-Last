using UnityEngine;
using Transport.Reception;
using Sirenix.OdinInspector;
using Transport;

namespace Route.Builder
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(RouteTransportControl))]
    public sealed class CreatorCurveRoadControl : MonoBehaviour, ICreatorCurveRoad
    {
        private const byte _indexPositionPointsFrom = 0, _indexPositionPointsTo = 1;

        private const byte _maxPositionPoints = 2;

        [SerializeField]
        private LineRenderer _lineRenderer;

        [ShowInInspector]
        private ITransportReception[] _positionPoints;

        [SerializeField, EnumToggleButtons]
        private TypeTransport.Type _typeRoute;
        TypeTransport.Type ICreatorCurveRoad.typeRoute => _typeRoute;

        [SerializeField]
        private float _offsetCenterPoint;

        [SerializeField]
        private float[] _numbersParameters;

        [SerializeField]
        private byte _numberOfPoints;

        [SerializeField]
        private byte _minLenghtLongRoute = 8;

        [SerializeField]
        private float _biasDividerMin = 2.0f, _biasDividerMax = 3.0f, _biasDividerCurrent = 1.0f;


        private CreatorCurveRoadControl() { }

        private void DrawBizierCurve()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _numberOfPoints;

            GetRandomBias();
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            float t;
            Vector3 positionPoint;

            for (byte i = 0; i < _numberOfPoints; i++)
            {
                t = i / (_numberOfPoints - _numbersParameters[0]);
                Vector3 p0 = (_numbersParameters[1] - t) * (_numbersParameters[2] - t)
                            * _positionPoints[0].GetPosition().position;
                Vector3 p1 = _numbersParameters[3] * t * (_numbersParameters[4] - t) * FindCenterPoint();
                Vector3 p2 = t * t * _positionPoints[1].GetPosition().position;
                positionPoint = p0 + p1 + p2;

                _lineRenderer.SetPosition(i, positionPoint);
            }
        }

        private Vector3 FindCenterPoint()
        {
            _offsetCenterPoint = Vector3.Distance(_positionPoints[_indexPositionPointsFrom].GetPosition().position,
                                                  _positionPoints[_indexPositionPointsTo].GetPosition().position) / _biasDividerCurrent;

            Vector3 centerPosition = (_positionPoints[_indexPositionPointsFrom].GetPosition().position
                + _positionPoints[_indexPositionPointsTo].GetPosition().position) / 2;

            CalculateDirectionOfCurvature(ref centerPosition);
            return centerPosition;
        }

        private void CalculateDirectionOfCurvature(ref Vector3 centerPosition)
        {
            var pointTo = _positionPoints[_indexPositionPointsTo].GetPosition().position.y;
            var pointFrom = _positionPoints[_indexPositionPointsFrom].GetPosition().position.y;

            if (pointTo > pointFrom)
            {
                centerPosition.x += _offsetCenterPoint;
                centerPosition.y += _offsetCenterPoint;
            }
            else if (pointFrom > pointTo)
            {
                centerPosition.x -= _offsetCenterPoint;
                centerPosition.y -= _offsetCenterPoint;
            }
        }

        //TODO: refactoring
        private float GetRandomBias()
        {
            Vector3 distancePointA = _positionPoints[_indexPositionPointsFrom].GetPosition().position;
            Vector3 distancePointB = _positionPoints[_indexPositionPointsTo].GetPosition().position;

            if (Vector3.Distance(distancePointA, distancePointB) >= _minLenghtLongRoute)
                return _biasDividerCurrent = Random.Range(_biasDividerMax * 0.5f, _biasDividerMax); //min\max long
            else
                return _biasDividerCurrent = Random.Range(_biasDividerMin * 0.7f, _biasDividerMin);
        }

        Vector3[] ICreatorCurveRoad.GetRoutePoints()
        {
            Vector3[] allRoutePoints = new Vector3[_numberOfPoints];

            for (byte i = 0; i < _lineRenderer.positionCount; i++)
                allRoutePoints[i] = _lineRenderer.GetPosition(i);

            return allRoutePoints;
        }

        Vector3 ICreatorCurveRoad.GetRouteMainPoint()
            => _positionPoints[_indexPositionPointsFrom].GetPosition().position;

        ITransportReception[] ICreatorCurveRoad.GetPointsConnectionRoute() => _positionPoints;

        public void Generate(in ITransportReception firstPoint, in ITransportReception secondPoint, in TypeTransport.Type typeRoute)
        {
            _positionPoints = new ITransportReception[_maxPositionPoints];
            _positionPoints[_indexPositionPointsFrom] = firstPoint;
            _positionPoints[_indexPositionPointsTo] = secondPoint;
            _typeRoute = typeRoute;

            DrawBizierCurve();
        }
    }
}
