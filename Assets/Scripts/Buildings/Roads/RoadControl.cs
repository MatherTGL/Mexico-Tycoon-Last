using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Transport;


namespace Road
{
    internal sealed class RoadControl : MonoBehaviour
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private TransportControl _transportControl;

        [SerializeField, Required, BoxGroup("Parameters"), AssetsOnly]
        private LineRenderer _linePrefab;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, RoadBuilded> d_buildedRoad = new Dictionary<string, RoadBuilded>();
        public Dictionary<string, RoadBuilded> dictionaryBuildedRoad => d_buildedRoad;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, GameObject> d_createdSceneRoadObjects = new Dictionary<string, GameObject>();

        private RoadBuilded _objectRoadBuilded;


        //? будет управлять вью, билдед
        public void BuildRoad(in Vector2 fromPosition, in Vector2 toPosition, string indexCreateRoad)
        {
            _objectRoadBuilded = new RoadBuilded(fromPosition, toPosition);
            CreateObjectRoad(fromPosition, toPosition);
            d_buildedRoad.Add(indexCreateRoad, _objectRoadBuilded);
            _transportControl.AddNewRoad(indexCreateRoad);

            GameObject CreateObjectRoad(in Vector2 fromPosition, in Vector2 toPosition)
            {
                var createdObject = Instantiate(_linePrefab, fromPosition, Quaternion.identity);
                d_createdSceneRoadObjects.Add(indexCreateRoad, createdObject.gameObject);
                createdObject.SetPosition(0, fromPosition);
                createdObject.SetPosition(1, toPosition);
                createdObject.transform.position = Vector2.zero;

                return createdObject.gameObject;
            }
        }

        public void DestroyRoad(string indexDestroyRoad)
        {
            _transportControl.DestroyRoad(indexDestroyRoad);
            d_buildedRoad.Remove(indexDestroyRoad);
            Destroy(d_createdSceneRoadObjects[indexDestroyRoad].gameObject);
            d_createdSceneRoadObjects.Remove(indexDestroyRoad);
        }

        public void DecliningDemandUpdate(in float addResEveryStep, string typeFabricDrug, string indexRoad)
        {
            if (d_buildedRoad.ContainsKey(indexRoad) is true)
                d_buildedRoad[indexRoad].DecliningDemandUpdate(addResEveryStep, typeFabricDrug);
        }
    }
}
