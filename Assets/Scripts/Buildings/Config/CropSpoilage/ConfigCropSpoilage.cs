using SerializableDictionary.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building.Additional.Crop
{
    [CreateAssetMenu(fileName = "ConfigCropSpoilage", menuName = "Config/Buildings/Crop/Spoilage")]
    public sealed class ConfigCropSpoilage : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters")]
        private SerializableDictionary<TypeResource, float> d_cropSpoilagePercent = new();

        public SerializableDictionary<TypeResource, float> cropSpoilagePercentInTime => d_cropSpoilagePercent;
    }
}
