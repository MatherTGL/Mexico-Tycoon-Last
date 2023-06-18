using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Data;


namespace City
{
    public sealed class CityControl : MonoBehaviour, IBoot
    {
        [SerializeField, BoxGroup("Parameters"), Title("Population in City"), HideLabel]
        [MinValue(0), MaxValue(double.MaxValue / 2)]
        private double _populationCity;

        [SerializeField, BoxGroup("Parameters"), Title("Police Level in star (0-10)"), HideLabel]
        [MinValue(0), MaxValue(10), Tooltip("Уровень полиции в данном городе")]
        private byte _policeLevel;

        [SerializeField, BoxGroup("Parameters"), Title("Max Capacity Stock in kg)"), HideLabel]
        [MinValue(0.0f), Tooltip("Максимальная вместимость хранилища города в кг")]
        private float _maxCapacityStock;

        [SerializeField, BoxGroup("Parameters"), Title("Current Capacity Stock in kg)"), HideLabel]
        [MinValue(0.0f), Tooltip("Текущая вместимость хранилища города в кг"), ReadOnly]
        private float _currentCapacityStock;

        [FoldoutGroup("Parameters/Drugs")]
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Percentage of Users Cocaine in %"), HideLabel]
        [MinValue(1.0f), MaxValue(80.0f), Tooltip("Процент наркоманов употребляющих кокаин в данном городе")]
        private float _percentageUsersCocaine;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Average Cost Cocaine in $/kg"), HideLabel]
        private uint _averageCostCocaine;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Average Cost Cocaine in $/kg"), HideLabel]
        [MinValue(1)]
        private ushort _weightToSellCocaine; //? сменить на float для игрока

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Current Drug Demand Cocaine in kg/day"), HideLabel]
        [MinValue(0.0f)]
        private float _currentDrugDemandCocaine;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Increased Demand Cocaine in kg/day"), HideLabel]
        [MinValue(0.01f)]
        private float _increasedDemandCocaine;


        public void InitAwake()
        {
            Debug.Log("Город успешно инициализирован");
        }

        public bool CheckCurrentCapacityStock()
        {
            if (_currentCapacityStock < _maxCapacityStock) return true;
            else return false;
        }

        public void IngestResources(float decliningDemand) //todo докинуть тип наркотика
        {
            if (_currentDrugDemandCocaine > decliningDemand)
            {
                _currentDrugDemandCocaine += _increasedDemandCocaine - decliningDemand;
                _currentCapacityStock += decliningDemand;
                SellDrugs();
            }
            else
                _currentDrugDemandCocaine += _increasedDemandCocaine;
            Debug.Log(decliningDemand);
        }

        private void SellDrugs()
        {
            if (_currentCapacityStock > _weightToSellCocaine)
            {
                _currentCapacityStock -= _weightToSellCocaine;
                DataControl.IdataPlayer.AddPlayerMoney(_averageCostCocaine);
                Debug.Log("Продано 1kg");
            }
        }
    }
}
