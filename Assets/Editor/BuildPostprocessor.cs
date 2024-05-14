using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using UnityEngine;

public class BuildPostprocessor
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget _, string pathToBuiltProject)
    {
        PlayerSettings.WebGL.threadsSupport = true;

        var sourceFileName = Path.Combine(Application.dataPath, "WebGLTemplates", "netlify.toml");
        var destFileName = Path.Combine(pathToBuiltProject, "netlify.toml");
        File.Copy(sourceFileName, destFileName);
    }
}
