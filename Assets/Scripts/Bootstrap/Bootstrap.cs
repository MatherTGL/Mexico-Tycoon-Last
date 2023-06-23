using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;
using System.Collections.Generic;
using Data;
using City;
using Player.Movement;


namespace Boot
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [BoxGroup("Parameters"), FoldoutGroup("Parameters/Super Important")]
        [SerializeField, Required, FoldoutGroup("Parameters/Super Important/Single")]
        [Title("Data Control", horizontalLine: false), HideLabel]
        private DataControl _dataControl;

        [SerializeField, Required, FoldoutGroup("Parameters/Super Important/Single")]
        [Title("Player Control Movement", horizontalLine: false), HideLabel]
        private PlayerControlMovement _playerControlMovement;

        private List<IBoot> _bootObjectList = new List<IBoot>();

        [SerializeField, FoldoutGroup("Parameters/Medium Important")]
        private List<FabricControl> _bootFabricControl = new List<FabricControl>();

        [SerializeField, FoldoutGroup("Parameters/Medium Important")]
        private List<CityControl> _bootCityControl = new List<CityControl>();


        private void Awake() => AddBootObjects();

        private void AddBootObjects() => SuperImportantObjects();

        private void SuperImportantObjects()
        {
            _bootObjectList.Add(_dataControl);
            _bootObjectList.Add(_playerControlMovement);
            MediumImportantObjects();
        }

        private void MediumImportantObjects()
        {
            _bootObjectList.AddRange(_bootFabricControl);
            _bootObjectList.AddRange(_bootCityControl);
            SimpleImportantObjects();
        }

        private void SimpleImportantObjects()
        {
            //*тут будут грузиться все остальные менее важные штуки
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

        [SerializeField, FoldoutGroup("Parameters/AutoSearch"), LabelText("Count Auto Search Iterations")]
        private ushort _countAutoSearchIterations = 40;


        [Button("Auto Search"), FoldoutGroup("Parameters/AutoSearch")]
        private void AutoSearch()
        {
            for (int i = 0; i < _countAutoSearchIterations; i++)
            {
                var _findedObjectFabricControl = FindObjectsOfType<FabricControl>();
                _bootFabricControl.Add(_findedObjectFabricControl[i]);

                var _findedObjectCityControl = FindObjectsOfType<CityControl>();
                _bootCityControl.Add(_findedObjectCityControl[i]);
            }

            if (_isUseAddAutoSingles)
            {
                _playerControlMovement = FindObjectOfType<PlayerControlMovement>();
            }
        }
#endif
    }
}
//? если находим новый объект по типу, то добавоям в лист и продолжаем поиск.
//? Если нет, то не добавляем