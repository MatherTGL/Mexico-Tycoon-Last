using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Config.Bank;
using System.Collections;
using TimeControl;
using System;
using static Boot.Bootstrap;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Bank
{
    public sealed class BankControl : MonoBehaviour, IBoot
    {
        private ConfigBankEditor[] _configBanks; //TODO: make load
        public ConfigBankEditor[] configBanks => _configBanks;

        private BankModel _bankModel;

        private BankView _bankView;

        private WaitForSeconds _waitForSeconds;

        public event Action updated;


        private BankControl() { }

        void IBoot.InitAwake()
        {
            float timeDateControl = FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay();
            _waitForSeconds = new WaitForSeconds(timeDateControl);

            StartCoroutine(LoadConfigs());

            _bankModel = new BankModel(this);
            _bankView = new BankView(this);
        }

        private IEnumerator LoadConfigs()
        {
            AsyncOperationHandle<IList<IResourceLocation>> handle
                = Addressables.LoadResourceLocationsAsync(
                    new string[] { "knight", "villager" },
                    Addressables.MergeMode.Union);

            yield return handle;
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
