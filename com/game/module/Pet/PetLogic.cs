using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Copy;
using Com.Game.Module.DaemonIsland;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using Proto;
using UnityEngine;

namespace Com.Game.Module.Pet
{
    //宠物功能逻辑类
    class PetLogic
    {
        public const float MAXAPTITUDE = 1000; //资质上限

        private static int[] starStoneNum = new int[5];
        public static int[] StartGrow = new int[5];

        public static int AllNum { get; private set; }
        public static int AttackNum { get; private set; }
        public static int ControlNum { get; private set; }
        public static int HelpNum { get; private set; }

        public const int Type_All = 0;
        public const int Type_Attack = 1;
        public const int Type_Control = 2;
        public const int Type_Help = 3;

        public static List<SysPet> SysPets { get; private set; }

        private static Dictionary<int, int[]> PetsConfig = new Dictionary<int, int[]>();
        
        private static int[][][] PetEquipConfig = new int[3][][]; //宠物装备配置：类型-阶-装备ID
        private static SysPetAdvancedVo[] GradeAdd = new SysPetAdvancedVo[42];
        private static string[][] skillName = new string[4][];

        //初始化宠物相关的配置信息
        public static void InitConfig()
        {

            string[] skill1 = {"生命之心", "生命之源", "生命之泉", "生命强化"};
            string[] skill2 = {"铜皮铁骨", "神圣之盾", "光明圣盾", "物理屏障"};
            string[] skill3 = { "魔法抵抗", "魔法护盾", "魔力屏障", "英勇抵抗" };
            string[] skill4 = { "战斗之心", "战斗之源", "战斗之泉", "攻击强化" };

            skillName[0] = skill1;
            skillName[1] = skill2;
            skillName[2] = skill3;
            skillName[3] = skill4;

            SysPets = new List<SysPet>();
            //第一次处理
            foreach (SysPet pet in BaseDataMgr.instance.GetDicByType<SysPet>().Values)
            {
                SysPets.Add(pet);

                //计算各类型数量
                AllNum++;
                if (pet.type == Type_Attack)
                {
                    AttackNum++;
                }
                else if (pet.type == Type_Control)
                {
                    ControlNum++;
                }
                else if (pet.type == Type_Help)
                {
                    HelpNum++;
                }
            }

            for (uint i = 1; i <= 5; i++)
            {
                SysPetEvolutionVo evo =  BaseDataMgr.instance.GetDataById<SysPetEvolutionVo>(i);
                StartGrow[i - 1] = evo.grow;

                if (i > 1)
                {
                   starStoneNum[i - 1] = evo.number - BaseDataMgr.instance.GetDataById<SysPetEvolutionVo>(i-1).number;
                }
                else
                {
                    starStoneNum[i - 1] = evo.number;
                }
            }

            //宠物装备配置表
            int[][] equipConfig1 = new int[14][];
            int[][] equipConfig2 = new int[14][];
            int[][] equipConfig3 = new int[14][];

            PetEquipConfig[0] = equipConfig1;
            PetEquipConfig[1] = equipConfig2;
            PetEquipConfig[2] = equipConfig3;

            foreach (SysPetAdvancedVo petEquip in BaseDataMgr.instance.GetDicByType<SysPetAdvancedVo>().Values)
            {
                int[] equip = new int[6];
                equip[0] = petEquip.equip_1;
                equip[1] = petEquip.equip_2;
                equip[2] = petEquip.equip_3;
                equip[3] = petEquip.equip_4;
                equip[4] = petEquip.equip_5;
                equip[5] = petEquip.equip_6;

                PetEquipConfig[petEquip.type-1][petEquip.advanced - 1] = equip;
                GradeAdd[petEquip.type*petEquip.advanced - 1] = petEquip;
            }
        }


        public static  SysPet GetSysPetByStoneId(uint stoneId)
        {
            foreach (SysPet pet in SysPets)
            {
                if (pet.stone_id == stoneId)
                {
                    return pet;
                }
            }
            return null;
        }

