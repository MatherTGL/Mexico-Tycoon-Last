using UnityEngine;
using Sirenix.OdinInspector;

namespace Resources
{
    [CreateAssetMenu(fileName = "CostResourcesConf", menuName = "Config/Data/Costs/Resources/Create New", order = 50)]
    public sealed class CostResourcesConfig : ScriptableObject
    {
        [SerializeField]
        private uint[] _costsSellResources;

        [SerializeField, ReadOnly]
        private uint[] _costBuyResources;

        [SerializeField, MinValue(0.5f)]
        private float _costDifference = 1.3f;


        private void OnValidate()
        {
            _costBuyResources = new uint[_costsSellResources.Length];

            for (int i = 0; i < _costsSellResources.Length; i++)
                _costBuyResources[i] = (uint)(_costsSellResources[i] * _costDifference);
        }

        public uint[] GetCostsSellResources() { return _costsSellResources; }

        public uint[] GetCostsBuyResources() { return _costBuyResources; }

#if UNITY_EDITOR
        [Button("Add Element")]
        private void AddElementCost(in TypeProductionResources.TypeResource _typeResource,
                                    in uint cost)
        {
            _costsSellResources[(int)_typeResource] = cost;
        }
#endif
    }
}
