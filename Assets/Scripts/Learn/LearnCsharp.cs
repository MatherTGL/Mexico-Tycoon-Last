using System;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Specialized;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Learn
{
    public sealed class Main : MonoBehaviour
    {
        private void Awake()
        {
            var car = new { Name = "", Speed = 5, Cost = 100_000 };
        }
    }
}