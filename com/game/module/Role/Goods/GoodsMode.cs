using System;
using com.game.consts;
using com.game.utils;
using com.game.vo;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.IO;
using Proto;
using com.game;
using com.game.data;
using com.game.manager;
using PCustomDataType;
using System.Collections.Generic;
using com.u3d.bases.debug;

namespace Com.Game.Module.Role
{
    public class GoodsMode : BaseMode<GoodsMode>
    {
        /// <summary>
        /// 背包物品分类
        /// </summary>
        public enum GoodsType
        {
            Total,//全部
            Equip,//装备
            Smelt,//宝石
            Pet,//宠物
            Other//其他
        }
        public readonly uint GoodsId = 100000; //装备id 和物品id 区分
        public readonly uint RefineGoodsId = 113758;//精炼石Id 
        public readonly byte GOODS_REPOS = 1;//背包栏
        public readonly byte EQUIP_REPOS = 2;//装备栏
         
		/*1为武器；2为头盔；3为衣服；4为裤子；5为鞋子；6为项链；
         * 7为手环；8为戒指；9为手套；10为徽章；11为翅膀 ；
         * 12为时装;13为婚戒;14为勋章
		 */
		public readonly int UPDATE_GOODS = 1;
		public readonly int UPDATE_EQUIP = 2;
		public readonly int UPDATE_GRID  = 4;
		public readonly int UPDATE_ADD_GOODS = 8;//增加物品，用于播放动画

		public readonly int UPDATE_OTHEE_EQUIP = 9;//其他人的装备信息（用于查看其他人的信息）
       
        public readonly int UPDATE_PET_GOODS = 12;//更新宠物
        public readonly int UPDATE_ROLE_EQUIP = 13;//更新普通装备
        public readonly int UPDATE_SMELT_GOODS = 14;//更新宝石
        public readonly int UPDATE_PROP_GOODS = 15;//更新道具
        public readonly int UPDATE_TIPS = 16;//更新tips

		
		//背包物品总列表
		public List<PGoods> goodsList = new List<PGoods>();
        /// <summary>
        /// 角色装备
        /// </summary>
        public List<PGoods> RoleEquipList = new List<PGoods>(); 
        /// <summary>
        /// 宝石列表
        /// </summary>
        public List<PGoods> SmeltGoodsList = new List<PGoods>();
        /// <summary>
        /// 宠物列表
        /// </summary>
        public List<PGoods> PetGoodsList = new List<PGoods>(); 
        /// <summary>
        /// 道具列表
        /// </summary>
        public List<PGoods> PropGoodsList = new List<PGoods>(); 
		//装备栏物品列表
		public List<PGoods> equipList = new List<PGoods>();
		//其他人装备信息
		public List<PGoods> otherList = new List<PGoods>();

        public override bool ShowTips
        {
            get
            {
                return IsShowTips();
            }
        }

        private bool IsShowTips()
        {
            bool isShow = false;
            foreach (PGoods goods in RoleEquipList)
            {
                if (IsShowEquipTips(goods.id))
                {
                    isShow = true;
                    break;
                }
            }
            return isShow;
        }
        //装备等级 x  品质 y 强化等级 m 精炼等级 n 鉴定费用钻石 z = 2*x*y  
        //分解数量 f = int(x/50+(y/3)^4 + m/5 + (n/2)^2.3) 继承费用 j = x ^ 2 /2 + (y+4)^4
        /// <summary>
        /// 是否可精炼
        /// </summary>
        /// <param name="uid"></param>
        /// <returns> 0 可继承 1 金币不足 2 精炼石不够</returns>
        public int IsRefine(uint uid)
        {
            SysRefineVo sv = GetRefineVo(uid);
            if (MeVo.instance.diam < sv.money ) 
                return 1;
            else if (GetRefineGoodsCount() < int.Parse(sv.goods) )
                return 2;
            return 0;
        }
        
