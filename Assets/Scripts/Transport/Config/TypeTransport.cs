using UnityEngine;
using Sirenix.OdinInspector;
using Resources;

namespace Transport
{
    [CreateAssetMenu(fileName = "ConfigTypeTransport", menuName = "Config/Transport/Create New", order = 50)]
    public sealed class TypeTransport : ScriptableObject
    {
        [SerializeField]
        private GameObject _prefab;
        public GameObject prefab => _prefab;

        [SerializeField]
        private TypeProductionResources.TypeResource _typeResource;
        public TypeProductionResources.TypeResource typeResource => _typeResource;

        [SerializeField, MinValue(0.5f)]
        private float _speed;
        public float speed => _speed;

        [SerializeField, MinValue(1.0f)]
        private float _capacity;
        public float capacity => _capacity;

        [SerializeField, MinValue(30)]
        private ushort _maintenanceExpenses = 30;
        public ushort maintenanceExpenses => _maintenanceExpenses;
    }
}
