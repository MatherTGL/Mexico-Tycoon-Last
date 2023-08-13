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

        void ICityView.Connect(ref SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = _configCityControlView.colorConnectFabric;
        }

        void ICityView.Disconnect(ref SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = _configCityControlView.colorDisconnectFabric;
        }
    }
}
