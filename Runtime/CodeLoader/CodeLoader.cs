using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using HybridCLR;
using UnityEngine;

namespace KC
{
    public class CodeLoader : Singleton<CodeLoader>,ISingletonAwake
    {
        private Assembly _assembly;
        private Dictionary<string, TextAsset> _dlls;
        private Dictionary<string, TextAsset> _aotDlls;
        private HotfixDllConfig _hotfixDllConfig;
        
        public void Awake()
        {
         
        }
        
        public async UniTask DownloadAsync()
        {
#if !UNITY_EDITOR
            _dlls = await ResourcesManager.Instance.LoadAllAssetsAsync<TextAsset>(
                "Packages/com.kc.hotloader/Bundles/Code/Unity.Hotfix.dll.bytes");

            _aotDlls = await ResourcesManager.Instance.LoadAllAssetsAsync<TextAsset>(
                "Packages/com.kc.hotloader/Bundles/Code/mscorlib.dll.bytes");
#endif
            await UniTask.CompletedTask;
        }
        
        public void Start(GameObject go)
        {
#if UNITY_EDITOR
                _assembly = System.AppDomain.CurrentDomain.GetAssemblies()
                .First(p => p.GetName().Name == "Unity.Hotfix");
#else
                var hotfixAss = _dlls["Unity.Hotfix.dll"].bytes;
                var hotfixPdb = _dlls["Unity.Hotfix.pdb"].bytes;
            
                foreach (var kv in this._aotDlls)
                {
                    TextAsset textAsset = kv.Value;
                    RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, HomologousImageMode.SuperSet);
                }
            
                _assembly = Assembly.Load(hotfixAss, hotfixPdb);
#endif
            var type = _assembly.GetType("Game.HotfixGameEntry");
            go.AddComponent(type);
        }
    }
}