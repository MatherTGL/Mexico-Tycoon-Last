using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Config.City.Business;


namespace City.Business
{
    public sealed class CityBusinessControl : MonoBehaviour, IBoot, IUpgradableCityBusiness
    {
        [SerializeField, BoxGroup("Parameters"), ReadOnly, InfoBox("Find in awake")]
        private TimeDateControl _timeDateControl;

        [SerializeField, BoxGroup("Parameters")]
        private byte _buildingSlots = 5; //? рандом, константа или под апгрейд?

        [SerializeField, BoxGroup("Parameters"), MinValue("@_buildingSlots"), MaxValue(100)]
        private byte _maxBuildingSlots = 5;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private IBuisinessBuilding[] _IbusinessBuilding;
        public IBuisinessBuilding[] IbusinessBuilding => _IbusinessBuilding;

        private enum TypeCreateBusiness { None, Casino, Bank }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel, Title("Type Business", horizontalLine: false)]
        private TypeCreateBusiness _typeCreateBusiness;

        [SerializeField, BoxGroup("Parameters"), Required, HideIf("_typeCreateBusiness", TypeCreateBusiness.None)]
        private BusinessDataSO _businessDataSO;


        public void InitAwake()
        {
            _IbusinessBuilding = new IBuisinessBuilding[_buildingSlots];

            if (_timeDateControl is null)
                _timeDateControl = FindObjectOfType<TimeDateControl>();

            StartCoroutine(WorkBusiness());
        }
        private IEnumerator WorkBusiness()
        {
            while (true)
            {
                for (int i = 0; i < _IbusinessBuilding.Length; i++)
                    if (_IbusinessBuilding[i] is not null)
                        _IbusinessBuilding[i].WorkBusiness();
                yield return new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));
            }
        }

        private void LoadResourceBusiness(in string typeBusiness)
        {
            foreach (var item in Resources.FindObjectsOfTypeAll<BusinessDataSO>())
                if (item.name == $"BusinessData{typeBusiness}") _businessDataSO = item;
        }


