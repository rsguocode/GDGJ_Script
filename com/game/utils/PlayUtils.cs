using AnimationOrTween;
using com.u3d.bases.display;
using com.u3d.bases.display.controler;
//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 yidao studio
//All rights reserved
//文件名称：PlayUtils;
//文件描述：;
//创建者：潘振峰;
//创建日期：2014/6/10 15:41:07;
//////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using com.u3d.bases.consts;

namespace Com.Game.Utils
{
    public class PlayUtils
    {
        /// <summary>
        /// 在当前场景中获取有对应tag的角色的部位;
        /// 注意：确保tag已经在编辑器中声明，不然会报错;
        /// </summary>
        /// <param name="_hostId"></param>
        /// <param name="_partBoneTag"></param>
        /// <returns></returns>
        public static GameObject GetPartBonesByHostAndTag(string _hostId, string _partBoneTag)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag(_partBoneTag);
            if (gos != null && gos.Length > 0)
            {
                BaseControler tmpBaseController = null;
                foreach (GameObject tmpGo in gos)
                {
                    tmpBaseController = tmpGo.transform.root.GetComponent<BaseControler>();
                    if (tmpBaseController != null)
                    {
                        if (tmpBaseController.Me.GetVo().Id.ToString() == _hostId)
                        {
                            return tmpGo;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 将A面向B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void MakeAFaceToB(BaseDisplay a, BaseDisplay b)
        {
            if (a.CurFaceDire == Directions.Left && a.Controller.transform.position.x < b.Controller.transform.position.x)
            {
                a.ChangeDire(Directions.Right);
            }
            else if (a.CurFaceDire == Directions.Right && a.Controller.transform.position.x > b.Controller.transform.position.x)
            {
                b.ChangeDire(Directions.Left);
            }
        }

        /// <summary>
        /// 设置ActionDisplay集合的boxCollider是否可用;
        /// </summary>
        /// <param name="actDisplays"></param>
        /// <param name="boxCollEnable"></param>
        /// <returns></returns>
        public static IDictionary<ActionDisplay, bool> SetActionDisplaysBoxColliderEnable(List<ActionDisplay> actDisplays, bool boxCollEnable)
        {
            IDictionary<ActionDisplay, bool> actDic = new Dictionary<ActionDisplay, bool>();
            foreach (ActionDisplay actD in actDisplays)
            {
                actDic[actD] = actD.BoxCollider2D.enabled;
                actD.BoxCollider2D.enabled = boxCollEnable;
            }
            return actDic;
        }

        /// <summary>
        /// 设置ActionDisplay集合的boxCollider是否可用;
        /// </summary>
        /// <param name="dicActPlays"></param>
        public static void SetActionDisplaysBoxColliderEnable(IDictionary<ActionDisplay, bool> dicActPlays)
        {
            foreach (KeyValuePair<ActionDisplay, bool> kvp in dicActPlays)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.BoxCollider2D.enabled = kvp.Value;
                }
            }
        }

    }
}
