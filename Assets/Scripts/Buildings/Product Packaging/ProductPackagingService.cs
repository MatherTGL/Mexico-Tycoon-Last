using System;
using Config.Building.Deliveries.Packaging;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Building.Additional
{
    //* The class is used for the service of packaging drugs in buildings during transportation
    //* Add in runtime
    public sealed class ProductPackagingService : MonoBehaviour, IProductPackaging
    {
        private ConfigProductPackagingEditor _config;
        ConfigProductPackagingEditor IProductPackaging.config => _config;

        [SerializeField, EnumToggleButtons]
        private PackagingType _currentPackagingType;
        PackagingType IProductPackaging.packagingType => _currentPackagingType;

        [SerializeField, ReadOnly]
        private bool _isActive;


        async void IProductPackaging.Init()
        {
            if (_config != null)
                return;

            var loadHandle = Addressables.LoadAssetAsync<ConfigProductPackagingEditor>("ProductPackagingService");
            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                _config = loadHandle.Result;
            else
                throw new Exception("AsyncOperationStatus.Failed and config not loaded");
        }

        bool IProductPackaging.IsActive() => _isActive;

#if UNITY_EDITOR
        [Button("Change State"), BoxGroup("Editor only")]
        private void ChangeStateEditor(in bool newBoolState)
            => _isActive = newBoolState;

        [Button("Get Actual State"), BoxGroup("Editor only")]
        private bool IsActive()
            => _isActive;
#endif
    }
}