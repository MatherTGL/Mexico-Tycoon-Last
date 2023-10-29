using UnityEngine;
using Sirenix.OdinInspector;
using Route.Builder;

namespace Config.Building.Route
{
    [CreateAssetMenu(fileName = "RouteBuilderConfigDefault", menuName = "Config/Route/Create New", order = 50)]
    public sealed class ConfigRouteBuilderEditor : ScriptableObject
    {
        [SerializeField, ReadOnly, BoxGroup("Parameters")]
        private byte _maxPointConnection = 2;
        public byte maxPointConnection => _maxPointConnection;

        [SerializeField, Required, BoxGroup("Parameters")]
        private CreatorCurveRoadControl _prefabRoute;
        public CreatorCurveRoadControl prefabRoute => _prefabRoute;

        [SerializeField, MinValue(10), MaxValue(250), BoxGroup("Parameters")]
        private byte _maxLengthRoute = 20;
        public byte maxLengthRoute => _maxLengthRoute;

        [SerializeField, MinValue(100), BoxGroup("Parameters")]
        [Tooltip("total cost = _costRoute * (Mathf.Abs distance) between objects")]
        private double _costRoute = 100;
        public double costRoute => _costRoute;
    }
}
