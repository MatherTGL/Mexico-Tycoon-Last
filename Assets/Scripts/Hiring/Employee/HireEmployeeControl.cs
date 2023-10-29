using UnityEngine;
using TimeControl;
using System.Collections;
using Sirenix.OdinInspector;

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
    }
}
