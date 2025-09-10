using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public static class OpenSaveFolderEditor
{
    [MenuItem("Tools/세이브파일 폴더 열기")]
    public static void OpenSaveFolder()
    {
        string path = Application.persistentDataPath;
        if (Directory.Exists(path))
        {
            Process.Start(path);
        }
        else
        {
            UnityEngine.Debug.LogWarning("세이브파일 폴더가 존재하지 않습니다: " + path);
        }
    }
}