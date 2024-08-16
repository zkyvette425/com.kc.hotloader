using System;
using UnityEngine;

namespace KC
{
    [CreateAssetMenu(menuName = "KC/HotfixDll",fileName = "HotfixDll",order = 0)]
    public class HotfixDllConfig : ScriptableObject
    {
        public string[] DllNames;
        
        
    }
}