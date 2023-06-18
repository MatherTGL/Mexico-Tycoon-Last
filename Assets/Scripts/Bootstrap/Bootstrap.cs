using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;
using System.Collections.Generic;
using Data;
using City;


namespace Boot
{
    public sealed class Bootstrap : MonoBehaviour
    {
        //todo сделать категории для супер важных, средних и остальных в инспекторе
        [SerializeField, Required]
        private DataControl _dataControl;

        private List<IBoot> _bootObjectList = new List<IBoot>();

        [SerializeField]
        private List<FabricControl> _bootFabricControl = new List<FabricControl>();

        [SerializeField]
        private List<CityControl> _bootCityControl = new List<CityControl>();


        private void Awake() => AddBootObjects();

        private void AddBootObjects()
        {
            SuperImportantObjects();
        }

        private void SuperImportantObjects()
        {
            _bootObjectList.Add(_dataControl);
            MediumImportantObjects();
        }

        private void MediumImportantObjects()
        {
            _bootObjectList.AddRange(_bootFabricControl);
            _bootObjectList.AddRange(_bootCityControl);
            SimpleImportantObjects();
        }

        private void SimpleImportantObjects()
        {
            //*тут будут грузиться все остальные менее важные штуки
            InitList();
        }

        private void InitList()
        {
            for (int i = 0; i < _bootObjectList.Count; i++)
            {
                _bootObjectList[i].InitAwake();
            }
        }
    }
}
