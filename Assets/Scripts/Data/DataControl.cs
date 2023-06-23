using UnityEngine;
using Sirenix.OdinInspector;
using Data.Player;
using Config.Data.Player;
using Boot;


namespace Data
{
    internal sealed class DataControl : MonoBehaviour, IBoot
    {
        private static IDataPlayer _IDataPlayer;
        public static IDataPlayer IdataPlayer => _IDataPlayer;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel]
        [Title("Config Data Player", horizontalLine: false)]
        private ConfigDataPlayer _configDataPlayer;


        public void InitAwake()
        {
            _IDataPlayer = new DataPlayer();
            _IDataPlayer.SetDataConfig(_configDataPlayer);
        }
    }
}

