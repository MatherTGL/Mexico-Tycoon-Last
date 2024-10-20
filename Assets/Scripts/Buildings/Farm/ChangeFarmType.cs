using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Config.Building;
using Data;
using GameSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Config.Building.ConfigBuildingFarmEditor;
using static Data.Player.DataPlayer;

namespace Building.Farm
{
    public sealed class ChangeFarmType
    {
        public async Task<ConfigBuildingFarmEditor> AsyncGetNewType(TypeFarm typeFarm)
        {
            var loadHandle = Addressables.LoadAssetsAsync<ConfigBuildingFarmEditor>("TypeFarm", null);
            await loadHandle.Task;
            var result = loadHandle.Result.Where(config => config.typeFarm == typeFarm).First();
            Addressables.Release(loadHandle);

            CoroutineManager.Instance.StartManagedCoroutine(
                DelayChangeType(result.timeChangeFarmType, result.typeChangeCost));

            return loadHandle.Status == AsyncOperationStatus.Succeeded
            ? result
            : throw new Exception("AsyncOperationStatus.Failed and config not loaded for farm");
        }

        private IEnumerator DelayChangeType(ushort seconds, double cost)
        {
            DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(cost, SpendAndCheckMoneyState.Spend);
            yield return new WaitForSeconds(seconds);
        }
    }
}
