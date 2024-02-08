using UnityEngine;
using Sirenix.OdinInspector;
using SerializableDictionary.Scripts;
using static Resources.TypeProductionResources;

namespace Config.Building.Deliveries
{
    [CreateAssetMenu(fileName = "ConfigDeliveries", menuName = "Config/Buildings/Additional/Deliveries/Create New", order = 50)]
    public sealed class ConfigContractsEditor : ScriptableObject
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

        [SerializeField, MinValue(10), MaxValue(1000)]
        private ushort _remainingContractTime = 180;
        public ushort remainingContractTime => _remainingContractTime;

        [SerializeField, MinValue(10), MaxValue(1000)]
        private ushort _contractRenewalTime = 30;
        public ushort contractRenewalTime => _contractRenewalTime;

        [SerializeField, BoxGroup("Individual Contracts")]
        private SerializableDictionary<TypeResource, float[]> d_percentageUsers = new();
        public SerializableDictionary<TypeResource, float[]> percentageUsers => d_percentageUsers;
    }
}
