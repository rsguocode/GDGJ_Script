using com.u3d.bases.consts;
using UnityEngine;

namespace com.game.module.fight.vo
{
	public class ActionVo
	{
        public uint SkillId;                    //动作对应的技能id
        public string ActionType;               //动作行为
        public Vector3 RunDestination;          //移动行为的目标点
        public Vector3 HurtDestination;         //受击后移动到的位置
        public Vector3 SkillUsePoint;           //攻击产生的位置，便于受击时在背对着攻击的情况下切回正面播放受击动作
        public Vector3 TargetPoint;             //目标所在的位置
        public GameObject Target;               //动作的目标对象
	    public int HurtEffectIndex;             //受击特效索引
	    public bool IsBullet = false;           //是否是受子弹攻击
	    public int HurtType = Actions.HurtNormal;//受击方式
	    public int FloatingValue;               //浮空系数
        public int ForceFeedBack;               //力反馈
        public int HitRecover;                  //攻击硬直（毫秒为单位的时间）
        public int HurtAnimation = Actions.Hurt1;//受击动作
        public int ProtectValue;                //保护值
        public float Velocity_Origin;             //初速度
        public float Angle;                       //初速度的角度
        public int FaceDirection;                 //攻击者的面朝向 1：右 -1：左

        public void Clear()
        {
            ActionType = null;
            RunDestination = Vector3.zero;
            Target = null;
        }
	}
}
