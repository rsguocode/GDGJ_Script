using System.Collections.Generic;
using com.game.manager;
using com.game.vo;
using com.game.module.test;
using com.game.data;

namespace Com.Game.Module.Role
{
    public class SkillViewLogic 
    {


        private  static  SysSkillBaseVo[][] DefaultSkill = new SysSkillBaseVo[6][]; //主动和被动技能的默认配置  职业-主动-被动-技能位置



        public static void InitConfig()
        {
            var skillBaseVos = BaseDataMgr.instance.GetDicByType<SysSkillBaseVo>().Values;

            SysSkillBaseVo[] defaut1 = new SysSkillBaseVo[12];
            SysSkillBaseVo[] defaut2 = new SysSkillBaseVo[12];
            SysSkillBaseVo[] defaut3 = new SysSkillBaseVo[12];

            SysSkillBaseVo[] defaut4 = new SysSkillBaseVo[12];
            SysSkillBaseVo[] defaut5 = new SysSkillBaseVo[12];
            SysSkillBaseVo[] defaut6 = new SysSkillBaseVo[12];

            DefaultSkill[0] = defaut1;
            DefaultSkill[1] = defaut2;
            DefaultSkill[2] = defaut3;
            DefaultSkill[3] = defaut4;
            DefaultSkill[4] = defaut5;
            DefaultSkill[5] = defaut6;

            foreach (SysSkillBaseVo skill in skillBaseVos)
            {
                if (skill.job!=0) //主角技能
                {
                    if (skill.skill_lvl == 0 && skill.next != 0) //初始技能
                    {
                        int active = 0;
                        if (skill.active)
                        {
                            active = 1;
                        }
                        DefaultSkill[(skill.job - 1)*2 + 1 - active][skill.position - 1] = skill;
                    }
                }
            }
        }


        //获取职业对应的默认技能
        public static SysSkillBaseVo[] GetDefaultSkill(int job,bool active)
        {
            int act = 0;
            if (active)
            {
                act = 1;
            }

            return DefaultSkill[(job - 1)*2 + 1 - act];
        }

        /// <summary>
        /// 获取当前此技能状态-学习-升级-进化
        /// </summary>
        /// <param name="skillVo"></param>
        /// <returns></returns>
        public static SkillState GetSkillState(SysSkillBaseVo skillVo)
        {
            if (IsLastLevel(skillVo))
            {
                return SkillState.Max;
            }else if (skillVo.skill_lvl == 0)
            {
                return SkillState.Learn;
            }
            else if (skillVo.evolve != 0)
            {
                return SkillState.Change;
            }
            else
            {
                return SkillState.Upgrade;
            }
        }

        /// <summary>
        /// 判断是否满级
        /// </summary>
        public static bool IsLastLevel(SysSkillBaseVo skillVo)
        {
            return skillVo.next == 0;
        }

        /// <summary>
        /// 判断是否满足升级要求
        /// </summary>
        public static bool IsConditionEnough(SysSkillBaseVo skillVo)
        {
            return IsLevelEnough(skillVo) && IsMoneyEnough(skillVo) && IsPreSkillEnough(skillVo) && IsSkillPointEnough(skillVo);
        }

        /// <summary>
        /// 判断等级要求
        /// </summary>
        public static bool IsLevelEnough(SysSkillBaseVo skillVo)
        {
            return skillVo.lvl <= MeVo.instance.Level;
        }

        /// <summary>
        /// 判断金币要求
        /// </summary>
        public static bool IsMoneyEnough(SysSkillBaseVo skillVo)
        {
            return skillVo.beiley <= MeVo.instance.diam;
        }
        /// <summary>
        /// 判断前置技能要求
        /// </summary>
        public static bool IsPreSkillEnough(SysSkillBaseVo skillVo)
        {
            return Singleton<SkillMode>.Instance.IsSkillLearned((uint)skillVo.pre);
        }

        /// <summary>
        /// 判断技能点
        /// </summary>
        public static bool IsSkillPointEnough(SysSkillBaseVo skillVo)
        {
            return Singleton<SkillMode>.Instance.LeftPoint() >= skillVo.point;
        }
        

    }
}