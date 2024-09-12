using UnityEngine;

namespace KC
{
    public enum UILayerType
    {
        /// <summary>
        /// 最底层(远离摄像机的层)
        /// </summary>
        Bottom = 0,
        
        /// <summary>
        /// 场景层,用于血条,伤害跳字等场景UI，它们一般需要被正常UI遮挡
        /// </summary>
        Scene,
        
        /// <summary>
        /// 面板层,用于所有面板
        /// </summary>
        Panel,
        
        /// <summary>
        /// 弹窗层,一般用于可多开的非全屏面板
        /// </summary>
        Popup,
        
        /// <summary>
        /// 提示层,一般用于飘字,确认弹窗,消息跑马灯等
        /// </summary>
        Tip,
        
        /// <summary>
        /// 最顶层(靠近摄像机的层),一般用于新手引导
        /// </summary>
        Top
    }
}