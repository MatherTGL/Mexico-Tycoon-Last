using System.Collections.Generic;
using UnityEngine;


public interface IPluggableingRoad
{
    void ConnectObjectToObject(string typeFabricDrug, string gameObjectConnectionTo, IPluggableingRoad FirstObject, IPluggableingRoad SecondObject);
    void DisconnectObjectToObject(string gameObjectDisconnectTo);
    void IngestResources(string typeFabricDrug, in bool isWork, in float addResEveryStep);
    Vector2 GetPositionVector2();
}