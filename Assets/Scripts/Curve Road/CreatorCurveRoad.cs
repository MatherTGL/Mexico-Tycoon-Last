using UnityEngine;
using Transport.Reception;


namespace Route.Builder
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class CreatorCurveRoad : MonoBehaviour, ICreatorCurveRoad
    {
        private const byte _indexPositionPointsFrom = 0, _indexPositionPointsTo = 1;

        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private ITransportReception[] _positionPoints;

        [SerializeField]
        private float _offsetCenterPoint;

        [SerializeField]
        private float[] _numbersParameters;

        [SerializeField]
        private byte _numberOfPoints;

        [SerializeField]
        private byte _minLenghtLongRoute = 8;

        [SerializeField]
        private float _biasDividerMin = 2, _biasDividerMax = 3, _biasDividerCurrent = 1.0f;


        private CreatorCurveRoad() { }

        private void DrawBizierCurve()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _numberOfPoints;
            RandomBias();
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            float t; Vector3 positionPoint;

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
                + _positionPoints[_indexPositionPointsTo].GetPosition().position / 2);

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

        public void SetPositionPoints(ITransportReception firstPoint, ITransportReception secondPoint)
        {
            _positionPoints = new ITransportReception[2]; // Hardcode
            _positionPoints[_indexPositionPointsFrom] = firstPoint;
            _positionPoints[_indexPositionPointsTo] = secondPoint;

            DrawBizierCurve();
        }

        public Vector3[] GetRoutePoints()
        {
            Vector3[] allRoutePoints = new Vector3[_numberOfPoints];

            for (byte i = 0; i < _lineRenderer.positionCount; i++)
                allRoutePoints[i] = _lineRenderer.GetPosition(i);

            return allRoutePoints;
        }

        // Todo remove hardcode
        private float RandomBias()
        {
            Vector3 distancePointA = _positionPoints[_indexPositionPointsFrom].GetPosition().position;
            Vector3 distancePointB = _positionPoints[_indexPositionPointsTo].GetPosition().position;

            if (Vector3.Distance(distancePointA, distancePointB) >= _minLenghtLongRoute)
                return _biasDividerCurrent = Random.Range(_biasDividerMax * 0.5f, _biasDividerMax); //min\max long
            else
                return _biasDividerCurrent = Random.Range(_biasDividerMin * 0.7f, _biasDividerMin);
        }

        public Vector3 GetRouteMainPoint() { return _positionPoints[_indexPositionPointsFrom].GetPosition().position; }

        public ITransportReception[] GetPointsConnectionRoute() { return _positionPoints; }
    }
}
