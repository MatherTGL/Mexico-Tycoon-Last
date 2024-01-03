using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Resources;

namespace Building.Farm
{
    public sealed class WarehouseCleaning : MonoBehaviour
    {
        private ICleaningResources _building;


        public void Init(in ICleaningResources cleaningResources)
        {
            _building = cleaningResources;
        }

        [Button("Clear")]
        private void Clear(TypeProductionResources.TypeResource typeResource, double amount) => _building.Clear(typeResource, amount);
    }
}
