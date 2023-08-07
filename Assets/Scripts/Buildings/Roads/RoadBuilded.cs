using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


namespace Road
{
    public sealed class RoadBuilded
    {
        private Dictionary<string, float> d_allTransportingDrugs = new Dictionary<string, float>();

        private RoadResourcesManagement _roadResourceManagement;
        public RoadResourcesManagement roadResourcesManagement => _roadResourceManagement;

        private float _resTransportationTrafficCapacityMax = 10;

        private byte _patrolLevel;

        private byte _levelRobberies;

        private double _costMaintenance;

        private Vector2[] _fromAndToPositions = new Vector2[2];
        public Vector2[] fromAndToPositions => _fromAndToPositions;


        public RoadBuilded(Vector2 fromPosition, Vector2 toPosition)
        {
            _fromAndToPositions[0] = fromPosition;
            _fromAndToPositions[1] = toPosition;
            _roadResourceManagement = new RoadResourcesManagement();
            Debug.Log(_roadResourceManagement);
        }

        public void DecliningDemandUpdate(in float addResEveryStep, in string typeFabricDrug)
        {
            if (d_allTransportingDrugs.ContainsKey(typeFabricDrug))
                d_allTransportingDrugs[typeFabricDrug] = addResEveryStep;
            else
                d_allTransportingDrugs.Add(typeFabricDrug, addResEveryStep);
        }
    }
}
