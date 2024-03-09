using Config.Building.Deliveries.Packaging;

namespace Building.Additional
{
    public interface IProductPackaging
    {
        ConfigProductPackagingEditor config { get; }

        PackagingType packagingType { get; }


        void Init();

        bool IsActive();
    }
}