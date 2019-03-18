using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 在打开项目的时候需要加载功能脚本
/// </summary>
[InitializeOnLoad]
public static class ShowFileExtensions
{
    static ShowFileExtensions()
    {
        //委托用于OnGUI事件在项目窗口每个可见列表项。
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
    }

    private static void ProjectWindowItemOnGUI(string guid, Rect rect)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

        if (obj != null && AssetDatabase.IsMainAsset(obj) && !IsDirectory(obj))
        {
            if (showBigIcon)
            {
                string extension = Path.GetExtension(assetPath);
                GUI.Label(rect, extension, EditorStyles.boldLabel);
            }
            else
            {
                var fileName = Path.GetFileName(assetPath);
                var labelRect = rect.Translate();
                GUI.Label(labelRect, fileName);
            }
        }

        EditorApplication.RepaintProjectWindow();
    }

    private static bool showBigIcon
    {
        get { return IsTwoColumnMode && listAreaGridSize > 16f; }
    }


    private static bool IsTwoColumnMode
    {
        get
        {
            var projectWindow = GetProjectWindow();
            var projectWindowType = projectWindow.GetType();
            var modeFileInfo = projectWindowType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic);
            int mode = (int) modeFileInfo.GetValue(projectWindow);
            return mode == 1;
        }
    }

    /// <summary>
    ///  获取每个类标的Item的GridSize
    /// </summary>
    private static float listAreaGridSize
    {
        get
        {
            var projectWindow = GetProjectWindow();
            var projectWindowType = projectWindow.GetType();
            var propertyInfo = projectWindowType.GetProperty("listAreaGridSize", BindingFlags.Instance | BindingFlags.Public);
            return (float) propertyInfo.GetValue(projectWindow, null);
        }
    }

    /// <summary>
    ///  获取Project的EditorWindow
    /// </summary>
    /// <returns></returns>
    private static EditorWindow GetProjectWindow()
    {
        if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Project")
        {
            return EditorWindow.focusedWindow;
        }

        return GetExistingWindowByName("Project");
    }

    private static EditorWindow GetExistingWindowByName(string name)
    {
        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var item in windows)
        {
            if (item.titleContent.text == name)
            {
                return item;
            }
        }

        return default(EditorWindow);
    }

    private static Rect Translate(this Rect rect)
    {
        rect.x += 15.8f;
        rect.y += 0.9f;
        return rect;
    }

    private static bool IsDirectory(Object obj)
    {
        if (obj == null)
        {
            return false;
        }

        return obj is DefaultAsset && !AssetDatabase.IsForeignAsset(obj);
    }
}