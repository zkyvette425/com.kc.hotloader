using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KC
{
    public sealed class Init : MonoBehaviour
    {
        private ResourcesManager _resourcesManager;

        private void Start()
        {
            DontDestroyOnLoad(this);
            _resourcesManager = new ResourcesManager();
            InternalInit().ToCoroutine();
        }

        private async UniTask InternalInit()
        {
            await _resourcesManager.CreatePackageAsync("DefaultPackage");
            var packageVersion = await _resourcesManager.UpdatePackageVersion();
            await _resourcesManager.UpdateManifest(packageVersion);
            var downloader = _resourcesManager.GetDownloader();
            Debug.Log($"TotalDownloadCount:{downloader.TotalDownloadCount}");
            if (downloader.TotalDownloadCount != 0)
            {
                var hotfixViewHandler = _resourcesManager.Package.LoadAssetAsync<GameObject>("HotfixLoadingView");
                await hotfixViewHandler.Task;
                var hotfixView = Instantiate((GameObject)hotfixViewHandler.AssetObject)
                    .AddComponent<HotfixLoadingView>();
                hotfixView.DownloaderOperation = downloader;
                await UniTask.DelayFrame(3);
                await _resourcesManager.Download(downloader);
                await UniTask.WaitUntil(() => hotfixView.IsComplete);
                Destroy(hotfixView.gameObject);
                hotfixViewHandler.Release();
            }

            var codeLoader = new CodeLoader(_resourcesManager);
            await codeLoader.DownloadAsync();
            codeLoader.Start();
            _resourcesManager = null;
        }
    }
}
