using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Transport.Reception;
using DebugCustomSystem;
using static Boot.Bootstrap;
using Data;
using static Data.Player.DataPlayer;
using Config.Building.Route;
using static Building.BuildingEnumType;
using Transport;

namespace Route.Builder
{
    public sealed class RouteBuilderControl : MonoBehaviour, IBoot
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private ConfigRouteBuilderEditor _config;
        public ConfigRouteBuilderEditor config => _config;

        private enum TypeConnect : byte { Connect, Disconnect }

        private InputControl _inputControl;

        [ShowInInspector, BoxGroup("Parameters")]
        private ITransportReception[] _connectionPoints;

        private float _routeLength;


        private RouteBuilderControl() { }

        void IBoot.InitAwake()
        {
            _inputControl = FindObjectOfType<InputControl>();
            _connectionPoints = new ITransportReception[_config.maxPointConnection];
        }

        void IBoot.InitStart() { }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);

        private void SendRequestConnect() => SendingRequest(TypeConnect.Connect);

        private void SendRequestDisconnect() => SendingRequest(TypeConnect.Disconnect);

        private void SendingRequest(in TypeConnect typeConnect)
        {
            try
            {
                TypeTransport.Type routeType = GetRouteType();
                if (_connectionPoints.Length == _config.maxPointConnection && IsRouteLength(routeType))
                {
                    if (IsBuyRoute() == false)
                        return;

                    if (typeConnect is TypeConnect.Connect)
                        _connectionPoints[1].ConnectionRequest(_connectionPoints[0], routeType);
                    else
                        _connectionPoints[1].DisconnectRequest(_connectionPoints[0]);
                }
            }
            catch (Exception ex)
            {
                DebugSystem.Log(ex, DebugSystem.SelectedColor.Red, "Exception", "Произошла ошибка: ");
            }
            finally
            {
                Array.Clear(_connectionPoints, 0, _connectionPoints.Length);
            }
        }

        private TypeTransport.Type GetRouteType()
        {
            var firstObject = _connectionPoints[0].typeCurrentBuilding;
            var secondObject = _connectionPoints[^1].typeCurrentBuilding;

            if (firstObject == TypeBuilding.Aerodrome && secondObject == TypeBuilding.Aerodrome)
                return TypeTransport.Type.Air;
            else if (firstObject == TypeBuilding.SeaPort && secondObject == TypeBuilding.SeaPort)
                return TypeTransport.Type.Marine;
            else
                return TypeTransport.Type.Ground;
        }

        private bool IsBuyRoute()
        {
            double totalCostRoute = _config.costRoute * Mathf.Abs(_routeLength);
            return DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(totalCostRoute, SpendAndCheckMoneyState.Spend);
        }

        private bool IsRouteLength(in TypeTransport.Type routeType)
        {
            _routeLength = Vector2.Distance(_connectionPoints[0].GetPosition().position,
                                            _connectionPoints[1].GetPosition().position);

            if (_config.maxLengthRoutes.ContainsKey(routeType) && _routeLength < _config.maxLengthRoutes[routeType])
                return true;
            else
                return false;
        }


        #region Editor
#if UNITY_EDITOR
        [BoxGroup("Editor Testing"), PropertySpace(10), Button("New Route"), DisableInEditorMode]
        [HorizontalGroup("Editor Testing/Hor"), ShowIf("@_connectionPoints[0] != null && _connectionPoints[1] != null")]
        private void CreateRoute() => SendRequestConnect();

        [BoxGroup("Editor Testing"), PropertySpace(10), Button("Destroy Route"), DisableInEditorMode]
        [HorizontalGroup("Editor Testing/Hor"), ShowIf("@_connectionPoints[0] != null && _connectionPoints[1] != null")]
        private void DestroyRoute() => SendRequestDisconnect();
#endif
        #endregion
    }
}
