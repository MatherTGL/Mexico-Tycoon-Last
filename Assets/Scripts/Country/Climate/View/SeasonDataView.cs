using Boot;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Climate
{
    public sealed class SeasonDataView : MonoBehaviour, IBoot
    {
        [SerializeField, BoxGroup("Texts"), Required]
        private TextMeshProUGUI _textCurrentSeason;


        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (Bootstrap.TypeLoadObject.UI, Bootstrap.TypeSingleOrLotsOf.Single);

        void IBoot.InitAwake()
        {
            ClimateZoneControl.IseasonControl.updatedSeason += DrawUI;
        }

        void IBoot.InitStart()
        {
            Debug.Log($"current season: {ClimateZoneControl.IseasonControl.currentSeason}");
        }

        private void DrawUI(float seasonNumber)
        {
            _textCurrentSeason.text = $"{ClimateZoneControl.IseasonControl.currentSeason}";
        }
    }
}
