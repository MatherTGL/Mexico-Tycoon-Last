using UnityEngine;
using Sirenix.OdinInspector;
using Fabric;


[CreateAssetMenu(fileName = "DrugParametersRandom", menuName = "Config/Drug/Parameters/Random/Create New", order = 50)]
public sealed class DrugParametersRandom : ScriptableObject
{
    [BoxGroup("Parameters"), SerializeField, EnumPaging]
    private FabricControl.TypeProductionResource _nameTypeDrug;

    [BoxGroup("Parameters"), HorizontalGroup("Parameters/horCost"), HideLabel, MinValue(2000.0f), SerializeField]
    [Title("Cost Drug", "Min")]
    private uint _minCostDrug;

    [BoxGroup("Parameters"), HorizontalGroup("Parameters/horCost"), HideLabel, MinValue("@_minCostDrug"), SerializeField]
    [Title("", "Max")]
    private uint _maxCostDrug;

    [BoxGroup("Parameters"), HorizontalGroup("Parameters/horDemand"), SerializeField, MinValue(1.0f), HideLabel]
    [Title("Demand Drug", "Min")]
    private float _minDemandDrug;

    [BoxGroup("Parameters"), HorizontalGroup("Parameters/horDemand"), SerializeField, MinValue("@_minDemandDrug"), HideLabel]
    [Title("", "Max")]
    private float _maxDemandDrug;

    [BoxGroup("Parameters"), HorizontalGroup("Parameters/horIncreasedDemand"), SerializeField, MinValue(0.1f), HideLabel]
    [Title("Increased Demand", "Min")]
    private float _minIncreasedDemand;

    [BoxGroup("Parameters"), HorizontalGroup("Parameters/horIncreasedDemand"), SerializeField, MinValue("@_minIncreasedDemand"), HideLabel]
    [Title("", "Max")]
    private float _maxIncreasedDemand;


    public uint GetMinCostDrugParameter() { return _minCostDrug; }

    public uint GetMaxCostDrugParameter() { return _maxCostDrug; }

    public float GetMinDemandDrugParameter() { return _minDemandDrug; }

    public float GetMaxDemandDrugParameter() { return _maxDemandDrug; }
}