using UnityEngine;
using TimeControl;
using System.Collections;
using Sirenix.OdinInspector;
using DebugCustomSystem;
using Config.Employees;
using System.Linq;

namespace Building.Hire
{
    //! Adding in runtime on start game
    public sealed class HireEmployeeControl : MonoBehaviour
    {
        private const string pathConfig = "Configs/Employees/";

        private IHiringModel _IhiringModel;
        public IHiringModel IhiringModel => _IhiringModel;

        private IHiringView _IhiringView;

        private TimeDateControl _timeControl;

        private ConfigPossibleEmployeesInShopEditor _config;

        private WaitForSeconds _coroutineTimeStep;

        private WaitForSeconds _coroutineTimeUpdateOffers;


        private HireEmployeeControl() { }

        public void Init()
        {
            try
            {
                _config = UnityEngine.Resources.LoadAll<ConfigPossibleEmployeesInShopEditor>(pathConfig)[0];
            }
            catch (System.Exception ex) { throw new System.Exception($"{ex}"); }

            _timeControl = FindObjectOfType<TimeDateControl>();
            _coroutineTimeStep = new WaitForSeconds(_timeControl.GetCurrentTimeOneDay());
            _coroutineTimeUpdateOffers = new WaitForSeconds(_config.timeUpdateOffers);
            _IhiringModel = new HireEmployeeModel();
            _IhiringView = new HireEmployeeView(this);

            StartCoroutine(UpdateInfo());
            StartCoroutine(UpdatePossibleEmployees());
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
                _IhiringModel.UpdateAllEmployees();
                yield return _coroutineTimeUpdateOffers;
            }
        }

        [Button("Hire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void HireEmployee(in byte indexEmployee) => _IhiringView.HireEmployee(indexEmployee);

        [Button("Fire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void FireEmployee(in byte indexEmployee) => _IhiringView.FireEmployee(indexEmployee);

#if UNITY_EDITOR
        [Button("Get All Employees"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void GetAllTypeEveryEmployee()
        {
            for (byte i = 0; i < _IhiringModel.a_possibleEmployeesInShop.Length; i++)
            {
                DebugSystem.Log($"Object {this} | Employee type: {_IhiringModel.a_possibleEmployeesInShop[i].typeEmployee} | Index: {i}",
                    DebugSystem.SelectedColor.Orange, tag: "Employee");
            }
        }
#endif
    }
}