        private static void SetPetProperty(SysPet pet)
        {
               int[] petconfig = new int[18];

                int[] hp = GetProperty(pet.hp);
                petconfig[0] = hp[0];
                petconfig[1] = hp[1];

                int[] att_max = GetProperty(pet.att_max);
                petconfig[2] = att_max[0];
                petconfig[3] = att_max[1];

                int[] def_p = GetProperty(pet.def_p);
                petconfig[4] = def_p[0];
                petconfig[5] = def_p[1];

                int[] def_m = GetProperty(pet.def_m);
                petconfig[6] = def_m[0];
                petconfig[7] = def_m[1];

                int[] hurt_re = GetProperty(pet.hurt_re);
                petconfig[8] = hurt_re[0];
                petconfig[9] = hurt_re[1];

                int[] skill = GetSkill(pet.passive_skill);

                petconfig[10] = skill[0];
                petconfig[11] = skill[1];
                petconfig[12] = skill[2];
                petconfig[13] = skill[3];
                petconfig[14] = skill[4];
                petconfig[15] = skill[5];
                petconfig[16] = skill[6];
                petconfig[17] = skill[7];

                PetsConfig.Add(pet.id,petconfig);
        }

        //获取宠物的属性信息
        public static int[] GetPetProperty(int petid)
        {
            if (PetsConfig.ContainsKey(petid))
            {
                return PetsConfig[petid];
            }
            else
            {
                SysPet pet = BaseDataMgr.instance.GetDataById<SysPet>((uint)petid);
                SetPetProperty(pet);
                return PetsConfig[petid];
            }
        }

        //解析宠物属性配置
        private static int[] GetProperty(String str)
        {
            Log.info("str:",str);
            int[] pro = new int[2];
            str = str.Substring(1, str.Length - 2);
            string[] proS = str.Split(',');
            pro[0] = int.Parse(proS[0]);
            pro[1] = int.Parse(proS[1]);
            return pro;

        }

        //解析技能配置
        private static int[] GetSkill(String str)
        {
            int[] pro = new int[8];

            string pStr = str.Substring(1, str.Length-2);
            pStr =  pStr.Replace("[","");
            pStr = pStr.Replace("]","");
            string[] proS = pStr.Split(',');

            pro[0] = int.Parse(proS[0]);
            pro[1] = int.Parse(proS[1]);
            pro[2] = int.Parse(proS[2]);
            pro[3] = int.Parse(proS[3]);
            pro[4] = int.Parse(proS[4]);
            pro[5] = int.Parse(proS[5]);
            pro[6] = int.Parse(proS[6]);
            pro[7] = int.Parse(proS[7]);

            return pro;

        }

        /// <summary>
        ///     获取宠物PetVo
        /// </summary>
        public static PetVo GetPetVo(PPet pet)
        {
            var petVo = new PetVo();
            SetPetVo(pet, petVo);
            return petVo;
        }

        /// <summary>
        ///     设置宠物PetVo
        /// </summary>
        public static void SetPetVo(PPet pet, PetVo vo)
        {
            vo.Id = pet.id;
            vo.petId = pet.petId;
            vo.SkillId = (uint)vo.SysPet.unique_skill + pet.star -1;
            CupBaseProperties(pet,vo); //基础属性
            EquipLogic.CupPetEquipProperties(pet,vo); //装备和阶属性
            CupSkillProperties(pet,vo); //技能属性
            vo.fight = GetFight(vo);
        }

    
        /// <summary>
        ///     计算属性基础加成
        /// </summary>
        public static void CupBaseProperties(PPet pet, PetVo vo)
        {
            vo.Hp = CupBaseProperty(pet, vo.SysPet.hp);
            vo.CurHp = vo.Hp;
            vo.AttPMax = CupBaseProperty(pet, vo.SysPet.att_max);
            vo.DefP = CupBaseProperty(pet, vo.SysPet.def_p);
            vo.DefM = CupBaseProperty(pet, vo.SysPet.def_m);

        }


