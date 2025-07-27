using System;
using Boot;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Data.Player.View
{
    public sealed class PlayerDataView : MonoBehaviour, IBoot
    {
        [SerializeField, Required, BoxGroup("Texts")]
        private TextMeshProUGUI _textCleanMoney;

        [SerializeField, Required, BoxGroup("Texts")]
        private TextMeshProUGUI _textDirtMoney;

        [SerializeField, Required, BoxGroup("Texts")]
        private TextMeshProUGUI _textReputation;

        [SerializeField, Required, BoxGroup("Texts")]
        private TextMeshProUGUI _textRP;


        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (Bootstrap.TypeLoadObject.UI, Bootstrap.TypeSingleOrLotsOf.Single);

        void IBoot.InitAwake()
            => DataControl.IdataPlayer.dataChanged += UpdateView;

        void IBoot.InitStart()
            => DataControl.IdataPlayer.dataChanged?.Invoke();

        private void UpdateView()
        {
            _textCleanMoney.text = $"C-${Math.Round(DataControl.IdataPlayer.GetPlayerMoney(Data.Player.MoneyTypes.Clean))}";
            _textDirtMoney.text = $"D-${Math.Round(DataControl.IdataPlayer.GetPlayerMoney(Data.Player.MoneyTypes.Dirt))}";
            _textRP.text = $"RP {DataControl.IdataPlayer.GetPlayerResearchPoints()}";
            _textReputation.text = $"Rep {DataControl.IdataPlayer.GetGlobalReputation()}";
            Debug.Log("Пора обновлять UI Data Player");
        }
    }
}
