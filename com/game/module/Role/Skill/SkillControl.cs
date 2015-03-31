using com.game.consts;
using com.game.module.fight.arpg;
using com.game.module.fight.vo;
using Com.Game.Module.Pet;
using Com.Game.Module.SystemSetting;
using com.game.Public.Message;
using com.game.vo;
using com.u3d.bases.consts;
using PCustomDataType;
using UnityEngine;
using com.game.module.test;
using com.net.interfaces;
using com.game.cmd;
using com.game;
using com.u3d.bases.debug;
using Proto;
using System.Collections.Generic;
using System.IO;
using com.game.manager;
using com.game.data;

namespace Com.Game.Module.Role
{
    public enum SkillSynType
    {
        Monster = 1,
        Player = 2,
        Pet = 3
    }

    public class SkillControl : BaseControl<SkillControl>
    {


        protected override void NetListener()
        {
            /*        
             * public const String CMD_13_4 = "3332"; // 技能信息 : SkillInfoMsg_13_4
             * public const String CMD_13_5 = "3333"; // 学习技能 : SkillStudyMsg_13_5
             * public const String CMD_13_6 = "3334"; // 一键学习 : SkillAllStudyMsg_13_6
             * public const String CMD_13_7 = "3335"; // 重置 : SkillResetMsg_13_7
             */
            AppNet.main.addCMD(CMD.CMD_13_3, GetSkillPointInfo_13_3); 
            AppNet.main.addCMD(CMD.CMD_13_4, GetSkillInfo_13_4); 
            AppNet.main.addCMD(CMD.CMD_13_5, GetSkillStudyInfo_13_5); 
            AppNet.main.addCMD(CMD.CMD_13_7, GetSkillResetInfo_13_7);
            AppNet.main.addCMD(CMD.CMD_13_11,GetSkillSynInfo13_11);
            AppNet.main.addCMD(CMD.CMD_13_12, GetSkillDamageSynInfo13_12);
            
            SendRequstForSKillsInfo();
            SkillViewLogic.InitConfig();
        }

        public void LearnFirstSkill() // 学习第一个技能
        {   
            SysSkillBaseVo skill = SkillViewLogic.GetDefaultSkill(MeVo.instance.job, true)[0];
            LearnSkill((uint)skill.id);
        }

        public void SetFirstSkillPos()
        {
            uint[] skillpos = Singleton<SkillMode>.Instance.SkillsPos;
            uint mark = 0;
            for (int i = 0; i < skillpos.Length; i++)
            {
                mark += skillpos[i];
            }
            if (mark == 0)
            {
                List<uint> skills = Singleton<SkillMode>.Instance.GetLearnedSkillIds();
                if (skills.Count == 1)
                {
                    skillpos[0] = 1;
                    SetSkillPosValue(skillpos);
                }
            }
        }


        //设置全部的技能值
        public void SetSkillPosValue(uint[] skillpos)
        {
            for (uint i = 0; i < skillpos.Length; i++)
            {
                MemoryStream msdata = new MemoryStream();
                Module_11.write_11_2(msdata, GameConst.SkillPos1 + i, skillpos[i]);
                AppNet.gameNet.send(msdata, 11, 2);
            }
            Singleton<SystemSettingMode>.Instance.GetAllSystemSetting();
        }


        public void GetSkillPointInfo_13_3(INetData data)
        {
            SkillPointMsg_13_3 poinMsg = new SkillPointMsg_13_3();
            poinMsg.read(data.GetMemoryStream());
            Singleton<SkillMode>.Instance.setSkillPoint(poinMsg.restPoint);

        }

        //请求技能信息
        public void SendRequstForSKillsInfo() {

            MemoryStream mem = new MemoryStream();
            Module_13.write_13_4(mem);
            AppNet.gameNet.send(mem,13,4);
        
        }

        //获取技能信息
        private void GetSkillInfo_13_4(INetData data)
        {
            SkillInfoMsg_13_4 skillInfo = new SkillInfoMsg_13_4();
            skillInfo.read(data.GetMemoryStream());
            Singleton<SkillMode>.Instance.setSkillInfo(skillInfo.skillids, skillInfo.restPoint);

            if (skillInfo.skillids.Count == 0)
            {
                LearnFirstSkill();
            }
        }

        //学习技能返回
        private void GetSkillStudyInfo_13_5(INetData data)
        {
            SkillStudyMsg_13_5 skillInfo = new SkillStudyMsg_13_5();
            skillInfo.read(data.GetMemoryStream());
            Log.info(this, "GetSkillStudyInfo_13_5:" + skillInfo.code);
            if (skillInfo.code != 0)
            {
                ErrorCodeManager.ShowError(skillInfo.code);
            }
        }


