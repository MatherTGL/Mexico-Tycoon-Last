using System;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Specialized;
using UnityEngine.Assertions;

namespace Learn
{
    public sealed class Main : MonoBehaviour
    {
        //private HybridDictionary hd_test = new HybridDictionary();

        private int b = 5;
        private int a = 10;

        [Button("Test")]
        private void TestButton()
        {
            Assert.AreEqual(b, 5);
            //_arrayList.Add(10);
            // Predicate<int> isPositive = (int x) => x > 5;

            // Debug.Log(isPositive.Invoke(50));

            // Func<int, int, string> createString = (a, b) => $"{a} {b}";
            // Debug.Log(createString.Invoke(50, 100));
        }
    }
}