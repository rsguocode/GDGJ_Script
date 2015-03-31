using UnityEngine;
using com.u3d.bases.display;

namespace com.game.module.fight.vo
{
	public class AttackVo
	{
        public string state;    //动作
        public float time;      //加入到队列的时间
        public Vector3 point;   //技能施法点(点击位置或者玩家位置)、移动位置
        public ActionDisplay followDisplay;    //追踪对象
        public int useType;     //手势类型
        public uint skillId;  //技能id
        public float injuredTime;   //受击点时间或者伤害浮动提示开始播放时间

        public bool isFromMe;   //是否来源自己客户端造成的伤害
        public bool isMe;       //是否我被造成伤害

        public bool isMove;             //是否移动
        public Vector3 MoveToPoint;     //受击后移动到的位置

        public float dire = -1f;       //攻击角度
        public Vector3 attackPoint;    //攻击产生的位置，便于受击时在背对着攻击的情况下切回正面播放受击动作
        public Vector3 targetPoint;    //目标所在的位置
        public string attackType;
        public GameObject target;

        public void clear()
        {
            state = null;
            time = 0;
            point = Vector3.zero;
        }
	}
}