        //重置技能返回
        private void GetSkillResetInfo_13_7(INetData data)
        {
            
            SkillResetMsg_13_7 skillInfo = new SkillResetMsg_13_7();
            skillInfo.read(data.GetMemoryStream());
            Log.info(this, "GetSkillResetInfo_13_7:" + skillInfo.code);
            if (skillInfo.code != 0)
            {
                ErrorCodeManager.ShowError(skillInfo.code);
            }
        }

        private void GetSkillSynInfo13_11(INetData data)
        {
            var skillSynInfo = new SkillUseSyncMsg_13_11();
            skillSynInfo.read(data.GetMemoryStream());
            if (skillSynInfo.type == (byte)SkillSynType.Player)
            {
                if (skillSynInfo.id != MeVo.instance.Id)
                {
                    var player = AppMap.Instance.GetPlayer(skillSynInfo.id + "");
                    if (player == null || player.Controller == null) return;
                    player.ChangeDire((int)(skillSynInfo.dir));
                    player.Controller.SkillController.UseSkillById(skillSynInfo.skillId);
                }
            }
            
        }

        private void GetSkillDamageSynInfo13_12(INetData data)
        {
            if (!AppMap.Instance.mapParser.NeedSyn || AppMap.Instance.me==null)
            {
                return;
            }
            var skillDamageSynInfo = new SkillDamageSyncMsg_13_12();
            skillDamageSynInfo.read(data.GetMemoryStream());
            List<PDamage> pDamages = skillDamageSynInfo.damage;
            foreach (var pDamage in pDamages)
            {
                bool isDodge = false;
                bool isCrit = false;
                bool isParry = false;
                switch (pDamage.dmgType)
                {
                    case 2:
                        isCrit = true;
                        break;
                    case 0:
                        isDodge = true;
                        break;
                }
                int synType = DamageCheck.Instance.GetDisplayType(pDamage.type);
                if (synType == DisplayType.ROLE)
                {
                    if (pDamage.id == AppMap.Instance.me.GetVo().Id)
                    {
                        if (MeVo.instance.CurHp > pDamage.dmg)
                        {
                            MeVo.instance.CurHp -= pDamage.dmg;
                        }
                        else
                        {
                            MeVo.instance.CurHp = 0;
                            var attackVo = new ActionVo();
                            attackVo.ActionType = Actions.DEATH;
                            AppMap.Instance.me.Controller.AttackController.AddAttackList(attackVo, true);
                        }
                        AppMap.Instance.me.Controller.AiController.AddHudView(isDodge, isCrit, isParry,
                            (int) pDamage.dmg,
                            (int) MeVo.instance.CurHp, Color.yellow);
                    }
                    else
                    {
                        var player = AppMap.Instance.GetPlayer(pDamage.id + "");
                        if(player==null||player.Controller==null) return;
                        if (player.GetVo().CurHp > pDamage.dmg)
                        {
                            player.GetVo().CurHp -= pDamage.dmg;
                        }
                        else
                        {
                            player.GetVo().CurHp = 0;
                        }  //血量由服务端广播
                        player.Controller.AiController.AddHudView(isDodge, isCrit, isParry,
                            (int)pDamage.dmg,
                            (int)player.GetVo().CurHp, Color.yellow);
                    }
                }
                else if (synType == DisplayType.MONSTER)
                {
                    var monster = AppMap.Instance.GetMonster(pDamage.id + "");
                    if(monster==null || monster.Controller == null ) return;
                    var monsterVo =  monster.GetMeVoByType<MonsterVo>();
                    if (monsterVo==null) return;
                    if (MeVo.instance.mapId != MapTypeConst.WORLD_BOSS)
                    {
                        if (monsterVo.CurHp > pDamage.dmg)
                        {
                            monsterVo.CurHp -= pDamage.dmg;
                        }
                        else
                        {
                            monsterVo.CurHp = 0;
                            /*var attackVo = new ActionVo();
                            attackVo.ActionType = Actions.DEATH;
                            monster.Controller.AttackController.AddAttackList(attackVo, true);*/
                        }  //血
                    }
                    monster.Controller.AiController.AddHudView(isDodge, isCrit, isParry,
                        (int)pDamage.dmg,
                        (int)monsterVo.CurHp, Color.red);
                }
            }
        }

        //学习-升级-升阶技能
        public void LearnSkill(uint skillId) {

            MemoryStream mem = new MemoryStream();
            SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(skillId);
            if (skill.next != 0)
            {
                Module_13.write_13_5(mem, skillId, (uint) skill.next);
            }
            else if (skill.evolve != 0)
            {
                Module_13.write_13_5(mem, skillId, (uint)skill.evolve);
            }
            else
            {
                return;   
            }

            AppNet.gameNet.send(mem, 13, 5);
        
        }
        
        /// <summary>
        /// 重置技能
        /// </summary>
        /// <param name="skillId"></param>
        public void ResetSkillPoint()
        {

        }

    }
}