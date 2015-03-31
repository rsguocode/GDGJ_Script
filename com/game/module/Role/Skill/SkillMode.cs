
using System.IO;
using System.Linq;
using com.game;
using Com.Game.Module.Chat;
using com.game.module.test;
using com.game.data;
using com.game.manager;
using System.Collections.Generic;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using Proto;

namespace Com.Game.Module.Role
{
    public class SkillMode : BaseMode<SkillMode>
    {
        public const int SkillPoint = 2;
        public const int SkillList = 1;
        public const int SkillPos = 3; //技能位置更新
        public const int FirstSkill = 4;

        private uint leftPoint; //剩余的技能点
        private List<uint> skillsId = new List<uint>(); //技能ID
        private List<SysSkillBaseVo> zdSkills; //主动技能
        private List<SysSkillBaseVo> bdSkills; //被动技

        public uint[] SkillsPos = new uint[5]; //技能位置-存储着位置栏上技能位置的信息

        public bool dataInit { get; private set; }

        public override bool ShowTips
        {
            get { return IfShowTips(true)||IfShowTips(false); }
        }

        /// <summary>
        /// 设置技能信息
        /// </summary>
        /// <param name="skillsId">技能ID</param>
        /// <param name="leftPoint">技能点</param>
        public void setSkillInfo(List<uint> skillsId,uint leftPoint){
            this.leftPoint = leftPoint;
            this.skillsId = skillsId;
            zdSkills = GetPlayerCurrentSkills(MeVo.instance.job,true);
            bdSkills = GetPlayerCurrentSkills(MeVo.instance.job, false);
            dataInit = true;
            DataUpdate(SkillList);
        }

        public void setSkillIds(List<uint> skillsId)
        {
            setSkillInfo(skillsId,leftPoint);
        }

        public void setSkillPoint(uint piont)
        {
            this.leftPoint = piont;
            DataUpdate(SkillPoint);
        }

        public void SetSkillPos(uint[] skillPos)
        {
            SkillsPos = skillPos;
            DataUpdate(SkillPos);
        }

        /// <summary>
        /// 获取玩家当前的技能
        /// </summary>
        /// <returns></returns>
        public List<SysSkillBaseVo> CurrentSkills(bool active)
        {
            if (active)
            {
                if (zdSkills == null)
                {
                    zdSkills = GetPlayerCurrentSkills(MeVo.instance.job, true);
                }
                return zdSkills;
            }
            else
            {
                if (bdSkills == null)
                {
                    bdSkills = GetPlayerCurrentSkills(MeVo.instance.job, false); 
                }
                return bdSkills;
            }

        }


        public List<uint> GetLearnedSkillIds()
        {
            return skillsId;
        }

        /// <summary>
        /// 当前的技能点
        /// </summary>
        /// <returns></returns>
        public uint LeftPoint()
        {
           return leftPoint;
        }



