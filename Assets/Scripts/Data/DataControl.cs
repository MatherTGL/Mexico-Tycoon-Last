using Data.Player;

namespace Data
{
    public sealed class DataControl
    {
        private static IDataPlayer _IDataPlayer;
        public static IDataPlayer IdataPlayer => _IDataPlayer;


        public DataControl()
        {
            _IDataPlayer = new DataPlayer();
        }
    }
}

