using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Random = UnityEngine.Random;

namespace KC
{
    public class HotfixLoadingView : MonoBehaviour
    {
        private HotfixLoadingBinder _binder;
        
        public ResourceDownloaderOperation DownloaderOperation { get; set; }

        public bool IsComplete { get; private set; }

        private void Awake()
        {
            _binder = new HotfixLoadingBinder(transform);
        }

        private void Start()
        {
            Refresh(1, 0);
            DownloaderOperation.OnDownloadProgressCallback += OnDownloadProgressCallback;
            DownloaderOperation.OnDownloadOverCallback += OnDownloadOverCallback;
        }

        private void OnDownloadOverCallback(bool issucceed)
        {
            OnComplete().Forget();
        }

        private async UniTaskVoid OnComplete()
        {
            _binder.UpdateMessageText.text = "正在应用中";
            _binder.Slider.value = 90;
            _binder.ProgressText.text = "90%";
            await UniTask.Delay(Random.Range(3000, 5000));
            _binder.Slider.value = 100;
            _binder.ProgressText.text = "100%";
            await UniTask.Delay(1000);
            IsComplete = true;
        }

        private void Refresh(long totaldownloadbytes, long currentdownloadbytes)
        {
            var size = currentdownloadbytes / 1024;
            var totalSize = totaldownloadbytes / 1024;
            _binder.Slider.value = size * 100f / totalSize;
            string text = string.Format("正在更新 {0}/{1}", size, totalSize);
            _binder.UpdateMessageText.text = text;
            _binder.ProgressText.text = $"{_binder.Slider.value:F1}%";
        }

        private void OnDownloadProgressCallback(int totaldownloadcount, int currentdownloadcount, long totaldownloadbytes, long currentdownloadbytes)
        {
            Refresh(totaldownloadbytes, currentdownloadbytes);
        }

        private void Update()
        {
            _binder.Icon.Rotate(Vector3.forward,60 * Time.deltaTime);
        }
    }
}