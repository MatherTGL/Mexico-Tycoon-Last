using UnityEngine;
using Sirenix.OdinInspector;
using Route.Builder;
using UnityEngine.Rendering;
using Transport;

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

        [SerializeField, BoxGroup("Parameters")]
        private SerializedDictionary<TypeTransport.Type, float> d_maxLengthRoute = new()
        {
            { TypeTransport.Type.Air, 100 },
            { TypeTransport.Type.Ground, 20 },
            { TypeTransport.Type.Marine, 200 }
        };
        public SerializedDictionary<TypeTransport.Type, float> maxLengthRoutes => d_maxLengthRoute;

        [SerializeField, MinValue(100), BoxGroup("Parameters")]
        [Tooltip("total cost = _costRoute * (Mathf.Abs distance) between objects")]
        private double _costRoute = 100;
        public double costRoute => _costRoute;
    }
}
