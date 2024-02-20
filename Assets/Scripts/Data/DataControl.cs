using UnityEngine;
using Sirenix.OdinInspector;
using Data.Player;
using Config.Data.Player;
using Boot;
using static Boot.Bootstrap;

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

        void IBoot.InitAwake()
        {
            _IDataPlayer = DataPlayer.GetInstance;
            _IDataPlayer.SetDataConfig(_configDataPlayer);
        }

        void IBoot.InitStart() { }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);
    }
}
