using UnityEngine;
using TimeControl;
using System.Collections;
using Sirenix.OdinInspector;
using DebugCustomSystem;

namespace Building.Hire
{
    public sealed class HireEmployeeControl : MonoBehaviour
    {
        private IHiring _Ihiring;
        public IHiring Ihiring => _Ihiring;

        private HireEmployeeView _hireEmployeeView;

        private TimeDateControl _timeControl;

        private WaitForSeconds _coroutineTimeStep;


        public void Init()
        {
            _timeControl = FindObjectOfType<TimeDateControl>();
            _coroutineTimeStep = new WaitForSeconds(_timeControl.GetCurrentTimeOneDay());
            _Ihiring = new HireEmployeeModel();
            _hireEmployeeView = new HireEmployeeView(this);

            StartCoroutine(UpdateInfo());
        }

        private IEnumerator UpdateInfo()
        {
            while (true)
            {
                _Ihiring.ConstantUpdatingInfo();
                yield return _coroutineTimeStep;
            }
        }

        [Button("Hire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void HireEmployee(in byte indexEmployee) => _hireEmployeeView.HireEmployee(indexEmployee);

        [Button("Fire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void FireEmployee(in byte indexEmployee) => _hireEmployeeView.FireEmployee(indexEmployee);

#if UNITY_EDITOR
        [Button("Get All Employees"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void GetAllTypeEveryEmployee()
        {
            for (byte i = 0; i < _Ihiring.a_possibleEmployeesInShop.Length; i++)
            {
                DebugSystem.Log($"Object {this} | Employee type: {_Ihiring.a_possibleEmployeesInShop[i].typeEmployee} | Index: {i}",
                    DebugSystem.SelectedColor.Orange, tag: "Employee");
            }
        }
#endif
    }
}
