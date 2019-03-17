using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;


public static class OpenFolder
{
    [MenuItem("Open Folder/Data Path", false, 10)]
    public static void OpenFolderDataPath()
    {
        Execute(Application.dataPath);
    }

    [MenuItem("Open Folder/Persistent Data Path", false, 11)]
    public static void OpenFolderPersistentDataPath()
    {
        Execute(Application.persistentDataPath);
    }

    [MenuItem("Open Folder/Streaming Assets Path", false, 12)]
    public static void OpenFolderStreamingAssetsPath()
    {
        Execute(Application.streamingAssetsPath);
    }

    private static void Execute(string folder)
    {
        folder = String.Format("\"{0}\"", folder);
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                break;
            case RuntimePlatform.OSXEditor:
                Process.Start("open", folder);
                break;
            default:
                throw new IOException(string.Format("Not support open folder on '{0}' platform.",
                    Application.platform.ToString()));
        }
    }
}