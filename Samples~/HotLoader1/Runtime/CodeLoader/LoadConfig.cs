using System;
using UnityEngine;

namespace KC
{
    [CreateAssetMenu(menuName = "KC/LoadConfig",fileName = "LoadConfig",order = 0)]
    public class LoadConfig : ScriptableObject
    {
        [Header("静态热更入口程序集资源名称")]
        public string hotfixDll = "KC.Demo";
        
        [Header("静态热更入口类名,比如KC.GameEntry")]
        public string staticHotfixEntryClass = "KC.HotfixStart";

        [Header("静态热更入口方法名,比如Start")]
        public string staticHotfixEntryMethod = "Start";
    }
}