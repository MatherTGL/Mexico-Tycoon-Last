using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using TimeControl;

namespace Building.Hire
{
    public sealed class HireEmployeeView : MonoBehaviour
    {
        private IHiring _IHiring;

        private TimeDateControl _timeControl;

        private WaitForSeconds _coroutineTimeStep;


        public void Init(in IBuilding building)
        {
            _timeControl = FindObjectOfType<TimeDateControl>();
            _coroutineTimeStep = new WaitForSeconds(_timeControl.GetCurrentTimeOneDay());
            _IHiring = building as IHiring;

            if (_IHiring != null)
                StartCoroutine(UpdateInfo());
        }

        private IEnumerator UpdateInfo()
        {
            while (true)
            {
                _IHiring.ConstantUpdatingInfo();
                yield return _coroutineTimeStep;
            }
        }

        [Button("Hire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void HireEmployee(in byte indexEmployee) => _IHiring?.Hire(indexEmployee);

        [Button("Fire Employee"), BoxGroup("Editor Control | Employees"), DisableInEditorMode]
        private void FireEmployee(in byte indexEmployee) => _IHiring?.Firing(indexEmployee);
    }
}
