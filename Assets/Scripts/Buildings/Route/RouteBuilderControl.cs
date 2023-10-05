using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Transport.Reception;
using DebugCustomSystem;
using static Boot.Bootstrap;
using Data;
using static Data.Player.DataPlayer;
using Unity.VisualScripting.Dependencies.Sqlite;


namespace Route.Builder
{
    public sealed class RouteBuilderControl : MonoBehaviour, IBoot
    {
        private enum TypeConnect : byte { Connect, Disconnect }

        private const byte _maxPointConnection = 2;

        private InputControl _inputControl;

        [SerializeField, Required, BoxGroup("Parameters")]
        private CreatorCurveRoadControl _prefabRoute;
        public CreatorCurveRoadControl prefabRoute => _prefabRoute;

        [ShowInInspector, BoxGroup("Parameters")]
        private ITransportReception[] _connectionPoints = new ITransportReception[_maxPointConnection];

        [SerializeField, MinValue(10), MaxValue(250), BoxGroup("Parameters")]
        private byte _maxLengthRoute = 20;

        [SerializeField, MinValue(100), BoxGroup("Parameters")]
        [Tooltip("total cost = _costRoute * (Mathf.Abs distance) between objects")]
        private double _costRoute = 100;

        private float _routeLength;


        private RouteBuilderControl() { }

        void IBoot.InitAwake() => _inputControl = FindObjectOfType<InputControl>();

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);
        }

        private void SendRequestConnect() => SendingRequest(TypeConnect.Connect);

        private void SendRequestDisconnect() => SendingRequest(TypeConnect.Disconnect);

        private void SendingRequest(in TypeConnect typeConnect)
        {
            try
            {
                if (_connectionPoints.Length == _maxPointConnection && CheckRouteLength())
                {
                    if (BuyRoute() == false)
                        return;

                    if (typeConnect is TypeConnect.Connect)
                        _connectionPoints[1].ConnectionRequest(_connectionPoints[0]);
                    else
                        _connectionPoints[1].DisconnectRequest(_connectionPoints[0]);
                }
                Array.Clear(_connectionPoints, 0, _connectionPoints.Length);
            }
            catch (Exception ex)
            {
                DebugSystem.Log(ex, DebugSystem.SelectedColor.Red, "Exception", "Пролизошла ошибка: ");
            }
        }

        private bool BuyRoute()
        {
            double totalCostRoute = _costRoute * Mathf.Abs(_routeLength);
            Debug.Log($"{totalCostRoute} / {_routeLength}");
            return DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(totalCostRoute, SpendAndCheckMoneyState.Spend);
        }

        private bool CheckRouteLength()
        {
            _routeLength = Vector2.Distance(_connectionPoints[0].GetPosition().position,
                                            _connectionPoints[1].GetPosition().position);

            if (Mathf.RoundToInt(_routeLength ) < _maxLengthRoute)
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
