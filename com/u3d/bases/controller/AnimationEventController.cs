using com.u3d.bases.display.controler;
using UnityEngine;


/**动画事件控制类
 * @author 骆琦
 * @date  2013-11-6
 * 动画事件的响应和中转，包括伤害检测，开始特效播放等
 * 该脚本是模型的默认挂载脚本，美术编辑动画时模型的prefab上需要有该脚本，便于美术或策划决定哪一帧进行伤害检测
 * **/
namespace com.u3d.bases.controller
{
    public class AnimationEventController : MonoBehaviour
    {

        public SkillController skillController;
        private Transform thisTransform;
        public GraspThrowController GraspThrowController;

        /// <summary>
        /// 伤害检测事件
        /// </summary>
        public void checkDamanage(int index)
        {
            skillController.CheckDamage(skillController.transform.position, index);
        }

        /// <summary>
        /// 技能冲刺
        /// </summary>
        public void skillRush(int index)
        {
            skillController.SkillRush(index);
        }

        /// <summary>
        /// 技能位移
        /// </summary>
        public void skillMove(int index)
        {
            skillController.SkillMove(index);
        }

        /// <summary>
        /// 技能开始可控制移动
        /// </summary>
        public void skillStartCtrlMove()
        {
            skillController.SkillStartCtrlMove();
        }

        /// <summary>
        /// 技能停止可控制移动
        /// </summary>
        public void skillStopCtrlMove()
        {
            skillController.SkillStopCtrlMove();
        }

        /// <summary>
        /// 播放动作特效事件
        /// </summary>
        public void showEffect()
        {

        }

        /// <summary>
        /// 拉伸屏幕
        /// </summary>
        public void ScaleCamera()
        {
            skillController.ScaleCamera();
        }

        /// <summary>
        /// 抓取;
        /// </summary>
        public void Grasp()
        {
            GraspThrowController.Grasp();
        }

        /// <summary>
        /// 扔出去;
        /// </summary>
        public void Throw()
        {
            GraspThrowController.Throw();
        }

    }
}
