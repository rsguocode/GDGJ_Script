using com.game.Public.Hud;
using com.game.vo;
using com.u3d.bases.display;
using com.u3d.bases.display.controler;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/28 03:54:49 
 * function: AI控制基类，玩家AI，怪物AI继承自该基类
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class AiControllerBase : MonoBehaviour
    {
        protected BaseRoleVo BaseRoleVo;
        public ActionControler MeController;
        private bool _isAi; //是否执行ai


        public bool IsAi
        {
            get { return _isAi; }
        }

        /// <summary>
        ///     设置Ai
        /// </summary>
        public virtual void SetAi(bool value)
        {
            _isAi = value;
        }

        /// <summary>
        ///     播放受伤血浮动
        /// </summary>
        /// <param name="isDodge">是否闪避</param>
        /// <param name="isCrit">是否暴击</param>
        /// <param name="isParry">是否格挡</param>
        /// <param name="cutHp">掉血量</param>
        /// <param name="curHp">总的血量</param>
        /// <param name="color">掉血数字颜色</param>
        public virtual void AddHudView(bool isDodge, bool isCrit, bool isParry, int cutHp, int curHp, Color color)
        {
            var type = HeadInfoItemType.TypeNormal;
            string value = "";
            Color showColor = color;
            if (isDodge) // 闪避
            {
                showColor = Color.green;
                value = "MISS!";
                type = HeadInfoItemType.TypeDodge;
            }
            else
            {
                if (isCrit)
                {
                    value = "暴击" + cutHp;
                    showColor = Color.cyan;
                    type = HeadInfoItemType.TypeCrit;
                }
                else
                {
                    if (isParry)
                    {
                        value = "格挡" + cutHp;
                    }
                    else
                    {
                        value = "-" + cutHp;
                    }
                }
            }
            Vector3 pos = MeController.transform.position;
            pos.y += MeController.GetMeByType<ActionDisplay>().BoxCollider2D.size.y*0.6f;
            HeadInfoItemManager.Instance.ShowHeadInfo(value, showColor, pos, MeController.Me.CurFaceDire, type);
            BaseRoleVo.CurHp = curHp >= 0 ? (uint) curHp : 0;
        }
    }
}