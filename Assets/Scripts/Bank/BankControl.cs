using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Config.Bank;
using System.Collections;
using TimeControl;
using System;
using static Boot.Bootstrap;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank
{
    public sealed class BankControl : MonoBehaviour, IBoot
    {
        private ConfigBankEditor[] _configBanks;
        public ConfigBankEditor[] configBanks => _configBanks;

        private BankModel _bankModel;

        private BankView _bankView;

        private WaitForSeconds _waitForSeconds;

        public event Action updated;


        private BankControl() { }

        async void IBoot.InitAwake()
        {
            float timeDateControl = FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay();
            _waitForSeconds = new WaitForSeconds(timeDateControl);

            await AsyncLoadConfigsAndCreateDependencies();
            Debug.Log(_configBanks[0]);

            _bankModel = new BankModel(this);
            _bankView = new BankView(this);
        }

        private async Task AsyncLoadConfigsAndCreateDependencies()
        {
            var loadHandle = Addressables.LoadAssetsAsync<ConfigBankEditor>(
                new List<string> { "Bank" }, config => { }, Addressables.MergeMode.None);

            await loadHandle.Task;

            if (loadHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                _configBanks = loadHandle.Result.ToArray();
            else
                throw new Exception("AsyncOperationStatus.Failed and config not loaded");
        }

        void IBoot.InitStart() => StartCoroutine(UpdateData());

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.SimpleImportant, TypeSingleOrLotsOf.Single);

        private IEnumerator UpdateData()
        {
            while (true)
            {
                yield return _waitForSeconds;
                updated();
            }
        }

        [SerializeField, MinValue(0.0f), MaxValue(100.0f)]
        private float _percentageLoan;

        [Button("Take Loan"), DisableInEditorMode]
        private void TakeLoan(in byte idBank)
        {
            _bankModel.TakeLoan(_percentageLoan, _configBanks[idBank]);
            _bankView.TakeLoan();
        }

        [Button("Loan Repayment"), DisableInEditorMode]
        private void LoanRepayment(in byte idBank)
        {
            _bankModel.LoanRepayment(_percentageLoan, _configBanks[idBank]);
            _bankView.LoanRepayment();
        }

        [Button("Put On Deposit"), DisableInEditorMode]
        private void PutOnDeposit(double sum, byte idBank)
            => _bankModel.PutOnDeposit(sum, _configBanks[idBank]);

        [Button("Get Money From Depo"), DisableInEditorMode]
        private void WithdrawMoneyFromTheDeposit(double sum, byte idBank)
            => _bankModel.WithdrawMoneyFromTheDeposit(sum, _configBanks[idBank]);

        [Button("Get Money On Depo"), DisableInEditorMode]
        private void GetMoneyOnDeposit(byte idBank)
            => Debug.Log($"Depo in this bank = {_bankModel.GetMoneyOnDeposit(_configBanks[idBank])}");
    }
}
