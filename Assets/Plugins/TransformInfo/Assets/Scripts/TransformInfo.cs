using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CanEditMultipleObjects, InitializeOnLoad, CustomEditor(typeof(object))]
public class TransformInfo : Editor
{
    #region VARIABLE

    // ↓ To change the text color set the default values below ↓ 

    static Color textColor = Color.white;

    const KeyCode xShortcut = KeyCode.B;
    const KeyCode yShortcut = KeyCode.N;
    const KeyCode zShortcut = KeyCode.M;

    // ↑ To change the default XYZ edit shortcuts ↑                                                             

    #region MENU STRINGS
    const string isEnabledString = "Transform Info/Enabled";
    const string followingHandleString = "Transform Info/Follow Handle";
    const string addDecimalPointString = "Transform Info/Add Decimal";
    const string removeDecimalPointString = "Transform Info/Remove Decimal";
    const string increaseSizeString = "Transform Info/Increase Size";
    const string decreaseSizeString = "Transform Info/Decrease Size";
    #endregion
    #region ENUMS
    static Tool currentTool = Tool.None;
    static Tool lastTool = Tool.Move;
    static PivotRotation pivotRotation;
    static KeyCode commandKey = KeyCode.None;
    #endregion
    #region GUI
    static SceneView sceneView;
    static Event currentEvent;
    static GUIStyle transformInfoStyle = new GUIStyle();
    #endregion
    #region VALUES
    static bool isEnabled, followingHandle, editingXValue, editingYValue, editingZValue, parsePassedCache, dragging = false;
    static Transform cam, activeTransform, lastActiveTransform;
    static GameObject[] activeGameObjects;
    static Vector3 activePosition, activeLocalPosition, handlePosition, activeRotation, activeScale, cameraRelative, xLabelPosition, yLabelPosition, zLabelPosition, LastPos = Vector3.zero, LastRot = Vector3.zero, LastScale = Vector3.zero, AveragePos, AverageRot, AverageScale;
    static string xStringValue, yStringValue, zStringValue, xEditStringValue, yEditStringValue, zEditStringValue;
    static float handleSize, checkedFloat;
    static int count, decimalPoints, textSize, controlId, lastActiveObjectsLength, currentToolIndex, currentObjIndex;
    static System.Collections.Generic.List<Vector3> revertValuesList = new System.Collections.Generic.List<Vector3>();
    static EditorWindow windowUnderMouse = null;
    static EditorWindow[] openWindows = null;
    #endregion

    #endregion

    #region BEHAVIOUR

    static TransformInfo()
    {
        #region LOAD EDITOR PREF VALUES
        isEnabled = EditorPrefs.GetBool(isEnabledString, true);
        followingHandle = EditorPrefs.GetBool(followingHandleString, false);
        decimalPoints = EditorPrefs.GetInt(addDecimalPointString, 2);
        textSize = EditorPrefs.GetInt(increaseSizeString, 20);
        #endregion
        #region INIT
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        #endregion
    }

