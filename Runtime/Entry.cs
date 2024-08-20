using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KC
{
    public sealed class Entry : MonoBehaviour
    {
        public WorldBinder WorldBinder { get; private set; }

        private void Start()
        {
            DontDestroyOnLoad(this);
            WorldBinder = new WorldBinder(transform);
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            SingleManager.Instance.AddSingleton<TimeInfo>();

            await SingleManager.Instance.AddSingleton<ResourcesManager>().CreatePackageAsync("DefaultPackage");
            
            var packageVersion = await ResourcesManager.Instance.UpdatePackageVersion();
            await ResourcesManager.Instance.UpdateManifest(packageVersion);
            
            var downloader = await ResourcesManager.Instance.GetDownloader();
            if (downloader.TotalDownloadCount != 0)
            {
                var hotfixViewHandler=   ResourcesManager.Instance.Package.LoadAssetAsync<GameObject>("HotfixLoadingView");
                await hotfixViewHandler.Task;
                var hotfixView = Instantiate((GameObject)hotfixViewHandler.AssetObject,WorldBinder.GetUI(UILayerType.Panel).transform)
                    .AddComponent<HotfixLoadingView>();
                await UniTask.DelayFrame(3);
                hotfixView.DownloaderOperation = downloader;
                await ResourcesManager.Instance.Download(downloader);
                await UniTask.WaitUntil(() => hotfixView.IsComplete);
                Destroy(hotfixView.gameObject);
                hotfixViewHandler.Release();
            }
            
            await CodeLoader.Instance.DownloadAsync();
            CodeLoader.Instance.Start();
        }

        private void Update()
        {
            TimeInfo.Instance.Update();
            World.Update();
        }

        private void LateUpdate()
        {
            World.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            SingleManager.Instance.Dispose();
        }
        
    }
}
