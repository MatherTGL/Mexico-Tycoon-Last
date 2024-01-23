using UnityEngine;
using Sirenix.OdinInspector;
using Config.Building.Deliveries;

namespace Resources
{
    [CreateAssetMenu(fileName = "CostResourcesConf", menuName = "Config/Data/Costs/Resources/Create New", order = 50)]
    public sealed class CostResourcesConfig : ScriptableObject
    {
        [SerializeField, Required]
        private ConfigContractsEditor _configForContracts;
        public ConfigContractsEditor configForContracts => _configForContracts;

        [SerializeField]
        private uint[] _costSellResources;

        [SerializeField, ReadOnly]
        private uint[] _costBuyResources;

        [SerializeField, MinValue(0.5f)]
        private float _costDifference = 1.3f;


        private void OnValidate()
        {
            _costBuyResources = new uint[_costSellResources.Length];

            for (int i = 0; i < _costSellResources.Length; i++)
                _costBuyResources[i] = (uint)(_costSellResources[i] * _costDifference);

#if UNITY_EDITOR
            if (_configForContracts == null)
                throw new System.Exception($"_configForContracts equal null in config({name})");
#endif
        }

        public uint[] GetCostsSellResources() { return _costSellResources; }

        public uint[] GetCostsBuyResources() { return _costBuyResources; }

#if UNITY_EDITOR
        [Button("Add Element")]
        private void AddElementCost(in TypeProductionResources.TypeResource _typeResource,
                                    in uint cost)
        {
            _costSellResources[(int)_typeResource] = cost;
        }
#endif
    }
}