    static void OnSceneGUI(SceneView scene)
    {
        if (sceneView == null)
            sceneView = scene;

        if (isEnabled)
        {
            if (currentEvent == null) currentEvent = Event.current;

            Refresh();

            if (currentEvent.isKey)
            {
                switch (currentEvent.type)
                {
                    case EventType.KeyDown:
                        switch (currentEvent.keyCode)
                        {
                            case xShortcut: editingXValue = true; break;
                            case yShortcut: editingYValue = true; break;
                            case zShortcut: editingZValue = true; break;
                            case KeyCode.Keypad0: CharacterAdded("0"); break;
                            case KeyCode.Keypad1: CharacterAdded("1"); break;
                            case KeyCode.Keypad2: CharacterAdded("2"); break;
                            case KeyCode.Keypad3: CharacterAdded("3"); break;
                            case KeyCode.Keypad4: CharacterAdded("4"); break;
                            case KeyCode.Keypad5: CharacterAdded("5"); break;
                            case KeyCode.Keypad6: CharacterAdded("6"); break;
                            case KeyCode.Keypad7: CharacterAdded("7"); break;
                            case KeyCode.Keypad8: CharacterAdded("8"); break;
                            case KeyCode.Keypad9: CharacterAdded("9"); break;
                            case KeyCode.Alpha0: CharacterAdded("0"); break;
                            case KeyCode.Alpha1: CharacterAdded("1"); break;
                            case KeyCode.Alpha2: CharacterAdded("2"); break;
                            case KeyCode.Alpha3: CharacterAdded("3"); break;
                            case KeyCode.Alpha4: CharacterAdded("4"); break;
                            case KeyCode.Alpha5: CharacterAdded("5"); break;
                            case KeyCode.Alpha6: CharacterAdded("6"); break;
                            case KeyCode.Alpha7: CharacterAdded("7"); break;
                            case KeyCode.Alpha8: CharacterAdded("8"); break;
                            case KeyCode.Alpha9: CharacterAdded("9"); break;
                            case KeyCode.KeypadPeriod: CharacterAdded("."); break;
                            case KeyCode.Period:          
                                if (editingXValue || editingYValue || editingZValue)
                                {
                                    CharacterAdded(".");
                                }
                                else
                                {
                                    NextObject();
                                }
                                break;
                            case KeyCode.Minus: CharacterAdded("-"); break;
                            case KeyCode.Plus: CharacterAdded("+"); break;
                            case KeyCode.KeypadMinus: CharacterAdded("-"); break;
                            case KeyCode.KeypadPlus: CharacterAdded("+"); break;
                            case KeyCode.KeypadDivide: CharacterAdded("/"); break;
                            case KeyCode.KeypadMultiply: CharacterAdded("*"); break;
                            case KeyCode.Backspace: ClearLastCharacter(); break;
                            case KeyCode.Delete: ClearLastCharacter(); break;
                            case KeyCode.KeypadEnter: ApplyValue(); break;
                            case KeyCode.Return: ApplyValue(); break;
                            case KeyCode.Comma: NextTool(); break;
                        }
                        break;

                    case EventType.KeyUp:
                        switch (currentEvent.keyCode) 
                        {
                            case xShortcut: editingXValue = false; xEditStringValue = ""; break;
                            case yShortcut: editingYValue = false; yEditStringValue = ""; break;
                            case zShortcut: editingZValue = false; zEditStringValue = ""; break;
                        }  
                        break;
                }
            }
            else if (currentEvent.isMouse)
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseDown: 
                    if (currentEvent.button == 0) 
                    {
                            if (currentTool == Tool.Transform && activeTransform != null && activeTransform.hasChanged)
                            {
                                LastPos = activeTransform.localPosition;
                                LastRot = TransformUtils.GetInspectorRotation(activeTransform);
                                LastScale = activeTransform.localScale;
                                activeTransform.hasChanged = false;
                            }
                            SaveRevertHistory();
                    }                   
                    break;
                    case EventType.MouseDrag: if (currentEvent.button == 0) dragging = true; UpdateLastTool(); break;

                    case EventType.MouseUp:
                        if (currentEvent.button == 0)
                        {
                            SaveRevertHistory();

                            ApplyValue();

                            dragging = false;
                        }
                        else if (currentEvent.button == 1)
                        {
                            if (dragging)
                                LoadRevertHistory();

                            xEditStringValue = "";
                            yEditStringValue = "";
                            zEditStringValue = "";

                            editingXValue = false;
                            editingYValue = false;
                            editingZValue = false;
                        }
                        break;
                }
            }
            else if (currentEvent.type == EventType.Repaint)
            {
                Draw();            
            }
        }
    }
    static void Refresh()
    {
        if (isEnabled)
        {      
            //Cache Tools
            currentTool = Tools.current;
            pivotRotation = Tools.pivotRotation;
            handlePosition = Tools.handlePosition;
            handleSize = HandleUtility.GetHandleSize(activePosition);

            //Cache active transform
            if (Selection.activeTransform != null)
            {
                activeTransform = Selection.activeTransform;
                activePosition = activeTransform.position;
                activeLocalPosition = activeTransform.localPosition;
                activeRotation = TransformUtils.GetInspectorRotation(activeTransform);
                activeScale = activeTransform.localScale;

                if (followingHandle && lastActiveTransform != activeTransform)
                {
                    sceneView.FrameSelected();
                    lastActiveTransform = activeTransform;
                }
            
                if (EditorWindow.mouseOverWindow != null)
                {
                    windowUnderMouse = EditorWindow.mouseOverWindow;

                    if (windowUnderMouse != null)
                    {
                        if ((windowUnderMouse.titleContent.text == "UnityEditor.AdvancedDropdown.AddComponentWindow" || windowUnderMouse.titleContent.text == "Hierarchy" || windowUnderMouse.titleContent.text == "Scene") && !IsMaterialWindowOpen())
                        {
                            sceneView.Focus();
                        }
                    }
                }
            }
            else
            {
                activeTransform = null;
            }

            //Cache selected gameobjects
            if (Selection.gameObjects != null)
            {
                activeGameObjects = Selection.gameObjects;

                AveragePos = Vector3.zero;
                for (int i = 0; i < activeGameObjects.Length; i++)
                {
                    AveragePos.x += activeGameObjects[i].transform.localPosition.x;
                    AveragePos.y += activeGameObjects[i].transform.localPosition.y;
                    AveragePos.z += activeGameObjects[i].transform.localPosition.z;
                }
                AveragePos /= activeGameObjects.Length;

                AverageScale = Vector3.zero;
                for (int i = 0; i < activeGameObjects.Length; i++)
                {
                    AverageScale.x += activeGameObjects[i].transform.localScale.x;
                    AverageScale.y += activeGameObjects[i].transform.localScale.y;
                    AverageScale.z += activeGameObjects[i].transform.localScale.z;
                }
                AverageScale /= activeGameObjects.Length;

                AverageRot = Vector3.zero;
                for (int i = 0; i < activeGameObjects.Length; i++)
                {
                    AverageRot.x += TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x;
                    AverageRot.y += TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y;
                    AverageRot.z += TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z;
                }
                AverageRot /= activeGameObjects.Length;

                if (followingHandle && lastActiveObjectsLength != activeGameObjects.Length)
                {
                    sceneView.FrameSelected();
                    lastActiveObjectsLength = activeGameObjects.Length;
                }
            }
            else
            {
                activeGameObjects = null;
            }

            //Cache cameras relative position
            if (cam == null)
            {
                cam = Camera.current.transform;

            }
            else
            {
                cameraRelative = cam.InverseTransformPoint(activePosition);
            }
        }
    }
    static void Draw()
    {
        if (activeTransform != null)
        {
            //Dont show if not in camera's view or in view tool mode
            if (cameraRelative.z >= 0 && currentTool != Tool.View && currentTool != Tool.None)
            {

                if (activeGameObjects.Length > 1)
                {
                    //Draw Average Values

                    //Set string values based on current tool
                    switch (currentTool)
                    {
                        case Tool.Move:
                            xStringValue = AveragePos.x.ToString("F" + decimalPoints);
                            yStringValue = AveragePos.y.ToString("F" + decimalPoints);
                            zStringValue = AveragePos.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Rotate:
                            xStringValue = AverageRot.x.ToString("F" + decimalPoints);
                            yStringValue = AverageRot.y.ToString("F" + decimalPoints);
                            zStringValue = AverageRot.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Scale:
                            xStringValue = AverageScale.x.ToString("F" + decimalPoints);
                            yStringValue = AverageScale.y.ToString("F" + decimalPoints);
                            zStringValue = AverageScale.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Rect:
                            xStringValue = AverageScale.x.ToString("F" + decimalPoints);
                            yStringValue = AverageScale.y.ToString("F" + decimalPoints);
                            zStringValue = AverageScale.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Transform:
                            switch (lastTool)
                            {
                                case Tool.Move:
                                    xStringValue = AveragePos.x.ToString("F" + decimalPoints);
                                    yStringValue = AveragePos.y.ToString("F" + decimalPoints);
                                    zStringValue = AveragePos.z.ToString("F" + decimalPoints);
                                    break;

                                case Tool.Rotate:
                                    xStringValue = AverageRot.x.ToString("F" + decimalPoints);
                                    yStringValue = AverageRot.y.ToString("F" + decimalPoints);
                                    zStringValue = AverageRot.z.ToString("F" + decimalPoints);
                                    break;

                                case Tool.Scale:
                                    xStringValue = AverageScale.x.ToString("F" + decimalPoints);
                                    yStringValue = AverageScale.y.ToString("F" + decimalPoints);
                                    zStringValue = AverageScale.z.ToString("F" + decimalPoints);
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    //Set string values based on current tool
                    switch (currentTool)
                    {
                        case Tool.Move:
                            xStringValue = activeLocalPosition.x.ToString("F" + decimalPoints);
                            yStringValue = activeLocalPosition.y.ToString("F" + decimalPoints);
                            zStringValue = activeLocalPosition.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Rotate:
                            xStringValue = activeRotation.x.ToString("F" + decimalPoints);
                            yStringValue = activeRotation.y.ToString("F" + decimalPoints);
                            zStringValue = activeRotation.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Scale:
                            xStringValue = activeScale.x.ToString("F" + decimalPoints);
                            yStringValue = activeScale.y.ToString("F" + decimalPoints);
                            zStringValue = activeScale.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Rect:
                            xStringValue = activeScale.x.ToString("F" + decimalPoints);
                            yStringValue = activeScale.y.ToString("F" + decimalPoints);
                            zStringValue = activeScale.z.ToString("F" + decimalPoints);
                            break;

                        case Tool.Transform:
                            switch (lastTool)
                            {
                                case Tool.Move:
                                    xStringValue = activeLocalPosition.x.ToString("F" + decimalPoints);
                                    yStringValue = activeLocalPosition.y.ToString("F" + decimalPoints);
                                    zStringValue = activeLocalPosition.z.ToString("F" + decimalPoints);
                                    break;

                                case Tool.Rotate:
                                    xStringValue = activeRotation.x.ToString("F" + decimalPoints);
                                    yStringValue = activeRotation.y.ToString("F" + decimalPoints);
                                    zStringValue = activeRotation.z.ToString("F" + decimalPoints);
                                    break;

                                case Tool.Scale:
                                    xStringValue = activeScale.x.ToString("F" + decimalPoints);
                                    yStringValue = activeScale.y.ToString("F" + decimalPoints);
                                    zStringValue = activeScale.z.ToString("F" + decimalPoints);
                                    break;
                            }
                            break;
                    }
                }

                //Set label positions based on if its local or global
                if (pivotRotation == PivotRotation.Local)
                {
                    xLabelPosition = (handlePosition + (activeTransform.right * handleSize));
                    yLabelPosition = (handlePosition + (activeTransform.up * handleSize));
                    zLabelPosition = (handlePosition + (activeTransform.forward * handleSize));
                }
                else
                {
                    xLabelPosition = (activePosition + (Vector3.right * handleSize));
                    yLabelPosition = (activePosition + (Vector3.up * handleSize));
                    zLabelPosition = (activePosition + (Vector3.forward * handleSize));
                }
                
                #region DISPLAY AXIS VALUES

                //Set style
                transformInfoStyle.normal.textColor = textColor;
                transformInfoStyle.fontSize = textSize;

                //Display values
                if (editingXValue)
                {
                    Handles.Label(xLabelPosition, "<b><color=#ff4925>X </color></b>" + xEditStringValue, transformInfoStyle);
                }
                else
                {
                    Handles.Label(xLabelPosition, "<b><color=#ff4925>X </color></b>" + xStringValue, transformInfoStyle);
                }

                if (editingYValue)
                {
                    Handles.Label(yLabelPosition, "<b><color=#b9ff5c>Y </color></b>" + yEditStringValue, transformInfoStyle);
                }
                else
                {
                    Handles.Label(yLabelPosition, "<b><color=#b9ff5c>Y </color></b>" + yStringValue, transformInfoStyle);
                }

                if (editingZValue)
                {
                    Handles.Label(zLabelPosition, "<b><color=#448dff>Z </color></b>" + zEditStringValue, transformInfoStyle);
                }
                else
                {
                    Handles.Label(zLabelPosition, "<b><color=#448dff>Z </color></b>" + zStringValue, transformInfoStyle);
                }

                #endregion

            }
        }
    }

    #endregion

    #region MENU ITEMS

    #region TOGGLE ENABLED
    [MenuItem(isEnabledString, false, 0)]
    private static void ToggleEnabled()
    {
        isEnabled = !isEnabled;
        EditorPrefs.SetBool(isEnabledString, isEnabled);
    }

    [MenuItem(isEnabledString, true, 0)]
    private static bool ToggleEnabledValidate()
    {
        Menu.SetChecked(isEnabledString, isEnabled);
        return true;
    }
    #endregion

    #region TOGGLE FOLLOW HANDLE
    [MenuItem(followingHandleString, false, 11)]
    private static void ToggleFollowHandle()
    {
        followingHandle = !followingHandle;
        EditorPrefs.SetBool(followingHandleString, followingHandle);
    }

    [MenuItem(followingHandleString, true, 11)]
    private static bool ToggleFollowHandleValidate()
    {
        Menu.SetChecked(followingHandleString, followingHandle);
        return true;
    }
    #endregion

    #region ADD/REMOVE DECIMAL
    [MenuItem(addDecimalPointString, false, 22)]
    static void AddDecimalPoint()
    {
        decimalPoints++;
        if (decimalPoints > 8) decimalPoints = 8;

        EditorPrefs.SetInt(addDecimalPointString, decimalPoints);
    }

    [MenuItem(removeDecimalPointString, false, 22)]
    static void RemoveDecimalPoint()
    {
        decimalPoints--;
        if (decimalPoints < 0) decimalPoints = 0;

        EditorPrefs.SetInt(addDecimalPointString, decimalPoints);
    }
    #endregion

    #region INCREASE/DECREASE FONT SIZE
    [MenuItem(increaseSizeString, false, 33)]
    static void IncreaseSize()
    {
        textSize += 1;
        if (textSize > 35) textSize = 35;

        EditorPrefs.SetInt(increaseSizeString, textSize);
    }

    [MenuItem(decreaseSizeString, false, 33)]
    static void DecreaseSize()
    {
        textSize -= 1;
        if (textSize < 20) textSize = 20;

        EditorPrefs.SetInt(increaseSizeString, textSize);
    }
    #endregion

    #region TRANSFORM CONTEXT MENU
    [MenuItem("CONTEXT/Transform/Toggle Transform Info", false, 0)]
    static void ContextToggle()
    {
        ToggleEnabled();
    }
    #endregion

    #endregion

    #region FUNCTION

    static void CharacterAdded(string key)
    {
        if (editingXValue)
        {
            xEditStringValue += key;
        }
        if (editingYValue)
        {
            yEditStringValue += key;
        }
        if (editingZValue)
        {
            zEditStringValue += key;
        }
    }
    static void ClearLastCharacter()
    {
        if (editingXValue)
        {
            if (xEditStringValue.Length != 0)
                xEditStringValue = xEditStringValue.Substring(0, xEditStringValue.Length - 1);
        }
        if (editingYValue)
        {
            if (yEditStringValue.Length != 0)
                yEditStringValue = yEditStringValue.Substring(0, yEditStringValue.Length - 1);
        }
        if (editingZValue)
        {
            if (zEditStringValue.Length != 0)
                zEditStringValue = zEditStringValue.Substring(0, zEditStringValue.Length - 1);
        }
    }
    static void ApplyValue()
    {
        if (activeGameObjects.Length > 0)
        {
            for (int i = 0; i < activeGameObjects.Length; i++)
            {
                Undo.RecordObject(activeGameObjects[i].transform, "Translate");

                if (activeGameObjects[i].transform != null)
                {
                    #region TRANSLATION
                    if (editingXValue && (xEditStringValue != null && xEditStringValue.Length < 8))
                    {                      
                        parsePassedCache = (float.TryParse(SendCommand(xEditStringValue), out checkedFloat));
                        if (parsePassedCache)
                        {
                            switch (currentTool)
                            {
                                case Tool.Move:
                                    switch(commandKey)
                                    {
                                        case KeyCode.None: 
                                            activeGameObjects[i].transform.localPosition = new Vector3(checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);                                        
                                            break;                 
                                       
                                        case KeyCode.KeypadPlus: 
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x + checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                            break;

                                        case KeyCode.KeypadMultiply: 
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x * checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                            break;

                                        case KeyCode.KeypadDivide: 
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x / checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                            break;
                                    }
                                    break;

                                case Tool.Rotate:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;

                                        case KeyCode.KeypadPlus:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x + checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x * checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;

                                        case KeyCode.KeypadDivide:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x / checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;
                                    }                                 
                                    break;

                                case Tool.Scale:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localScale = new Vector3(checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x + checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x * checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x / checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;
                                    }                                  
                                    break;

                                case Tool.Rect:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localScale = new Vector3(checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x + checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x * checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x / checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                            break;
                                    }
                                    break;

                                case Tool.Transform:
                                    switch (lastTool)
                                    {
                                        case Tool.Move:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x + checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x * checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x / checkedFloat, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z);
                                                    break;
                                            }
                                            break;

                                        case Tool.Rotate:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x + checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x * checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x / checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;
                                            }
                                            break;

                                        case Tool.Scale:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    activeGameObjects[i].transform.localScale = new Vector3(checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x + checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x * checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x / checkedFloat, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z);
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }

                        if (parsePassedCache == false) sceneView.ShowNotification(new GUIContent("Invalid Value"));
                    }
                    if (editingYValue && (yEditStringValue != null && yEditStringValue.Length < 8))
                    {
                        parsePassedCache = (float.TryParse(SendCommand(yEditStringValue), out checkedFloat));
                        if (parsePassedCache)
                        {
                            switch (currentTool)
                            {
                                case Tool.Move:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y + checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y * checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x + activeGameObjects[i].transform.localPosition.y / checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                            break;
                                    }
                                    break;

                                case Tool.Rotate:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;

                                        case KeyCode.KeypadPlus:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y + checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y * checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;

                                        case KeyCode.KeypadDivide:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y / checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                            break;
                                    }
                                    break;

                                case Tool.Scale:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y + checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y * checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y / checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;
                                    }
                                    break;

                                case Tool.Rect:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y + checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y * checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y / checkedFloat, activeGameObjects[i].transform.localScale.z);
                                            break;
                                    }
                                    break;

                                case Tool.Transform:
                                    switch (lastTool)
                                    {
                                        case Tool.Move:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y + checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y * checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x,  activeGameObjects[i].transform.localPosition.y / checkedFloat, activeGameObjects[i].transform.localPosition.z);
                                                    break;
                                            }
                                            break;

                                        case Tool.Rotate:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y + checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y * checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y / checkedFloat, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z));
                                                    break;
                                            }
                                            break;

                                        case Tool.Scale:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, checkedFloat, activeGameObjects[i].transform.localScale.z);
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y + checkedFloat, activeGameObjects[i].transform.localScale.z);
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y * checkedFloat, activeGameObjects[i].transform.localScale.z);
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y / checkedFloat, activeGameObjects[i].transform.localScale.z);
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }

                        if (parsePassedCache == false) sceneView.ShowNotification(new GUIContent("Invalid Value"));
                    }
                    if (editingZValue && (zEditStringValue != null && zEditStringValue.Length < 8))
                    {
                        parsePassedCache = (float.TryParse(SendCommand(zEditStringValue), out checkedFloat));
                        if (parsePassedCache)
                        {
                            switch (currentTool)
                            {
                                case Tool.Move:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, checkedFloat);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z + checkedFloat);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z * checkedFloat);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z / checkedFloat);
                                            break;
                                    }
                                    break;

                                case Tool.Rotate:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, checkedFloat));
                                            break;

                                        case KeyCode.KeypadPlus:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z + checkedFloat));
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z * checkedFloat));
                                            break;

                                        case KeyCode.KeypadDivide:
                                            TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z / checkedFloat));
                                            break;
                                    }
                                    break;

                                case Tool.Scale:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, checkedFloat);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z + checkedFloat);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z * checkedFloat);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z / checkedFloat);
                                            break;
                                    }
                                    break;

                                case Tool.Rect:
                                    switch (commandKey)
                                    {
                                        case KeyCode.None:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, checkedFloat);
                                            break;

                                        case KeyCode.KeypadPlus:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z + checkedFloat);
                                            break;

                                        case KeyCode.KeypadMultiply:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z * checkedFloat);
                                            break;

                                        case KeyCode.KeypadDivide:
                                            activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z / checkedFloat);
                                            break;
                                    }
                                    break;

                                case Tool.Transform:
                                    switch (lastTool)
                                    {
                                        case Tool.Move:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, checkedFloat);
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z + checkedFloat);
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z * checkedFloat);
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    activeGameObjects[i].transform.localPosition = new Vector3(activeGameObjects[i].transform.localPosition.x, activeGameObjects[i].transform.localPosition.y, activeGameObjects[i].transform.localPosition.z / checkedFloat);
                                                    break;
                                            }
                                            break;

                                        case Tool.Rotate:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, checkedFloat));
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z + checkedFloat));
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z * checkedFloat));
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, new Vector3(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).x, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).y, TransformUtils.GetInspectorRotation(activeGameObjects[i].transform).z / checkedFloat));
                                                    break;
                                            }
                                            break;

                                        case Tool.Scale:
                                            switch (commandKey)
                                            {
                                                case KeyCode.None:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, checkedFloat);
                                                    break;

                                                case KeyCode.KeypadPlus:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z + checkedFloat);
                                                    break;

                                                case KeyCode.KeypadMultiply:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z * checkedFloat);
                                                    break;

                                                case KeyCode.KeypadDivide:
                                                    activeGameObjects[i].transform.localScale = new Vector3(activeGameObjects[i].transform.localScale.x, activeGameObjects[i].transform.localScale.y, activeGameObjects[i].transform.localScale.z / checkedFloat);
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }

                        if (parsePassedCache == false) sceneView.ShowNotification(new GUIContent("Invalid Value"));
                    }
                    #endregion
                }

                PrefabUtility.RecordPrefabInstancePropertyModifications(activeGameObjects[i].transform);
            }

            //If value passed
            if (parsePassedCache)
            {
                //Hide axis value
                xEditStringValue = "";
                yEditStringValue = "";
                zEditStringValue = "";

                //Stop editing
                editingXValue = false;
                editingYValue = false;
                editingZValue = false;

                //Reset command key
                commandKey = KeyCode.None;
            }
        }

        FollowHandle();
    }
    static string SendCommand(string value)
    {
        if (value.StartsWith("+"))
        {
            commandKey = KeyCode.KeypadPlus;
            value = value.Substring(1);
        }
        if (value.StartsWith("/"))
        {
            commandKey = KeyCode.KeypadDivide;
            value = value.Substring(1);
        }
        if (value.StartsWith("*"))
        {
            commandKey = KeyCode.KeypadMultiply;
            value = value.Substring(1);
        }
        return value;
    }

    static void FollowHandle()
    {
        if (currentTool != Tool.None && currentTool != Tool.View && followingHandle)
        {
            sceneView.FrameSelected();
        } 
    }
    static void NextTool()
    {
        currentToolIndex = (int)Tools.current;
        currentToolIndex++;

        if (currentToolIndex == 6) currentToolIndex = 0;

        currentTool = (Tool)currentToolIndex;
        Tools.current = currentTool;       
    }
    static void NextObject()
    {
        if (activeTransform.parent == null)
        {
            if (activeTransform.childCount == 0) return;

            //Root >> First Child
            Selection.activeTransform = activeTransform.root.GetChild(0);
        }
        else
        {
            if (activeTransform.GetSiblingIndex() + 1 < activeTransform.parent.childCount)
            {
                if (activeTransform.childCount > 0)
                {
                    //Has Children >> Next Child
                    Selection.activeTransform = activeTransform.GetChild(0);
                }
                else
                {
                    //No Children >> Next Child

                    Selection.activeTransform = activeTransform.parent.GetChild(activeTransform.GetSiblingIndex() + 1);
                }
            }
            else
            {
                if (activeTransform.childCount > 0)
                {
                    //Has Children >> Last Child
                    Selection.activeTransform = activeTransform.GetChild(0);
                }
                else
                {
                    //No Children >> Last Child
                    try
                    {
                        Selection.activeTransform = activeTransform.parent.parent.GetChild(activeTransform.parent.GetSiblingIndex() + 1);
                    }
                    catch
                    {
                        try
                        {
                            Selection.activeTransform = activeTransform.parent.parent.parent.GetChild(activeTransform.parent.parent.GetSiblingIndex() + 1);
                        }
                        catch
                        {
                            try
                            {
                                Selection.activeTransform = activeTransform.parent.parent.parent.parent.GetChild(activeTransform.parent.parent.parent.GetSiblingIndex() + 1);
                            }
                            catch
                            {

                                try
                                {
                                    Selection.activeTransform = activeTransform.parent.parent.parent.parent.parent.GetChild(activeTransform.parent.parent.parent.parent.GetSiblingIndex() + 1);
                                }
                                catch
                                {
                                    Selection.activeTransform = Selection.activeTransform.root;
                                }
                            }
                        }


                    }
                }

            }
        }      
    }

    static void SaveRevertHistory()
    {
        revertValuesList.Clear();
        if (activeGameObjects != null && activeGameObjects.Length > 0)
        {
            for (int i = 0; i < activeGameObjects.Length; i++)
            {
                if (currentTool == Tool.Move)
                    revertValuesList.Add(activeGameObjects[i].transform.localPosition);

                if (currentTool == Tool.Rotate)
                    revertValuesList.Add(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform));

                if (currentTool == Tool.Scale)
                    revertValuesList.Add(activeGameObjects[i].transform.localScale);

                if (currentTool == Tool.Rect)
                    revertValuesList.Add(activeGameObjects[i].transform.localScale);

                if (currentTool == Tool.Transform)
                {
                    if (lastTool == Tool.Move)
                        revertValuesList.Add(activeGameObjects[i].transform.localPosition);

                    if (lastTool == Tool.Rotate)
                        revertValuesList.Add(TransformUtils.GetInspectorRotation(activeGameObjects[i].transform));

                    if (lastTool == Tool.Scale)
                        revertValuesList.Add(activeGameObjects[i].transform.localScale);

                    if (currentTool == Tool.Rect)
                        revertValuesList.Add(activeGameObjects[i].transform.localScale);

                }

            }
        }    
    }
    static void LoadRevertHistory()
    {
        if (revertValuesList != null && revertValuesList.Count > 0)
        {
            for (int i = 0; i < revertValuesList.Count; i++)
            {
                if (currentTool == Tool.Move)
                    activeGameObjects[i].transform.localPosition = revertValuesList[i];

                if (currentTool == Tool.Rotate)
                    TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, revertValuesList[i]);

                if (currentTool == Tool.Scale)
                    activeGameObjects[i].transform.localScale = revertValuesList[i];

                if (currentTool == Tool.Rect)
                    activeGameObjects[i].transform.localScale = revertValuesList[i];

                if (currentTool == Tool.Transform)
                {
                    if (lastTool == Tool.Move)
                        activeGameObjects[i].transform.localPosition = revertValuesList[i];

                    if (lastTool == Tool.Rotate)
                        TransformUtils.SetInspectorRotation(activeGameObjects[i].transform, revertValuesList[i]);

                    if (lastTool == Tool.Scale)
                        activeGameObjects[i].transform.localScale = revertValuesList[i];
                }

            }


            FollowHandle();

        }
    }

    static void UpdateLastTool()
    {
        if (currentTool == Tool.Transform && activeTransform != null)
        {
            switch (currentTool)
            {
                case Tool.Move: if (LastPos != activeTransform.localPosition) { lastTool = Tool.Move; } break;
                case Tool.Rotate: if (LastRot != activeRotation) { lastTool = Tool.Rotate; } break;
                case Tool.Scale:  if (LastScale != activeTransform.localScale) { lastTool = Tool.Scale; } break;
                case Tool.Transform:
                    if (LastPos != activeTransform.localPosition) lastTool = Tool.Move; 
                    if (LastRot != activeRotation) lastTool = Tool.Rotate;
                    if (LastScale != activeTransform.localScale) lastTool = Tool.Scale;
                    break;
            }
        }
    }

    static bool IsMaterialWindowOpen()
    {
        openWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        for (int i = 0; i < openWindows.Length; i++)
        {
            if (openWindows[i].titleContent.text.Contains("Select") || openWindows[i].titleContent.text.Contains("Add") || openWindows[i].titleContent.text.Contains("Color")) { return true; }
        }

        return false;
    }
    #endregion

}
#endif