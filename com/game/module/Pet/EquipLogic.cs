
using System.Linq;
using com.game.data;
using com.game.manager;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;

namespace Com.Game.Module.Pet
{
    class EquipLogic
    {
        private static BaseRoleVo[] gradeVos = new BaseRoleVo[39]; //14阶装备

        /// <summary>
        /// 计算宠物的装备属性
        /// </summary>
        /// <param name="petid"></param>
        /// <returns></returns>
        public static void CupPetEquipProperties(PPet pet,PetVo petVo)
        {
            //添加当前装备
            for (int i = 0; i < pet.equip.Count;)
            {
                uint equipId = pet.equip[i + 1];
                SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>(equipId);
                AddEquipProperty(petVo, equipVo);
                i = i + 2;
            }
            //添加当前阶的装备
            
            BaseRoleVo gradeVo = GetGradeProperties((uint)petVo.SysPet.type, pet.grade);
            PetLogic.AddPetALL(petVo,gradeVo);
        }

        /// <summary>
        /// 获取阶对应的加成
        /// </summary>
        /// <param name="type"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static BaseRoleVo GetGradeProperties(uint type, uint grade)
        {
                    BaseRoleVo gradeVo = new BaseRoleVo();
                    SysPetAdvancedVo gradeAdd = PetLogic.GetPetGradeAdd(type, grade);
                    gradeVo.AttPMax = (uint)gradeAdd.att_max;
                    gradeVo.Hp = (uint)gradeAdd.hp;
                    gradeVo.DefP = (uint)gradeAdd.def_p;
                    gradeVo.DefM = (uint)gradeAdd.def_m;
                    gradeVo.HurtRe = (uint)gradeAdd.hurt_re;
                    return gradeVo;
        }



        /// <summary>
        /// 计算装备属性
        /// </summary>
        public static void AddEquipProperty(BaseRoleVo roleVo, SysEquipVo equipVo)
        {
            uint[] hpP = GetDescript(equipVo.hp);
            roleVo.Hp += hpP[0];
            
            roleVo.Mp += (uint)equipVo.mp;

            roleVo.AttPMax += (uint)equipVo.att_max;

            uint[] pd = GetDescript(equipVo.def_p);
            roleVo.DefP += pd[0];

            uint[] md = GetDescript(equipVo.def_m);
            roleVo.DefM += md[0];

            uint[] hurtr = GetDescript(equipVo.hurt_re);
            roleVo.HurtRe += hurtr[0];
        }

        private static  uint[] GetDescript(string propertyDes)
        {
            uint[] property = new uint[2];
            string[] des = propertyDes.Substring(1, propertyDes.Length - 2).Split(',');
            if (des.Length > 1)
            {
                property[0] = uint.Parse(des[0]);
                property[1] = uint.Parse(des[1]);
            }
            return property;
        }

    }
}

