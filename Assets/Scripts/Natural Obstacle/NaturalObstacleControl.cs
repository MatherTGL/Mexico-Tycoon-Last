using UnityEngine;
using Sirenix.OdinInspector;
using Config.Obstacle;

namespace Obstacle
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public sealed class NaturalObstacleControl : MonoBehaviour, IObstacle
    {
        [SerializeField, Required, HideLabel]
        private ConfigObstacleEditor _config;
        ConfigObstacleEditor IObstacle.config => _config;


        private NaturalObstacleControl() { }
    }
}
