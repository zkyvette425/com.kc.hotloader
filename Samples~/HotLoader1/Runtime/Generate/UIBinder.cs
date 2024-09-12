namespace KC
{
    public class UIBinder
    {
         public readonly UnityEngine.Transform Self;

         public readonly UnityEngine.Canvas[] Canvas;

         /// <summary>
         /// 构建
         /// <param name="transform">根节点</param>
         /// </summary>
         public UIBinder(UnityEngine.Transform transform)
         {
            Canvas = new UnityEngine.Canvas[6];
            Self = transform;
            Canvas[0] = transform.Find("Bottom").GetComponent<UnityEngine.Canvas>();
            Canvas[1] = transform.Find("Scene").GetComponent<UnityEngine.Canvas>();
            Canvas[2] = transform.Find("Panel").GetComponent<UnityEngine.Canvas>();
            Canvas[3] = transform.Find("Popup").GetComponent<UnityEngine.Canvas>();
            Canvas[4] = transform.Find("Tip").GetComponent<UnityEngine.Canvas>();
            Canvas[5] = transform.Find("Top").GetComponent<UnityEngine.Canvas>();
         }
    }
}
