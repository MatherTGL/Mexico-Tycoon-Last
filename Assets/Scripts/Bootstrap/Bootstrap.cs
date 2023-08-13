using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;


namespace Boot
{
    public sealed class Bootstrap : MonoBehaviour
    {
        public enum TypeLoadObject { SuperImportant, MediumImportant, SimpleImportant }

        [ShowInInspector, ReadOnly, BoxGroup("Parameters")]
        private List<IBoot> _bootObjectList = new List<IBoot>();


        private void Awake() => LoadToList();

        private async void LoadToList()
        {
            IBoot[] bootObjects = GameObject.FindObjectsOfType<MonoBehaviour>()
                .OfType<IBoot>()
                .Where(item => ((MonoBehaviour)item).enabled)
                .Distinct()
                .ToArray<IBoot>();

            await Task.Run(() =>
            {
                _bootObjectList.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad == TypeLoadObject.SuperImportant
                        && item.GetTypeLoad().isSingle));

                _bootObjectList.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad == TypeLoadObject.SuperImportant
                        && !item.GetTypeLoad().isSingle));

                _bootObjectList.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad == TypeLoadObject.MediumImportant
                        && !item.GetTypeLoad().isSingle));

                _bootObjectList.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad == TypeLoadObject.SimpleImportant
                        && !item.GetTypeLoad().isSingle));

                bootObjects = null;
            });

            InitList();
        }

        private void InitList()
        {
            for (int i = 0; i < _bootObjectList.Count; i++)
                _bootObjectList[i].InitAwake();
        }
    }
}