        public SysRefineVo GetRefineVo(uint uid)
        {
            PGoods currentGoods = GetPGoodsById(uid);
            SysRefineVo sv = BaseDataMgr.instance.GetDataById<SysRefineVo>((currentGoods.equip[0].refine));
            return sv;
        }
        /// <summary>
        /// 是否可强化
        /// </summary>
        /// <param name="uid"></param>
        /// <returns> 0 可强化 1 金币不足 2 超过玩家等级 3 强化至顶级</returns>
        public int IsStren(uint uid)
        {
            SysStrenthVo vo = GetStrenCost(uid);
            if (IsStrenTop(uid))
                return 3;
            else if (IsStrenOverLvl(uid))
                return 2;
			else if (vo.consume > MeVo.instance.diam)
                return 1;
            return 0;
        }
        /// <summary>
        /// 是否超过玩家等级
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsStrenOverLvl(uint uid)
        {
            PGoods currentGoods = GetPGoodsById(uid);
            int stren = currentGoods.equip[0].stren;
            return stren >= MeVo.instance.Level;
        }
        /// <summary>
        /// 是否强化至最高等级
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsStrenTop(uint uid)
        {
            PGoods currentGoods = GetPGoodsById(uid);
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(currentGoods.goodsId);
            int stren = currentGoods.equip[0].stren;
            return stren >= vo.max_stren;
        }
        /// <summary>
        /// 金币是否够强化
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsAffordStren(uint uid)
        {
            SysStrenthVo vo = GetStrenCost(uid);
			return vo.consume <= MeVo.instance.diam;
        }
        /// <summary>
        /// 获取强化费用
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public SysStrenthVo GetStrenCost(uint uid)
        {
            PGoods currentGoods = GetPGoodsById(uid);
            if (currentGoods.equip.Count > 0)
            {
                SysStrenthVo sv = BaseDataMgr.instance.GetDataById<SysStrenthVo>((byte)(currentGoods.equip[0].stren + (byte)1));
                return sv;
            }
            return null;
        }

        /// <summary>
        /// 是否可继承
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsInherit(uint uid)
        {
            PGoods currentGoods = GetPGoodsById(uid);
			return GetInheritCost(currentGoods) <= MeVo.instance.diam;
        }
        /// <summary>
        /// 获取继承费用
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        public int GetInheritCost(PGoods goods)
        {
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            int lvl = int.Parse(StringUtils.SplitVoString(vo.lvl)[0]);
            return (int)Mathf.Ceil(Mathf.Pow((float)lvl, 2f) / 2 + Mathf.Pow((vo.color + 4), 4));
        }
        /// <summary>
        /// 是否顶级宝石
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsTopSmelt(uint id)
        {
            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(id);
            if (id % 10== 0 && vo.type == GoodsConst.SMELT_GOODS)  //宝石类型约束
                return true;
            return false;
        }

        public int GetTopExp()
        {
            return 16000;
        }


