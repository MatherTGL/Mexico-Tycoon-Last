using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building.View
{
    public sealed partial class BuildingView : MonoBehaviour
    {
        private void BuyBuilding()
        {
            _IgetBuildingViewFunctions?.BuyBuilding();
            _dataChanged?.Invoke();
        }

        private void SellBuilding()
        {
            _IgetBuildingViewFunctions?.SellBuilding();
            _dataChanged?.Invoke();
        }

        private void ActivateBuilding()
        {
            _IgetBuildingViewFunctions?.SetActivateBuilding();
            _dataChanged?.Invoke();
        }

        private void DeactivateBuilding()
        {
            _IgetBuildingViewFunctions?.SetDeactivateBuilding();
            _dataChanged?.Invoke();
        }

        private void SetNewProductionResource(TypeResource typeResource)
        {

        }

        private void ChangeFarmType()
        {

        }
    }
}
