using System;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Specialized;


namespace Learn
{
    public sealed class Main : MonoBehaviour
    {
        //private HybridDictionary hd_test = new HybridDictionary();


        [Button("Test")]
        private void TestButton()
        {
            //_arrayList.Add(10);
            // Predicate<int> isPositive = (int x) => x > 5;

            // Debug.Log(isPositive.Invoke(50));

            // Func<int, int, string> createString = (a, b) => $"{a} {b}";
            // Debug.Log(createString.Invoke(50, 100));
        }
    }
}