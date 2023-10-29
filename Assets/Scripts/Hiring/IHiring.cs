namespace Building.Hire
{
    public interface IHiring
    {
        double currentExpenses { get; set; }


        void ConstantUpdatingInfo();
        void Hire(in byte indexEmployee);
        void Firing(in byte indexEmployee);
    }
}
