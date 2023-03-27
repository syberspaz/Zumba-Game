#if UNITY_EDITOR_OSX
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class AddVideoToolboxFramework : MonoBehaviour
{
#if UNITY_IOS
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            PBXProject project = new PBXProject();
            string sPath = PBXProject.GetPBXProjectPath(path);
            project.ReadFromFile(sPath);

            string tn = "UnityFramework";
            string g = project.TargetGuidByName(tn);

            ModifyAsDesired(project, g);

            File.WriteAllText(sPath, project.WriteToString());
        }
    }

    static void ModifyAsDesired(PBXProject project, string g)
    {

        project.AddFrameworkToProject(g, "VideoToolbox.framework", false);

        project.AddBuildProperty(g,
            "OTHER_LDFLAGS",
            "-force_load $(PROJECT_DIR)/Libraries/Plugins/iOS/libturbojpegiOS.a");

        project.AddBuildProperty(g,
            "OTHER_LDFLAGS",
            "-force_load $(PROJECT_DIR)/Libraries/Plugins/iOS/libBento4_iOS.a");
    }
#endif
}
#endif
