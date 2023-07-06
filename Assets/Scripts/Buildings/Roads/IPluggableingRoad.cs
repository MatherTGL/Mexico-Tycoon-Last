using System.Collections.Generic;
using UnityEngine;


public interface IPluggableingRoad
{
    byte c_maxConnectionObjects { get; }
    byte connectObjectsCount { get; }
    float uploadResourceAddWay { get; }
    List<IPluggableingRoad> l_allConnectedObject { get; }
    Dictionary<string, float> d_allInfoObjectClientsTransition { get; }
    IPluggableingRoad connectingObject { get; }
    void ConnectObjectToObject(string typeFabricDrug, string gameObjectConnectionTo, IPluggableingRoad FirstObject, IPluggableingRoad SecondObject);
    void DisconnectObjectToObject(string gameObjectDisconnectTo);
    void IngestResources(string typeFabricDrug, in bool isWork, in float addResEveryStep);
    Vector2 GetPositionVector2();
}