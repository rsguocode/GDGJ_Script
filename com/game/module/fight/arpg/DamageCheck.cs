using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.module.battle;
using Com.Game.Module.Boss;
using com.game.module.fight.vo;
using Com.Game.Module.GoldHit;
using com.game.module.map;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.controler;
using PCustomDataType;
using UnityEngine;
using com.game.utils;

namespace com.game.module.fight.arpg
{
    public class DamageCheck
    {
        private const int ForwardDirection = 0; //检测时只往正方向检测
        private const int BackwardDirection = 1; //检测时只往反方向检测
        private const int DoubleDirection = 2; //检测时往两个方向检测
        private const float SkillCoverRangeChangeRate = 0.001f; //攻击范围转化率，表中1000代表1个unity单位，为了表中没有小数
        public static DamageCheck Instance = (Instance ?? new DamageCheck());

        private MeControler meControler
        {
            get { return (AppMap.Instance.me.Controller as MeControler); }
        }

        private AiControllerBase PlayerAiController
        {
            get { return meControler.AiController; }
        }

        /// <summary>
        ///     主角伤害检测
        /// </summary>
        /// <param name="atker">攻击者</param>
        /// <param name="skillVo">技能Vo</param>
        /// <param name="playerVo"></param>
        /// <param name="skillType">技能类型</param>
        /// <param name="effectPos">技能特效位置</param>
        /// <param name="ownerPos">技能施放者位置</param>
        /// <param name="dir">技能施放方向</param>
        /// <param name="enemyDisplays">敌人列表</param>
        /// <param name="isBullet">是否是子弹技能</param>
        /// <param name="checkedTime"></param>
        /// <param name="index">索引</param>
        /// <param name="needSyn">是否需要同步伤害</param>
        /// <returns></returns>
        public bool CheckPlayerInjured2D(BaseControler atker, SysSkillBaseVo skillVo, PlayerVo playerVo, Vector3 effectPos,
            Vector3 ownerPos, int dir,
            IList<ActionDisplay> enemyDisplays, bool isBullet, int checkedTime, int index = 0, bool needSyn = false)
        {
            bool result = false;
            if (playerVo.CurHp == 0)
            {
                return false;
            }
            foreach (ActionDisplay enemyDisplay in enemyDisplays)
            {
                var enemyVo = enemyDisplay.GetMeVoByType<BaseRoleVo>(); //怪物vo
                if (enemyDisplay.Controller == null || enemyVo.CurHp == 0)
                {
                    continue;
                }
                Vector3 target = enemyDisplay.Controller.transform.position; //敌方位置 
                BoxCollider2D myBoxCollider2D = (meControler.Me as ActionDisplay).BoxCollider2D;
                BoxCollider2D enemyBoxCollider2D = enemyDisplay.BoxCollider2D;
                if (IsSkillCovered(skillVo, effectPos, ownerPos, target, dir, myBoxCollider2D, enemyBoxCollider2D))
                {
                    result = true;
                    ActionVo attackVo = GetSkillActionVo(skillVo, effectPos, dir, enemyDisplay, index, checkedTime,
                        enemyVo.MoveRatio);
                    //attackVo.HurtEffectIndex = index;
                    attackVo.HurtType = Actions.HurtNormal;
                    enemyDisplay.Controller.BeAttackedController.BeAttacked(attackVo);
                    meControler.AttackController.ForceFeedBack(attackVo);
                    if (!needSyn)
                    {
                        int damageVal;
                        uint damageType;
                        uint stateType;
                        //攻击伤害计算
                        GameFormula.CalcDamage(playerVo, enemyVo, skillVo, out damageVal, out damageType, out stateType);
                        var cutHp = (uint) damageVal; //伤害值
                        bool isDodge = false; //是否闪避
                        bool isCrit = false; //是否暴击
                        const bool isParry = false; //是否格挡
                        switch (damageType)
                        {
                            case GameConst.DAMAGE_TYPE_MISS:
                                isDodge = true;
                                break;
                            case GameConst.DAMAGE_TYPE_CRIT:
                                isCrit = true;
                                break;
                        }
                        if (enemyVo.CurHp <= cutHp)
                        {
                            attackVo.HurtType = Actions.HurtFly;
                            enemyDisplay.Controller.BeAttackedController.BeAttacked(attackVo);
                            if (!needSyn)
                            {
                                enemyVo.CurHp = 0;
                                enemyDisplay.Controller.DisposeHudView();
                                attackVo = new ActionVo();
                                attackVo.ActionType = Actions.DEATH;
                                enemyDisplay.Controller.AttackController.AddAttackList(attackVo, true);
                            }
                        }
                        else //Miss也播放受击效果，所以这里是>=0
                        {
                            enemyVo.lastHp = enemyVo.CurHp;
                            if (AppMap.Instance.mapParser.MapId != MapTypeConst.WORLD_BOSS)
                            {
                                enemyVo.CurHp -= cutHp;
                            }
                        }
                        enemyDisplay.Controller.AiController.AddHudView(isDodge, isCrit, isParry, (int) cutHp,
                            (int) enemyVo.CurHp, ColorConst.Blood);
                    }
                }
            }
            return result;
        }


