using System.IO;
using System.Reflection;
using HybridCLR.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace KC
{
    public static class HybridClrEditor
    {
        [MenuItem("KC/UpdateAotDlls")]
        public static void CopyAotDll()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            string fromDir = Path.Combine(HybridCLRSettings.Instance.strippedAOTDllOutputRootDir, target.ToString());
            string toDir = GetPath();
            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }
            Directory.CreateDirectory(toDir);

            foreach (string aotDll in HybridCLRSettings.Instance.patchAOTAssemblies)
            {
                File.Copy(Path.Combine(fromDir, aotDll), Path.Combine(toDir, $"{aotDll}.bytes"), true);
            }
            Debug.Log($"CopyAotDll Finish!");
            AssetDatabase.Refresh();
        }

        private static string GetPath()
        {
            return Directory.Exists(Path.Combine(Application.dataPath, "com.kc.hotloader"))
                ? "Assets/com.kc.hotloader/Bundle/Aot"
                : "Packages/com.kc.hotloader/Bundle/Aot";
        }
    }
}