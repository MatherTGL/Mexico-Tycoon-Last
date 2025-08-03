using Sirenix.OdinInspector;
using UnityEngine;

namespace Building.View
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class BuildingSelectViewHandle : MonoBehaviour
    {
        [SerializeField, Required, BoxGroup("Links")]
        private BuildingView _buildingView;


        private void OnMouseDown() => HandleClick();

        private void HandleClick()
        {
            _buildingView.GetComponent<IBuildingView>().FirstLoad(GetComponent<IGetBuildingViewFunctions>());
            //todo открывается панель о здании. Она будет общая, но инфа будет подгружаться из префабов
            // Обработка будет через интерфейс передаваться в уникальные классы, которые
            // дальше будут обрабатывать инфу
        }

        private void HandleExit()
        {
            //todo сделать обработку и освобождение данных при нажатии крестика или выходе
            // из панели здания
        }
    }
}
