using Config.FabricControl.View;
using UnityEngine;


namespace Fabric
{
    public sealed class FabricControlView : IFabricView
    {
        private ConfigFabricControlView _configFabricControlView;


        public FabricControlView(in ConfigFabricControlView configFabricControlView)
        {
            _configFabricControlView = configFabricControlView;
        }


        void IFabricView.BuyFabricView(ref SpriteRenderer spriteRendererObject)
        {
            spriteRendererObject.color = _configFabricControlView.colorBuyed;
        }

        void IFabricView.ChangeWorkStateFabricView(ref SpriteRenderer spriteRendererObject, in bool isWork)
        {
            if (isWork) { spriteRendererObject.color = _configFabricControlView.colorActiveWork; }
            else { spriteRendererObject.color = _configFabricControlView.colorDeactiveWork; }
        }

        void IFabricView.SellFabricView(ref SpriteRenderer spriteRendererObject)
        {
            spriteRendererObject.color = _configFabricControlView.colorSelled;
        }
    }
}
