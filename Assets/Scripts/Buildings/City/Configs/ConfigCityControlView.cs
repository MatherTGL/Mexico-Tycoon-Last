using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.CityControl.View
{
    [CreateAssetMenu(fileName = "ConfigCityControlViewDefault", menuName = "Config/Buildings/City/City Control/View/Create New", order = 50)]
    public sealed class ConfigCityControlView : ScriptableObject
    {
        //todo сделать общую палитру цветов
        [SerializeField, BoxGroup("Color parameters"), Title("Connect"), HideLabel]
        private Color _colorConnectFabric;
        public Color colorConnectFabric => _colorConnectFabric;

        [SerializeField, BoxGroup("Color parameters"), Title("Disconnect"), HideLabel]
        private Color _colorDisconnectFabric;
        public Color colorDisconnectFabric => _colorDisconnectFabric;
    }
}
