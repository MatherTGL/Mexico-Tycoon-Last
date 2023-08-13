using UnityEngine;


namespace City
{
    public interface ICityView
    {
        void Connect(ref SpriteRenderer spriteRenderer);
        void Disconnect(ref SpriteRenderer spriteRenderer);
    }
}
