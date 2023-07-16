using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


namespace Road
{
    internal sealed class RoadControl : MonoBehaviour
    {
        [SerializeField, Required, BoxGroup("Parameters"), AssetsOnly]
        private LineRenderer _linePrefab;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, RoadBuilded> d_dictionaryBuildedRoad = new Dictionary<string, RoadBuilded>();

        private RoadBuilded _objectRoadBuilded;


        //? будет управлять вью, билдед
        public void BuildRoad(in Vector2 fromPosition, in Vector2 toPosition, string indexDestroyRoad)
        {
            _objectRoadBuilded = new RoadBuilded();
            CreateObjectRoad(fromPosition, toPosition);
            d_dictionaryBuildedRoad.Add(indexDestroyRoad, _objectRoadBuilded);
        }

        private GameObject CreateObjectRoad(in Vector2 fromPosition, in Vector2 toPosition)
        {
            var createdObject = Instantiate(_linePrefab, fromPosition, Quaternion.identity);
            createdObject.SetPosition(0, fromPosition);
            createdObject.SetPosition(1, toPosition);
            createdObject.transform.position = Vector2.zero;

            return createdObject.gameObject;
        }

        public void DestroyRoad(string indexDestroyRoad)
        {
            d_dictionaryBuildedRoad.Remove(indexDestroyRoad);
        }

        public void DecliningDemandUpdate(in float addResEveryStep, string typeFabricDrug, string indexRoad)
        {
            Debug.Log(indexRoad);
            if (d_dictionaryBuildedRoad.ContainsKey(indexRoad) is true)
                d_dictionaryBuildedRoad[indexRoad].DecliningDemandUpdate(addResEveryStep, typeFabricDrug);
            //_objectRoadBuilded.DecliningDemandUpdate(addResEveryStep, typeFabricDrug);
        }
    }
}
