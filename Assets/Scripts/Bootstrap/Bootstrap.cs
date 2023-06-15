using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;
using System.Collections.Generic;


namespace Boot
{
    public class Bootstrap : MonoBehaviour
    {
        private List<IBoot> _bootObjectList = new List<IBoot>();

        [SerializeField]
        private List<FabricControl> _bootFabricControl = new List<FabricControl>();


        private void Awake()
        {
            AddBootObjects();

            for (int i = 0; i < _bootObjectList.Count; i++)
            {
                _bootObjectList[i].Init();
            }
        }

        private void AddBootObjects()
        {
            _bootObjectList.AddRange(_bootFabricControl);
        }
    }
}
