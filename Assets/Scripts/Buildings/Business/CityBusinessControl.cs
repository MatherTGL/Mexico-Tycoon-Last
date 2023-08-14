using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Config.City.Business;
using System.Linq;


namespace City.Business
{
    public sealed class CityBusinessControl : MonoBehaviour, IUpgradableCityBusiness
    {
        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private IBuisinessBuilding[] _IbusinessBuilding;
        public IBuisinessBuilding[] IbusinessBuilding => _IbusinessBuilding;

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel, Title("Type Business", horizontalLine: false)]
        private TypeCreateBusiness _typeCreateBusiness;

        private BusinessDataSO _businessDataSO;

        private WaitForSeconds _coroutineTimeStep;

        private TimeDateControl _timeDateControl;

        private enum TypeCreateBusiness { None, Casino, Bank }

        [SerializeField, BoxGroup("Parameters"), ReadOnly, DisableInEditorMode]
        private byte _maxBuildingSlots = 5;
        public byte maxBuildingSlots { get => _maxBuildingSlots; set => _maxBuildingSlots = value; }

        [SerializeField, BoxGroup("Parameters"), ReadOnly, DisableInEditorMode]
        private byte _occupiedSlots = 0;


        public void InitAwake()
        {
            _IbusinessBuilding = new IBuisinessBuilding[CalculationNumberSlots()];
            _timeDateControl = FindObjectOfType<TimeDateControl>();

            _coroutineTimeStep = new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));

            StartCoroutine(WorkBusiness());
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.MediumImportant, isSingle: false);
        }

        public byte CalculationNumberSlots()
        {
            return (byte)(_maxBuildingSlots - _occupiedSlots);
        }

        private IEnumerator WorkBusiness()
        {
            while (true)
            {
                for (int i = 0; i < _IbusinessBuilding.Length; i++)
                    if (_IbusinessBuilding[i] is not null)
                        _IbusinessBuilding[i].WorkBusiness();
                yield return _coroutineTimeStep;
            }
        }

        private void LoadResourceBusiness(string typeBusiness)
        {
            foreach (var item in Resources.FindObjectsOfTypeAll<BusinessDataSO>()
                .Where(item => item.name.Contains(typeBusiness))
                .Select(item => _businessDataSO = item)) return;
        }


#if UNITY_EDITOR
        [Button("Add new Business"), BoxGroup("Parameters/Buttons", false), DisableInEditorMode]
        [HideIf("@_typeCreateBusiness == TypeCreateBusiness.None || CalculationNumberSlots() == 0")]
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
                    else Debug.Log("Не выбран тип создаваемого бизнеса"); ;
                    _businessDataSO = null;
                    return;
                }
            }

            bool CheckBuildingSlots(IBuisinessBuilding createdBusiness, in byte arrayIndexBusinessBuilding)
            {
                if (CalculationNumberSlots() >= createdBusiness.GetOccupiedNumberSlots())
                {
                    _occupiedSlots += createdBusiness.GetOccupiedNumberSlots();
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

        [SerializeField, BoxGroup("Parameters/Control Business", false), MaxValue(1.0f), MinValue(0.0f), Title("Institution Popularity", horizontalLine: false)]
        [InlineButton("SetInstitutionPopularity", "Set"), Tooltip("Установить популярность заведения"), HideLabel, PropertySpace(0, 10f)]
        private float _businessInstitutionPopularity;

        private void SetInstitutionPopularity()  //* используется в InlineButton у _businessInstitutionPopularity
        {
            _IbusinessBuilding[_indexBusiness].SetInstitutionPopularity(_businessInstitutionPopularity);
        }
        #endregion
#endif
    }
}
