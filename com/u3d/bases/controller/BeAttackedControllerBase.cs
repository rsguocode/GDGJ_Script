using com.game.module.fight.vo;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.game.vo;
using com.u3d.bases.consts;

/**受击控制类
 * @author 骆琦
 * @date  2013-11-5
 * 实现受击控制处理
 * **/
namespace com.u3d.bases.controller
{
    public class BeAttackedControllerBase : MonoBehaviour {

        public ActionControler meController;

        virtual public void BeAttacked(ActionVo actionVo) {
            //判断抓取打断;
            meController.JudgeGraspIntercept();
        }

        /// <summary>
        /// 带硬直的受击1
        /// </summary>
        /// <param name="_actionVo">行为</param>
        /// <param name="_monsterVo">怪物属性</param>
        protected void SetHurt1(ActionVo _actionVo)
        {
            meController.StatuController.SetHurt1Immediately();
            meController.GetMeVo().HitRecoverTime = _actionVo.HitRecover * 0.001f;
            meController.GetMeVo().IsHitRecover = true;
        }


        /// <summary>
        /// 带硬直的受击2
        /// </summary>
        /// <param name="_actionVo">行为</param>
        /// <param name="_monsterVo">怪物属性</param>
        protected void SetHurt2(ActionVo _actionVo)
        {
            meController.StatuController.SetHurt2Immediately();
            meController.GetMeVo().HitRecoverTime = _actionVo.HitRecover * 0.001f;
            meController.GetMeVo().IsHitRecover = true;
        }

        /// <summary>
        /// 不带硬直受击3
        /// </summary>
        /// <param name="_actionVo"></param>
        /// <param name="_monsterVo"></param>
        protected void SetHurt3(ActionVo _actionVo)
        {
            meController.StatuController.SetStatu(Status.HURT3);
        }

        //带硬直受击4
        protected void SetHurt4(ActionVo _actionVo)
        {
            meController.StatuController.SetHurt4Immediately();
            meController.GetMeVo().HitRecoverTime = _actionVo.ForceFeedBack * 0.001f;//这里策划要求空中受击时应硬直时间为攻击者的力反馈时间
            meController.GetMeVo().IsHitRecover = true;
        }
    }
}