        /// <summary>
        ///     主角伤害检测
        /// </summary>
        /// <param name="atker">攻击者</param>
        /// <param name="skillVo">技能Vo</param>
        /// <param name="skillType">技能类型</param>
        /// <param name="effectPos">技能特效位置</param>
        /// <param name="ownerPos">技能施放者位置</param>
        /// <param name="dir">技能施放方向</param>
        /// <param name="enemyDisplays">敌人列表</param>
        /// <param name="isBullet">是否是子弹技能</param>
        /// <param name="index">攻击序列的序号</param>
        /// <param name="checkedTime"></param>
        /// <param name="needSyn">是否需要同步伤害</param>
        /// <returns></returns>
        public bool CheckMeInjured2D(BaseControler atker, SysSkillBaseVo skillVo, Vector3 effectPos, Vector3 ownerPos, int dir,
            IList<ActionDisplay> enemyDisplays, bool isBullet, int index, int checkedTime, bool needSyn = false)
        {
            bool result = false;
            var damageList = new List<PDamage>();
            int damageNum = 0;
            if (MeVo.instance.CurHp == 0) return false;
            foreach (ActionDisplay enemyDisplay in enemyDisplays)
            {
                var enemyVo = enemyDisplay.GetMeVoByType<BaseRoleVo>(); //怪物vo
                if (enemyDisplay.Controller == null || enemyVo.CurHp == 0)
                {
                    continue;
                }
                Vector3 target = enemyDisplay.Controller.transform.position; //敌方位置 
                BoxCollider2D myBoxCollider2D = (meControler.Me as ActionDisplay).BoxCollider2D;
                BoxCollider2D enemyBoxCollider2D = enemyDisplay.BoxCollider2D;
                if (IsSkillCovered(skillVo, effectPos, ownerPos, target, dir, myBoxCollider2D, enemyBoxCollider2D))
                {
                    damageNum += 1;
                    result = true;
                    ActionVo attackVo = GetSkillActionVo(skillVo, effectPos, dir, enemyDisplay, index, checkedTime,
                        enemyVo.MoveRatio);
                    //attackVo.HurtEffectIndex = index;
                    attackVo.HurtType = Actions.HurtNormal;
                    int damageVal;
                    uint damageType;
                    uint stateType;
                    var myVo = AppMap.Instance.me.GetVo() as PlayerVo;
                    //攻击伤害计算
                    GameFormula.CalcDamage(myVo, enemyVo, skillVo, out damageVal, out damageType, out stateType);
                    var cutHp = (uint) damageVal; //伤害值
                    if (AppMap.Instance.mapParser.MapId == MapTypeConst.WORLD_BOSS) //如果是世界Boss 副本发送伤害值
                    {
                        float attackAddRate = BossMode.Instance.AttackGuwuRate;  //攻击加成
                        cutHp = (uint)Mathf.RoundToInt(cutHp * (1f + attackAddRate));
                        float critRate = BossMode.Instance.CritGuwuRate;
                        float randomRate = Random.Range(0f, 1f);
                        if (critRate >= randomRate)  //暴击
                        {
                            damageType = GameConst.DAMAGE_TYPE_CRIT;
                            cutHp = (uint)Mathf.RoundToInt(cutHp * 1.5f);
                        }
                        Singleton<BossControl>.Instance.Attack(cutHp);
                    }
                    if (AppMap.Instance.mapParser.MapId == MapTypeConst.GoldHit_MAP) //如果是击石成金的副本，计算伤害值，暂时用英雄榜的替换
                    {
                        Singleton<GoldHitControl>.Instance.Attack(cutHp);
                        Singleton<GoldHitMode>.Instance.Monster = target;
                    }
                    bool isDodge = false; //是否闪避
                    bool isCrit = false; //是否暴击
                    const bool isParry = false; //是否格挡
                    switch (damageType)
                    {
                        case GameConst.DAMAGE_TYPE_MISS:
                            isDodge = true;
                            break;
                        case GameConst.DAMAGE_TYPE_CRIT:
                            isCrit = true;
                            break;
                    }
                    if (!isDodge)//命中时攻击者产生力反馈
                    {
                        atker.AttackController.ForceFeedBack(attackVo);
                    }
                    if (enemyVo.CurHp <= cutHp)
                    {
                        uint previousHp = enemyVo.CurHp;
                        attackVo.HurtType = Actions.HurtFly;
                        if (!isDodge)
                        {
                            //enemyDisplay.Controller.BeAttackedController.BeAttacked(attackVo);
                        }
                        if (needSyn)
                        {
                            PDamage pdamage = GetSkillPDamage(enemyVo, enemyDisplay, cutHp, isDodge, isCrit);
                            damageList.Add(pdamage);
                        }
                        else
                        {
                            enemyVo.CurHp = 0;
                            enemyDisplay.Controller.DisposeHudView();
                            attackVo = new ActionVo();
                            attackVo.ActionType = Actions.DEATH;
                            enemyDisplay.Controller.AttackController.AddAttackList(attackVo, true);
                            MonsterMgr.Instance.LiveMonsterNumber--;
                            if (MonsterMgr.Instance.LiveMonsterNumber == 0 && MapMode.CUR_MAP_PHASE == 3)
                            {
                                Singleton<MapControl>.Instance.MyCamera.ShowCopyWinEffect();
                                MeVo.instance.IsUnbeatable = true;
                            }
                            if (enemyDisplay.Type == DisplayType.MONSTER)
                            {
                                var monsterVo = enemyVo as MonsterVo;
                                if (monsterVo != null)
                                {
                                    Singleton<BattleMode>.Instance.SetCurrentHp(monsterVo.MonsterVO.name, enemyVo.Hp,
                                        previousHp, enemyVo.CurHp, monsterVo.Level, monsterVo.MonsterVO.quality,monsterVo.MonsterVO.icon,
                                        monsterVo.MonsterVO.hp_count);
                                }
                            }
                        }
                    }
                    else //Miss也播放受击效果，所以这里是>=0
                    {
                        if (!isDodge)
                        {
                            enemyDisplay.Controller.BeAttackedController.BeAttacked(attackVo);
                        }
                        if (needSyn)
                        {
                            PDamage pdamage = GetSkillPDamage(enemyVo, enemyDisplay, cutHp, isDodge, isCrit);
                            damageList.Add(pdamage);
                        }
                        else
                        {
                            //不是世界Boss 副本，才扣血 ，世界Boss血量有后端控制
                            uint previousHp = enemyVo.CurHp;
                            enemyVo.lastHp = enemyVo.CurHp;
                            if (AppMap.Instance.mapParser.MapId != MapTypeConst.WORLD_BOSS)
                            {
                                enemyVo.CurHp -= cutHp;
                            }
                            if (enemyDisplay.Type == DisplayType.MONSTER)
                            {
                                var monsterVo = enemyVo as MonsterVo;
                                if (monsterVo != null)
                                {
                                    Singleton<BattleMode>.Instance.SetCurrentHp(monsterVo.MonsterVO.name, monsterVo.Hp,
                                        previousHp, monsterVo.CurHp, monsterVo.Level, monsterVo.MonsterVO.quality, monsterVo.MonsterVO.icon,
                                        monsterVo.MonsterVO.hp_count);
                                }
                            }
                        }
                    }
                    if (!needSyn)
                    {
                        enemyDisplay.Controller.AiController.AddHudView(isDodge, isCrit, isParry, (int) cutHp,
                            (int) enemyVo.CurHp, ColorConst.Blood);
                    }
                    else
                    {
                        if (damageList.Count > 0)
                        {
                            //向服务端发送主角自己的技能伤害
                            SkillMode.Instance.SendHeroSkillDamageList(skillVo.unikey, damageList);
                        }
                    }
                }
            }
            if (damageNum > 0)
            {
                if (AppMap.Instance.mapParser.MapId != MapTypeConst.GoldHit_MAP)
                    AppMap.Instance.me.Controller.ContCutMgr.AddAttackNum(damageNum); //连击统计
            }
            return result;
        }

