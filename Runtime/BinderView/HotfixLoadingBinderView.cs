using UnityEngine;
using UnityEngine.UI;

namespace KC
{
    [Binder("com.kc.hotloader/Runtime/Generate")]
    public class HotfixLoadingBinderView : BaseBinderView
    {
        public RawImage BackgroundImage;

        public Slider Slider;

        public RectTransform Icon;

        public Text GameNameText;

        public Text ProgressText;

        public Text UpdateMessageText;
    }
}