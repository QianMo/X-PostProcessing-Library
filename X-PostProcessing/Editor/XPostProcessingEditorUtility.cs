using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

public class XPostProcessingEditorUtility : Editor
{

    #region Collape Hierarchy相关工具函数


    public static void Collapse(GameObject go, bool collapse)
    {
        if (go == null || go.transform == null)
        {
            return;
        }
        // bail out immediately if the go doesn't have children
        if (go.transform.childCount == 0) return;
        // get a reference to the hierarchy window
        var hierarchy = GetFocusedWindow("Hierarchy");
        // select our go
        SelectObject(go);
        // create a new key event (RightArrow for collapsing, LeftArrow for folding)
        var key = new Event { keyCode = collapse ? KeyCode.RightArrow : KeyCode.LeftArrow, type = EventType.KeyDown };
        // finally, send the window the event
        hierarchy.SendEvent(key);
    }

    public static void SelectObject(Object obj)
    {
        Selection.activeObject = obj;
    }
    public static EditorWindow GetFocusedWindow(string window)
    {
        FocusOnWindow(window);
        return EditorWindow.focusedWindow;
    }
    public static void FocusOnWindow(string window)
    {
        EditorApplication.ExecuteMenuItem("Window/" + window);
    }

    #endregion




    public static readonly string DISPLAY_TITLE_PREFIX =  "X-" ;

    public static string GetEnumName(SerializedParameterOverride prop)
    {
        return " (" + prop.value.enumDisplayNames[prop.value.intValue] + ")";
    }

    public static string GetEnumNameEX(SerializedParameterOverride prop)
    {
        return ((prop.overrideState.boolValue) ? " (" + prop.value.enumDisplayNames[prop.value.intValue] + ")" : string.Empty);
    }

    [MenuItem("Windows/OpenFrameDebugger %F")]
    public static void OpenFrameDebugger()
    {
        EditorApplication.ExecuteMenuItem("Window/Frame Debugger");
    }



}
