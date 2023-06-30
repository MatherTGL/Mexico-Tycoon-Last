using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MenuCustomizer
{
    /// <summary>Main class of MenuCustomizer</summary>
    [InitializeOnLoad]
    public static class MenuCustomizer
    {
        /// <summary>The path of customized menu setting.</summary>
        public const string SettingXmlPath = "Data/MCSetting.xml";

        #region Variables

        /// <summary>Menu for hierarchy.</summary>
        private static GenericMenu hierarchyMenu = new GenericMenu();

        /// <summary>Menu for Project.</summary>
        private static GenericMenu projectMenu = new GenericMenu();

        /// <summary>Whether you use the customized hierarchy menu.</summary>
        private static bool useCustomizedHierarchy = false;

        /// <summary>Whether you use the customized project menu.</summary>
        private static bool useCustomizedProject = false;

        #endregion

        /// <summary>Refresh this editor extension.</summary>
        public static void Refresh()
        {
            Setup();
        }

        #region Private Methods
        
        static MenuCustomizer()
        {
            // initialize and regist callback methods.
            Setup();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowGUI;
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
        }

        /// <summary>The callback called on drawing the GUI of hierarchy window.</summary>
        static void OnHierarchyWindowGUI(int instanceID, Rect selectionRect)
        {
            if(!useCustomizedHierarchy) { return; }
            Event current = Event.current;
            if(current.type != EventType.ContextClick) { return; }

            hierarchyMenu.ShowAsContext();

            current.Use();
        }

        /// <summary>The callback called on drawing the GUI of project window.</summary>
        static void OnProjectWindowGUI(string guid, Rect selectionRect)
        {
            if(!useCustomizedProject) { return; }
            Event current = Event.current;
            if(current.type != EventType.ContextClick) { return; }

            projectMenu.ShowAsContext();

            current.Use();
        }

        /// <summary>Load the setting file, and create the menu.</summary>
        private static void Setup()
        {
            if(!File.Exists(SettingXmlPath))
            {
                useCustomizedHierarchy = false;
                useCustomizedProject = false;
                return;
            }

            MCSetting setting = MCUtil.ImportXml<MCSetting>(SettingXmlPath);
            if(setting != null)
            {
                useCustomizedHierarchy = setting.useHierarchy;
                useCustomizedProject = setting.useProject;
                hierarchyMenu = CreateMenu(setting.hierarchyItemList, true, setting.showPreferenceItem);
                projectMenu = CreateMenu(setting.projectItemList, false, setting.showPreferenceItem);
                MCDebug.LogOutputEnable = setting.logOutput;
            }
            else
            {
                useCustomizedHierarchy = false;
                useCustomizedProject = false;
            }
        }

        /// <summary>Create the menu from the MCItem list.</summary>
        /// <param name="itemList">MCItem list</param>
        /// <param name="isHierarchy">If you wanna create Hierarchy Menu, please assign 'true'.</param>
        /// <param name="addPreferenceItem">If you wanna 'open preference window item', please assign 'true'.</param>
        private static GenericMenu CreateMenu(List<MCItem> itemList, bool isHierarchy, bool addPreferenceItem)
        {
            GenericMenu menu = new GenericMenu();

            string prevLabel = "";

            int itemCount = itemList.Count;

            for(int i = 0; i < itemCount; ++i)
            {
                var item = itemList[i];
                switch(item.type)
                {
                    case MCType.Separator:
                        {
                            // if the item is first or last, skip the separator.
                            if(i == 0 || i == itemCount - 1 || string.IsNullOrEmpty(prevLabel)) { continue; }

                            var nextItem = itemList[i + 1];

                            // skip the multiple separator.
                            if(nextItem.type == MCType.Separator) { continue; }

                            menu.AddSeparator(CreateSeparatorLabel(prevLabel, nextItem.label));
                        }
                        break;
                    case MCType.MenuPath: // fall through
                    case MCType.Input:
                        {
                            menu.AddItem(new GUIContent(item.label), false, MCExecutor.ExecuteMenuPath, item.command as object);
                            prevLabel = item.label;
                        }
                        break;
                    case MCType.Original:
                        {
                            if(isHierarchy)
                            {
                                menu.AddItem(new GUIContent(item.label), false, MCExecutor.ExecuteOriginalForHierarchy, item.original as object);
                                prevLabel = item.label;
                            }
                            else
                            {
                                menu.AddItem(new GUIContent(item.label), false, MCExecutor.ExecuteOriginalForProject, item.original as object);
                                prevLabel = item.label;
                            }
                        }
                        break;
                }
            }

            if(addPreferenceItem)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Preference..."), false, MCExecutor.ExecuteMenuPath, MCPreference.MenuItemPath);
            }

            return menu;
        }

        /// <summary>Create the separator label based on before and after label.</summary>
        /// <param name="prevLabel">previous item label</param>
        /// <param name="nextLabel">next item label</param>
        private static string CreateSeparatorLabel(string prevLabel, string nextLabel)
        {
            // If previous and next label contains slash,
            // check if the separator label requires the special path.
            if(prevLabel.Contains("/") && nextLabel.Contains("/"))
            {
                int prevSlashIndex = prevLabel.LastIndexOf('/');
                int nextSlashIndex = nextLabel.LastIndexOf('/');
                string prevLabelParent = (prevSlashIndex == prevLabel.Length - 1) ? prevLabel : prevLabel.Remove(prevSlashIndex + 1);
                string nextLabelParent = (nextSlashIndex == nextLabel.Length - 1) ? nextLabel : nextLabel.Remove(nextSlashIndex + 1);

                if(prevLabelParent == nextLabelParent)
                {
                    return prevLabelParent;
                }

                if(prevLabelParent.Length <= nextLabelParent.Length)
                {
                    if(nextLabelParent.StartsWith(prevLabelParent))
                    {
                        return prevLabelParent;
                    }
                }
                else if(prevLabelParent.StartsWith(nextLabelParent))
                {
                    return nextLabelParent;
                }
            }

            return "";
        }

        #endregion
    }
}