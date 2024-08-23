using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using HybridCLR;
using UnityEngine;

namespace KC
{
    public class CodeLoader 
    {
        private Assembly _assembly;
        private Dictionary<string, TextAsset> _dlls;
        private Dictionary<string, TextAsset> _aotDlls;
        private string _hotfixClass;
        private string _hotfixMethod;
        private string _hotfixDll;
        private ResourcesManager _resourcesManager;

        public CodeLoader(ResourcesManager resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }

        public async UniTask DownloadAsync()
        {
            try
            {
                var loadConfig = Resources.Load<LoadConfig>("LoadConfig");
                if (string.IsNullOrEmpty(loadConfig.hotfixDll))
                {
                    Debug.LogError("热更入口LoadConfig文件填写的热更Dll资源名为空,无法进行热更");
                    return;
                }

                if (string.IsNullOrEmpty(loadConfig.staticHotfixEntryClass) ||
                    string.IsNullOrEmpty(loadConfig.staticHotfixEntryMethod))
                {
                    Debug.LogError("热更入口LoadConfig文件填写的类名或者方法名为空,无法进行热更");
                    return;
                }

                _hotfixClass = loadConfig.staticHotfixEntryClass;
                _hotfixMethod = loadConfig.staticHotfixEntryMethod;
                _hotfixDll = loadConfig.hotfixDll;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载热更入口LoadConfig文件失败,原因:{e}");
                return;
            }


#if !UNITY_EDITOR
            _dlls = await _resourcesManager.LoadAllAssetsAsync<TextAsset>($"{_hotfixDll}.dll");
            _aotDlls = await _resourcesManager.LoadAllAssetsAsync<TextAsset>("mscorlib.dll");
#endif

            await UniTask.CompletedTask;
        }

        public void Start()
        {
#if UNITY_EDITOR
            _assembly = System.AppDomain.CurrentDomain.GetAssemblies()
                .First(p => p.GetName().Name == _hotfixDll);
#else
            var entryDll = $"{_hotfixDll}.dll";
            var entryPdb = $"{_hotfixDll}.pdb";
            var hotfixAss = _dlls[entryDll].bytes;
            var hotfixPdb = _dlls[entryPdb].bytes;

            _dlls.Remove(entryDll);
            _dlls.Remove(entryPdb);
            
            foreach (var kv in _aotDlls)
            {
                TextAsset textAsset = kv.Value;
                RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, HomologousImageMode.SuperSet);
            }

            var hotfixLoads = GetLoads();
            while (hotfixLoads.Count > 0)
            {
                var list = hotfixLoads.Dequeue();
                if (list.Count == 1)
                {
                    Assembly.Load(list[0]);
                }
                else if (list.Count == 2)
                {
                    Assembly.Load(list[0],list[1]);
                }
            }
            _assembly = Assembly.Load(hotfixAss, hotfixPdb);
#endif
            var type = _assembly.GetType(_hotfixClass);
            MethodInfo method = type.GetMethod(_hotfixMethod);
            method?.Invoke(null, null);
        }

        private Queue<List<byte[]>> GetLoads()
        {
            HashSet<string> set = new HashSet<string>();
            Queue<List<byte[]>> queue = new Queue<List<byte[]>>();
            foreach (var key in _dlls.Keys)
            {
                var name = string.Empty;
                if (key.EndsWith(".dll") || key.EndsWith(".pdb"))
                {
                    name = key[..^4];
                }
                if (!set.Contains(name))
                {
                    var list = new List<byte[]> { _dlls[$"{name}.dll"].bytes, _dlls[$"{name}.pdb"].bytes };
                    queue.Enqueue(list);
                    set.Add(name);
                }
            }

            return queue;
        }
    }
}