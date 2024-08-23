using System.IO;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace KC
{
    public static class AssemblyTool
    {
        /// <summary>
        /// 菜单和快捷键编译按钮
        /// </summary>
        [MenuItem("KC/UpdateHotfix _F6")]
        static void MenuItemOfCompile()
        {
            // 强制刷新一下，防止关闭auto refresh，文件修改时间不准确
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            DoCompile();
        }

        /// <summary>
        /// 执行编译代码流程
        /// </summary>
        public static void DoCompile()
        {
            // 强制刷新一下，防止关闭auto refresh，编译出老代码
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            CompileDllCommand.CompileDllActiveBuildTarget();

            CopyHotUpdateDlls();
            Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();

            Debug.Log("Compile Finish!");
        }

        /// <summary>
        /// 将dll文件复制到加载目录
        /// </summary>
        private static void CopyHotUpdateDlls()
        {
            string path = GetPath();
            Debug.Log(path);
            Directory.CreateDirectory(path);
            ClearDirectory(path);

            var outputDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);
            foreach (string dllName in SettingsUtil.HotUpdateAssemblyNamesIncludePreserved)
            {
                string sourceDll = $"{outputDir}/{dllName}.dll";
                string sourcePdb = $"{outputDir}/{dllName}.pdb";
                File.Copy(sourceDll, $"{path}/{dllName}.dll.bytes", true);
                File.Copy(sourcePdb, $"{path}/{dllName}.pdb.bytes", true);
            }

            AssetDatabase.Refresh();
        }

        private static void ClearDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }
            foreach (string subdir in Directory.GetDirectories(dir))
            {
                Directory.Delete(subdir, true);
            }

            foreach (string subFile in Directory.GetFiles(dir))
            {
                File.Delete(subFile);
            }
        }
        
        private static string GetPath()
        {
            return Directory.Exists(Path.Combine(Application.dataPath, "com.kc.hotloader"))
                ? "Assets/com.kc.hotloader/Bundle/Hotfix"
                : "Packages/com.kc.hotloader/Bundle/Hotfix";
        }
    }
}

