using UnityEditor;
using UnityEngine;

namespace MenuCustomizer
{
    /// <summary>Execution class of each menu command.</summary>
    public static class MCExecutor
    {
        /// <summary>Execute the customized menu path.</summary>
        public static void ExecuteMenuPath(object command)
        {
            ExecuteMenuPathInternal(command as string);
        }

        /// <summary>Execute the Unity's original command on hierarchy window.</summary>
        public static void ExecuteOriginalForHierarchy(object command)
        {
            MCOriginalCommand original = (MCOriginalCommand)command;

            switch(original)
            {
                case MCOriginalCommand.Copy:        // fall through
                case MCOriginalCommand.Paste:       // fall through
                case MCOriginalCommand.Duplicate:   // fall through
                case MCOriginalCommand.Delete:
                    {
                        SendMessageToHierarchy(original.ToCommand());
                    }
                    break;
                case MCOriginalCommand.Rename:
                    {
                        RenameGameObject();
                    }
                    break;
                case MCOriginalCommand.SelectPrefab:
                    {
                        SelectPrefab();
                    }
                    break;
            }
        }

        /// <summary>Execute the Unity's original command on project window.</summary>
        public static void ExecuteOriginalForProject(object command)
        {
            MCOriginalCommand original = (MCOriginalCommand)command;

            switch(original)
            {
                case MCOriginalCommand.Duplicate:   // fall through
                case MCOriginalCommand.Delete:
                    {
                        SendMessageToProject(original.ToCommand());
                    }
                    break;
                case MCOriginalCommand.Rename:
                    {
                        RenameAsset();
                    }
                    break;
                default:
                    {
                        MCDebug.LogWarning(original.ToCommand() + " command doesn't work on Project window...");
                    }
                    break;
            }
        }

        /// <summary>Internal method of 'ExecuteMenuPath'.</summary>
        private static void ExecuteMenuPathInternal(string menuPath)
        {
            MCDebug.Log("Execute : " + menuPath);
            EditorApplication.ExecuteMenuItem(menuPath);
        }

        /// <summary>Send a command event to hierarchy window.</summary>
        private static void SendMessageToHierarchy(string command)
        {
            EditorApplication.ExecuteMenuItem("Window/Hierarchy");
            EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent(command));
        }

        /// <summary>Send a command event to project window.</summary>
        private static void SendMessageToProject(string command)
        {
            EditorApplication.ExecuteMenuItem("Window/Project");
            EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent(command));
        }

        /// <summary>Rename the GameObject on the hierarchy window.</summary>
        private static void RenameGameObject()
        {
            EditorApplication.ExecuteMenuItem("Window/Hierarchy");
#if UNITY_EDITOR_WIN
            EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.keyDown });
#elif UNITY_EDITOR_OSX
            EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.Return, type = EventType.keyDown });
#endif
        }

        /// <summary>Rename the selected Asset on the project window.</summary>
        private static void RenameAsset()
        {
            EditorApplication.ExecuteMenuItem("Window/Project");
#if UNITY_EDITOR_WIN
            EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.keyDown });
#elif UNITY_EDITOR_OSX
            EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.Return, type = EventType.keyDown });
#endif
        }

        /// <summary>Select the parent prefab of the selected GameObject.</summary>
        private static void SelectPrefab()
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if(selectedGameObject != null)
            {
                Object prefabParent = PrefabUtility.GetPrefabParent(selectedGameObject);
                if(prefabParent != null)
                {
                    Selection.activeObject = prefabParent;
                }
                else
                {
                    MCDebug.Log("Selected GameObject isn't any prefab instance!");
                }
            }
        }
    }
}
