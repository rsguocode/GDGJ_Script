using System;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;

/** 伤害公式相关计算 */

namespace com.game.module.fight.arpg
{
    public class GameFormula
    {
        /** 计算伤害 */

        public static void CalcDamage(BaseRoleVo aer, BaseRoleVo der, SysSkillBaseVo skillVo,
            out int damageVal, out uint damageType, out uint stateType)
        {
            CalcDamage(aer, der, skillVo, 1, out damageVal, out damageType, out stateType);
        }

        /**
		 * 计算命中和暴击概率，获得暴击伤害比
		 */

        private static void CalcHitCrit(BaseRoleVo aer, BaseRoleVo der,
            out uint hit, out uint crit,uint derFlex)
        {
            // aer
            int aerType = aer.Type;
            uint aerHit = aer.Hit;
            uint aerCrit = aer.Crit;
            var aerLvl = (uint) aer.Level;

            // der
            int derType = der.Type;
            int derDodge = der.Dodge;
            var derLvl = (uint) der.Level;

            if (aerType == DisplayType.ROLE && derType == DisplayType.ROLE)
            {
                // role -> role                
                hit = (uint) Math.Round(((4500f + aerHit)/(5000f + derDodge))*(aerLvl + 50)/(derLvl + 50)*10000d);
                crit = (uint) Math.Round((1000f + aerCrit)/(10000f + aerCrit + derFlex)*10000f);
            }
            else if (aerType == DisplayType.PET && derType == DisplayType.ROLE)
            {
                // pet -> role
                hit = (uint) Math.Round(((45000 + aerHit/5d)/(50000 + derDodge/5d))*(aerLvl + 50)
                                        /(derLvl + 50)*10000d);
                crit = (uint) Math.Round((1000 + aerCrit/8d)/(10000 + aerCrit/8d + derFlex)*10000d);
            }
            else
            {
                // other
                hit = (uint) Math.Round(((4900 + aerHit*0.8f)/(5000d + derDodge))*(aerLvl + 50)
                                        /(derLvl + 50)*10000);
                crit = (uint) Math.Round((2000 + aerCrit)/(10000d +aerCrit+ derFlex)*10000d);
            }
        }

        /**
		 * 计算伤害类型
		 * 1,攻击者Hit几率 -> 不满足MISS
		 * 2,攻击者crit几率 ->
		 * 		满足，伤害*2
		 *		不满足，伤害
		 */

        private static uint CalcDamageType(uint aerHit, uint aerCrit)
        {
            int hitRand = MathUtils.Random();
            // 1
            if (hitRand > aerHit)
            {
                return GameConst.DAMAGE_TYPE_MISS;
            }
            // 2
            int critRand = MathUtils.Random();
            return critRand <= aerCrit ? GameConst.DAMAGE_TYPE_CRIT : GameConst.DAMAGE_TYPE_NORMAL;
        }


        /**
		 * 计算连击伤害,伤害加成=int(连斩数/100）*5%
		 */

        private static uint GetSeqAttDamage(BaseRoleVo aer, uint aerAtt)
        {
            if (aer.Type == DisplayType.ROLE)
            {
                var player = aer as PlayerVo;
                return (uint) (aerAtt*(1 + player.SeqAttack*0.05f));
            }
            return aerAtt;
        }

        /**
		 * 获取攻击者攻击力
		 * 1,人物
		 * 2,怪物
		 * 3,宠物
		 */

        private static uint GetAerAtt(BaseRoleVo aer, BaseRoleVo der, int attType)
        {
            int aerType = aer.Type;
            int derType = der.Type;
            uint aerAtt = 0;
            uint attMin, attMax;

            uint luck = (uint) (aer.Luck /(2500f+aer.Luck) * 10000);
            if (attType == GameConst.ATT_TYPE_PHY)
            {
                attMin = aer.AttPMin;
                attMax = aer.AttPMax;
            }
            else
            {
                attMin = aer.AttMMin;
                attMax = aer.AttMMax;
            }

            if (aerType == DisplayType.ROLE)
            {
                // 1		
                int var = MathUtils.Random((int) luck, 10000);
                aerAtt = (uint) Math.Round(attMin + (attMax - attMin)*var/10000f);
                aerAtt = (derType == DisplayType.MONSTER ? GetSeqAttDamage(aer, aerAtt) : aerAtt);
            }
            else if (aerType == DisplayType.PET)
            {
                // 2			
                int var = (luck >= 10000 ? MathUtils.Random(10000, (int) luck) : MathUtils.Random((int) luck, 10000));
                aerAtt = (uint) Math.Round(attMin + (attMax - attMin)*var/10000d);
            }
            else if (aerType == DisplayType.MONSTER)
            {
                // 3
                aerAtt = MathUtils.Random(attMin, attMax);
            }
            return aerAtt;
        }

        /**
		 * 获取被攻击者防御
         */

        private static uint GetDerDef(BaseRoleVo der, BaseRoleVo aer, int attType)
        {
            /* int aerType = aer.Type;
            int derType = der.Type;*/
            uint def = attType == GameConst.ATT_TYPE_PHY ? der.DefP : der.DefM;

            /* if (derType == DisplayType.ROLE && aerType == DisplayType.ROLE)
            {
                // role -> role
                var player = (der as PlayerVo);
                //TODO
                int defDec = 1; //player.GetDefDec();
                if (attType == GameConst.ATT_TYPE_PHY)
                {
                    def = (uint) MathUtils.PercentSub((int) der.defP, defDec);
                }
                else
                {
                    def = (uint) MathUtils.PercentSub((int) der.defM, defDec);
                }
            }
            else
            {
                def = (attType == GameConst.ATT_TYPE_PHY ? der.defP : der.defM);
            }*/
            return def;
        }

