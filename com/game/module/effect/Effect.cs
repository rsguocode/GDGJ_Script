using com.game.data;
using com.game.module.fight.vo;
using com.game.vo;
using com.u3d.bases.controller;
using UnityEngine;


namespace com.game.module.effect
{
    public class Effect
    {   
		public delegate void EffectCallback();
        public delegate void EffectCreateCallback(GameObject effectObj);

        public uint Id;
		public Vector3 BasePosition; //特效基础位置        
		public Vector3 Offset;  //特效偏移        
		public Vector3 EulerAngles; //旋转角度        
		public float Speed; //移动速度  0:不移动        
		public int Direction; //特效方向 1：北  2：右 3：南  4：左   
		public string URL;
		public GameObject Target; //跟随目标 
		public bool AutoDestroy;  //播放完毕后是否自动销毁
		public bool UIEffect;    //是否为UI特效
		public string DictKey;  //字典Key
        public EffectCreateCallback CreatedCallback;  //特效创建后回调
        public EffectCallback PlayedCallback;  //特效播放完毕后回调
        public bool IsBullet;
        public int BulletType;
        public int CheckInterval;
        public SkillController SkillController;
		public float Zoom;  //特效缩放系数
        public float DelayTime = 0f;   //延迟生效时间
        public int EffectIndex;       //特效索引，支持配表中一个技能有多个子弹，不同的子弹有不同的受击特效
        public bool NeedCache;   //是否需要缓存,切场景时不被销毁
        public float LastTime;   //持续时间
        public float MaxTravelDistance;   //移动距离

        public Effect()
        {            
        }        
    }
}


