namespace Building.Hire
{
    public sealed class HireEmployeeView
    {
        private readonly HireEmployeeControl _hireEmployeeControl;


        public HireEmployeeView(in HireEmployeeControl hireEmployeeControl)
        {
            _hireEmployeeControl = hireEmployeeControl;
        }

        public void HireEmployee(in byte indexEmployee)
        {
            _hireEmployeeControl.Ihiring?.Hire(indexEmployee);
        }

        public void FireEmployee(in byte indexEmployee)
        {
            _hireEmployeeControl.Ihiring?.Firing(indexEmployee);
        }
    }
}
