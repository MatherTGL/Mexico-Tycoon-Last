using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;


public sealed class LINQResearch : MonoBehaviour
{
    [Button("Test")]
    private void Main()
    {
        var collection = new List<int>();
        var collection2 = new List<int>();

        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            collection.Add(i);
            collection2.Add(i * i);
        }

        var collectionString = new List<string>
        {
            "hello", "buy", "pidor", "lola", "hello", "pidor"
        };
        //var newList = collection.Union(collection2).Distinct();
        //var newList = collectionString.Where(item => item.Contains("i"));
        //var newList = collectionString.Where(item => item.Length > 3);
        //System.Random random = new System.Random();
        //var newList = collectionString.Select(item => $"{collection.Count + random.Next(1, 250)}");

        //var newList = collectionString.OrderByDescending(item => item.Contains("l")).Skip(1);
        //var newList = collectionString.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => "papa");

        // Debug.Log(string.Join(Environment.NewLine, collection.Where(item => item > 5)
        //     .Concat(collection2)
        //     .Any(item => item > 20)));

        if (collection.Where(item => item > 5).Concat(collection2).All(item => item < 0) is false)
        {
            Debug.Log("хуй");
        }
        else
        {
            Debug.Log("пизда");
        }

        for (int i = 0; i < collection.Where(item => item > 5).Concat(collection2).Distinct().Sum(); i++)
        {
            Debug.Log("Заебался кодить сука");
        }

        Debug.Log($"WWWWAU: {string.Join(Environment.NewLine, collection.AsParallel().Where(item => item > 5).Sum())}");

        Debug.Log(string.Join(Environment.NewLine, collection.Where(item => item > 5)
                .Concat(collection2)
                .All(item => item < 0)));


        Enumerable.Range(0, collection.Where(item => item > 5).Concat(collection2).Distinct().Sum()).AsParallel().ForAll(item =>
        {
            Test();
        });
    }

    private void Test()
    {
        for (int i = 0; i < 2; i++)
        {
            Debug.Log("Заебался кодить блять того нахуй");
        }
    }
}