using System.Net.Mime;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Transport
{
    public class KindTransport : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters"), PreviewField]
        protected Image _icon;

        protected enum TypeTransport
        {
            car, aircraft, ship, boat
        }

        [SerializeField, BoxGroup("Parameters"), EnumPaging]
        protected TypeTransport _typeTransport;

        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f)]
        protected float _maxSpeed;
        public float maxSpeed => _maxSpeed;

        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f)]
        protected float _capacity;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        protected double _buyCost;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        protected double _sellCost;

        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f)]
        protected double _maintenanceCost;

        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f)]
        protected float _reliabilityLevelInPercent;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        protected uint _transportationDistance;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        protected ushort _enginePower;
    }
}
