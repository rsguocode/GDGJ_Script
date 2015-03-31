

using System;
using System.Collections.Generic;
using com.game.data;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using Proto;

namespace Com.Game.Module.Pet
{
    internal class PetMode : BaseMode<PetMode>
    {
        public const int PetList = 1; //宠物列表更新
        public const int NewPet = 2; //新宠物
        public const int UpdatedPet = 3; //更新宠物
        public const int EvolvePet = 4; //宠物进化
        public const int FightPet = 5; //出战宠物

        public const int SkillUpgrade = 6; //技能升级
        public const int WearEquip = 7; //穿装备
        public const int GradeUpgrade = 8; // 升阶
        public const int SkillPointInfo = 9; //技能点
        public const int LevelUpgrade = 10; //等级

        public PPet CurrentPet { get; private set; }
        public uint[] UpgradedSkill { get; private set; }
        public uint[] WearedEquip { get; private set; }

        private PPet fightPet = null;

        public List<PPet> AllPets { get; private set; }

        public Dictionary<uint, PetVo> PetVos = new Dictionary<uint, PetVo>();
        public PetVo totalAdd = new PetVo();
        public PetSkillPointInfoMsg_21_7 SkillPoint { get; private set; }

        public void SetPetList(List<PPet> pets)
        {
            AllPets = pets;
            //计算宠物的数据
            PetVos.Clear();
            foreach (PPet pet in AllPets)
            {
                PetVos.Add(pet.id, PetLogic.GetPetVo(pet));
            }
            AddTotal();

            AllPets.Sort(PetLogic.SortPet);


            DataUpdate(PetList);
            SetFightPet();
        }

        private void AddTotal()
        {
            totalAdd = new PetVo();
            foreach (PetVo petVo in PetVos.Values)
            {
                PetLogic.AddPetALL(totalAdd, petVo);
            }
            totalAdd.fight = PetLogic.GetFight(totalAdd);
        }

