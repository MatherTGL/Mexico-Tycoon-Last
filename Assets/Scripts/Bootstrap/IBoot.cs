namespace Boot
{
    public interface IBoot
    {
        void InitAwake();

        (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad();
    }
}
