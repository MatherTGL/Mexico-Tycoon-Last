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


        private DataControl() { }

        public void InitAwake()
        {
            _IDataPlayer = DataPlayer.GetInstance;
            _IDataPlayer.SetDataConfig(_configDataPlayer);
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.SuperImportant, isSingle: true);
        }
    }
}
