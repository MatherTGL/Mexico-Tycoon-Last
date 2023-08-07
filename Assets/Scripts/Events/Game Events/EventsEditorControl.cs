using UnityEngine;
using Sirenix.OdinInspector;


internal sealed class EventsEditorControl : MonoBehaviour
{
    [SerializeField, BoxGroup("Parameters"), Tooltip("Название ключа для доступа к dictionary"), LabelText("Key")]
    private string _keyEvent;

    private enum TypeEvent { city, fabric }

    [SerializeField, BoxGroup("Parameters"), EnumPaging, ShowIf("@_keyEvent.Length != 0"), LabelText("Type")]
    [PropertySpace(5, 10)]
    private TypeEvent _typeEvent;

    [SerializeField, BoxGroup("Parameters"), ShowIf("@_keyEvent.Length != 0 && _typeEvent == TypeEvent.city")]
    [SuffixLabel("amount days")]
    private byte _durationEvent;

    [SerializeField, BoxGroup("Parameters/Population Impact"), ShowIf("@_keyEvent.Length != 0 && _typeEvent == TypeEvent.city")]
    [SuffixLabel("%"), LabelText("Min")]
    private float _minImpactPopulation;

    [SerializeField, BoxGroup("Parameters/Population Impact"), ShowIf("@_keyEvent.Length != 0 && _typeEvent == TypeEvent.city")]
    [SuffixLabel("%"), LabelText("Max")]
    private float _maxImpactPopulation;


    [Button("Add new Event"), BoxGroup("Parameters"), ShowIf("@_keyEvent.Length != 0")]
    private void AddNewEvent()
    {

    }
}