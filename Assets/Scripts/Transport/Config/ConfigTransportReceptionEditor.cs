using UnityEngine;
using Sirenix.OdinInspector;
using static Building.BuildingEnumType;

namespace Config.Transport.Reception
{
    [CreateAssetMenu(fileName = "ConfigTransportReception", menuName = "Config/Reception/Create New", order = 50)]
    public sealed class ConfigTransportReceptionEditor : ScriptableObject
    {
        [SerializeField, EnumPaging]
        private TypeBuilding[] _typeConnectBuildings;
        public TypeBuilding[] typeConnectBuildings => _typeConnectBuildings;

        [SerializeField, EnumPaging]
        private TypeBuilding _typeCurrentBuilding;
        public TypeBuilding typeCurrentBuilding => _typeCurrentBuilding;

        [SerializeField]
        private byte _defaultConnectionCount;
        public byte defaultConnectionCount => _defaultConnectionCount;
    }
}
