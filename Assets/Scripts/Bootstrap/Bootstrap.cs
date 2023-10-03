using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Boot
{
    public sealed class Bootstrap : MonoBehaviour
    {
        public enum TypeLoadObject { SuperImportant, MediumImportant, SimpleImportant }

        public enum TypeSingleOrLotsOf { Single, LotsOf }

        [ShowInInspector, ReadOnly, BoxGroup("Parameters")]
        private List<IBoot> l_bootObject = new List<IBoot>();


        private void Awake() => LoadToList();

        private void LoadToList()
        {
            IBoot[] bootObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IBoot>().Where(item => ((MonoBehaviour)item).enabled)
                .Distinct().ToArray<IBoot>();

            SortingObjectsList(ref bootObjects);
        }

        private void SortingObjectsList(ref IBoot[] bootObjects)
        {
            foreach (var typeLoad in Enum.GetValues(typeof(TypeLoadObject)))
            {
                if (typeLoad is TypeLoadObject.SuperImportant)
                {
                    l_bootObject.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad.Equals(typeLoad)
                        && item.GetTypeLoad().singleOrLotsOf is TypeSingleOrLotsOf.Single));
                }

                l_bootObject.AddRange(bootObjects.Where(item => item.GetTypeLoad().typeLoad.Equals(typeLoad)
                        && item.GetTypeLoad().singleOrLotsOf is TypeSingleOrLotsOf.LotsOf));
            }
            Array.Clear(bootObjects, 0, bootObjects.Length);
            StartInit();
        }

        private void StartInit()
        {
            for (ushort i = 0; i < l_bootObject.Count; i++)
                l_bootObject[i].InitAwake();
        }
    }
}