#if UNITY_EDITOR
        [Button("Add new Business"), BoxGroup("Parameters/Buttons", false), DisableInEditorMode]
        [HideIf("@_typeCreateBusiness == TypeCreateBusiness.None || _buildingSlots == 0")]
        private void AddNewBusiness()
        {
            LoadResourceBusiness(_typeCreateBusiness.ToString());

            for (int i = 0; i < _IbusinessBuilding.Length; i++)
            {
                if (_IbusinessBuilding[i] is null && _businessDataSO is not null)
                {
                    if (_typeCreateBusiness is TypeCreateBusiness.Casino)
                    {
                        if (CheckBuildingSlots(new BusinessCasino(GetComponent<CityControl>().cityTreasury,
                                                                  GetComponent<CityControl>(), _businessDataSO), (byte)i)) return;
                    }
                    else if (_typeCreateBusiness is TypeCreateBusiness.Bank)
                    {
                        if (CheckBuildingSlots(new BusinessBank(GetComponent<CityControl>().cityTreasury,
                                                                GetComponent<CityControl>(), _businessDataSO), (byte)i)) return;
                    }
                    else { Debug.Log("Не выбран тип создаваемого бизнеса"); }
                    _businessDataSO = null;
                    return;
                }
            }

            bool CheckBuildingSlots(IBuisinessBuilding createdBusiness, in byte arrayIndexBusinessBuilding)
            {
                if (_buildingSlots >= createdBusiness.GetOccupiedNumberSlots())
                {
                    _buildingSlots -= createdBusiness.GetOccupiedNumberSlots();
                    _IbusinessBuilding[arrayIndexBusinessBuilding] = createdBusiness;
                    return true;
                }
                else
                {
                    createdBusiness = null;
                    return false;
                }
            }
        }

        [SerializeField, BoxGroup("Parameters/Buttons"), HorizontalGroup("Parameters/Buttons/RemoveBusiness")]
        [HideLabel, ShowIf("@_IbusinessBuilding.Length > 0"), Tooltip("Индекс удаляемого бизнеса")]
        private byte _indexRemovingBusiness;

        [Button("Remove Business", 24), BoxGroup("Parameters/Buttons", false), HorizontalGroup("Parameters/Buttons/RemoveBusiness")]
        [ShowIf("@_IbusinessBuilding.Length > 0"), DisableInEditorMode]
        private void RemoveBusiness()
        {
            _IbusinessBuilding[_indexRemovingBusiness] = null;
        }

        #region Control Business Parameters

        [SerializeField, BoxGroup("Parameters/Control Business"), HorizontalGroup("Parameters/Control Business/Work")]
        [HideLabel, Title("State Work / Index Business")]
        private bool _isWorkBusiness;

        [SerializeField, BoxGroup("Parameters/Control Business"), HorizontalGroup("Parameters/Control Business/Work")]
        [MaxValue("@_IbusinessBuilding.Length"), HideLabel, Title("")]
        private byte _indexBusiness;

        [Button("Change State Work", 21), BoxGroup("Parameters/Control Business"), EnableIf("@_IbusinessBuilding[_indexBusiness] != null")]
        [HorizontalGroup("Parameters/Control Business/Work"), Title("")]
        private void ChangeStateWorkBusiness()
        {
            _IbusinessBuilding[_indexBusiness].isWork = _isWorkBusiness;
        }

        [SerializeField, BoxGroup("Parameters/Control Business", false), HorizontalGroup("Parameters/Control Business/Laundered")]
        [MaxValue(0.5f), HideLabel, Title("Change Percent Money Laundered", horizontalLine: false), MinValue(0.0f)]
        [InlineButton("ChangePercentMoneyLaunderedBusiness", "Set"), InlineButton("GetPercentMoneyLaunderedBusiness", "Get")]
        private float _percentMoneyLaundered;

        private void ChangePercentMoneyLaunderedBusiness() //* используется в InlineButton у _percentMoneyLaundered
        {
            if (Application.isPlaying)
                _IbusinessBuilding[_indexBusiness].ChangePercentageMoneyLaundered(_percentMoneyLaundered);
            else
                Debug.Log("Отсутствует объект в массиве");
        }

        private void GetPercentMoneyLaunderedBusiness() //* используется в InlineButton у _percentMoneyLaundered
        {
            if (Application.isPlaying)
                Debug.Log($"Current percentage money laundered business (Index: {_indexBusiness} / {_IbusinessBuilding[_indexBusiness].GetPercentageMoneyLaundered()})");
            else
                Debug.Log("Отсутствует объект в массиве");
        }

        void IUpgradableCityBusiness.UpgradeBuildingSlots()
        {
            if (Application.isPlaying)
            {
                if (_buildingSlots < _maxBuildingSlots)
                {
                    _buildingSlots++;
                    Debug.Log("Upgrade building slots");
                }
            }
            else { Debug.Log("Изменения доступны только в play mode"); }
        }


        [SerializeField, BoxGroup("Parameters/Control Business", false), MaxValue(1.0f), MinValue(0.0f), Title("Institution Popularity", horizontalLine: false)]
        [InlineButton("SetInstitutionPopularity", "Set"), Tooltip("Установить популярность заведения"), HideLabel, PropertySpace(0, 10f)]
        private float _businessInstitutionPopularity;

        private void SetInstitutionPopularity()  //* используется в InlineButton у _businessInstitutionPopularity
        {
            _IbusinessBuilding[_indexBusiness].SetInstitutionPopularity(_businessInstitutionPopularity);
        }

        void IUpgradableCityBusiness.UpgradeBusinessMaxNumberVisitors()
        {
            _IbusinessBuilding[_indexBusiness].UpgradeMaxNumberVisitors();
        }
        #endregion
#endif
    }
}
