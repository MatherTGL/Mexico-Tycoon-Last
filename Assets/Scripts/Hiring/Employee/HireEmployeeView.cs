namespace Building.Hire
{
    public sealed class HireEmployeeView : IHiringView
    {
        private readonly HireEmployeeControl _hireEmployeeControl;


        public HireEmployeeView(in HireEmployeeControl hireEmployeeControl)
            => _hireEmployeeControl = hireEmployeeControl;

        void IHiringView.FireEmployee(in byte indexEmployee, in IPossibleEmployees IpossibleEmployees)
        => _hireEmployeeControl.IhiringModel?.Firing(indexEmployee, IpossibleEmployees);

        void IHiringView.HireEmployee(in byte indexEmployee, in IPossibleEmployees IpossibleEmployees)
            => _hireEmployeeControl.IhiringModel?.AsyncHire(indexEmployee, IpossibleEmployees);
    }
}
