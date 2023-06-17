using UnityEngine;
using Sirenix.OdinInspector;
using Data.Player;
using Config.Data.Player;
using Boot;


namespace Data
{
    public sealed class DataControl : MonoBehaviour, IBoot
    {
        [SerializeField, Required, BoxGroup("Parameters"), HideLabel]
        [Title("Config Data Player", horizontalLine:false)]
        private ConfigDataPlayer _configDataPlayer;


        private static IDataPlayer _IDataPlayer;
        public static IDataPlayer IdataPlayer => _IDataPlayer;

        //todo конфиги


        public void InitAwake()
        {
            _IDataPlayer = new DataPlayer();
            _IDataPlayer.SetDataConfig(_configDataPlayer);
            //todo сетить данные только один раз
        }
    }
}

