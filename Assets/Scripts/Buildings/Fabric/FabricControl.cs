using UnityEngine;
using Sirenix.OdinInspector;
using Fabric.Interface;


namespace Fabric
{
    public sealed class FabricControl : MonoBehaviour, IBoot
    {
        private static IFabricProduction _IFabricProduction;


        void IBoot.Init()
        {
            _IFabricProduction = new FabricProduction();
            _IFabricProduction.Init();
            Debug.Log("Инициализация фабрик успешна");
        }
    }
}