        /// <summary>
        /// 返回升到下一级需要的总经验,如果是最高级宝石的话返回自身能量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetSmeltNextExp(uint id)
        {
            if (IsTopSmelt(id))
            {
                SysItemVo vo2 = BaseDataMgr.instance.GetDataById<SysItemVo>(id);
                return Int32.Parse(StringUtils.SplitVoString(vo2.other)[0]);
            }
            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(id + 1);
            SysItemVo vo1 = BaseDataMgr.instance.GetDataById<SysItemVo>(id);
            return Int32.Parse(StringUtils.SplitVoString(vo.other)[0]) - Int32.Parse(StringUtils.SplitVoString(vo1.other)[0]);
        }
        /// <summary>
        /// 宝石初始经验
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetSmeltInitExp(uint id)
        {
            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(id);
            return Int32.Parse(StringUtils.SplitVoString(vo.other)[0]);
        }
        /// <summary>
        /// 获取宝石充灵的经验：PGoods.energy 是包含自身的能力值的，要减去自身的能力值
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetSmeltMerge(uint uid)
        {
            PGoods goods = GetPGoodsById(uid);
            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goods.goodsId);
            return goods.energy - Int32.Parse(StringUtils.SplitVoString(vo.other)[0]);
        }
        /// <summary>
        /// 获取背包宝石的总经验值  + goods.energy  ; 10 级宝石返回自身值
        /// </summary>
        /// <param name="uid">唯一Id</param>
        /// <returns></returns>
        public int GetSmeltExp(uint uid)
        {
            PGoods goods = GetPGoodsById(uid);
            if (IsTopSmelt(goods.goodsId))
                return GetSmeltNextExp(goods.goodsId);
            return goods.energy;// - Int32.Parse(StringUtils.SplitVoString(vo.other)[0]);
        }
        //返回装备存储位置  0 表示没有找到
        public int GetReposById(uint uid)
        {
            foreach (PGoods goods in goodsList)
            {
                if (goods.id == uid)
                {
                    return GOODS_REPOS;

                }
            }
            foreach (PGoods goods in equipList)
            {
                if (goods.id == uid)
                {
                    return EQUIP_REPOS;

                }
            }
            return 0;
        }
        //获得装备分级精炼石的数量
        public int GetDestroyCount(PGoods goods)
        {
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            int lvl = int.Parse(StringUtils.SplitVoString(vo.lvl)[0]);
            int refine = goods.equip[0].refine;
            int stren = goods.equip[0].stren;
			float aa = Mathf.Pow((float) (refine/2), 2.3f);
			return (int)(Mathf.Ceil(lvl/50f + Mathf.Pow((vo.color/3f),4f) + stren/5f + aa));
        }
        /// <summary>
        /// 获取装备分解获取精炼石数量
        /// </summary>
        /// <param name="uid">唯一id</param>
        /// <returns></returns>
        public int GetDestroyCount(uint uid)
        {
            return this.GetDestroyCount(GetPGoodsById(uid));
        }
        
        //返回精炼石数量
        public int GetRefineGoodsCount()
        {
            return GetCountByGoodsId(this.RefineGoodsId);
        }

        /// <summary>
        /// 返回指定位置的普通装备,并且过滤职业
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<PGoods> GetEquipByPosInBag(int pos)
        {
            List<PGoods> equips = new List<PGoods>();
            List<PGoods> goodsList1 = GetPGoodsByType(GoodsType.Equip);
            SysEquipVo vo;
            foreach (PGoods goods in goodsList1)
            {
                if (goods.goodsId < this.GoodsId  )
                {
                    vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
                    if( vo.pos == pos  && (vo.job == MeVo.instance.job || vo.job == 0 ))
                        equips.Add(goods);
                }
            }
            return equips;
        }
        /// <summary>
        /// 从背包中获取装备可镶嵌的宝石
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<PGoods> GetSmeltGemListByGoodsId(uint goodsId)
        {
            List<PGoods> tempList = new List<PGoods>();
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);

