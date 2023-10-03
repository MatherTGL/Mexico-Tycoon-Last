using static Boot.Bootstrap;

namespace Boot
{
    public interface IBoot
    {
        void InitAwake();

        (Bootstrap.TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) GetTypeLoad();
    }
}
