using System;
using System.Linq;
using Config.Building.Deliveries.Packaging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Building.Additional
{
    //* The class is used for the service of packaging drugs in buildings during transportation
    //! Add in runtime
    public sealed class ProductPackagingService : MonoBehaviour, IProductPackaging
    {
        private const string pathToConfig = "Configs/Transportation";

        private ConfigProductPackagingEditor _config; //TODO: load from resource folder

        [SerializeField, ReadOnly]
        private bool _isActive;


        void IProductPackaging.Init()
        {
            try
            {
                _config = UnityEngine.Resources.LoadAll<ConfigProductPackagingEditor>(pathToConfig).First();
            }
            catch (Exception ex) { throw new Exception($"{ex}"); }
        }

        bool IProductPackaging.IsActive()
            => _isActive;

#if UNITY_EDITOR
        [Button("Change State"), BoxGroup("Editor only")]
        private void ChangeStateEditor(in bool newState)
            => _isActive = newState;

        [Button("Get Actual State"), BoxGroup("Editor only")]
        private bool IsActive()
            => _isActive;
#endif
    }
}