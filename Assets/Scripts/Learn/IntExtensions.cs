using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public static class IntExtensions
{
    public static int GetCustomPi(this List<int> i, in int index)
    {
        return i[index];
    }
}