            if (vo != null)
            {
                string[] types = com.game.utils.StringUtils.SplitVoString(vo.gem_type, ",");

                List<int> typeInt = new List<int>();
                foreach (var t in types)
                {
                    typeInt.Add(int.Parse(t));
                }
                foreach (PGoods goods in goodsList)
                {
                    SysItemVo vo1 = BaseDataMgr.instance.GetDataById<SysItemVo>(goods.goodsId);
                    if (vo1 != null && vo1.type == GoodsConst.SMELT_GOODS && typeInt.Contains(vo1.subtype))
                    {
                        tempList.Add(goods);
                    }
                }
            }
            return tempList;
        }
        public List<PGoods> GetSmeltGemListById(uint uid)
        {
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(GetPGoodsById(uid).goodsId);
            return GetSmeltGemListByGoodsId((uint)vo.id);
        }
        //获取背包里面的所有装备List
        public List<PGoods> GetEquipListInBag()
        {
            List<PGoods> equips = new List<PGoods>();
            foreach (PGoods goods in goodsList)
            {
                if (goods.goodsId < this.GoodsId)
                {
                    equips.Add(goods);
                }
            }
            return equips;
        }
		/// <summary>
		/// 获取其他人装备信息
		/// </summary>
		/// <returns>装备</returns>
		/// <param name="goodsId">装备Id</param>
		public PGoods GetOtherPGoods(uint goodsId)
		{
			foreach (PGoods goods in otherList)
			{
				if (goods.id == goodsId)
				{
					return goods;
				}
			}
			return null;
		}
        //判断当前 装备id 是否存在
        public bool IsEquipIdExist(uint uid)
        {
            return GetPGoodsById(uid) != null;
        }

        /// <summary>
        ///根据背包显示类型返回物品列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<PGoods> GetPGoodsByType(GoodsType type)
        {
            List<PGoods> goodsShow = new List<PGoods>();
            if (type == GoodsType.Total)
                return goodsList;
            else if (type == GoodsType.Equip)
                return RoleEquipList;
            else if (type == GoodsType.Smelt)
                return SmeltGoodsList;
            else if (type == GoodsType.Pet)
                return PetGoodsList;
            else if (type == GoodsType.Other)
                return PropGoodsList;
            
            return goodsShow;
        }
        /// <summary>
        /// 根据唯一id返回PGoods
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public PGoods GetPGoodsById(uint id)
		{
			foreach (PGoods goods in goodsList)
			{
				if (goods.id == id)
				{
					return goods;
					
				}
			}
			foreach (PGoods goods in equipList)
			{
				if (goods.id == id)
				{
					return goods;
					
				}
			}
			
			return null;
		}
        /// <summary>
        /// 返回背包物品数量
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public int GetCountByGoodsId(uint goodsId)
        {
            int num = 0;
            List<PGoods> tempList = GetPGoodsByType(VoUtils.GetGoodsType(goodsId));
            foreach (PGoods goods in tempList)
            {
                if (goods.goodsId == goodsId)
                {
                    num += goods.count;
                }
            }
            return num;
        }
        /// <summary>
        /// 返回后端推送的物品的数据结构
        /// </summary>
        /// <param name="goodsId">物品Id</param>
        /// <returns></returns>
        public PGoods GetPGoodsByGoodsId(uint goodsId)
        {
            List<PGoods> tempList = GetPGoodsByType(VoUtils.GetGoodsType(goodsId));
            foreach (PGoods goods in tempList)
            {
                if (goods.goodsId == goodsId)
                {
                    return goods;
                }
            }
            return null;
        }
        /// <summary>
        /// 是否显示可装备的箭头提示
        /// </summary>
        /// <param name="id">唯一Id</param>
        /// <returns></returns>
        public bool IsShowEquipTips(uint id)
        {
            PGoods goods = GetPGoodsById(id);
            if (goods != null && goods.goodsId < GoodsId)  //装备
            {
                SysEquipVo vo = goods.GetPGoodsVo<SysEquipVo>();
                int fightPoint = GoodsMode.Instance.CalculateFightPoint(goods);  //本装备的战斗力
                int fightPoint1 = GoodsMode.Instance.GetFightPointByPos(vo.pos);  //装备栏对应位置的战斗力
                if (fightPoint > fightPoint1 && goods.IsJobCanEquip() && vo.GetLvl() <= MeVo.instance.Level) //战斗力  是否可装备(角色类型）,等级
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 返回装备栏对应位置的装备的战斗力，没有装备则返回 0 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int GetFightPointByPos(int pos)
        {
            int value = 0;
            foreach (PGoods goods in equipList)
            {
                if (goods.pos == pos)
                {
                    value = CalculateFightPoint(goods);
                    break;
                }
            }
            return value;
        }

        public int CalculateFightPoint(PGoods goods)
        {
            //物品为空，或者不是装备，直接返回 0
            if (goods == null || goods.goodsId > GoodsId)
                return 0;
            List<PSuitAttr> attrList = new List<PSuitAttr>();
            attrList = goods.GetAttrTotalValue();
            float fightpoint = 0;
            foreach (PSuitAttr attr in attrList)
            {
                switch (attr.id)
                {
                    case GoodsConst.ATTR_ID_HP: //生命
                        fightpoint += (float)attr.attr * 0.2f;
                        break;
                    case GoodsConst.ATTR_ID_MP: //魔法
                        fightpoint += (float)attr.attr * 0.1f;
                        break;
                    case GoodsConst.ATTR_ID_ATT_DEF_M: //魔防
                    case GoodsConst.ATTR_ID_ATT_DEF_P: //物防
                    case GoodsConst.ATTR_ID_DODGE: //闪避
                    case GoodsConst.ATTR_ID_HIT: //命中
                        fightpoint += (float)attr.attr * 0.5f;
                        break;
                    case GoodsConst.ATTR_ID_CRIT_RATIO: //暴伤
                        fightpoint += (float)attr.attr * 0.7f;
                        break;
                    case GoodsConst.ATTR_ID_LUCK: //幸运
                        fightpoint += (float)attr.attr * 0.8f;
                        break;
                    case GoodsConst.ATTR_ID_HURT_RE: //减伤
                        fightpoint += (float)attr.attr;
                        break;
                    case GoodsConst.ATTR_ID_ATT_P_MIN: //最小物攻
                    case GoodsConst.ATTR_ID_ATT_P_MAX: //最大物攻
                    case GoodsConst.ATTR_ID_ATT_M_MIN: //最小魔防
                    case GoodsConst.ATTR_ID_ATT_M_MAX: //最大魔防
                    case GoodsConst.ATTR_ID_ATT_MAX: //最大攻击
                    case GoodsConst.ATTR_ID_CRIT: //暴击
                    case GoodsConst.ATTR_ID_FLEX: //韧性
                        fightpoint += (float)attr.attr * 0.6f;
                        break;
                }
            }
            return Mathf.RoundToInt(fightpoint);
        }
        /// <summary>
        /// 计算装备战斗力
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="isGoodsId">是否是物品id 为 true uid表示物品id, 为 false uid 表示装备唯一id</param>
        /// <returns></returns>
        public int CalculateFightPoint(uint uid,bool isGoods = false)
        {
            
            if (!isGoods)
            {
                PGoods goods = new PGoods();
                goods.goodsId = uid;
                return CalculateFightPoint(goods);
            }
            else
            {
                PGoods goods = this.GetPGoodsById(uid);
                if(goods != null)
                    return CalculateFightPoint(goods);
            }
            return 0;
        }

        #region 协议
        
        //根据 id排序 方便比对
        public int CompareByIdFunc(PGoods left,PGoods right)
        {
            if (left.id > right.id)
                return 1;
			else if (left.id < right.id)
                return -1;
            else
                return 0;
        }

        private void SplitGoodsList()
        {
            RoleEquipList.Clear();
            SmeltGoodsList.Clear();
            PropGoodsList.Clear();
            PetGoodsList.Clear();
            foreach (PGoods goods in goodsList)
            {
                //普通装备
                if (goods.goodsId < GoodsId
                    && goods.GetPGoodsVo<SysEquipVo>().type == 0)
                {
                    RoleEquipList.Add(goods);
                }
                //宝石
                else if (goods.goodsId > GoodsId && goods.GetPGoodsVo<SysItemVo>().type == 3)
                {
                    SmeltGoodsList.Add(goods);
                }
                //宠物：宠物 和宠物装备
                else if ((goods.goodsId > GoodsId && goods.GetPGoodsVo<SysItemVo>().type == 5) ||
                    (goods.goodsId < GoodsId && goods.GetPGoodsVo<SysEquipVo>().type == 3))
                {
                    PetGoodsList.Add(goods);
                }
                else
                {
                    PropGoodsList.Add(goods);
                }
            }
        }

        //更新物品信息 1.背包 2.装备
        public void UpdateGoodsInfo(List<PGoods> goodsInfo,byte repos)
        {
            goodsInfo.Sort(CompareByIdFunc);
            bool updateRoleEquip = false;
            bool updateSmeltGoods = false;
            bool updatePetGoods = false;
            bool updatePropGoods = false;
            List<PGoods> tempList = null; 
            if (repos == GOODS_REPOS)
                tempList = goodsList;
            else if (repos == EQUIP_REPOS)
                tempList = equipList;
            tempList.Sort(CompareByIdFunc);
            int index = 0;
            PGoods temp;
            if (tempList.Count > 0)
            {
                foreach (PGoods goods in goodsInfo)  //保证迭代次数为 m+ n
                {
                    if (index > tempList.Count - 1)
                    {
                        tempList.Add(goods);
                    }
                    else 
                    {
                        temp = tempList[index];
                        while (goods.id > temp.id)
                        {
                            index ++;
                            if (index > tempList.Count - 1)
                            {
                                index = tempList.Count - 1;
                                break;
                            }
                            temp = tempList[index];
                        }
                        if (goods.id == temp.id  )
                        {
                            tempList.Remove(temp);
                            index--;
                        }
                        if (goods.count != 0 ) //数量为 0 表示删除,即不加入
                        {
                            index ++;
                            tempList.Insert(index ,goods);
                        }
                        
                        switch (goods.GetGoodsType())   //忽略goods.count == 0 的新物品判断
                        {
                            case GoodsType.Equip:
                                updateRoleEquip = true;
                                break;
                            case GoodsType.Pet:
                                updatePetGoods = true;
                                break;
                            case GoodsType.Smelt:
                                updateSmeltGoods = true;
                                break;
                            case GoodsType.Other:
                                updatePropGoods = true;
                                break;
                        }
                        
                    }
                    
                }
            }
            else
            {
                tempList.AddRange(goodsInfo);
            }
            if (repos == GOODS_REPOS)
            {
                SplitGoodsList();
                DataUpdate(UPDATE_GOODS);
                if (updateRoleEquip)
                {
                    DataUpdate(UPDATE_ROLE_EQUIP);
                    DataUpdate(this.UPDATE_TIPS);
                }
                if (updatePetGoods)
                    DataUpdate(UPDATE_PET_GOODS);
                if (updateSmeltGoods)
                    DataUpdate(UPDATE_SMELT_GOODS);
                if (updatePropGoods)
                    DataUpdate(UPDATE_PROP_GOODS);
            }
            else if (repos == EQUIP_REPOS)
            {
                DataUpdate(this.UPDATE_TIPS);
                DataUpdate(UPDATE_EQUIP);
            }
                
            
        }

        private PGoods FindGoodsById(List<PGoods> list,uint id)
        {
            foreach (PGoods goods in list)
            {
                if (goods.id == id)
                    return goods;
            }
            return null;
        }
        //丢弃物品
        public void DeleteGoods(List<uint> ids,byte repos)
        {
            ids.Sort();
            int index = 0;
            bool updateRoleEquip = false;
            bool updateSmeltGoods = false;
            bool updatePetGoods = false;
            bool updatePropGoods = false;
            PGoods goods;
            List<PGoods> tempList = new List<PGoods>(); ;
            if (repos == 1)
                tempList = goodsList;
            else if (repos == 2)
                tempList = equipList;
            if (ids.Count > 0)
            {
                foreach (uint id in ids)
                {
                    goods = FindGoodsById(tempList, id);
                    if (goods != null)
                    {
                        tempList.Remove(goods);
                        switch (goods.GetGoodsType())   //忽略goods.count == 0 的新物品判断
                        {
                            case GoodsType.Equip:
                                updateRoleEquip = true;
                                break;
                            case GoodsType.Pet:
                                updatePetGoods = true;
                                break;
                            case GoodsType.Smelt:
                                updateSmeltGoods = true;
                                break;
                            case GoodsType.Other:
                                updatePropGoods = true;
                                break;
                        }
                    }
                }
                if (repos == GOODS_REPOS)
                {
                    SplitGoodsList();
                    DataUpdate(UPDATE_GOODS);
                    if (updateRoleEquip)
                    {
                        DataUpdate(UPDATE_ROLE_EQUIP);
                        DataUpdate(this.UPDATE_TIPS);
                    }
                    if (updatePetGoods)
                        DataUpdate(UPDATE_PET_GOODS);
                    if (updateSmeltGoods)
                        DataUpdate(UPDATE_SMELT_GOODS);
                    if (updatePropGoods)
                        DataUpdate(UPDATE_PROP_GOODS);
                }
                else if (repos == EQUIP_REPOS)
                {
                    DataUpdate(UPDATE_EQUIP);
                    DataUpdate(this.UPDATE_TIPS);
                }
            }
        }
		//更新其他人的角色装备信息
		public void UpdateOtherEquips(List<PGoods> goodsList)
		{
			otherList = goodsList;
			DataUpdate(UPDATE_OTHEE_EQUIP);
		}

        #endregion
        
    }
}

