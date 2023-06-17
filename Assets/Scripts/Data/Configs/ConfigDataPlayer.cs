using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.Data.Player
{
    [CreateAssetMenu(fileName = "ConfigDataPlayerDefault", menuName = "Config/Data/Data Player/Create New", order = 50)]
    public sealed class ConfigDataPlayer : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters")]
        private double _startPlayerMoney;
        public double startPlayerMoney => _startPlayerMoney;

        [SerializeField, BoxGroup("Parameters")]
        private ushort _startPlayerResearchPoints;
        public ushort startPlayerResearchPoints => _startPlayerResearchPoints;
    }
}
