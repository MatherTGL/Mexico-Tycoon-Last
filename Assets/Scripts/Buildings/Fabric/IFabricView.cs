using UnityEngine;


namespace Fabric
{
    public interface IFabricView
    {
        void BuyFabricView(ref SpriteRenderer spriteRendererObject);
        void SellFabricView(ref SpriteRenderer spriteRendererObject);
        void ChangeWorkStateFabricView(ref SpriteRenderer spriteRendererObject, in bool isWork);
    }
}