        /// <summary>
        /// 获取宠物
        /// </summary>
        /// <param name="petId">宠物类ID</param>
        public PPet GetPetByPetId(uint petId)
        {
            if (AllPets != null)
            {
                foreach (PPet pet in AllPets)
                {
                    if (pet.petId == petId)
                    {
                        return pet;
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// 获取宠物
        /// </summary>
        /// <param name="uid">宠物唯一ID</param>
        public PPet GetPetById(uint uid)
        {
            if (AllPets != null)
            {
                foreach (PPet pet in AllPets)
                {
                    if (pet.id == uid)
                    {
                        return pet;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取出战宠物信息
        /// </summary>
        /// <returns></returns>
        public PetVo GetFightPetVo()
        {
            if (fightPet != null)
            {
                return PetVos[fightPet.id];
            }
            return null;
        }

        private PPet GetFightPet()
        {
            foreach (PPet pet in AllPets)
            {
                if (pet.state == 2)
                {
                    return pet;
                }
            }
            return null;
        }

        public void AddPet(List<PPet> pets)
        {
            if (AllPets == null)
            {
                AllPets = new List<PPet>();
            }

            AllPets.AddRange(pets);
            foreach (PPet pet in pets)
            {
                PetVos.Add(pet.id, PetLogic.GetPetVo(pet));
            }
            AddTotal();
            AllPets.Sort(PetLogic.SortPet);

            DataUpdate(PetList);
            foreach (PPet pet in pets)
            {
                CurrentPet = pet;
                DataUpdate(NewPet);
            }
        }

        public void UpdatePet(uint id, List<PItem> items)
        {
            PPet pet = GetPetById(id);
            bool stateChange = false;
            bool evolve = false;
            bool skillUpgrade = false;
            bool equipWear = false;
            bool gradeUpgrade = false;
            bool levelUpgrade = false;
            bool sortChange = false;

            if (pet == null)
            {

                Log.error(this, id + "当前宠物数据不存在");
            }
            else
            {
                CurrentPet = pet;
                foreach (PItem item in items)
                {
                    if (item.key == 5)
                    {
                        if (item.value[0] > pet.lvl)
                        {
                            pet.lvl = item.value[0];
                            sortChange = true;
                            levelUpgrade = true;
                        }
                    }
                    else if (item.key == 6)
                    {
                        pet.exp = item.value[0];
                    }
                    else if (item.key == 7)
                    {
                        pet.state = (byte) item.value[0];
                        stateChange = true;
                    }
                    else if (item.key == 8)
                    {
                        pet.star = item.value[0];
                        evolve = true;
                        sortChange = true;
                    }
                    else if (item.key == 9)
                    {
                        //升阶
                        if ((byte) item.value[0] > pet.grade)
                        {
                            pet.grade = (byte) item.value[0];
                            gradeUpgrade = true;
                            sortChange = true;
                        }

                    }
                    else if (item.key == 10)
                    {
                        //装备升阶
                        if (item.value.Count == 0)
                        {
                            pet.equip = item.value;
                        }
                        else
                        {
                            //装备信息变化
                            for (int i = 0; i < item.value.Count;)
                            {
                                uint pos = item.value[i];
                                uint equipId = item.value[i + 1];

                                //当前装备
                                int cEquipId = GetPetEquipByPos(pet.id, pos);
                                if (cEquipId == -1)
                                {
                                    //添加装备
                                    pet.equip.Add(pos);
                                    pet.equip.Add(equipId);
                                    equipWear = true;
                                    WearedEquip = new uint[] {pos, equipId};
                                }
                                i = i + 2;
                            }
                        }
                    }
                    else if (item.key == 15)
                    {
                        for (int i = 0; i < item.value.Count;)
                        {
                            byte pos = (byte) item.value[i];
                            byte lvl = (byte) item.value[i + 1];

                            int index = GetPetSkillIndexByPos(pet.id, pos);
                            if (index == -1)
                            {
                                //添加技能
                                pet.skills.Add(pos);
                                pet.skills.Add(lvl);
                            }
                            else
                            {
                                uint clvl = pet.skills[index + 1];
                                if (clvl < lvl)
                                {
                                    pet.skills[index + 1] = lvl;
                                    skillUpgrade = true;
                                    UpgradedSkill = new uint[] {pos, lvl};
                                }
                            }
                            i = i + 2;
                        }
                    }
                }
                //更新属性
                PetLogic.SetPetVo(pet, PetVos[pet.id]);
                AddTotal();

                //发出通知数据更新
                if (stateChange)
                {
                    SetFightPet();
                }
                if (levelUpgrade)
                {
                    DataUpdate(LevelUpgrade);
                }
                if (evolve)
                {
                    DataUpdate(EvolvePet);
                    DataUpdate(FightPet);
                }
                if (skillUpgrade)
                {
                    //技能升级的通知
                    DataUpdate(SkillUpgrade);
                }
                if (equipWear)
                {
                    //穿新装备通知
                    DataUpdate(WearEquip);
                }
                if (gradeUpgrade)
                {
                    DataUpdate(GradeUpgrade);
                }
                DataUpdate(UpdatedPet);
                if (sortChange)
                {
                    AllPets.Sort(PetLogic.SortPet);
                }
                DataUpdate(PetList);
            }
        }

        private void SetFightPet()
        {

            PPet pet = GetFightPet();
            if (fightPet != pet)
            {
                fightPet = pet;
                if (fightPet == null)
                {
                    MeVo.instance.PetId = 0;
                }
                else
                {
                    MeVo.instance.PetId = fightPet.petId;
                }
                DataUpdate(FightPet);
            }
        }

        //查询宠物的技能信息
        public uint[] GetPetSkill(uint uid, uint pos)
        {
            PPet pet = GetPetById(uid);
            if (pet != null)
            {
                for (int i = 0; i < pet.skills.Count;)
                {
                    if (pet.skills[i] == pos)
                    {
                        uint[] skill = new uint[2];
                        skill[0] = pos;
                        skill[1] = pet.skills[i + 1];
                        return skill;
                    }
                    i += 2;
                }
            }
            return null;

        }

        //获得宠物的下标位置
        public int GetPetSkillIndexByPos(uint uid, uint pos)
        {
            PPet pet = GetPetById(uid);
            if (pet != null)
            {
                for (int i = 0; i < pet.skills.Count;)
                {
                    if (pet.skills[i] == pos)
                    {
                        return i;
                    }
                    i += 2;
                }
            }
            return -1;
        }

        //获取装备所在的位置，不存在则返回-1
        public int GetPetEquipByPos(uint uid, uint pos)
        {
            PPet pet = GetPetById(uid);
            if (pet != null)
            {
                for (int i = 0; i < pet.equip.Count;)
                {
                    if (pet.equip[i] == pos)
                    {
                        return (int) pet.equip[i + 1];
                    }
                    i += 2;
                }
            }
            return -1;
        }

        public void SetSkillPointInfo(PetSkillPointInfoMsg_21_7 skillPoint)
        {
            SkillPoint = skillPoint;
            DataUpdate(SkillPointInfo);
        }

        public override bool ShowTips
        {
            get { return CheckTips(); }
        }

        private bool CheckTips()
        {
            foreach (SysPet spet in PetLogic.SysPets)
            {
                if (PetLogic.CanOwn(spet))
                {
                    return true;
                }
            }

            if (AllPets != null)
            {
                foreach (PPet pet in AllPets)
                {
                    for (int i = 1; i < 7; i++) //检查装备
                    {
                        int result = PetLogic.CheckEquip(pet.petId, i);
                        if (result == 1 || result == 3)
                        {
                            return true;
                        }
                    }

                    if (PetLogic.CanEvolve(pet))
                    {
                        return true;
                    }
                    if (PetLogic.CanUpgrade(pet))
                    {
                        return true;
                    }
                }
            }

            return false;

        }
    }
}
