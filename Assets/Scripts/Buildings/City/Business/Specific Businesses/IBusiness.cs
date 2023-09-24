namespace Business
{
    public interface IBusiness
    {
        void ToLaunderMoney(in double amountDirtyMoney, in float percentageMoneyCleared);
    }
}