        /// <summary>
        ///     宠物伤害检测
        /// </summary>
        /// <param name="atker">攻击者</param>
        /// <param name="skillVo">技能Vo</param>
        /// <param name="skillType">技能类型</param>
        /// <param name="excutePos">技能特效位置</param>
        /// <param name="selfPos">技能施放者位置</param>
        /// <param name="dir">技能施放方向</param>
        /// <param name="enemyDisplays">敌人列表</param>
        /// <param name="isBullet">是否是子弹技能</param>
        /// <param name="checkedTime"></param>
        /// <param name="index">攻击序列的序号</param>
        /// <param name="needSyn">是否需要同步伤害</param>
        /// <returns></returns>
        public bool CheckPetInjured2D(BaseControler atker, SysSkillBaseVo skillVo, Vector3 excutePos, Vector3 selfPos, int dir,
            IList<ActionDisplay> enemyDisplays, bool isBullet, int checkedTime, int index = 0, bool needSyn = false)
        {
            bool result = false;
            var damageList = new List<PDamage>();
            foreach (ActionDisplay enemyDisplay in enemyDisplays)
            {
                var enemyVo = enemyDisplay.GetVo() as BaseRoleVo; //怪物vo
                if (enemyDisplay.Controller == null || enemyVo.CurHp == 0)
                {
                    continue;
                }
                Vector3 target = enemyDisplay.Controller.transform.position; //敌方位置 
                BoxCollider2D myBoxCollider2D = (meControler.Me as ActionDisplay).BoxCollider2D;
                BoxCollider2D enemyBoxCollider2D = enemyDisplay.BoxCollider2D;
                if (IsSkillCovered(skillVo, excutePos, selfPos, target, dir, myBoxCollider2D, enemyBoxCollider2D))
                {
                    result = true;
                    ActionVo attackVo = GetSkillActionVo(skillVo, excutePos, dir, enemyDisplay, checkedTime);
                    //attackVo.HurtEffectIndex = index;
                    int damageVal;
                    uint damageType;
                    uint stateType;
                    var myVo = AppMap.Instance.me.GetVo() as PlayerVo;
                    //攻击伤害计算
                    GameFormula.CalcDamage(myVo, enemyVo, skillVo, out damageVal, out damageType, out stateType);
                    var cutHp = (uint) damageVal; //伤害值
                    /*if (cutHp < 300)
                    {
                        cutHp = cutHp + 300; //原来是200
                    }*/
                    if (AppMap.Instance.mapParser.MapId == MapTypeConst.WORLD_BOSS) //如果是世界Boss 副本发送伤害值
                    {
                        Singleton<BossControl>.Instance.Attack(cutHp);
                    }
                    if (AppMap.Instance.mapParser.MapId == MapTypeConst.GoldHit_MAP) //如果是击石成金的副本，计算伤害值，暂时用英雄榜的替换
                    {
                        Singleton<GoldHitControl>.Instance.Attack(cutHp);
                        Singleton<GoldHitMode>.Instance.Monster = target;
                    }
                    bool isDodge = false; //是否闪避
                    bool isCrit = false; //是否暴击
                    bool isParry = false; //是否格挡
                    switch (damageType)
                    {
                        case GameConst.DAMAGE_TYPE_MISS:
                            isDodge = true;
                            break;
                        case GameConst.DAMAGE_TYPE_CRIT:
                            isCrit = true;
                            break;
                    }
                    if(!isDodge)//命中后攻击者产生力反馈
                    {
                        atker.AttackController.ForceFeedBack(attackVo);
                    }
                    if (enemyVo.CurHp <= cutHp)
                    {
                        uint previousHp = enemyVo.CurHp;
                        attackVo.HurtType = Actions.HurtFly;
                        enemyDisplay.Controller.BeAttackedController.BeAttacked(attackVo);
                        if (needSyn)
                        {
                            PDamage pdamage = GetSkillPDamage(enemyVo, enemyDisplay, cutHp, isDodge, isCrit);
                            damageList.Add(pdamage);
                        }
                        else
                        {
                            enemyVo.CurHp = 0;
                            enemyDisplay.Controller.DisposeHudView();
                            attackVo = new ActionVo();
                            attackVo.ActionType = Actions.DEATH;
                            enemyDisplay.Controller.AttackController.AddAttackList(attackVo, true);
                            MonsterMgr.Instance.LiveMonsterNumber--;
                            if (MonsterMgr.Instance.LiveMonsterNumber == 0 && MapMode.CUR_MAP_PHASE == 3)
                            {
                                Singleton<MapControl>.Instance.MyCamera.ShowCopyWinEffect();
                            }
                            if (enemyDisplay.Type == DisplayType.MONSTER)
                            {
                                var monsterVo = enemyVo as MonsterVo;
                                if (monsterVo != null && monsterVo.MonsterVO.quality == 3)
                                {
                                    Singleton<BattleMode>.Instance.SetCurrentHp(monsterVo.MonsterVO.name, enemyVo.Hp,
                                        previousHp, enemyVo.CurHp, monsterVo.Level, monsterVo.MonsterVO.quality, monsterVo.MonsterVO.icon,
                                        monsterVo.MonsterVO.hp_count);
                                }
                            }
                        }
                    }
                    else //Miss也播放受击效果，所以这里是>=0
                    {
                        enemyDisplay.Controller.BeAttackedController.BeAttacked(attackVo);
                        if (needSyn)
                        {
                            PDamage pdamage = GetSkillPDamage(enemyVo, enemyDisplay, cutHp, isDodge, isCrit);
                            damageList.Add(pdamage);
                        }
                        else
                        {
                            //不是世界Boss 副本，才扣血 ，世界Boss血量有后端控制
                            uint previousHp = enemyVo.CurHp;
                            enemyVo.lastHp = enemyVo.CurHp;
                            if (AppMap.Instance.mapParser.MapId != MapTypeConst.WORLD_BOSS)
                            {
                                enemyVo.CurHp -= cutHp;
                            }
                            if (enemyDisplay.Type == DisplayType.MONSTER)
                            {
                                var monsterVo = enemyVo as MonsterVo;
                                if (monsterVo != null && monsterVo.MonsterVO.quality == 3)
                                {
                                    Singleton<BattleMode>.Instance.SetCurrentHp(monsterVo.MonsterVO.name, enemyVo.Hp,
                                        previousHp, monsterVo.CurHp, monsterVo.Level, monsterVo.MonsterVO.quality, monsterVo.MonsterVO.icon,
                                        monsterVo.MonsterVO.hp_count);
                                }
                            }
                        }
                    }
                    if (!needSyn)
                    {
                        enemyDisplay.Controller.AiController.AddHudView(isDodge, isCrit, isParry, (int) cutHp,
                            (int) enemyVo.CurHp, ColorConst.Blood);
                    }
                    else
                    {
                        if (damageList.Count > 0)
                        {
                            //向服务端发送主角自己的技能伤害
                            //SkillMode.Instance.SendHeroSkillDamageList(skillVo.unikey, damageList);
                        }
                    }
                }
            }
            return result;
        }

