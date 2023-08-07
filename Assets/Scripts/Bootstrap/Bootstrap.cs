using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;
using System.Collections.Generic;
using Data;
using City;
using Player.Movement;
using TimeControl;
using Upgrade;
using City.Business;


namespace Boot
{
    internal sealed class Bootstrap : MonoBehaviour
    {
        [BoxGroup("Parameters"), FoldoutGroup("Parameters/Super Important")]
        [SerializeField, Required, FoldoutGroup("Parameters/Super Important/Single")]
        [Title("Data Control", horizontalLine: false), HideLabel]
        private DataControl _dataControl;

        [SerializeField, Required, FoldoutGroup("Parameters/Super Important/Single")]
        [Title("Player Control Movement", horizontalLine: false), HideLabel]
        private PlayerControlMovement _playerControlMovement;

        [SerializeField, Required, FoldoutGroup("Parameters/Super Important/Single")]
        [Title("Time Date Control", horizontalLine: false), HideLabel]
        private TimeDateControl _timeDateControl;

        private List<IBoot> _bootObjectList = new List<IBoot>();

        [SerializeField, FoldoutGroup("Parameters/Medium Important")]
        private List<FabricControl> _bootFabricControl = new List<FabricControl>();

        [SerializeField, FoldoutGroup("Parameters/Medium Important")]
        private List<CityControl> _bootCityControl = new List<CityControl>();

        [SerializeField, FoldoutGroup("Parameters/Medium Important")]
        private List<UpgradeControl> _bootUpgradeControl = new List<UpgradeControl>();

        [SerializeField, FoldoutGroup("Parameters/Simple Important")]
        private List<CityBusinessControl> _bootCityBusinessControl = new List<CityBusinessControl>();


        private void Awake() => AddBootObjects();

        private void AddBootObjects() => SuperImportantObjects();

        private void SuperImportantObjects()
        {
            _bootObjectList.Add(_dataControl);
            _bootObjectList.Add(_playerControlMovement);
            _bootObjectList.Add(_timeDateControl);
            MediumImportantObjects();
        }

        private void MediumImportantObjects()
        {
            _bootObjectList.AddRange(_bootFabricControl);
            _bootObjectList.AddRange(_bootCityControl);
            _bootObjectList.AddRange(_bootUpgradeControl);
            SimpleImportantObjects();
        }

        private void SimpleImportantObjects()
        {
            _bootObjectList.AddRange(_bootCityBusinessControl);
            InitList();
        }

        private void InitList()
        {
            for (int i = 0; i < _bootObjectList.Count; i++)
            {
                _bootObjectList[i].InitAwake();
            }
        }

#if UNITY_EDITOR
        [SerializeField, FoldoutGroup("Parameters/AutoSearch"), ToggleLeft, LabelText("Use Add Auto Singles")]
        private bool _isUseAddAutoSingles;

        [SerializeField, FoldoutGroup("Parameters/AutoSearch"), ToggleLeft, LabelText("Use Add Auto Array Objects")]
        private bool _isUseAddAutoArrayObjects;

        [SerializeField, FoldoutGroup("Parameters/AutoSearch")]
        [HideLabel, Title("Count Auto Search Iterations", horizontalLine: false)]
        private ushort _countAutoSearchIterations = 10;


        [Button("Auto Search"), FoldoutGroup("Parameters/AutoSearch")]
        private void AutoSearch()
        {
            if (_isUseAddAutoArrayObjects)
            {
                ClearBootObjectsList();

                for (int i = 0; i < _countAutoSearchIterations; i++)
                {
                    var _findedObjectCityControl = FindObjectsOfType<CityControl>();
                    Debug.Log(_findedObjectCityControl.Length);
                    if (_findedObjectCityControl.Length > i)
                        _bootCityControl.Add(_findedObjectCityControl[i]);

                    var _findedObjectFabricControl = FindObjectsOfType<FabricControl>();
                    Debug.Log(_findedObjectFabricControl.Length);
                    if (_findedObjectFabricControl.Length > i)
                        _bootFabricControl.Add(_findedObjectFabricControl[i]);

                    var _findedObjectsUpgradeControl = FindObjectsOfType<UpgradeControl>();
                    Debug.Log(_findedObjectsUpgradeControl.Length);
                    if (_findedObjectsUpgradeControl.Length > i)
                        _bootUpgradeControl.Add(_findedObjectsUpgradeControl[i]);

                    var _findedObjectsCityBusinessControl = FindObjectsOfType<CityBusinessControl>();
                    Debug.Log(_findedObjectsCityBusinessControl.Length);
                    if (_findedObjectsCityBusinessControl.Length > i)
                        _bootCityBusinessControl.Add(_findedObjectsCityBusinessControl[i]);
                }
            }

            if (_isUseAddAutoSingles)
            {
                _playerControlMovement = FindObjectOfType<PlayerControlMovement>();
                _timeDateControl = FindObjectOfType<TimeDateControl>();
            }
        }

        private void ClearBootObjectsList()
        {
            _bootCityControl.Clear();
            _bootFabricControl.Clear();
            _bootUpgradeControl.Clear();
            _bootCityBusinessControl.Clear();
        }
#endif
    }
}