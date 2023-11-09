namespace Bank
{
    public sealed class BankView
    {
        private BankControl _bankControl;


        public BankView(in BankControl bankControl)
        {
            _bankControl = bankControl;
        }

        public void TakeLoan()
        {

        }

        public void LoanRepayment()
        {

        }
    }
}
