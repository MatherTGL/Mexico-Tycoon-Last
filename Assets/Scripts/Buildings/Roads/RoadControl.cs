using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


namespace Road
{
    public sealed class RoadControl : MonoBehaviour
    {
        [SerializeField, Required, BoxGroup("Parameters"), AssetsOnly]
        private LineRenderer _linePrefab;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, RoadBuilded> _dictionaryBuildedRoad = new Dictionary<string, RoadBuilded>();

        private RoadBuilded _objectRoadBuilded;


        //? будет управлять вью, билдед
        public void BuildRoad(in Vector2 fromPosition, in Vector2 toPosition, string indexDestroyRoad)
        {
            _objectRoadBuilded = CreateObjectRoad(fromPosition, toPosition).GetComponent<RoadBuilded>();

            _dictionaryBuildedRoad.Add(indexDestroyRoad, _objectRoadBuilded);
            _objectRoadBuilded.InitRoad();
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
            Destroy(_dictionaryBuildedRoad[indexDestroyRoad].gameObject);
            _dictionaryBuildedRoad.Remove(indexDestroyRoad);
        }

        public void DecliningDemandUpdate(in float addResEveryStep, string typeFabricDrug)
        {
            _objectRoadBuilded.DecliningDemandUpdate(addResEveryStep, typeFabricDrug);
        }
    }
}
