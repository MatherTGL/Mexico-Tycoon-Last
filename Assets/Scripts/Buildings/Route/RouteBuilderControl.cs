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
                if (_connectionPoints.Length == _config.maxPointConnection && IsRouteLength())
                {
                    if (IsBuyRoute() == false)
                        return;

                    if (typeConnect is TypeConnect.Connect)
                        _connectionPoints[1].ConnectionRequest(_connectionPoints[0]);
                    else
                        _connectionPoints[1].DisconnectRequest(_connectionPoints[0]);
                }
            }
            catch (Exception ex)
            {
                DebugSystem.Log(ex, DebugSystem.SelectedColor.Red, "Exception", "Пролизошла ошибка: ");
            }
            finally
            {
                Array.Clear(_connectionPoints, 0, _connectionPoints.Length);
            }
        }

        private bool IsBuyRoute()
        {
            double totalCostRoute = _config.costRoute * Mathf.Abs(_routeLength);
            return DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(totalCostRoute, SpendAndCheckMoneyState.Spend);
        }

        private bool IsRouteLength()
        {
            _routeLength = Vector2.Distance(_connectionPoints[0].GetPosition().position,
                                            _connectionPoints[1].GetPosition().position);

            if (Mathf.RoundToInt(_routeLength) < _config.maxLengthRoute)
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
