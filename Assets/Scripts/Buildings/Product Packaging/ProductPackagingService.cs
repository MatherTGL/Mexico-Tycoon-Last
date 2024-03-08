using System;
using System.Collections.Generic;
using Config.Building.Deliveries.Packaging;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Building.Additional
{
    //* The class is used for the service of packaging drugs in buildings during transportation
    //! Add in runtime
    public sealed class ProductPackagingService : MonoBehaviour, IProductPackaging
    {
        private ConfigProductPackagingEditor _config;

        [SerializeField, ReadOnly]
        private bool _isActive;


        async void IProductPackaging.Init()
        {
            var loadHandle = Addressables.LoadAssetAsync<ConfigProductPackagingEditor>(
               new List<string> { "ProductPackagingService" });

            await loadHandle.Task;

            if (loadHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                _config = loadHandle.Result;
            else
                throw new Exception("AsyncOperationStatus.Failed and config not loaded");
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