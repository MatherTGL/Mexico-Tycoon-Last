using UnityEngine;


namespace City
{
    public interface ICityView
    {
        void ConnectFabric(ref SpriteRenderer spriteRenderer);
        void DisconnectFabric(ref SpriteRenderer spriteRenderer);
    }
}
