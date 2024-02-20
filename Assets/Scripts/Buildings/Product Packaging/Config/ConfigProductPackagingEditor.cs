using System;
using Building.Additional;
using SerializableDictionary.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config.Building.Deliveries.Packaging
{
    [CreateAssetMenu(fileName = "ConfigProductPackaging", menuName = "Config/Buildings/Additional/Deliveries/Packaging/Create New", order = 50)]
    public sealed class ConfigProductPackagingEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters")]
        private SerializableDictionary<PackagingType, double> d_packagingCost = new();

        [SerializeField, BoxGroup("Parameters")]
        private SerializableDictionary<PackagingType, float> d_packagingPercentRisk = new();
    }
}