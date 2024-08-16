using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;

namespace KC
{
    [CustomEditor(typeof(HotfixDllConfig))]
    public class CollectHotfixDll : Editor
    {
        /// <summary>
        ///   <para>Implement this function to make a custom inspector.</para>
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            if (!EditorApplication.isPlaying )
            {
                return;
            }
            
            if (GUILayout.Button("Import"))
            {
                HotfixDllConfig config = (HotfixDllConfig)target;
                config.DllNames = SettingsUtil.HotUpdateAssemblyFilesExcludePreserved.ToArray();
            }
        }
    }
}