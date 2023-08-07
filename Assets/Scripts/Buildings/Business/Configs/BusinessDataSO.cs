using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.City.Business
{
    [CreateAssetMenu(menuName = "Config/Business/Data", fileName = "BusinessDataSO")]
    public sealed class BusinessDataSO : ScriptableObject
    {
        [SerializeField, BoxGroup("Parameters")]
        private float _minPercentageVisitors;
        public float minPercentageVisitors => _minPercentageVisitors;

        [SerializeField, BoxGroup("Parameters"), MinValue("@_minPercentageVisitors")]
        private float _maxPercentageVisitors;
        public float maxPercentageVisitors => _minPercentageVisitors;

        [SerializeField, BoxGroup("Parameters"), MinValue(100)]
        private ushort _maxNumberVisitors = 100;
        public ushort maxNumberVisitors => _maxNumberVisitors;

        [SerializeField, BoxGroup("Parameters"), MinValue(5)]
        private ushort _averageCheckVisitor = 5;
        public ushort averageCheckVisitor => _averageCheckVisitor;

        [SerializeField, BoxGroup("Parameters"), MinValue(1)]
        private ushort _averageSpendPerVisitor = 1;
        public ushort averageSpendPerVisitor => _averageSpendPerVisitor;
    }
}