        /// <summary>
        ///     计算基础属性
        /// </summary>
        private static uint CupBaseProperty(PPet pet, string des)
        {
            string[] values = des.Substring(1, des.Length - 2).Split(',');
            uint init = uint.Parse(values[0]);
            uint levelAdd = uint.Parse(values[1]);
            SysPetEvolutionVo starVo = BaseDataMgr.instance.GetDataById<SysPetEvolutionVo>((uint)pet.star);
            float result = (init+ levelAdd*(pet.lvl-1))*starVo.grow/10000f;
            return (uint)result;
        }


        /// <summary>
        ///     计算技能加成
        /// </summary>
        private static void CupSkillProperties(PPet pet ,PetVo vo)
        {
            for (int i = 1; i <= 4; i++)
            {
                int[] skillConfig = GetPetSkillInfo((int) pet.petId, i);
                uint[] skill = Singleton<PetMode>.Instance.GetPetSkill(pet.id, (uint)i);

                if (skill != null)
                {
                    AddSkillProperty(vo, skillConfig, skill[1]);
                }
            }

        }

        private static void AddSkillProperty(PetVo vo,int[] skillConfig ,uint level)
        {    ///生命之源（生命）5
            ///铜皮铁骨（物防）11
            ///魔法抵抗（魔防）12
            ///战斗之心（攻击）22
            if (skillConfig[0] == 5)
            {
                vo.Hp += (uint) (skillConfig[1]*level);
            }
            else if (skillConfig[0] == 11)
            {
                vo.DefP += (uint)(skillConfig[1] * level);
            }
            else if (skillConfig[0] == 12)
            {
                vo.DefM += (uint)(skillConfig[1] * level);
            }
            else if (skillConfig[0] == 22)
            {
                vo.AttPMax += (uint)(skillConfig[1] * level);
            }
        }


        /// <summary>
        ///     Vo的数值加总
        /// </summary>
        public static void AddPetALL(BaseRoleVo petVo, BaseRoleVo vo)
        {
            if (petVo != null && vo != null)
            {
                petVo.Hp += vo.Hp;
                petVo.AttPMax += vo.AttPMax;
                petVo.DefP += vo.DefP;
                petVo.DefM += vo.DefM;
                petVo.HurtRe += vo.HurtRe;
            }
        }

        //获取战斗力
        public static uint GetFight(BaseRoleVo vo)
        {
            double fight = 0;
            fight = vo.AttPMax*0.6 + vo.AttPMin*0.6;
            fight += vo.Hp*0.2 + vo.Mp*0.1 + vo.DefP*0.5 + vo.DefM*0.5 + vo.Hit*0.5 + vo.Dodge*0.5 + vo.Crit*0.6 +
                     vo.Flex*0.6 + vo.CritRatio*0.7 + vo.HurtRe*1 + vo.Luck*0.8;
            return (uint) fight;
        }

        //是否已召唤此宠物
        public static bool IsOwn(SysPet pet)
        {
            return Singleton<PetMode>.Instance.GetPetByPetId((uint)pet.id) != null;
        }

