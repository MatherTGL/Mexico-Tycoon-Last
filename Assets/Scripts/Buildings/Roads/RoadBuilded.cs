using UnityEngine;
using Sirenix.OdinInspector;


namespace Road
{
    public sealed class RoadBuilded : MonoBehaviour
    {
        //*лист с транспоротом на маршруте

        [SerializeField, BoxGroup("Parameters"), HideLabel, MinValue(0.0f)]
        [Title("Resource Transportation Traffic Capacity in %", horizontalLine: false)]
        private float _resTransportationTrafficCapacityCurrent;

        [SerializeField, BoxGroup("Parameters"), HideLabel, MinValue(0.0f)]
        [Title("Resource Transportation Traffic Capacity Max in kg/day", horizontalLine: false)]
        private float _resTransportationTrafficCapacityMax = 10;

        [SerializeField, BoxGroup("Parameters"), MinValue(1), MaxValue(10)]
        [Title("Patrol level", horizontalLine: false), HideLabel]
        private byte _patrolLevel;

        [SerializeField, BoxGroup("Parameters"), HideLabel, MinValue(1), MaxValue(10)]
        [Title("Robberies level", horizontalLine: false)]
        private byte _levelRobberies;

        [SerializeField, BoxGroup("Parameters"), HideLabel, MinValue(0)]
        [Title("Cost Maintenance", horizontalLine: false)]
        private double _costMaintenance;


        //? будет содержать инфу по дороге
        public void InitRoad()
        {
            Debug.Log("Дорога инициализирована");
        }

        public void AddDecliningDemand(float decliningDemand)
        {
            if (_resTransportationTrafficCapacityMax > _resTransportationTrafficCapacityCurrent)
            {
                _resTransportationTrafficCapacityCurrent += decliningDemand;
                Debug.Log(_resTransportationTrafficCapacityCurrent);
            }
            Debug.Log(decliningDemand);
        }

        public void ReduceDecliningDemand(float decliningDemand)
        {
            Debug.Log($"Reduce {decliningDemand}");
        }
    }
}
