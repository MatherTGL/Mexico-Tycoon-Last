using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MenuCustomizer
{
    /// <summary>EditorWindow for setting the tool.</summary>
    public class MCPreference : EditorWindow
    {
        /// <summary>Menu path for opening this window.</summary>
        public const string MenuItemPath = "Window/Menu Customizer";

        /// <summary>The name of setting window.</summary>
        public const string WindowName = "MCPreference";

        /// <summary>response on the control of this window.</summary>
        public enum UIResponse
        {
            None,
            ValueChanged,
            Up,
            Down,
            Insert,
            Remove,
            Duplicate,
        }

        /// <summary>Control option for the setting GUI.</summary>
        private class UIOP
        {
            const int itemHeight = 16;
            const int bigButtonHeight = 40;
            public static readonly GUILayoutOption Selection = GUILayout.Height(bigButtonHeight);
            public static readonly GUILayoutOption[] UpDown = { GUILayout.Width(24), GUILayout.Height(itemHeight + 2) };
            public static readonly GUILayoutOption[] InsertRemove = { GUILayout.Width(24), GUILayout.Height(itemHeight + 2) };
            public static readonly GUILayoutOption[] Duplicate = { GUILayout.Width(70), GUILayout.Height(itemHeight + 2) };
            public static readonly GUILayoutOption[] ItemType = { GUILayout.Width(100), GUILayout.Height(itemHeight) };
            public static readonly GUILayoutOption[] ItemCommand = { GUILayout.Width(300), GUILayout.Height(itemHeight + 3) };
            public static readonly GUILayoutOption[] ItemCheck = { GUILayout.Width(16), GUILayout.Height(itemHeight + 3) };
            public static readonly GUILayoutOption[] ItemLabel = { GUILayout.Height(itemHeight + 3) };
            public static readonly GUILayoutOption[] CreateDefault = { GUILayout.Width(200), GUILayout.Height(bigButtonHeight) };
            public static readonly GUILayoutOption[] RevealInFinder = { GUILayout.Width(200), GUILayout.Height(bigButtonHeight) };
            public static readonly GUILayoutOption[] Apply = { GUILayout.Width(200), GUILayout.Height(bigButtonHeight) };
        }
        
        /// <summary>The texts that is displayed on GUI.</summary>
        private class UIText
        {
            public static readonly GUIContent Empty = new GUIContent("");
            public static readonly GUIContent Up = new GUIContent("▲");
            public static readonly GUIContent Down= new GUIContent("▼");
            public static readonly GUIContent Insert = new GUIContent("+");
            public static readonly GUIContent Remove = new GUIContent("-");
            public static readonly GUIContent Separator = new GUIContent("--------------------");
            public static readonly GUIContent SelectType = new GUIContent("<- Please select menu type.");
        }

        /// <summary>Label for menu type selection control</summary>
        static readonly string[] TypeSelectionText = { "Hierarchy window menu", "Project window menu" };

        /// <summary>Setting data.</summary>
        MCSetting setting;

        /// <summary>Type of edit menu.</summary>
        int typeSelectionIndex = 0;

        /// <summary>Scroll position.</summary>
        Vector2 scrollPosition = Vector2.zero;

        /// <summary>Default menu item for hierarchy.</summary>
        GUIContent[] menuHierarchyDefault;

        /// <summary>Default menu item for hierarchy.</summary>
        GUIContent[] menuProjectDefault;

        /// <summary>Open the window</summary>
        [MenuItem(MenuItemPath)]
        public static void OpenWindow()
        {
            var window = EditorWindow.GetWindow<MCPreference>(WindowName);
            window.minSize = new Vector2(800, 600);
        }

        void OnEnable()
        {
            if(File.Exists(MenuCustomizer.SettingXmlPath))
            {
                setting = MCUtil.ImportXml<MCSetting>(MenuCustomizer.SettingXmlPath);
            }
            else
            {
                setting = new MCSetting();
            }

            Initialize();
        }

        /// <summary>Apply the setting for local file.</summary>
        void Apply()
        {
            MCUtil.ExportXml(MenuCustomizer.SettingXmlPath, this.setting);
            MenuCustomizer.Refresh();
            ShowNotification(new GUIContent("settings applied!\n" + MenuCustomizer.SettingXmlPath));
        }

        void Initialize()
        {
            UpdateExistMenuPath();
        }

        void UpdateExistMenuPath()
        {
            List<string> menuPathList = new List<string>();
            menuPathList.Add("None");
            menuPathList.AddRange(Unsupported.GetSubmenus("GameObject"));
            menuPathList.AddRange(Unsupported.GetSubmenus("Component"));
            this.setting.customHierarchyRoot.ForEach(root => menuPathList.AddRange(Unsupported.GetSubmenus(root)));
            this.menuHierarchyDefault = menuPathList
                .Distinct()
                .Select(path => new GUIContent(path))
                .ToArray();

            List<string> projectPathList = new List<string>();
            projectPathList.Add("None");
            projectPathList.AddRange(Unsupported.GetSubmenus("Assets"));
            this.setting.customProjectRoot.ForEach(root => projectPathList.AddRange(Unsupported.GetSubmenus(root)));
            this.menuProjectDefault = projectPathList
                .Distinct()
                .Select(path => new GUIContent(path))
                .ToArray();
        }

        void OnGUI()
        {
            GUILayout.Space(5);

            #region Option

            GUILayout.BeginHorizontal();
            GUILayout.Space(3);
            GUILayout.Toggle(true, "\u25BC <b><size=11>Option</size></b>", "dragtab");
            GUILayout.Space(2);
            GUILayout.EndHorizontal();
            GUILayout.Space(-2);
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            GUILayout.BeginVertical(EditorStyles.textArea);
            {
                this.setting.logOutput = GUILayout.Toggle(this.setting.logOutput, "Output debug log related with 'MenuCustomizer'.");
                this.setting.useHierarchy = GUILayout.Toggle(this.setting.useHierarchy, "Use customized hierarchy menu.");
                this.setting.useProject = GUILayout.Toggle(this.setting.useProject, "Use customized project menu.");
                this.setting.showPreferenceItem = GUILayout.Toggle(this.setting.showPreferenceItem, "Add the menu item for opening this window.");
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(3);
            GUILayout.Toggle(true, "\u25BC <b><size=11>Customize</size></b>", "dragtab");
            GUILayout.Space(2);
            GUILayout.EndHorizontal();
            GUILayout.Space(-2);
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            GUILayout.BeginVertical(EditorStyles.textArea);
            {
                // switch the edit menu type.
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(2);
                    int newSelectionIndex = GUILayout.SelectionGrid(this.typeSelectionIndex, TypeSelectionText, 2, UIOP.Selection);
                    if(newSelectionIndex != this.typeSelectionIndex)
                    {
                        this.typeSelectionIndex = newSelectionIndex;
                        this.scrollPosition = Vector2.zero;
                    }
                    GUILayout.Space(2);
                }
                GUILayout.EndHorizontal();

                List<string> editRootList = (this.typeSelectionIndex == 0) ? this.setting.customHierarchyRoot : this.setting.customProjectRoot;
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Additional menu path root", GUILayout.Width(200));
                    for(int i = 0; i < editRootList.Count; ++i)
                    {
                        string editRootName = EditorGUILayout.TextField(editRootList[i], GUILayout.Width(160));
                        if(editRootName != editRootList[i])
                        {
                            editRootList[i] = editRootName;
                            UpdateExistMenuPath();
                        }
                    }
                    editRootList.RemoveAll(rootName => string.IsNullOrEmpty(rootName));

                    string addRoot = EditorGUILayout.TextField("", GUILayout.Width(160));
                    if(!string.IsNullOrEmpty(addRoot))
                    {
                        editRootList.Add(addRoot);
                        UpdateExistMenuPath();
                    }
                }
                GUILayout.EndHorizontal();

                DrawMCItemHeader();

                this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);
                {
                    List<MCItem> editList = (this.typeSelectionIndex == 0) ? this.setting.hierarchyItemList : this.setting.projectItemList;

                    UIResponse userResponse = UIResponse.None;
                    int resIndex = -1;

                    for(int i = 0; i < editList.Count; ++i)
                    {
                        UIResponse res = DrawMCItem(editList[i]);
                        if(res != UIResponse.None)
                        {
                            userResponse = res;
                            resIndex = i;
                        }
                    }

                    #region Process response

                    if(userResponse != UIResponse.None)
                    {
                        switch(userResponse)
                        {
                            case UIResponse.Up:
                                {
                                    if(resIndex > 0)
                                    {
                                        MCItem swap = editList[resIndex - 1];
                                        editList[resIndex - 1] = editList[resIndex];
                                        editList[resIndex] = swap;
                                    }
                                }
                                break;
                            case UIResponse.Down:
                                {
                                    if(resIndex < editList.Count - 1)
                                    {
                                        MCItem swap = editList[resIndex + 1];
                                        editList[resIndex + 1] = editList[resIndex];
                                        editList[resIndex] = swap;
                                    }
                                }
                                break;
                            case UIResponse.Insert:
                                {
                                    editList.Insert(resIndex, new MCItem());
                                }
                                break;
                            case UIResponse.Remove:
                                {
                                    editList.RemoveAt(resIndex);
                                }
                                break;
                            case UIResponse.Duplicate:
                                {

                                }
                                break;
                            default:
                                break;
                        }
                    }

                    #endregion

                    // If the last item has been edited, add the last of item list.
                    MCItem add = new MCItem();
                    UIResponse addResponse = DrawMCItem(add);
                    if(addResponse != UIResponse.None && addResponse != UIResponse.Remove)
                    {
                        editList.Add(add);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            #region Buttons

            GUILayout.BeginHorizontal();
            {
                if(this.typeSelectionIndex == 0)
                {
                    if(GUILayout.Button("Use unity's default", UIOP.CreateDefault))
                    {
                        this.setting.hierarchyItemList = MCUtil.CreateHierarchyListLikeUnity();
                    }
                }
                else
                {
                    if(GUILayout.Button("Use unity's default", UIOP.CreateDefault))
                    {
                        this.setting.projectItemList = MCUtil.CreateProjectListLikeUnity();
                    }
                }

                GUILayout.Label(" ");

                if(GUILayout.Button("Show setting file", UIOP.RevealInFinder))
                {
                    EditorUtility.RevealInFinder(Application.dataPath.Replace("/Assets", "/") + MenuCustomizer.SettingXmlPath);
                }

                if(GUILayout.Button("Apply", UIOP.Apply))
                {
                    Apply();
                }
            }
            GUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(10);
        }

        void DrawMCItemHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3);
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginHorizontal(EditorStyles.textArea, GUILayout.Height(16));
            GUI.backgroundColor = backgroundColor;
            {
                GUILayout.Label(UIText.Up, UIOP.UpDown);
                GUILayout.Label(UIText.Down, UIOP.UpDown);
                GUILayout.Label(UIText.Insert, UIOP.InsertRemove);
                GUILayout.Label(UIText.Remove, UIOP.InsertRemove);
                GUILayout.Label("Type", UIOP.ItemType);
                GUILayout.Label("Command", UIOP.ItemCommand);
                GUILayout.Label("Label(If you want to override, please toggle on!)", UIOP.ItemLabel);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
            GUILayout.EndHorizontal();
        }

        UIResponse DrawMCItem(MCItem item)
        {
            UIResponse res = UIResponse.None;

            GUILayout.BeginHorizontal(EditorStyles.textArea, GUILayout.Height(22));
            {
                #region Controls

                if(GUILayout.Button(UIText.Up, UIOP.UpDown))
                {
                    res = UIResponse.Up;
                }

                if(GUILayout.Button(UIText.Down, UIOP.UpDown))
                {
                    res = UIResponse.Down;
                }

                if(GUILayout.Button(UIText.Insert, UIOP.InsertRemove))
                {
                    res = UIResponse.Insert;
                }

                if(GUILayout.Button(UIText.Remove, UIOP.InsertRemove))
                {
                    res = UIResponse.Remove;
                }

                #endregion

                if(DrawTypeField(item))
                {
                    res = UIResponse.ValueChanged;
                }

                if(DrawCommandField(item))
                {
                    item.UpdateLabelFromCommand();
                    res = UIResponse.ValueChanged;
                }

                if(DrawCheckField(item))
                {
                    res = UIResponse.ValueChanged;
                }

                if(DrawLabelField(item))
                {
                    res = UIResponse.ValueChanged;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(-3);

            return res;
        }

        bool DrawCheckField(MCItem target)
        {
            switch(target.type)
            {
                case MCType.None:       // fall through
                case MCType.Separator:
                    {
                        GUILayout.Label(UIText.Empty, UIOP.ItemCheck);
                    }
                    break;
                case MCType.Original:   // fall through
                case MCType.Input:      // fall through
                case MCType.MenuPath:
                    {
                        bool newCheck = GUILayout.Toggle(target.overrideLabel, "", UIOP.ItemCheck);
                        if(newCheck != target.overrideLabel)
                        {
                            target.overrideLabel = newCheck;
                            target.UpdateLabelFromCommand();
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        bool DrawLabelField(MCItem target)
        {
            switch(target.type)
            {
                case MCType.Separator:
                    GUILayout.Label(UIText.Separator, UIOP.ItemLabel);
                    break;
                case MCType.MenuPath:     // fall through
                case MCType.Input:      // fall through
                case MCType.Original:
                    {
                        if(target.overrideLabel)
                        {
                            string newLabel = EditorGUILayout.TextField(target.label, UIOP.ItemLabel);
                            if(newLabel != target.label)
                            {
                                target.label = newLabel;
                                return true;
                            }
                        }
                        else
                        {
                            GUILayout.Label(target.label, UIOP.ItemLabel);
                        }
                    }
                    break;
            }

            return false;
        }

        bool DrawTypeField(MCItem target)
        {
            MCType newType = (MCType)EditorGUILayout.EnumPopup(target.type, UIOP.ItemType);
            if(newType != target.type)
            {
                target.type = newType;
                target.UpdateLabelFromCommand();
                return true;
            }
            return false;
        }

        bool DrawCommandField(MCItem target)
        {
            switch(target.type)
            {
                case MCType.Separator:
                    {
                        GUILayout.Label(UIText.Empty, UIOP.ItemCommand);
                    }
                    break;
                case MCType.None:
                    {
                        GUILayout.Label(UIText.SelectType, UIOP.ItemCommand);
                    }
                    break;
                case MCType.MenuPath:
                    {
                        var selectMenu = (this.typeSelectionIndex == 0) ? this.menuHierarchyDefault : this.menuProjectDefault;
                        int commandIndex = Mathf.Max(0, IndexOf(selectMenu, con => con.text == target.command));
                        int newCommandIndex = EditorGUILayout.Popup(commandIndex, selectMenu, UIOP.ItemCommand);
                        if(newCommandIndex != commandIndex)
                        {
                            target.command = selectMenu[newCommandIndex].text;
                            return true;
                        }
                    }
                    break;
                case MCType.Input:
                    {
                        string newCommand = EditorGUILayout.TextField(target.command, UIOP.ItemCommand);
                        if(newCommand != target.command)
                        {
                            target.command = newCommand;
                            return true;
                        }
                    }
                    break;
                case MCType.Original:
                    {
                        MCOriginalCommand newOriginal = (MCOriginalCommand)EditorGUILayout.EnumPopup(
                            target.original, UIOP.ItemCommand);

                        if(newOriginal != target.original)
                        {
                            target.original = newOriginal;
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        /// <summary>Get the index of the element that is match to the condition.</summary>
        /// <returns>When the element is not found, this method returns the value "-1".</returns>
        int IndexOf<T>(IEnumerable<T> source, System.Func<T, bool> selector)
        {
            if(source != null && selector != null)
            {
                int index = 0;
                foreach(T element in source)
                {
                    if(selector(element))
                    {
                        return index;
                    }
                    ++index;
                }
            }
            return -1;
        }
    }
}
