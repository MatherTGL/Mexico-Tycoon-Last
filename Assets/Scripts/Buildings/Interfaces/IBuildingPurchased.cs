namespace Building.Additional
{
    public interface IBuildingPurchased
    {
        bool isBuyed { get; protected set; }


        void Buy()
        {
            if (!isBuyed)
                isBuyed = true;
        }

        void Sell()
        {
            if (isBuyed)
                isBuyed = false;
        }
    }
}
