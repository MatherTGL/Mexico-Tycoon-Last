using UnityEngine;
using Sirenix.OdinInspector;
using Transport;


namespace Config.Obstacle
{
    [CreateAssetMenu(menuName = "Config/Obstacle/Create new", fileName = "ConfigObstacle", order = 50)]
    public sealed class ConfigObstacleEditor : ScriptableObject
    {
        [SerializeField]
        private TypeTransport.Type _typeObstacle;
        public TypeTransport.Type typeObstacle => _typeObstacle;

        [SerializeField, MinValue(0.01f), MaxValue(0.40f)]
        private float _percentageImpactSpeed = 0.05f;
        public float percentageImpactSpeed => _percentageImpactSpeed;
    }
}
