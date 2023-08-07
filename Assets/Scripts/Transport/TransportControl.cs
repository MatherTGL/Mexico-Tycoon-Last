using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


namespace Transport
{
    public sealed class TransportControl : MonoBehaviour
    {
        [SerializeField, BoxGroup("Parameters"), Tooltip("Массив всех видов транспорта использующегося в игре"), ReadOnly]
        private KindTransport[] _kindTransports;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, TransportObject[]> d_allTransportRoad = new Dictionary<string, TransportObject[]>();

        [SerializeField, BoxGroup("Parameters"), Required, PreviewField]
        private TransportObject _prefabTransportObject;


#if UNITY_EDITOR
        [PropertySpace(10.0f, 0.0f), Button("Update Kind Transport"), BoxGroup("Parameters"), HorizontalGroup("Parameters/Buttons")]
        private void UpdateArrayKindTransport()
        {
            _kindTransports = (KindTransport[])Resources.FindObjectsOfTypeAll(typeof(KindTransport));
        }

        [SerializeField, BoxGroup("Parameters"), MinValue(0), MaxValue("@d_allTransportRoad.Count")]
        [InlineButton("NextRoadIndex", SdfIconType.ArrowRight), InlineButton("PreviousRoadIndex", SdfIconType.ArrowLeft)]
        private byte _indexRoad;

        [PropertySpace(10.0f, 0.0f), Button("Add New Transport"), BoxGroup("Parameters"), HorizontalGroup("Parameters/Buttons")]
        private void AddNewTransport()
        {
            TransportObject createdObjectTransport = Instantiate(_prefabTransportObject, Vector3.zero, Quaternion.identity);

            for (int i = 0; i < d_allTransportRoad.ElementAt(_indexRoad).Value.Length; i++)
            {
                if (d_allTransportRoad.ElementAt(_indexRoad).Value[i] is null)
                {
                    d_allTransportRoad.ElementAt(_indexRoad).Value[i] = createdObjectTransport;
                    createdObjectTransport.SetKindTransportAndIndexRoad(_kindTransports[_indexTransport], d_allTransportRoad.ElementAt(_indexRoad).Key);
                    createdObjectTransport = null;
                    return;
                }
            }
        }

        private void PreviousRoadIndex()
        {
            if (_indexRoad > 0)
            {
                _indexRoad--;
                Debug.Log(d_allTransportRoad.ElementAt(_indexRoad));
            }
        }

        private void NextRoadIndex()
        {
            if (_indexRoad < d_allTransportRoad.Count - 1)
            {
                _indexRoad++;
                Debug.Log(d_allTransportRoad.ElementAt(_indexRoad));
            }
        }

        [SerializeField, BoxGroup("Parameters"), MinValue(0), MaxValue("@_kindTransports.Length - 1")]
        [InlineButton("NextTransportIndex", SdfIconType.ArrowRight), InlineButton("PreviousTransportIndex", SdfIconType.ArrowLeft)]
        private byte _indexTransport;

        private void PreviousTransportIndex()
        {
            if (_indexTransport > 0)
            {
                _indexTransport--;
                Debug.Log(_kindTransports[_indexTransport]);
            }
        }

        private void NextTransportIndex()
        {
            if (_indexTransport < _kindTransports.Length - 1)
            {
                _indexTransport++;
                Debug.Log(_kindTransports[_indexTransport]);
            }
        }
#endif


        public void AddNewRoad(string indexRoad)
        {
            d_allTransportRoad.Add(indexRoad, new TransportObject[5]); //? можно добавить улучшения гаража или что-то типо того
        }
    }
}
