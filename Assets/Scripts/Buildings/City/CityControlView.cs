using Config.CityControl.View;
using UnityEngine;


namespace City
{
    public sealed class CityControlView : ICityView
    {
        private ConfigCityControlView _configCityControlView;


        public CityControlView(in ConfigCityControlView configCityControlView)
        {
            _configCityControlView = configCityControlView;
        }

        void ICityView.ConnectFabric(ref SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = _configCityControlView.colorConnectFabric;
        }

        void ICityView.DisconnectFabric(ref SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = _configCityControlView.colorDisconnectFabric;
        }
    }
}
