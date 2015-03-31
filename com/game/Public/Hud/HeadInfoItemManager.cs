using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/05/04 01:43:18 
 * function: 头顶信息管理
 * *******************************************************/

namespace com.game.Public.Hud
{
    public class HeadInfoItemManager
    {
        public static HeadInfoItemManager Instance = new HeadInfoItemManager();


        /// <summary>
        ///     显示头顶信息
        /// </summary>
        /// <param name="value">信息内容</param>
        /// <param name="color">颜色</param>
        /// <param name="startPosition">初始位置</param>
        /// <param name="dir">方向</param>
        /// <param name="type">类型</param>
        /// <param name="totalShowTime">显示时间</param>
        /// <param name="labelSize">字体大小</param>
        public void ShowHeadInfo(string value, Color color, Vector3 startPosition, int dir, HeadInfoItemType type,
            float totalShowTime = 0.8f, int labelSize = 26)
        {
            GameObject headInfoObject = HeadInfoItemPool.Instance.SpawnHeadInfoItem().gameObject;
            var headInfoItem = headInfoObject.GetComponent<HeadInfoItem>();
            if (headInfoItem == null)
            {
                headInfoItem = headInfoObject.AddComponent<HeadInfoItem>();
                headInfoItem.Init();
            }
            headInfoItem.SetValue(value, color, startPosition, dir, type, totalShowTime, labelSize);
        }
    }
}