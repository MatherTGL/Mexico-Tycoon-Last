using UnityEngine;
using Sirenix.OdinInspector;
using Road;
using System.Collections;
using System.Threading;

namespace Transport
{
    public sealed class TransportObject : MonoBehaviour
    {
        private const byte _startedPosition = 1;
        private const byte _endPosition = 0;

        [SerializeField, BoxGroup("Parameters"), ReadOnly]
        private RoadControl _roadControl;

        [SerializeField, BoxGroup("Parameters"), ReadOnly]
        private KindTransport _kindTransport;

        [SerializeField, BoxGroup("Parameters"), ReadOnly]
        private string _routeIndex;

        [SerializeField, BoxGroup("Parameters"), ReadOnly]
        private Vector2[] _fromAndToPositions = new Vector2[2];

        [SerializeField, BoxGroup("Parameters")]
        private bool _isStartedPosition = true;

        [SerializeField, BoxGroup("Parameters")]
        private float _lerpSpeedMove = 0.4f;


        public void SetKindTransportAndIndexRoad(KindTransport kindTransport, string indexRoad)
        {
            _kindTransport = kindTransport;
            _routeIndex = indexRoad;
            _roadControl = FindObjectOfType<RoadControl>();
            GetPositions();
        }

        private void GetPositions()
        {
            _fromAndToPositions[_endPosition] = _roadControl.dictionaryBuildedRoad[_routeIndex].fromAndToPositions[_endPosition];
            _fromAndToPositions[_startedPosition] = _roadControl.dictionaryBuildedRoad[_routeIndex].fromAndToPositions[_startedPosition];
            SetStartedPosition();
        }

        private void SetStartedPosition()
        {
            transform.position = _fromAndToPositions[_startedPosition];
        }

        private void Start() => StartCoroutine(RepearLerpTransport());

        private void FixedUpdate()
        {
            if (_isStartedPosition is false)
                transform.position = Vector3.Lerp(transform.position, _fromAndToPositions[_endPosition], _lerpSpeedMove
                                                                                                         * Time.deltaTime
                                                                                                         * _kindTransport.maxSpeed);
            else
                transform.position = Vector3.Lerp(transform.position, _fromAndToPositions[_startedPosition], _lerpSpeedMove
                                                                                                             * Time.deltaTime
                                                                                                             * _kindTransport.maxSpeed);
        }

        private IEnumerator RepearLerpTransport()
        {
            while (true)
            {
                _isStartedPosition = !_isStartedPosition;
                yield return new WaitForSecondsRealtime(4);
            }
        }
    }
}