        private PDamage GetSkillPDamage(BaseRoleVo enemyVo, ActionDisplay enemyDisplay, uint cutHp, bool isDodge,
            bool isCrit)
        {
            var pdamage = new PDamage
            {
                id = enemyVo.Id,
                type = (byte) GetSynType(enemyDisplay.Type),
                x = 0,
                y = 0,
                dmg = cutHp,
                hp = enemyVo.CurHp,
                dmgType = (byte) GetDamageType(isDodge, isCrit),
                stateType = 0
            };
            return pdamage;
        }

        /// <summary>
        ///     获取技能攻击后的目标表现Vo
        /// </summary>
        /// <param name="skillVo"></param>
        /// <param name="excutePos"></param>
        /// <param name="dir"></param>
        /// <param name="enemyDisplay"></param>
        /// <param name="checkedTime"></param>
        /// <param name="moveRatio"></param>
        /// <returns></returns>
        private ActionVo GetSkillActionVo(SysSkillBaseVo skillVo, Vector3 excutePos, int dir, ActionDisplay enemyDisplay, int index,
            int checkedTime = 0, float moveRatio = 1)
        {
            var attackVo = new ActionVo
            {
                ActionType = Actions.INJURED,
                SkillUsePoint = excutePos,
                SkillId = skillVo.unikey
            };

            /*
            Vector3 moveToPoint = enemyDisplay.GoBase.transform.position;
            if (skillVo.target_dir != 2 && checkedTime == 0)
            {
                if (dir == Directions.Left)
                {
                    moveToPoint.x -= skillVo.back_dis*0.001f*moveRatio;
                }
                else
                {
                    moveToPoint.x += skillVo.back_dis*0.001f*moveRatio;
                }
            }
            if (MeVo.instance.mapId != MapTypeConst.WORLD_BOSS) //世界BOSS的位置不受影响
            {
                moveToPoint.x = AppMap.Instance.mapParser.GetFinalMonsterX(moveToPoint.x); //限制怪物被攻击后不要出屏幕
            }
            moveToPoint.y = enemyDisplay.GoBase.transform.position.y; //后退时保持高度不变
            attackVo.HurtDestination = moveToPoint;*/
            
            attackVo.FloatingValue = skillVo.Floating_Value;

            if (MeVo.instance.mapId != MapTypeConst.WORLD_BOSS) //世界BOSS的位置不受影响
            {
                //获取当前攻击点的攻击数据
                int[][] atkList = StringUtils.Get2DArrayStringToInt(skillVo.Per_Atk_Data);
                if (atkList != null)
                {
                    if (index < atkList.Length)
                    {
                        attackVo.Velocity_Origin = atkList[index][Actions.VELOCITY_ORIGIN] * 0.001f;
                        attackVo.Angle = atkList[index][Actions.ANGLE];
                        attackVo.ProtectValue = atkList[index][Actions.PROTECTVALUE];
                        attackVo.ForceFeedBack = atkList[index][Actions.FORCEFEEDBACK];
                        attackVo.HitRecover = atkList[index][Actions.HITRECOVER];
                        attackVo.HurtAnimation = atkList[index][Actions.HURTANIMATION];
                        attackVo.FaceDirection = (dir == Directions.Left ? -1 : 1);
                    }
                }
            }
            return attackVo;
        }