        //可以召唤此宠物
        public static bool CanOwn(SysPet pet)
        {
            SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>((uint)pet.id);

            int ownStone = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint)pet.stone_id);
            int needStone = GetNeedStone((uint)spet.star, false);
            return Singleton<PetMode>.Instance.GetPetByPetId((uint)pet.id) == null && ownStone >= needStone;
        }

        //是否可以进化
        public static bool CanEvolve(PPet pet)
        {
            if (pet.star < 5)
            {
                SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>(pet.petId);

                int ownStone = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint) spet.stone_id);
                int needStone = GetNeedStone(pet.star, true);
                return ownStone >= needStone;
            }
            return false;
        }

        //是否可以进阶
        public static bool CanUpgrade(PPet pet)
        {
            if (pet.grade < 14)
            {
                if (pet.equip.Count >=12)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取阶对应的Icn
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static string GetGradeIcn(int grade)
        {
            switch (grade)
            {
                case 1:
                    return "hslv";
                case 2:
                    return "hslv1";
                case 3:
                    return "hslan";
                case 4:
                    return "hslan1";
                case 5:
                    return "hslan2";
                case 6:
                    return "hszi";
                case 7:
                    return "hszi1";
                case 8:
                    return "hszi2";
                case 9:
                    return "hszi3";
                case 10:
                    return "hschen";
                case 11:
                    return "hschen1";
                case 12:
                    return "hschen2";
                case 13:
                    return "hschen3";
                case 14:
                    return "hschen4";
                default:
                    return "hslv";
            }
        }


        //获取对应的阶描述
        public static string GetGradeDes(int grade,string name)
        {
            if (grade == 1 ) //整阶
            {
                return ColorConst.GREEN + name + ColorConst.GREEN;
            }else if (grade == 3)
            {
                return ColorConst.BLUE + name +  ColorConst.BLUE;
            }else if (grade == 6)
            {
                return ColorConst.PURPLE + name  + ColorConst.PURPLE;
            }else if (grade == 10)
            {
                return ColorConst.ORANGE + name + ColorConst.ORANGE;
            }
            else if (grade == 2) //绿
            {
                return ColorConst.GREEN + name + "+1" + ColorConst.GREEN;
            }
            else if (grade < 6) //蓝
            {
                return ColorConst.BLUE + name + "+" + (grade - 3) + ColorConst.BLUE;
            }
            else if (grade < 10) //紫
            {
                return ColorConst.PURPLE + name + "+" + (grade - 6) + ColorConst.PURPLE;
            }
            else
            {
                return ColorConst.ORANGE + name + "+" + (grade - 10) + ColorConst.ORANGE;
            }
        }

        //获取对应的阶
        public static int GetGrade(int grade)
        {
            if (grade < 3) //绿
            {
                return 1;
            }
            else if (grade < 6) //蓝
            {
                return 2;
            }else if (grade < 10) //紫
            {
                return 3;
            }else //橙色
            {
                return 4;
            }

        }

        public static string GetTypeIcnName(int type)
        {
            if (type == PetLogic.Type_Attack)
            {
               return  "gong1";
            }
            else if (type == PetLogic.Type_Control)
            {
                return "kong";
            }
            else if (type == PetLogic.Type_Help)
            {
                return "fuzhu";
            }
            else
            {
                return "";
            }
        }

        //获取宠物的技能信息
        public static int[] GetPetSkillInfo(int petid,int num)
        {
            int[] petProperty = GetPetProperty(petid);
            int[] skill = new int[2];
            skill[0] = petProperty[8 + (num*2)];
            skill[1] = petProperty[9 + (num * 2)];
            return skill;
        }

        public static string GetSkillName(int skillId,int pos)
        {
            ///生命之源（生命）5 生命之心、生命之源、生命之泉、生命强化
            ///铜皮铁骨（物防）11 战斗之心、战斗之源、战斗之泉、攻击强化
            ///魔法抵抗（魔防）12 铜皮铁骨 神圣之盾 光明圣盾 物理屏障
            ///战斗之心（攻击）22 魔法抵抗 魔法护盾 魔力屏障 英勇抵抗   
            if (skillId == 5)
            {
                return skillName[0][pos];
            }
            else if(skillId == 11)
            {
                return skillName[1][pos]; 
            }
            else if (skillId == 12)
            {
                return skillName[2][pos];
            }else if (skillId == 22)
            {
                return skillName[3][pos];
            }
            return "";
        }


        public static string GetSkillIcn(int skillId)
        {
            ///生命之源（生命）5
            ///铜皮铁骨（物防）11
            ///魔法抵抗（魔防）12
            ///战斗之心（攻击）22
            if (skillId == 5)
            {
                return "51005";
            }
            else if (skillId == 11)
            {
                return "51003";
            }
            else if (skillId == 12)
            {
                return "51032";
            }
            else if (skillId == 22)
            {
                return "51023";
            }
            return "";
        }

        public static string GetSkillDes(int petid,int num,int lvl)
        {
            ///生命之源（生命）5
            ///铜皮铁骨（物防）11
            ///魔法抵抗（魔防）12
            ///战斗之心（攻击）22

            int[] skill = GetPetSkillInfo(petid, num);
            int skillId = skill[0];
            if (skillId == 5)
            {
                if (lvl == 0)
                {
                    return "每级增加" + skill[1] + "点生命";
                }
                return "已增加" + skill[1] * lvl + "点生命";
            }
            else if (skillId == 11)
            {
                if (lvl == 0)
                {
                    return "每级增加" + skill[1] + "点物防";
                }
                return "已增加" + skill[1] * lvl + "点物防";
            }
            else if (skillId == 12)
            {
                if (lvl == 0)
                {
                    return "每级增加" + skill[1] + "点魔防";
                }
                return "已增加" + skill[1] * lvl + "点魔防";
            }
            else if (skillId == 22)
            {
                if (lvl == 0)
                {
                    return "每级增加" + skill[1] + "点攻击上限";
                }
                return "已增加" + skill[1] * lvl + "点攻击上限";
            }
            return "";
        }


        public static string GetSkillAddDes(int petid, int num )
        {
            ///生命之源（生命）5
            ///铜皮铁骨（物防）11
            ///魔法抵抗（魔防）12
            ///战斗之心（攻击）22

            int[] skill = GetPetSkillInfo(petid, num);
            int skillId = skill[0];
            if (skillId == 5)
            {
                return "生命+" + skill[1] ;
            }
            else if (skillId == 11)
            {
                return "物防+" + skill[1] ;
            }
            else if (skillId == 12)
            {
                return "魔防+" + skill[1] ;
            }
            else if (skillId == 22)
            {
                return "攻击上限+" + skill[1];
            }
            return "";
        }


        //获取宠物的某个技能的顺序
        public static int GetSkillNum(int petid, uint skillId)
        {
            int[] petProperty = GetPetProperty(petid);
            if (petProperty[10] == skillId)
            {
                return 1;
            }
            else if (petProperty[12] == skillId)
            {
                return 2;
            }
            else if (petProperty[14] == skillId)
            {
                return 3;
            }
            else 
            {
                return 4;
            }
        }


        //获取技能消费值
        public static int GetSkillSpend(int lvl, int num)
        {
            SysPetSkillSpendVo spendVo = BaseDataMgr.instance.GetDataById<SysPetSkillSpendVo>((uint)lvl+1);

            if (num == 1)
            {
                return spendVo.spend_1;
            }
            else if (num == 2)
            {
                return spendVo.spend_2;
            }else 
            if (num == 3)
            {
                return spendVo.spend_3;
            }else 
            {
                return spendVo.spend_4;
            }
        }

        /// <summary>
        /// 查看宠物装备情况
        /// 0不存在，1存在可装备，2存在不可装备，3可合成可装备,4可合成不可装备,>4已装备的id
        /// </summary>
        /// <param name="petId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int CheckEquip(uint petId, int num)
        {
            PPet pet = Singleton<PetMode>.Instance.GetPetByPetId(petId);
            
            //需要的装备id
            int need = GetNeedEquip( petId,  num);

            //检查是否已经装备
            if (Singleton<PetMode>.Instance.GetPetEquipByPos(pet.id,(uint)num) == need)
            {
                //已装备
                return need;
            }
            else
            {
                SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>((uint)need);
                int ownNum = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint)need);

                string l = equipVo.lvl.Replace("[", "");
                l = l.Replace("]", "");

                int needlvl = 0;
                if (l.Length > 0)
                {
                    needlvl = int.Parse(l);
                }
                
                if (ownNum > 0 )
                {
                    if (needlvl > pet.lvl)
                    {
                        return 2;
                    }
                    else
                    {
                        //存在可装备
                        return 1;
                    }
                }
                else
                {
                    int cnum = CanCombine((uint)equipVo.id);
                    if (cnum > 0)
                    {
                        if (needlvl > pet.lvl)
                        {
                            return 4;
                        }
                        else
                        {
                            //可合成可装备
                            return 3;
                        }
                    }
                }
            }
            return 0;
        }

        public  static  int GetNeedEquip(uint petId, int num)
        {
            PPet pet = Singleton<PetMode>.Instance.GetPetByPetId(petId);
            SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>(petId);

            //需要的装备id
            int need = PetEquipConfig[spet.type - 1][pet.grade - 1][num - 1];
            return need;
        }

        public static int[] GetPetNeedEquips(uint type,uint grade)
        {
            return PetEquipConfig[type - 1][grade - 1];
        }

        /// <summary>
        /// 返回可以合成的数目
        /// </summary>
        /// <param name="equipId">装备id</param>
        /// <param name="exist">是否包含一存在的物品</param>
        /// <param name="neednum">合成一个需要的个数</param>
        /// <returns></returns>
        public static int CanCombine(uint equipId,bool exist = false,int neednum = 0)
        {
            SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>((uint)equipId);

            //是否包括已经存在的
            if (exist)
            {
                //已存在的
                int ownNum = Singleton<GoodsMode>.Instance.GetCountByGoodsId(equipId);

                //可合成的，不包括已存在的
                int cNum = CanCombine(equipId);

                //返回可以合成上一级所需的个数
                return (ownNum + cNum) / neednum;
                
            }

            //检查可以合成的个数
            if (equipVo.material.Length > 2)
            {
                //检查是否可以合成
                int[] goods = StringUtils.GetArrayStringToInt(equipVo.material);
                int num = -1;
                for (int i = 0; i < goods.Count();)
                {   
                    //包括已存在的，获得可以合成的个数
                    int cNum = CanCombine((uint)goods[i], true, goods[i+1]);
                    if (cNum < num || num==-1)
                    {
                        num = cNum;
                    }
                    i = i + 2;
                }
                return num;
            }
            else //装备不可合成
            {
                return 0;
            }

        }

        //查询进化或者召唤需要的灵魂石数量
        public static  int GetNeedStone(uint star ,bool evolve)
        {
            if (star == 5)
            {
                return 0;
            }

            if (evolve)
            {
                return starStoneNum[star];
            }
            else
            {
                int need = 0;
                for (int i=0; i < star;i++)
                {
                    need += starStoneNum[i];
                }
                return need;
            }
        }

        public static SysPetAdvancedVo GetPetGradeAdd(uint type,uint grade)
        {
            return GradeAdd[type*grade - 1];
        }

        //排序算法
        public static int SortPet(PPet a,PPet b)
        {
            return ((int)b.star - (int)a.star) * 10000 + (b.grade - a.grade) * 100 +((int)b.lvl - (int)a.lvl);
        }


        // 
        public  static string GetEquipProperty(string property)
        {
            string pro = property.Substring(1, property.Length - 2);
            pro = pro.Split(',')[0];
            return pro;
        }

        public static string GetEquipPropertyDes(SysEquipVo equipVo)
        {
            StringBuilder sb = new StringBuilder();
            if (equipVo.hp.Length > 4)
            {
                sb.Append("生命+").Append( PetLogic.GetEquipProperty(equipVo.hp));

            }
            if (equipVo.att_max > 0)
            {
                sb.Append("\n").Append("攻击+").Append(equipVo.att_max);
            }
            if (equipVo.def_p.Length > 4)
            {
                sb.Append("\n").Append("物防+").Append(PetLogic.GetEquipProperty(equipVo.def_p));
            }
            if (equipVo.def_m.Length > 4)
            {
                sb.Append("\n").Append("魔防+").Append(PetLogic.GetEquipProperty(equipVo.def_m));
            }
            if (equipVo.hurt_re.Length > 4)
            {
                sb.Append("\n").Append("减伤+").Append(PetLogic.GetEquipProperty(equipVo.hurt_re));
            }
            return sb.ToString();
        }

        public static void SetFBInfo(List<GameObject> fbList,int[] fbs)
        {
            UILabel fbtips = NGUITools.FindInChild<UILabel>(fbList[1].transform.parent.gameObject, "tips");
            if (fbs == null || fbs.Length == 0)
            {
                 fbList[0].SetActive(false);
                 fbList[1].SetActive(false);
                 fbList[2].SetActive(false);

                 if (fbtips != null)
                {
                    fbtips.text = "该物品暂不由关卡掉落。";
                    fbtips.gameObject.SetActive(true);
                }
                return;
            }
            if (fbtips != null)
            {
                fbtips.gameObject.SetActive(false);
            }

            //当副本产出点多于3个时，最多显示一个未开启
            if (fbs.Length > 3)
            {

            }

            for (int i = 0; i < fbList.Count; i++)
            {
                if (i < fbs.Length)
                {
                    //副本设置的问题
                    fbList[i].SetActive(true);
                    fbList[i].name = fbs[i].ToString();
                    SysMapVo mapVo = BaseDataMgr.instance.GetDataById<SysMapVo>((uint)fbs[i]);
                    if (mapVo.subtype == 7)
                    {
                        SysDaemonislandVo treeVo = BaseDataMgr.instance.GetDataById<SysDaemonislandVo>((uint)fbs[i]);
                        treeVo = BaseDataMgr.instance.GetDataById<SysDaemonislandVo>((uint)treeVo.parentId);
                        NGUITools.FindInChild<UILabel>(fbList[i], "num").text = treeVo.name;
                        NGUITools.FindInChild<UISprite>(fbList[i], "fbicn").spriteName = "island";
                    }
                    else
                    {
                        SysDungeonTreeVo treeVo = BaseDataMgr.instance.GetDataById<SysDungeonTreeVo>((uint)fbs[i]);
                        treeVo = BaseDataMgr.instance.GetDataById<SysDungeonTreeVo>((uint)treeVo.parentId);
                        treeVo = BaseDataMgr.instance.GetDataById<SysDungeonTreeVo>((uint)treeVo.parentId);
                        NGUITools.FindInChild<UILabel>(fbList[i], "num").text = treeVo.name;
                        NGUITools.FindInChild<UISprite>(fbList[i], "fbicn").spriteName = treeVo.icon;
                    }
                    NGUITools.FindInChild<UILabel>(fbList[i], "name").text = mapVo.name;
                    UILabel tips = NGUITools.FindInChild<UILabel>(fbList[i], "tips");
                    if (!Singleton<CopyControl>.Instance.IsCopyOpened((uint)fbs[i]))
                    {
                        tips.text = "(未开启)";
                        tips.color = ColorConst.FONT_RED;
                        tips.transform.gameObject.SetActive(true);
                    }
                    else
                    {
                        //显示次数
                        if (mapVo.subtype == 7)
                        {
                            DaemonCopyVo copy = Singleton<DaemonIslandMode>.Instance.GetDaemonCopyInfo((uint)fbs[i]);
                            tips.text = "(" + copy.usedTimes + "/" +
                                        BaseDataMgr.instance.GetMapVo((uint)fbs[i]).enter_count + ")";
                            tips.color = ColorConst.FONT_YELLOW;
                            tips.transform.gameObject.SetActive(true);
                        }
                        else
                        {
                            tips.transform.gameObject.SetActive(false);
                        }

                    }

                }
                else
                {
                    fbList[i].SetActive(false);
                }
            }
        }
    }
}