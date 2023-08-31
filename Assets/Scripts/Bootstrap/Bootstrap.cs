using System;
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

        private async ValueTask LoadToList()
        {
            IBoot[] bootObjects = GameObject.FindObjectsOfType<MonoBehaviour>()
                .OfType<IBoot>().Where(item => ((MonoBehaviour)item).enabled)
                .Distinct().ToArray<IBoot>();

            await Task.Run(() => { SortingObjectsList(ref bootObjects); });
            StartInit();
        }

        private void SortingObjectsList(ref IBoot[] bootObjects)
        {
            foreach (var typeLoad in Enum.GetValues(typeof(TypeLoadObject)))
            {
                if (typeLoad is TypeLoadObject.SuperImportant)
                {
                    _bootObjectList.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad.Equals(typeLoad)
                        && item.GetTypeLoad().isSingle));
                }

                _bootObjectList.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad.Equals(typeLoad)
                        && !item.GetTypeLoad().isSingle));
            }
            Array.Clear(bootObjects, 0, bootObjects.Length);
        }

        private void StartInit()
        {
            for (int i = 0; i < _bootObjectList.Count; i++)
                _bootObjectList[i].InitAwake();
        }
    }
}