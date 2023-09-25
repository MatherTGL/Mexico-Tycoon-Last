using UnityEngine;
using Sirenix.OdinInspector;
using Config.Obstacle;

namespace Obstacle
{
    public sealed class NaturalObstacleControl : MonoBehaviour, IObstacle
    {
        [SerializeField, Required]
        private ConfigObstacleEditor _config;
        ConfigObstacleEditor IObstacle.config => _config;
    }
}
