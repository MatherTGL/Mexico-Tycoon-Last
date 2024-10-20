using UnityEngine;

namespace Building.Additional.Crop
{
    public sealed class CropSpoilage : ICrop
    {
        private readonly ConfigCropSpoilage _config;


        public CropSpoilage(ConfigCropSpoilage config)
            => _config = config;

        //TODO пробегаем по всему словарю и ухудшаем качество каждого продукта и после уничтожаем его
        void ICrop.Spoilage(IGetAllResourcesForCropSpoilage getAllResources)
        {
            foreach (var resource in getAllResources.amountResources.Keys)
            {
                if (_config.cropSpoilagePercentInTime.ContainsKey(resource))
                {
                    // работаем с качеством.
                    Debug.Log("Качестов продукции ухудшилось");
                }
            }
            Debug.Log("CropSpoilage updated!");
        }
    }
}
