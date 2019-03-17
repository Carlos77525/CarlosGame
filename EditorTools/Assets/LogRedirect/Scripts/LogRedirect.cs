using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

public static class LogRedirect
{
    private static readonly Regex LogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");

    /// <summary>
    /// 用于在Unity中打开资产的回调属性（例如，在项目浏览器中双击资源时会触发回调）。
    ///将此属性添加到静态方法将使Unity在打开资产时调用该方法。该方法应具有以下签名：
    ///static bool OnOpenAsset(int instanceID, int line) 
    /// 如果处理资产的开放则返回true;如果外部工具应打开它，则返回false。
    /// </summary>
    /// <param name="instanceId"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    [OnOpenAssetAttribute(0)]
    private static bool OnOpenAsset(int instanceId, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instanceId).name;

        string msg = GetSelectedStackTrace();
        Tools.FileHelper.CreateFile(Path.Combine(Application.dataPath, "ADebug/LogFile"), msg);
        if (string.IsNullOrEmpty(msg)) return false;
        if (!msg.Contains("Debugger.cs")) return false;
        Match match = LogRegex.Match(msg);
        if (!match.Success) return false;

        match = match.NextMatch();
        if (!match.Success) return false;

        InternalEditorUtility.OpenFileAtLineExternal(
            Path.Combine(Application.dataPath, match.Groups[1].Value.Substring(7)), int.Parse(match.Groups[2].Value));
        return true;
    }


    /// <summary>
    /// 获取点击Log的文本信息
    /// </summary>
    /// <returns></returns>
    private static string GetSelectedStackTrace()
    {
        Assembly editorWindowAssembly = typeof(EditorWindow).Assembly;
        if (editorWindowAssembly == null)
        {
            return null;
        }

        System.Type consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
        if (consoleWindowType == null)
        {
            return null;
        }

        FieldInfo consoleWindowFieldInfo =
            consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        if (consoleWindowFieldInfo == null)
        {
            return null;
        }

        EditorWindow consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
        if (consoleWindow == null)
        {
            return null;
        }

        if (consoleWindow != EditorWindow.focusedWindow)
        {
            return null;
        }

        FieldInfo activeTextFieldInfo =
            consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
        if (activeTextFieldInfo == null)
        {
            return null;
        }

        return (string) activeTextFieldInfo.GetValue(consoleWindow);
    }
}