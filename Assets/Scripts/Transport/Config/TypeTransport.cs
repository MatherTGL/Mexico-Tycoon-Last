using UnityEngine;
using Sirenix.OdinInspector;
using Resources;

namespace Transport
{
    [CreateAssetMenu(fileName = "ConfigTypeTransport", menuName = "Config/Transport/Create New", order = 50)]
    public sealed class TypeTransport : ScriptableObject
    {
        [SerializeField]
        private GameObject _prefab;
        public GameObject prefab => _prefab;

        [SerializeField]
        private TypeProductionResources.TypeResource _typeResource;
        public TypeProductionResources.TypeResource typeResource => _typeResource;

        public enum Type : byte
        {
            Ground, Air, Marine
        }

        [SerializeField, EnumToggleButtons]
        private Type _type;
        public Type type => _type;

        [SerializeField, MinValue(0.5f)]
        private float _maxSpeed;
        public float maxSpeed => _maxSpeed;

        [SerializeField, Tooltip("Percentage of maximum minimum speed from max speed")]
        private float _minSpeedPercentageMaxSpeed = 0.9f;
        public float minSpeedPercentageMaxSpeed => _minSpeedPercentageMaxSpeed; 

        [SerializeField, MinValue(1.0f)]
        private float _capacity;
        public float capacity => _capacity;

        [SerializeField, MinValue(30)]
        private ushort _maintenanceExpenses = 30;
        public ushort maintenanceExpenses => _maintenanceExpenses;

        [SerializeField, MinValue(50.0f)]
        private float _maxFuelLoad = 50.0f;
        public float maxFuelLoad => _maxFuelLoad;

        [SerializeField, MinValue(1.0f), MaxValue("@_maxFuelConsumptionInTimeStep")]
        private float _minFuelConsumptionInTimeStep = 1.0f;
        public float minFuelConsumptionInTimeStep => _minFuelConsumptionInTimeStep;

        [SerializeField, MinValue("@_minFuelConsumptionInTimeStep")]
        private float _maxFuelConsumptionInTimeStep = 2.0f;
        public float maxFuelConsumptionInTimeStep => _maxFuelConsumptionInTimeStep;

        [SerializeField, MinValue(1.0f)]
        private float _fillingFuelRatePerTimeStep = 1.0f;
        public float fillingFuelRatePerTimeStep => _fillingFuelRatePerTimeStep;

        [SerializeField, MinValue(0.1f)]
        private float _fuelCostPerLiter = 0.2f;
        public float fuelCostPerLiter => _fuelCostPerLiter;

        [SerializeField, MinValue(10)]
        private double _costPurchase;
        public double costPurchase => _costPurchase;

        [SerializeField, MinValue(10)]
        private double _repairCost = 10;
        public double repairCost => _repairCost;

        [SerializeField, ReadOnly]
        private byte _minStrength;
        public byte minStrength => _minStrength;

        [SerializeField, MinValue(100)]
        private ushort _maxStrength = 100;
        public ushort maxStrength => _maxStrength;

        [SerializeField, MinValue(1)]
        private byte _damageInflicted = 10;
        public byte damageInflicted => _damageInflicted;

        [SerializeField, MinValue(1)]
        private byte _speedOfRepairPerTimeStep = 1;
        public byte speedOfRepairPerTimeStep => _speedOfRepairPerTimeStep;

        [SerializeField, MinValue(1), MaxValue(10)]
        private byte _timeLoadAndUnloadInSeconds = 3;
        public byte timeLoadAndUnloadInSeconds => _timeLoadAndUnloadInSeconds;
    }
}
