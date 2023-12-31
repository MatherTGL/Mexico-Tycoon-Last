using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Building.Deliveries
{
    [CreateAssetMenu(fileName = "ConfigDeliveries", menuName = "Config/Buildings/Additional/Deliveries/Create New", order = 50)]
    public sealed class ConfigDeliveriesEditor : ScriptableObject
    {
        [SerializeField, MinValue(0), MaxValue(10)]
        private byte _maxIndividualContracts = 2;
        public byte maxIndividualContracts => _maxIndividualContracts;

        [SerializeField, MinValue(10), MaxValue("@_maxPercentageOfMarketValue")]
        [Tooltip("Max high value difference from the total on the market")]
        private byte _minPercentageOfMarketValue = 10;
        public byte minPercentageOfMarketValue => _minPercentageOfMarketValue;
        
        [SerializeField, MinValue("@_minPercentageOfMarketValue"), MaxValue(90)]
        [Tooltip("Max high value difference from the total on the market")]
        private byte _maxPercentageOfMarketValue = 30;
        public byte maxPercentageOfMarketValue => _maxPercentageOfMarketValue;
    }
}
