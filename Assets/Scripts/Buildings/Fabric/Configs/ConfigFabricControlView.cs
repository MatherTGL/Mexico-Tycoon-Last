using UnityEngine;
using Sirenix.OdinInspector;


namespace Config.FabricControl.View
{
    [CreateAssetMenu(fileName = "ConfigFabricControlViewDefault", menuName = "Config/Buildings/Fabric/Fabric Control/View/Create New", order = 50)]
    public sealed class ConfigFabricControlView : ScriptableObject
    {
        [SerializeField, BoxGroup("Colors Parameters"), Title("Buyed"), HideLabel, ColorPalette]
        private Color _colorBuyed;
        public Color colorBuyed => _colorBuyed;

        [SerializeField, BoxGroup("Colors Parameters"), Title("Selled"), HideLabel, ColorPalette]
        private Color _colorSelled;
        public Color colorSelled => _colorSelled;

        [SerializeField, BoxGroup("Colors Parameters"), Title("Active Work"), HideLabel, ColorPalette]
        private Color _colorActiveWork;
        public Color colorActiveWork => _colorActiveWork;

        [SerializeField, BoxGroup("Colors Parameters"), Title("Deactive Work"), HideLabel, ColorPalette]
        private Color _colorDeactiveWork;
        public Color colorDeactiveWork => _colorDeactiveWork;
    }
}
