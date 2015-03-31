using System.Globalization;
﻿﻿﻿using com.game.consts;
﻿﻿﻿using com.game.module.effect;
﻿﻿﻿using com.game.module.map;
﻿﻿﻿using com.game.module.test;
﻿﻿﻿using com.game.utils;
﻿﻿﻿using com.game.vo;
﻿﻿﻿﻿using com.u3d.bases.consts;
﻿﻿﻿using com.u3d.bases.debug;
﻿﻿﻿using UnityEngine;
using com.u3d.bases.display.character;


/**玩家状态控制器
 * @author 骆琦
 * @date   2013-11-6
 * 基于animator的怪物状态控制器
 * **/
namespace com.u3d.bases.controller
{
    public class MonsterStatuController : StatuControllerBase
    {
        private const float COMB_INTERVAL = 6.0f;  //连击响应间隔，超过这个间隔后连击重置
        private int _currentCombStatu;

        override public int CurrentCombStatu
        {
            get { return _currentCombStatu; }
            set
            {
                CancelInvoke("ResetCombStatu");
                _currentCombStatu = value;
                Invoke("ResetCombStatu", COMB_INTERVAL);  // 6.0S后没有再处理则重置连击状态
            }
        }

        private void ResetCombStatu()
        {
            _currentCombStatu = Status.COMB_0;
        }

        /// <summary>
        /// 动作切换完成后执行这个方法，状态开始切换后动作不一定立刻就切换完成，中间有个动画过渡的阶段，动画过渡完后就会触发这个方法
        /// </summary>
        override protected void DoStatuTransfered()
        {
            base.DoStatuTransfered();
            if (StateInfo.nameHash == Status.NAME_HASH_IDLE)
            {
                DoIdleBegin();
            }
            else if (StateInfo.nameHash == Status.NAME_HASH_ATTACK1)
            {
                DoAttack1Begin();
            }
            else if (StateInfo.nameHash == Status.NAME_HASH_ATTACK2)
            {
                DoAttack2Begin();
            }
            else if (StateInfo.nameHash == Status.NAME_HASH_DEATH)
            {
                DoDeathBegin();
            }
            //从倒地保护切换过来的状态
            if (PreStatuNameHash != Status.NAME_HASH_HURT3 && StateInfo.nameHash == Status.NAME_HASH_HURT3)
            {
                EnterHurt3Status();
            }
            else if(PreStatuNameHash == Status.NAME_HASH_HURT3 && StateInfo.nameHash != Status.NAME_HASH_HURT3)
            {
                LeaveHurt3Status();
            }
        }

        private void DoIdleBegin()
        {
            MeControler.SpeakWord();
        }

        private void DoAttack1Begin()
        {
            MeControler.SpeakWord();
        }

        private void DoAttack2Begin()
        {
            MeControler.SpeakWord();
        }

        private void DoDeathBegin()
        {
            MeControler.SpeakWord();
        }

        private void ResetSpeed()
        {
            Animator.speed = 1;
        }

        private void EnterHurt3Status()
        {
            (MeControler.Me as MonsterDisplay).SetBoxColliderDown();
            MeControler.GetMeVo().ProtectValue = 0;
        }

        private void LeaveHurt3Status()
        {
            (MeControler.Me as MonsterDisplay).SetBoxColliderStand();
        }

        /// <summary>
        /// 受击后倒地状态时的过程处理
        /// </summary>
        /// <param name="stateInfo">当前动画状态信息</param>
        override protected void ProcessHurtDownState(AnimatorStateInfo stateInfo)
        {
            if (Animator.IsInTransition(0) && CurrentStatu == Status.HURTDOWN)
            {
                if ((MeControler.GetMeVoByType<MonsterVo>()).MonsterVO.quality == 3) // 怪物
                {
                    MeControler.SkillController.RequestUseMonsterStandUpSkill();
                }
                else
                {
                    SetStatu(Status.STANDUP);
                }
            }
        }

        override protected void ProcessDeathState(AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > 0.85)
            {
                MonsterMgr.Instance.RemoveMonster(MeControler.GetMeVo().Id.ToString(CultureInfo.InvariantCulture));
                Log.info(this, "-endCallback() 发送怪物死亡信息给服务器，怪物死亡ID： " + MeControler.GetMeVo().Id);
				Log.info(this, "死亡怪物坐标："+ MeControler.transform.position.x + "," + MeControler.transform.position.y);
//				MeControler.GetMeVoByType<MonsterVo>().goods
                Singleton<MapMode>.Instance.MonsterDeath(MeControler.GetMeVo().Id); //发送怪物死亡信息给服务器
                Singleton<MapControl>.Instance.MonsterID = (uint)MeControler.GetMeVoByType<MonsterVo>().MonsterVO.id;
                //播放怪物死亡特效
                if (MeVo.instance.mapId == MapTypeConst.GoldHit_MAP)
                {
                    var effectVo = new Effect
                    {
                        URL = UrlUtils.GetSkillEffectUrl(EffectId.Skill_StoneDiam),
                        BasePosition = transform.position,
                        Target = gameObject,
                        NeedCache = true
                    };
                    EffectMgr.Instance.CreateSkillEffect(effectVo);
                }
            }
        }

        override protected int[,] GetStatuMatrix()
        {
            return statuChangeMatrix;
        }

        /// <summary>
        /// 用于查找从一个状态到另一个状态的切换是否是允许的
        /// </summary>
        private static readonly int[,] statuChangeMatrix =
        {
           //ID|RU|RO|A1|A2|A3|A4|S1|S2|S3|H1|DE|H2|H3|H4|HD|SU
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, //IDLE
            {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0}, //RUN
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0}, //ROLL
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0}, //ATTACK1
            {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0}, //ATTACK2
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0}, //ATTACK3
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0}, //ATTACK4
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, //SKILL1
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, //SKILL2
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, //SKILL3
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0}, //HURT
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0}, //DEATH
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0}, //HURT2
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0}, //HURT3
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0}, //HURT4
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1}, //HURTDOWN
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0}  //STANDUP
           //ID|RU|RO|A1|A2|A3|A4|S1|S2|S3|H1|DE|H2|H3|H4|HD|SU
        };
    }
}