        /// <summary>
        /// 获取玩家已分配的技能点
        /// </summary>
        /// <returns></returns>
        private uint GetUsedPoint()
        {
            uint usedPoint = 0;

            foreach (uint skillId in skillsId)
            {
                SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(skillId);

                if (skill.reset_skillid != 0 && skill.reset_skillid != skill.id)
                {
                    SysSkillBaseVo preSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint)skill.reset_skillid);
                    do
                    {
                        preSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint)preSkill.next);
                        usedPoint += (uint) preSkill.point;

                    } while (preSkill.id!=skill.id);

                }
            }

            return usedPoint;
        }

        /// <summary>
        /// 获取当前玩家的全部技能
        /// </summary>
        private List<SysSkillBaseVo> GetPlayerCurrentSkills(int job,bool active)
        {
            List<SysSkillBaseVo> defaultSkills;

            if (skillsId.Count < 24) //默认最多24个技能位
            {
                defaultSkills = SkillViewLogic.GetDefaultSkill(job, active).ToList();
            }
            else
            {
                defaultSkills = new List<SysSkillBaseVo>();
            }
            foreach (uint skillid in skillsId)
            {
                SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(skillid);
                if (skill.active == active)
                {
                    defaultSkills[skill.position - 1] = skill;
                }
            }
            return defaultSkills;
        }

        /// <summary>
        /// 获取当前职业的默认技能
        /// </summary>
        /// <param name="carser">职业类型</param>
        /// <returns>当前职业</returns>
        private List<SysSkillBaseVo> GetDefaultSkills(int carser)
        {
            List<SysSkillBaseVo> defaultSkill = new List<SysSkillBaseVo>();

            Dictionary<uint,object> skillDict = BaseDataMgr.instance.GetDicByType<SysSkillBaseVo>();

            foreach (SysSkillBaseVo skill in skillDict.Values)
            {
                if (skill.position > 0 && skill.position<=8&&defaultSkill[skill.position - 1] == null)
                {
                    if (skill.active && skill.pre == 0 && skill.skill_lvl == 0 && skill.job == carser)
                    {
                        defaultSkill[skill.position - 1] = skill;
                    }
                    else if (!skill.active && skill.skill_lvl == 0 && skill.job == carser)
                    {
                        //前置技能为0
                        if (skill.pre == 0)
                        {
                            defaultSkill[skill.position - 1] = skill;
                        }
                        else
                        {
                            //前置技能为某个主动技能
                            SysSkillBaseVo preskill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint) skill.pre);
                            if (preskill.active)
                            {
                                defaultSkill[skill.position - 1] = skill;
                            }

                        }

                    }
                }
            }
            return defaultSkill;
        }

        /// <summary>
        /// 技能是否已经学习
        /// </summary>
        /// <param name="skillId">技能ID</param>
        /// <returns></returns>
        public bool IsSkillLearned(uint skillId)
        {
            if (skillId == 0)
            {
                return true;
            }
            SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(skillId);
            if (skill == null)
            {
                Log.error(this,"前置技能值为空"+skillId);
            }
            if (skill.active)
            {
                foreach (SysSkillBaseVo skillLearn in zdSkills)
                {
                    if (skillLearn != null)
                    {
                        if (skillLearn.id == skillId)
                        {
                            return true;
                        }

                        if (skill.group == skillLearn.group)
                        {
                            if (skill.skill_lvl <= skillLearn.skill_lvl)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (SysSkillBaseVo skillLearn in bdSkills)
                {
                    if (skillLearn != null)
                    {
                        if (skillLearn.id == skillId)
                        {
                            return true;
                        }

                        if (skill.group == skillLearn.group)
                        {
                            if (skill.skill_lvl <= skillLearn.skill_lvl)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 同步主角使用技能信息
        /// </summary>
        /// <param name="id">主角Id</param>
        /// <param name="type">攻击方类型</param>
        /// <param name="skillid">技能ID</param>
        /// <param name="dir">主角方向</param>
        public void SendHeroUseSkill(uint id,byte type,uint skillid,byte dir)
        {
            var mem = new MemoryStream();
            Module_13.write_13_11(mem,id, type, skillid, dir);
            AppNet.gameNet.send(mem, 13, 11);
        }

        /// <summary>
        /// 技能伤害同步
        /// </summary>
        /// <param name="skillId">技能Id</param>
        /// <param name="damage">技能伤害</param>
        public void SendHeroSkillDamageList(uint skillId, List<PDamage> damage)
        {
            var mem = new MemoryStream();
            Module_13.write_13_12(mem, skillId, damage);
            AppNet.gameNet.send(mem, 13, 12);
        }

        /// <summary>
        /// 获取使用的技能-返回技能id数值,技能位未使用技能id为0;
        /// </summary>
        /// <returns></returns>
        public uint[] GetUsedSkill()
        {
            uint[] skillIds = new uint[5];
            for (int i = 0; i < SkillsPos.Length; i++)
            {
                if (SkillsPos[i] != 0)
                {
                    skillIds[i] = (uint)zdSkills[(int)(SkillsPos[i] - 1)].id;
                }
            }
            return skillIds;
        }

        public bool IfShowTips(bool active)
        {
            List<SysSkillBaseVo> skills = CurrentSkills(active);
            foreach (SysSkillBaseVo skill in skills)
            {
                if (skill!=null&&skill.next != 0)
                {
                    SysSkillBaseVo nextSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint) skill.next);

                    if (SkillViewLogic.IsConditionEnough(nextSkill))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
