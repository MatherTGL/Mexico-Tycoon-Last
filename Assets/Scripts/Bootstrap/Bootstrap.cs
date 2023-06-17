using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;
using System.Collections.Generic;
using Data;


namespace Boot
{
    public class Bootstrap : MonoBehaviour
    {
        private static DataControl _dataControl;

        private List<IBoot> _bootObjectList = new List<IBoot>();

        [SerializeField]
        private List<FabricControl> _bootFabricControl = new List<FabricControl>();


        private void Awake()
        {
            _dataControl = new DataControl();

            AddBootObjects();
        }

        private void AddBootObjects()
        {
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
