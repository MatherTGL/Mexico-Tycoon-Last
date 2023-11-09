using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Config.Bank;

namespace Bank
{
    public sealed class BankControl : MonoBehaviour, IBoot
    {
        [SerializeField, Required]
        private ConfigBankEditor _configBank; //? array
        public ConfigBankEditor configBank => _configBank;

        private BankModel _bankModel;

        private BankView _bankView;


        void IBoot.InitAwake()
        {
            _bankModel = new BankModel(this);
            _bankView = new BankView(this);
        }

        void IBoot.InitStart() { }

        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SimpleImportant, Bootstrap.TypeSingleOrLotsOf.Single);
        }

#if UNITY_EDITOR
        [SerializeField, MinValue(0.0f), MaxValue(100.0f)]
        private float _percentageLoan;
#endif

        [Button("Take Loan"), DisableInEditorMode]
        private void TakeLoan()
        {
            _bankModel.TakeLoan(_percentageLoan);
            _bankView.TakeLoan();
        }

        [Button("Loan Repayment"), DisableInEditorMode]
        private void LoanRepayment()
        {
            _bankModel.LoanRepayment(_percentageLoan);
            _bankView.LoanRepayment();
        }
    }
}