        private int GetSynType(int type)
        {
            int result = 0;
            switch (type)
            {
                case DisplayType.ROLE:
                    result = 1;
                    break;
                case DisplayType.MONSTER:
                    result = 2;
                    break;
            }
            return result;
        }

        public int GetDisplayType(byte synType)
        {
            var type = (int) synType;
            int result = DisplayType.ROLE;
            switch (type)
            {
                case 1:
                    result = DisplayType.ROLE;
                    break;
                case 2:
                    result = DisplayType.MONSTER;
                    break;
            }
            return result;
        }

        private int GetDamageType(bool isDodge, bool isCrit)
        {
            int result = 3;
            if (isCrit)
            {
                result = 2;
            }
            if (isDodge)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        ///     怪物攻击受击目标检测(只对自己的客户端检测)
        /// </summary>
        /// <param name="atker">攻击者</param>
        /// <param name="skillVo">技能VO</param>
        /// <param name="effectPos">施法点，怪物位置或远程施法点</param>
        /// <param name="ownerPos">施法点，怪物位置或远程施法点</param>
        /// <param name="dir">施法方向</param>
        /// <param name="monsterVo">怪物VO</param>
        /// <param name="isSend">是否同步给服务器</param>
        public bool MonsterCheckInjured2D(BaseControler atker, SysSkillBaseVo skillVo, Vector3 effectPos, Vector3 ownerPos, int dir,
            MonsterVo monsterVo, int index,
            bool isSend = false)
        {
            if (AppMap.Instance.me == null || AppMap.Instance.me.GetVo().CurHp <= 0) return false;
            if (monsterVo.CurHp <= 0) return false; //如果检测时当前怪物的血量已经为0，则玩家忽略当前怪物的攻击
            var meVo = AppMap.Instance.me.GetMeVoByType<PlayerVo>();
            if (MeVo.instance.mapId == MapTypeConst.WORLD_BOSS)
            {
                if (AppMap.Instance.me.Controller.transform.position.x > 7)
                {
                    meVo.CurHp = 0;
                    var attackVo = new ActionVo();
                    attackVo.ActionType = Actions.DEATH;
                    meControler.AttackController.AddAttackList(attackVo, true);
                    return true;
                }
                return false;
            }

            Vector3 target = AppMap.Instance.me.Controller.transform.position; //敌方位置 
            bool isDodge = false; //是否闪避
            bool isCrit = false; //是否暴击
            bool isParry = false; //是否格挡
            BoxCollider2D myBoxCollider2D = (meControler.Me as ActionDisplay).BoxCollider2D;
            BoxCollider2D enemyBoxCollider2D = AppMap.Instance.me.BoxCollider2D;
            if (IsSkillCovered(skillVo, effectPos, ownerPos, target, dir, myBoxCollider2D, enemyBoxCollider2D))
            {
                if (!isSend)
                {
                    int damageVal;
                    uint damageType;
                    uint stateType;
                    if (meVo != null &&
                        (meVo.IsUnbeatable || AppMap.Instance.me.Controller.StatuController.CurrentStatu == Status.ROLL))
                    {
                        return false; //无敌状态不播放受击
                    }
                    ActionVo attackVo = GetSkillActionVo(skillVo, effectPos, dir, AppMap.Instance.me, index);
                    //怪物的伤害计算
                    GameFormula.CalcDamage(monsterVo, meVo, skillVo, out damageVal, out damageType, out stateType);
                    switch (damageType)
                    {
                        case GameConst.DAMAGE_TYPE_MISS:
                            isDodge = true;
                            break;
                        case GameConst.DAMAGE_TYPE_CRIT:
                            isCrit = true;
                            break;
                    }
                    if (!isDodge)//命中后，攻击者产生力反馈，受击者处理受击逻辑
                    {
                        atker.AttackController.ForceFeedBack(attackVo);
                        meControler.BeAttackedController.BeAttacked(attackVo);
                    }
                    int cutHp = damageVal; //伤害值
                    if (MeVo.instance.mapId == MapTypeConst.FirstFightMap &&
                        (meVo.CurHp < meVo.Hp*0.3f || meVo.CurHp <= (uint) cutHp))
                    {
                        cutHp = 0;
                        isDodge = true;
                    }
                    else
                    {
                        if (meVo.CurHp <= (uint)cutHp)
                        {
                            meVo.CurHp = 0;
                            attackVo = new ActionVo();
                            attackVo.ActionType = Actions.DEATH;
                            meControler.AttackController.AddAttackList(attackVo, true);
                        }
                        else
                        {
                            meVo.CurHp -= (uint)cutHp;
                        }
                    }
                    PlayerAiController.AddHudView(isDodge, isCrit, isParry, cutHp, (int) meVo.CurHp, Color.yellow);
                    //冒血直接冒
                    //攻击后重置受击抵抗属性
                    monsterVo.HurtResist = (uint) monsterVo.MonsterVO.hurt_resist;
                    return true;
                }
            }
            monsterVo.HurtResist = (uint) monsterVo.MonsterVO.hurt_resist;
            return false;
        }


        /// <summary>
        ///     陷阱攻击受击目标检测(只对自己的客户端检测)
        /// </summary>
        /// <param name="skillVo">技能VO</param>
        /// <param name="excutePos">施法点，陷阱位置或远程施法点</param>
        /// <param name="dir">施法方向</param>
        /// <param name="boxCollider2D"></param>
        /// <param name="isSend">是否同步给服务器</param>
        public bool TrapCheckInjured2D(SysSkillBaseVo skillVo, Vector3 excutePos, int dir, BoxCollider2D boxCollider2D,
            bool isSend = false)
        {
            if (AppMap.Instance.me.GetVo().CurHp <= 0) return false;

            Vector3 target = AppMap.Instance.me.Controller.transform.position; //敌方位置 
            bool isDodge = false; //是否闪避
            bool isCrit = false; //是否暴击
            bool isParry = false; //是否格挡
            if (IsTrapSkillCovered(boxCollider2D, AppMap.Instance.me.BoxCollider2D))
            {
                if (!isSend)
                {
                    var meVo = AppMap.Instance.me.GetVo() as PlayerVo;
                    if (meVo != null &&
                        (meVo.IsUnbeatable || AppMap.Instance.me.Controller.StatuController.CurrentStatu == Status.ROLL))
                    {
                        return false; //无敌状态不播放受击
                    }

                    var attackVo = new ActionVo
                    {
                        SkillId = skillVo.unikey,
                        ActionType = Actions.INJURED,
                        SkillUsePoint = excutePos,
                        Target = AppMap.Instance.me.GoBase
                    }; //动作vo
                    meControler.BeAttackedController.BeAttacked(attackVo);
                    int cutHp = skillVo.value; //伤害值
                    if (meVo.CurHp <= (uint) cutHp)
                    {
                        meVo.CurHp = 0;
                        attackVo = new ActionVo();
                        attackVo.ActionType = Actions.DEATH;
                        meControler.AttackController.AddAttackList(attackVo, true);
                    }
                    else
                    {
                        meVo.CurHp -= (uint) cutHp;
                    }
                    PlayerAiController.AddHudView(isDodge, isCrit, isParry, cutHp, (int) meVo.CurHp, Color.red); //冒血直接冒
                    return true;
                }
            }
            return false;
        }

        public bool TrapColliderCheckInjured2D(SysSkillBaseVo skillVo, Vector3 excutePos, int dir,
            BoxCollider2D boxCollider2D, List<ActionDisplay> lastAttackedActionDisplay,
            bool isSend = false)
        {
            if (AppMap.Instance.me.GetVo().CurHp <= 0)
            {
                lastAttackedActionDisplay.Clear();
                return false;
            }

            Vector3 target = AppMap.Instance.me.Controller.transform.position; //敌方位置 
            bool isDodge = false; //是否闪避
            bool isCrit = false; //是否暴击
            bool isParry = false; //是否格挡
            if (IsTrapSkillCovered(boxCollider2D, AppMap.Instance.me.BoxCollider2D))
            {
                if (!isSend)
                {
                    if (lastAttackedActionDisplay.Contains(AppMap.Instance.me))
                    {
                        return false;
                    }
                    var meVo = AppMap.Instance.me.GetMeVoByType<PlayerVo>();
                    if (meVo != null &&
                        (meVo.IsUnbeatable || AppMap.Instance.me.Controller.StatuController.CurrentStatu == Status.ROLL))
                    {
                        return false; //无敌状态不播放受击
                    }

                    var attackVo = new ActionVo
                    {
                        SkillId = skillVo.unikey,
                        ActionType = Actions.INJURED,
                        SkillUsePoint = excutePos,
                        Target = AppMap.Instance.me.GoBase
                    }; //动作vo
                    meControler.BeAttackedController.BeAttacked(attackVo);
                    int cutHp = skillVo.value; //伤害值
                    if (meVo.CurHp <= (uint) cutHp)
                    {
                        meVo.CurHp = 0;
                        attackVo = new ActionVo();
                        attackVo.ActionType = Actions.DEATH;
                        meControler.AttackController.AddAttackList(attackVo, true);
                    }
                    else
                    {
                        meVo.CurHp -= (uint) cutHp;
                    }
                    lastAttackedActionDisplay.Add(AppMap.Instance.me);
                    PlayerAiController.AddHudView(isDodge, isCrit, isParry, cutHp, (int) meVo.CurHp, Color.red); //冒血直接冒
                    return true;
                }
            }
            lastAttackedActionDisplay.Clear();
            return false;
        }


        /// <summary>
        ///     根据技能表的技能配置计算目标是否被技能覆盖
        /// </summary>
        /// <param name="skillVo">技能数据</param>
        /// <param name="effectPos">技能所在点</param>
        /// <param name="ownerPos">技能施放者所在点</param>
        /// <param name="targetPos">目标所在点</param>
        /// <param name="dir">技能执行者执行技能时的方向</param>
        /// <param name="boxCollider2D">受击者碰撞体，可知身体高宽</param>
        /// <returns>true：技能攻击覆盖到 false：技能攻击没有覆盖到</returns>
        public bool IsSkillCovered(SysSkillBaseVo skillVo, Vector3 effectPos, Vector3 ownerPos, Vector3 targetPos,
            int dir, BoxCollider2D myBoxCollider2D, BoxCollider2D enemyBoxCollider2D)
        {
            Vector2 skillCoverOrigin = myBoxCollider2D.center;
            int[] list = StringUtils.GetStringToInt(skillVo.cover_start_pos);
            if(list.Length > 0)
            {
                skillCoverOrigin = Vector2.right * list[0] * 0.001f + Vector2.up * list[1] * 0.001f;
            }
            Vector2 size = enemyBoxCollider2D.size;
            Vector2 center = enemyBoxCollider2D.center;
            float disY = targetPos.y - ownerPos.y;
            float maxDis = skillVo.cover_thickness * SkillCoverRangeChangeRate * 0.5f;
            if (disY > maxDis || disY < -maxDis)//厚度（Z轴攻击区域判断）
            {
                return false;
            }
            switch (skillVo.target_dir)//宽度（X轴）
            {
                case ForwardDirection:
                    if (dir == Directions.Left &&
                        (targetPos.x - size.x > effectPos.x ||
                         targetPos.x + center.x + size.x*0.5f <
                         effectPos.x - skillVo.cover_width*SkillCoverRangeChangeRate)) //这里用乘法比除法效率高
                    {
                        return false;
                    }
                    if (dir == Directions.Right &&
                        (targetPos.x + size.x * 0.5f < effectPos.x ||
                         targetPos.x - center.x - size.x*0.5f >
                         effectPos.x + skillVo.cover_width*SkillCoverRangeChangeRate))
                    {
                        return false;
                    }
                    break;
                case BackwardDirection:
                    if (dir == Directions.Left &&
                        (targetPos.x + size.x * 0.5f < effectPos.x ||
                         targetPos.x - center.x - size.x*0.5f >
                         effectPos.x + skillVo.cover_width*SkillCoverRangeChangeRate))
                    {
                        return false;
                    }
                    if (dir == Directions.Right &&
                        (targetPos.x - size.x > effectPos.x ||
                         targetPos.x + center.x + size.x*0.5f <
                         effectPos.x - skillVo.cover_width*SkillCoverRangeChangeRate))
                    {
                        return false;
                    }
                    break;
                case DoubleDirection:
                    if (targetPos.x + center.x + size.x*0.5f <
                        effectPos.x - skillVo.cover_width*SkillCoverRangeChangeRate*0.5f ||
                        targetPos.x - center.x - size.x*0.5f >
                        effectPos.x + skillVo.cover_width*SkillCoverRangeChangeRate*0.5f)
                    {
                        return false;
                    }
                    break;
            }
            if (effectPos.y + skillCoverOrigin.y - skillVo.cover_height * SkillCoverRangeChangeRate * 0.5f >
               targetPos.y + center.y + size.y * 0.5f ||
               effectPos.y + skillCoverOrigin.y + skillVo.cover_height * SkillCoverRangeChangeRate * 0.5f <
               targetPos.y)//高度（Y轴）
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     判断两个矩形是否相交
        /// </summary>
        /// <param name="boxCollider1"></param>
        /// <param name="boxCollider2"></param>
        /// <returns></returns>
        private bool IsTrapSkillCovered(BoxCollider2D boxCollider1, BoxCollider2D boxCollider2)
        {
            bool result = false;
            float x11 = boxCollider1.transform.position.x + boxCollider1.center.x - boxCollider1.size.x*0.5f;
            float x12 = boxCollider1.transform.position.x + boxCollider1.center.x + boxCollider1.size.x*0.5f;
            float y11 = boxCollider1.transform.position.y + boxCollider1.center.y - boxCollider1.size.y*0.5f;
            float y12 = boxCollider1.transform.position.y + boxCollider1.center.y + boxCollider1.size.y*0.5f;

            float x21 = boxCollider2.transform.position.x + boxCollider2.center.x - boxCollider2.size.x*0.5f;
            float x22 = boxCollider2.transform.position.x + boxCollider2.center.x + boxCollider2.size.x*0.5f;
            float y21 = boxCollider2.transform.position.y + boxCollider2.center.y - boxCollider2.size.y*0.5f;
            float y22 = boxCollider2.transform.position.y + boxCollider2.center.y + boxCollider2.size.y*0.5f;

            float minX = Mathf.Max(x11, x21);
            float maxX = Mathf.Min(x12, x22);
            float minY = Mathf.Max(y11, y21);
            float maxY = Mathf.Min(y12, y22);
            result = (minX > maxX) || (minY > maxY);
            return !result;
        }
    }
}