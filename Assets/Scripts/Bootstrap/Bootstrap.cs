using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;
using System.Collections.Generic;
using Data;


namespace Boot
{
    public sealed class Bootstrap : MonoBehaviour
    {
        private List<IBoot> _bootObjectList = new List<IBoot>();

        [SerializeField]
        private List<FabricControl> _bootFabricControl = new List<FabricControl>();

        [SerializeField, Required]
        private DataControl _dataControl;


        private void Awake() => AddBootObjects();

        private void AddBootObjects()
        {
            _bootObjectList.Add(_dataControl);
            _bootObjectList.AddRange(_bootFabricControl);
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
