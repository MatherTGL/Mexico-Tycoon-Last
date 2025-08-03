using System;
using System.Collections.Generic;
using Building.Additional;
using Building.Additional.Production;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Building.View
{
    //todo переписать на загрузку через addressables
    public sealed partial class BuildingView : MonoBehaviour, IBuildingView
    {
        [SerializeField, Required, BoxGroup("Main")]
        private GameObject _rootForButtons;

        [SerializeField, Required, BoxGroup("Links")]
        private GameObject _buildingViewPanel;

        [SerializeField, Required, BoxGroup("Buttons")]
        private Button _buttonPrefab;

        [SerializeField, Required, BoxGroup("Dropdown")]
        private TMP_Dropdown _productionResourcesDropdown;

        [SerializeField, Required, BoxGroup("Texts")]
        private TextMeshProUGUI _textWorkStatus;

        [SerializeField, Required, BoxGroup("Texts")]
        private TextMeshProUGUI _textBuyedStatus;

        private Action _dataChanged;

        private static IGetBuildingViewFunctions _IgetBuildingViewFunctions;

        private static List<Button> _spawnedButtons = new();

        private static bool _isActived;


        void IBuildingView.FirstLoad(IGetBuildingViewFunctions IBuildingViewFunctions)
        {
            if (_isActived != false)
                return;

            _dataChanged += UpdateView;
            _isActived = true;
            _IgetBuildingViewFunctions = IBuildingViewFunctions;
            InitBuildingView();
        }

        private void InitBuildingView()
        {
            List<Type> types = new()
            {
                typeof(IBuildingPurchased),
                typeof(IBuildingJobStatus)
            };

            Dictionary<Type, InteractionBuilding> methodsInInterfaces = new()
            {
                {
                    typeof(IBuildingPurchased), new InteractionBuilding
                    {
                     methods = new List<Action> { BuyBuilding, SellBuilding },
                     texts = new List<string> { "Buy", "Sell" }
                    }
                },
                {
                    typeof(IBuildingJobStatus), new InteractionBuilding
                    {
                     methods = new List<Action> { ActivateBuilding, DeactivateBuilding },
                     texts = new List<string> { "Activate", "Deactivate" }
                    }
                }
            };

            for (byte i = 0; i < types.Count; i++)
                if (!types[i].IsAssignableFrom(_IgetBuildingViewFunctions.GetBuilding().GetType()))
                    types.RemoveAt(i);

            GenerateButtons(methodsInInterfaces);
            GenerateTextData();
            GenerateDropdown();
            _buildingViewPanel.gameObject.SetActive(true);
        }

        //TODO рефакторинг
        private void GenerateButtons(Dictionary<Type, InteractionBuilding> methodsInInterfaces)
        {
            foreach (var item in methodsInInterfaces.Keys)
            {
                for (byte i = 0; i < methodsInInterfaces[item].methods.Count; i++)
                {
                    int currentIndex = i;
                    _spawnedButtons.Add(Instantiate(_buttonPrefab, transform.position, Quaternion.identity, _rootForButtons.transform));
                    _spawnedButtons[^1].onClick.AddListener(() => methodsInInterfaces[item].methods[currentIndex]?.Invoke());
                    _spawnedButtons[^1].GetComponentInChildren<TextMeshProUGUI>().text = methodsInInterfaces[item].texts[i];
                }
            }
        }

        private void GenerateTextData()
        {
            var jobStatus = _IgetBuildingViewFunctions.GetBuilding() as IBuildingJobStatus;
            var buyedStatus = _IgetBuildingViewFunctions.GetBuilding() as IBuildingPurchased;

            if (jobStatus == null)
            {
                _textWorkStatus.enabled = false;
                Debug.Log("Здание не реализует IBuildingJobStatus");
                return;
            }
            _textWorkStatus.enabled = true;

            if (buyedStatus == null)
            {
                _textBuyedStatus.enabled = false;
                return;
            }
            _textBuyedStatus.enabled = true;
            _dataChanged?.Invoke();
            Debug.Log("Игрок может получить статус активности здания");
        }

        private void GenerateDropdown()
        {
            //_productionResources
            _productionResourcesDropdown.ClearOptions();
            var productionBuilding = _IgetBuildingViewFunctions.GetBuilding() as IProductionBuilding;

            _productionResourcesDropdown.enabled = productionBuilding != null;

            if (productionBuilding == null)
                return;

            List<Resources.TypeProductionResources.TypeResource> resourceKeys = new(productionBuilding.GetProducedResources().Keys);
            List<string> stringList = resourceKeys.ConvertAll(key => key.ToString());
            _productionResourcesDropdown.AddOptions(stringList);
            Debug.Log($"Текущий продукт производства у здания: {productionBuilding.typeProductionResource}");
        }

        private void UpdateView()
        {
            var buyStatus = _IgetBuildingViewFunctions.GetBuilding() as IBuildingPurchased;

            if (buyStatus != null)
                _textBuyedStatus.text = $"Buyed status: {buyStatus.isBuyed}";
            else
                return;

            if (buyStatus.isBuyed == false)
                return;

            var jobStatus = _IgetBuildingViewFunctions.GetBuilding() as IBuildingJobStatus;

            if (jobStatus != null)
                _textWorkStatus.text = $"Work status: {jobStatus.isWorked}";
        }

        void IBuildingView.Reload()
        {
            //todo тут перезагружаем инфу в UI, чтобы отобразить новые данные
            // не удаляя объект
        }

        void IBuildingView.End()
        {
            _IgetBuildingViewFunctions = null;
            _isActived = false;
            //todo тут выгружаем
        }
    }

    public struct InteractionBuilding
    {
        public List<Action> methods;

        public List<string> texts;
    }
}
