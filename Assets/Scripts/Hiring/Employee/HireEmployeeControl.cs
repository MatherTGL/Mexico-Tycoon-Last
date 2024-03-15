using UnityEngine;
using TimeControl;
using System.Collections;
using Sirenix.OdinInspector;
using DebugCustomSystem;
using UnityEngine.AddressableAssets;
using Config.Employees;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Building.Hire
{
    //* Adding in runtime on start game
    public sealed class HireEmployeeControl : MonoBehaviour
    {
        private IPossibleEmployees _IpossibleEmployees;

        private IHiringModel _IhiringModel;
        public IHiringModel IhiringModel => _IhiringModel;

        private IHiringView _IhiringView;

        private TimeDateControl _timeControl;

        private WaitForSeconds _coroutineTimeStep;

        private WaitForSeconds _coroutineTimeUpdateOffers;


        private HireEmployeeControl() { }

        public async void Init()
        {
            _timeControl = FindObjectOfType<TimeDateControl>();
            _IhiringModel = new HireEmployeeModel();
            _IhiringView = new HireEmployeeView(this);

            await AsyncLoadConfigPossibleEmployees();
            _coroutineTimeUpdateOffers = new WaitForSeconds(_IpossibleEmployees.config.timeUpdateOffers);
            _coroutineTimeStep = new WaitForSeconds(_timeControl.GetCurrentTimeOneDay());

            StartCoroutine(UpdateInfo());
            StartCoroutine(UpdatePossibleEmployees());
        }

        async Task AsyncLoadConfigPossibleEmployees()
        {
            var loadHandle = Addressables.LoadAssetAsync<ConfigPossibleEmployeesInShopEditor>("PossibleEmployeesInShop");
            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                _IpossibleEmployees = new PossibleEmployeesInShop(loadHandle.Result);
            else
                throw new System.Exception("AsyncOperationStatus.Failed and config not loaded");
        }

        private IEnumerator UpdateInfo()
        {
            while (true)
            {
                _IhiringModel.ConstantUpdatingInfo();
                yield return _coroutineTimeStep;
            }
        }

        private IEnumerator UpdatePossibleEmployees()
        {
            while (true)
            {
                _IpossibleEmployees.UpdateOffers();
                yield return _coroutineTimeUpdateOffers;
            }
        }

        [Button("Hire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void HireEmployee(in byte indexEmployee) => _IhiringView.HireEmployee(indexEmployee, _IpossibleEmployees);

        [Button("Fire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void FireEmployee(in byte indexEmployee) => _IhiringView.FireEmployee(indexEmployee, _IpossibleEmployees);

#if UNITY_EDITOR
        [Button("Get All Employees"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void GetAllTypeEveryEmployee()
        {
            for (byte i = 0; i < _IpossibleEmployees.possibleEmployeesInShop.Length; i++)
            {
                DebugSystem.Log($"Object {this} | Employee type: {_IpossibleEmployees.possibleEmployeesInShop[i].type} | Index: {i}",
                    DebugSystem.SelectedColor.Orange, tag: "Employee");
            }
        }
#endif
    }
}