        /**
		 * 真正的伤害计算
		 */

        private static double CalcDamageInternal(BaseRoleVo aer, BaseRoleVo der, SysSkillBaseVo skillVo)
        {
            int aerType = aer.Type;
            int derType = der.Type;
            double damage;

            if (aerType == DisplayType.MONSTER && aer.mapId == MapTypeConst.MAPID_DUNGEON_GIANT)
            {
                // 伤害计算(特殊处理：巨人副本boss对城墙伤害只掉一滴血)
                return 1;
            }
            /**
				 * 1,伤害计算
				 * 	a,pvp
				 * 	b,pve
				 * 	C,宠vp
				 * 	d,其它
				 * 2,职业伤害系数
				 */
            // 1
            int attType = skillVo.att_type;
            int damagePer = skillVo.data_per;
            int damageFixed = skillVo.data_fixed;
            uint aerAtt = GetAerAtt(aer, der, attType);
            uint derDef = GetDerDef(der, aer, attType);

            if (aerType == DisplayType.ROLE && derType == DisplayType.ROLE)
            {
                //a
                aerAtt = aerAtt/12;
                if (aerAtt == 0) aerAtt = 1;
                derDef = derDef/10;
				damage = (aerAtt * aerAtt) / (aerAtt + derDef) * damagePer / GameConst.PROB_FULL_D						+ damageFixed;
            }
            else if (aerType == DisplayType.PET && derType == DisplayType.ROLE)
            {
                //b
                aerAtt = aerAtt/12;
                if (aerAtt == 0) aerAtt = 1;
                derDef = derDef/10;
                damage = (aerAtt*aerAtt)/(aerAtt + derDef)*damagePer/GameConst.PROB_FULL_D
                         + damageFixed;
            }
            else if (aerType == DisplayType.PET && derType == DisplayType.MONSTER)
            {
                //c
                damage = (aerAtt*4 - derDef)*0.05
                         + Math.Max(0, aerAtt - derDef)*damagePer/GameConst.PROB_FULL_D;
                damage = Math.Max(0, damage) + damageFixed;
            }
            else
            {
                //d
                damage = ((int) aerAtt*4 - (int) derDef)*0.05
                         + Math.Max(0, (int) aerAtt - (int) derDef)*damagePer/GameConst.PROB_FULL_D
                         + damageFixed;
            }

            // 2
            SysJobDamageRatioVo jobRatioVo = BaseDataMgr.instance.GetJobDamageRatio(aer.job, der.job);
            damage = Math.Round(damage*jobRatioVo.ratio/GameConst.PROB_FULL_D);
            damage = damage*(1 - der.AttackDecrease/10000f);
            return Math.Max(1d, damage);
        }

        /** 
         * 计算伤害的状态类型(僵直，击退等)
		 */

        private static uint CalcDamageStateType(BaseRoleVo aer, BaseRoleVo der)
        {
            int derType = der.Type;
            if (derType == DisplayType.MONSTER)
            {
                var mon = (der as MonsterVo);
                int stiffOpp = mon.MonsterVO.stiff_opp;
                if (MathUtils.Random() < stiffOpp)
                {
                    return GameConst.SKILL_DAMAGE_STATE_STIFF;
                }
                return GameConst.SKILL_DAMAGE_STATE_NONE;
            }
            return GameConst.SKILL_DAMAGE_STATE_NONE;
        }


        /*
		 * 计算伤害
		 * 1,判断Der是否无敌状态
		 * 2,计算攻击类型
		 * 3,计算伤害值
		 * 4,计算暴击效果
		 * 5,伤害减免
		 * 6,计算伤害的状态类型(僵直，击退等)
		 * 7,群攻目标伤害递减
		 */

        public static void CalcDamage(BaseRoleVo aer, BaseRoleVo der, SysSkillBaseVo skillVo, uint targetNum,
            out int damageVal, out uint damageType, out uint stateType)
        {
            int derType = der.Type;
            // 1
            if (derType == DisplayType.ROLE && der.IsUnbeatable)
            {
                damageVal = 0;
                damageType = GameConst.DAMAGE_TYPE_NORMAL;
                stateType = GameConst.SKILL_DAMAGE_STATE_NONE;
                return;
            }

            // 2
            uint hit;
            uint crit;
            uint critRatio = aer.CritRatio;
            uint derFlex = der.Flex;
            CalcHitCrit(aer, der, out hit, out crit, derFlex);
            damageType = CalcDamageType(hit, crit);

            // 3
            double damage = 0;
            switch (damageType)
            {
                case GameConst.DAMAGE_TYPE_ABSORB:
                    damage = 0;
                    break;
                case GameConst.DAMAGE_TYPE_MISS:
                    damage = 0;
                    break;
                case GameConst.DAMAGE_TYPE_NORMAL:
                    damage = CalcDamageInternal(aer, der, skillVo);
                    break;
                case GameConst.DAMAGE_TYPE_CRIT:
                    damage = CalcDamageInternal(aer, der, skillVo);
                    break;
            }

            // 4
            if (damageType == GameConst.DAMAGE_TYPE_CRIT)
            {
                damage = damage*1.5 +
                         Math.Max(0, damage*Math.Max(0, ((int) critRatio - (int) derFlex/2)))/GameConst.PROB_FULL_D;
            }

            // 5 TODO
            //damage = CalcHurtRe(der, damage, aerType);
            // 多倍伤害
            //Damage4 = do_damage_again(Aer, Damage3, DamageType, Der),


            // 6 TODO
            stateType = CalcDamageStateType(aer, der);

            // 7
            double groupDecRatio = Math.Max(1000, (10000 - (targetNum - 1)*700d))/10000;
            damageVal = (int) Math.Max(0, Math.Round(damage*groupDecRatio));
        }
    }
}