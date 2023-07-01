using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.CityControl.View
{
    [CreateAssetMenu(fileName = "ConfigCityControlViewDefault", menuName = "Config/Buildings/City/City Control/View/Create New", order = 50)]
    public sealed class ConfigCityControlView : ScriptableObject
    {
        [SerializeField, BoxGroup("Color parameters"), Title("Connect"), HideLabel, ColorPalette]
        private Color _colorConnectFabric;
        public Color colorConnectFabric => _colorConnectFabric;

        [SerializeField, BoxGroup("Color parameters"), Title("Disconnect"), HideLabel, ColorPalette]
        private Color _colorDisconnectFabric;
        public Color colorDisconnectFabric => _colorDisconnectFabric;
    }
}
