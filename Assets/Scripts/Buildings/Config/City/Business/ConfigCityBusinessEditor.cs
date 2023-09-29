using UnityEngine;
using Sirenix.OdinInspector;
using static Building.City.Business.CityBusiness;

namespace Config.Building.Business
{
    [CreateAssetMenu(fileName = "CityBusiness", menuName = "Config/Buildings/City/Business/Create New", order = 50)]
    public sealed class ConfigCityBusinessEditor : ScriptableObject
    {
        [SerializeField, EnumPaging]
        private TypeBusiness _typeBusiness;
        public TypeBusiness typeBusiness => _typeBusiness;

        [SerializeField]
        private float _percentageMoneyCleared = 0.6f;
        public float percentageMoneyCleared => _percentageMoneyCleared;

        [SerializeField, MinValue(0)]
        private double _costPurchase;
        public double costPurchase => _costPurchase;

        [SerializeField, MinValue(10)]
        private double _maintenanceCost = 10;
        public double maintenanceCost => _maintenanceCost; 
    }
}
