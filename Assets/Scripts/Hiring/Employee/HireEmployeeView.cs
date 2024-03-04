namespace Building.Hire
{
    public sealed class HireEmployeeView : IHiringView
    {
        private readonly HireEmployeeControl _hireEmployeeControl;


        public HireEmployeeView(in HireEmployeeControl hireEmployeeControl)
            => _hireEmployeeControl = hireEmployeeControl;

        void IHiringView.FireEmployee(in byte indexEmployee)
        => _hireEmployeeControl.IhiringModel?.Firing(indexEmployee);

        void IHiringView.HireEmployee(in byte indexEmployee)
            => _hireEmployeeControl.IhiringModel?.Hire(indexEmployee);
    }
}
