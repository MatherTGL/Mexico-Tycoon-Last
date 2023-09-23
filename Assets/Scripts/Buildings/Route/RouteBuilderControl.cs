using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Transport.Reception;
using DebugCustomSystem;


namespace Route.Builder
{
    public sealed class RouteBuilderControl : MonoBehaviour, IBoot
    {
        private const byte _maxPointConnection = 2;

        private InputControl _inputControl;

        [SerializeField, Required, BoxGroup("Parameters")]
        private CreatorCurveRoad _prefabRoute;
        public CreatorCurveRoad prefabRoute => _prefabRoute;

        [ShowInInspector, BoxGroup("Parameters")]
        private ITransportReception[] _connectionPoints = new ITransportReception[_maxPointConnection];


        private RouteBuilderControl() { }

        void IBoot.InitAwake() => _inputControl = FindObjectOfType<InputControl>();

        (Bootstrap.TypeLoadObject typeLoad, bool isSingle) IBoot.GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SuperImportant, true);
        }

        private void SendRequestConnect() => SendingRequest(true);

        private void SendRequestDisconnect() => SendingRequest(false);

        private void SendingRequest(in bool isConnect)
        {
            try
            {
                if (_connectionPoints.Length == _maxPointConnection)
                {
                    if (isConnect == true)
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
