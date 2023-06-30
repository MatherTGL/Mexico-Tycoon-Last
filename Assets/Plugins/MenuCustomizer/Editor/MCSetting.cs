using System.Collections.Generic;

namespace MenuCustomizer
{
    /// <summary>Setting class for MenuCustomizer</summary>
    public class MCSetting
    {
        /// <summary>Whether you use the customized hierarchy menu.</summary>
        public bool useHierarchy = true;

        /// <summary>Whether you use the customized project menu.</summary>
        public bool useProject = true;

        /// <summary>Whether you add the 'Open Preference' command to the end of the customized menu.</summary>
        public bool showPreferenceItem = true;

        /// <summary>Whether this tool output log.</summary>
        public bool logOutput = false;

        /// <summary>The list of the additional menu path root for hierarchy.</summary>
        public List<string> customHierarchyRoot = new List<string>();

        /// <summary>The list of the additional menu path root for project.</summary>
        public List<string> customProjectRoot = new List<string>();

        /// <summary>The list of the menu item for hierarchy.</summary>
        public List<MCItem> hierarchyItemList = new List<MCItem>();

        /// <summary>The list of the menu item for project.</summary>
        public List<MCItem> projectItemList = new List<MCItem>();
    }
}
