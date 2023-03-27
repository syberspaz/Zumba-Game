#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AddShadersToBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        if(report.summary.platform == UnityEditor.BuildTarget.StandaloneWindows64)
        {
            string appOutputPath = report.summary.outputPath;
            string vertexDataFolderPath = appOutputPath.Replace(".exe", "_Data/Plugins/x86_64/tile_renderer_v.glsl");
            string fragDataFolderPath = appOutputPath.Replace(".exe", "_Data/Plugins/x86_64/tile_renderer_f.glsl");
            string vertexShaderPath = Application.dataPath + "/Plugins/tile_renderer_v.glsl";
            string fragShaderPath = Application.dataPath + "/Plugins/tile_renderer_f.glsl";
            File.Copy(vertexShaderPath, vertexDataFolderPath, true);
            File.Copy(fragShaderPath, fragDataFolderPath, true);
        }
    }
}
#endif