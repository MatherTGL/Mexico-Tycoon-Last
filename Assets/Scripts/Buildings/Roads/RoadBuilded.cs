using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


namespace Road
{
    public sealed class RoadBuilded : MonoBehaviour
    {
        //*лист с транспоротом на маршруте
        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, float> _allTransportingDrugs = new Dictionary<string, float>();

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
            Debug.Log("Road Init");
        }

        public void DecliningDemandUpdate(in float addResEveryStep, in string typeFabricDrug)
        {
            if (_allTransportingDrugs.ContainsKey(typeFabricDrug))
                _allTransportingDrugs[typeFabricDrug] = addResEveryStep;
            else
                _allTransportingDrugs.Add(typeFabricDrug, addResEveryStep);
        }
    }
}
