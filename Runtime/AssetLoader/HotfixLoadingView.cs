using UnityEngine;
using YooAsset;

namespace KC
{
    public partial class HotfixLoadingView : MonoBehaviour
    {
        public ResourceDownloaderOperation DownloaderOperation { get; set; }

        public bool IsComplete { get; private set; }
    }
}