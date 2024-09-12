namespace KC
{
    public class HotfixLoadingBinder
    {
         public readonly UnityEngine.Transform Self;

         public readonly UnityEngine.UI.RawImage BackgroundImage;

         public readonly UnityEngine.UI.Slider Slider;

         public readonly UnityEngine.RectTransform Icon;

         public readonly UnityEngine.UI.Text GameNameText;

         public readonly UnityEngine.UI.Text ProgressText;

         public readonly UnityEngine.UI.Text UpdateMessageText;

         /// <summary>
         /// 构建
         /// <param name="transform">根节点</param>
         /// </summary>
         public HotfixLoadingBinder(UnityEngine.Transform transform)
         {
            Self = transform;
            BackgroundImage = transform.Find("Background").GetComponent<UnityEngine.UI.RawImage>();
            Slider = transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>();
            Icon = transform.Find("Icon").GetComponent<UnityEngine.RectTransform>();
            GameNameText = transform.Find("GameName").GetComponent<UnityEngine.UI.Text>();
            ProgressText = transform.Find("Slider/ProgressText").GetComponent<UnityEngine.UI.Text>();
            UpdateMessageText = transform.Find("Slider/UpdateMessageText").GetComponent<UnityEngine.UI.Text>();
         }
    }
}
