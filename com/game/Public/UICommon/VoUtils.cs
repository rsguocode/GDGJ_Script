using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Role;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;

static public class VoUtils
{
    #region VoUtils
    /// <summary>
    /// 返回角色的属性值
    /// </summary>
    /// <param name="vo"></param>
    /// <param name="type">属性类型</param>
    /// <returns></returns>
    public static uint GetAttrValueById(this MeVo vo, int type)
    {
        switch (type)
        {
            case GoodsConst.ATTR_ID_STR:         //力
                return vo.Str;
                break;
            case GoodsConst.ATTR_ID_AGI:         //敏
                return vo.Agi;
            case GoodsConst.ATTR_ID_PHY:         //体
                return vo.Phy;
            case GoodsConst.ATTR_ID_WIT:         //智
                return vo.Wit;
            case GoodsConst.ATTR_ID_HP:          //生命
                return vo.Hp;
            case GoodsConst.ATTR_ID_MP:          //魔法
                return vo.Mp;
            case GoodsConst.ATTR_ID_ATT_P_MIN:   //最小物功
                return vo.AttPMin;
            case GoodsConst.ATTR_ID_ATT_P_MAX:   //最大物功
                return vo.AttPMax;
            case GoodsConst.ATTR_ID_ATT_M_MIN:   //最小魔功
                return vo.AttMMin;
            case GoodsConst.ATTR_ID_ATT_M_MAX:   //最大魔功
                return vo.AttMMax;
            case GoodsConst.ATTR_ID_ATT_DEF_P:   //物防
                return vo.DefP;
            case GoodsConst.ATTR_ID_ATT_DEF_M:   //魔防
                return vo.DefM;
            case GoodsConst.ATTR_ID_HIT:         //命中
                return vo.Hit;
            case GoodsConst.ATTR_ID_DODGE:       //闪避
                return (uint)vo.Dodge;
            case GoodsConst.ATTR_ID_CRIT:        //暴击
                return vo.Crit;
            case GoodsConst.ATTR_ID_CRIT_RATIO:  //暴击伤害比例
                return vo.CritRatio;
            case GoodsConst.ATTR_ID_FLEX:        //韧性
                return vo.Flex;
            case GoodsConst.ATTR_ID_HURT_RE:     //格挡
                return vo.HurtRe;
            case GoodsConst.ATTR_ID_SPEED:       //速度
                return vo.Speed;
            case GoodsConst.ATTR_ID_LUCK:        //幸运值
                return vo.Luck;
            case GoodsConst.ATTR_ID_ATT_MIN:     //最小攻击
                if (vo.job == GameConst.JOB_JIAN || vo.job == GameConst.JOB_CHIKE)
                    return vo.AttPMin;
                return vo.AttMMin;
            case GoodsConst.ATTR_ID_ATT_MAX:     //最大攻击  (人物攻击)
                if (vo.job == GameConst.JOB_JIAN || vo.job == GameConst.JOB_CHIKE)
                    return vo.AttPMax;
                return vo.AttMMax;
        }
        return 0;
    }
    /// <summary>
    /// 返回勋章的加成属性
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    public static List<int> GetAddAttrs(this SysMedalVo vo)
    {
        string[] attrStringList = StringUtils.SplitVoString(vo.attrs, "],[");
        List<int> attrList = new List<int>();
        foreach (var attrString in attrStringList)
        {
            foreach (var attr in StringUtils.SplitVoString(attrString))
            {
                attrList.Add(int.Parse(attr));
            }
        }
        return attrList;
    }
    //装备是否满足职业类型
    public static bool IsJobCanEquip(this PGoods goods)
    {
        SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
        if (vo != null && (vo.job == 0 || vo.job == MeVo.instance.job))
            return true;
        return false;
    }

    public static GoodsMode.GoodsType GetGoodsType(uint goodsId)
    {
        //普通装备
        if (goodsId < GoodsMode.Instance.GoodsId && BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId).type == 0)
        {
            return GoodsMode.GoodsType.Equip;
        }
        //宝石
        else if (goodsId > GoodsMode.Instance.GoodsId && BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId).type == 3)
        {
            return GoodsMode.GoodsType.Smelt;
        }
        //宠物：宠物 和宠物装备
        else if ((goodsId > GoodsMode.Instance.GoodsId && BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId).type == 5) ||
            (goodsId < GoodsMode.Instance.GoodsId && BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId).type == 3))
        {
            return GoodsMode.GoodsType.Pet;
        }
        //道具 其他
        else
        {
            return GoodsMode.GoodsType.Other;
        }
    }
    public static GoodsMode.GoodsType GetGoodsType(this PGoods goods)
    {
        //普通装备
        if (goods.goodsId < GoodsMode.Instance.GoodsId
            && goods.GetPGoodsVo<SysEquipVo>().type == 0)
        {
            return GoodsMode.GoodsType.Equip;
        }
        //宝石
        else if (goods.goodsId > GoodsMode.Instance.GoodsId && goods.GetPGoodsVo<SysItemVo>().type == 3)
        {
            return GoodsMode.GoodsType.Smelt;
        }
        //宠物：宠物 和宠物装备
        else if ((goods.goodsId > GoodsMode.Instance.GoodsId && goods.GetPGoodsVo<SysItemVo>().type == 5) ||
            (goods.goodsId < GoodsMode.Instance.GoodsId && goods.GetPGoodsVo<SysEquipVo>().type == 3))
        {
            return GoodsMode.GoodsType.Pet;
        }
        else
        {
            return GoodsMode.GoodsType.Other;
        }
    }
    /// <summary>
    /// 增加属性加成值
    /// </summary>
    /// <param name="suitList"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    private static void AddSuitAttrValue(List<PSuitAttr> suitList,int type, int value)
    {
        foreach (PSuitAttr suitAttr in suitList)
        {
            if (suitAttr.id == type)
            {
                suitAttr.attr += (ushort)value;
                return;
            }
        }
        PSuitAttr suit = new PSuitAttr();
        suit.id = (byte)type;
        suit.attr = (ushort) value;
        suitList.Add(suit);
    }
    /// <summary>
    /// 返回装备的最终加成值
    /// </summary>
    /// <param name="goods"></param>
    /// <returns></returns>
    public static List<PSuitAttr> GetAttrTotalValue(this PGoods goods)
    {
        List<PSuitAttr> suitAttr = new List<PSuitAttr>();
        SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
        
        if (vo != null)  
        {
            for (int i = 5; i< 23;i++ )   //遍历 5 - 22 的属性值
            {
                List<int> attr = vo.GetEquipAttrValue(i);
                if (attr.Count > 0)  //取出第一个值，第二值是加成值
                {
                    AddSuitAttrValue(suitAttr,i,attr[0]);
                }
            }
            int refine = (goods.equip.Count == 0) ? 0 : (int)(goods.equip[0].refine);
            int stren = (goods.equip.Count == 0) ? 0 : (int)(goods.equip[0].stren);
            //附加属性
            if (goods.equip.Count != 0)
            {
                foreach (var pSuitAttr in goods.equip[0].suitList)
                {
                    AddSuitAttrValue(suitAttr,pSuitAttr.id,pSuitAttr.attr);
                }
            }
            string[] strenType = StringUtils.SplitVoString(vo.stren_type);
            List<int> strs = new List<int>();
            foreach (string s in strenType)
            {
                strs.AddRange(vo.GetEquipAttrValue(int.Parse(s)));
            }
            int type;
            SysRefineVo refineVo;
            //强化，精炼 加成值
            for (int i = 0; i < 2; i++)
            {
                if (i < strenType.Length)
                {
                    type = int.Parse(strenType[i]);

                    int addValue = strs[i * 2 + 1];
                    int strenValue = strs[i * 2] + addValue * stren;  //强化加成值
                    refineVo = BaseDataMgr.instance.GetDataById<SysRefineVo>((uint)refine);

                    float rate = refineVo.GetRefineRate();
                    int refineValue = Mathf.RoundToInt(strenValue * rate);  //精炼加成值
                    AddSuitAttrValue(suitAttr, type, refineValue + addValue * stren);
                    
                }
            }
            //宝石镶嵌
            PEquip equip = (goods.equip.Count == 0) ? new PEquip() : goods.equip[0];
            int count = equip.gemList.Count;
            PGemInHole gem;
            SysItemVo smeltVo;
            for (int i = 0; i < 5; i++)   //宝石镶嵌信息
            {
                if (i < count)   //已经镶嵌
                {
                    gem = equip.gemList[i];
                    smeltVo = BaseDataMgr.instance.GetDataById<SysItemVo>(gem.gemId);
                    AddSuitAttrValue(suitAttr, smeltVo.subtype, smeltVo.value);
                }
            }
        }
        return suitAttr;
    }

    

    public static T GetPGoodsVo<T>(this PGoods gooods) where T : class
    {
        return BaseDataMgr.instance.GetDataById<T>(gooods.goodsId) as T;
    }
    /// <summary>
    /// 返回宠物装备的tips 加成值
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    public static List<string> GetPetAttrList(this SysEquipVo vo)
    {
        List<string> attrList = new List<string>();
        if (vo.type == GoodsConst.PET_EQUIP)
        {
            string[] attr = StringUtils.SplitVoString(vo.hp);  //生命
            if (attr != null && attr.Length > 1)
            {
                attrList.Add(RoleAttrFormat1(GoodsConst.ATTR_ID_HP, "+" + attr[0]));
            }
            attr = StringUtils.SplitVoString(vo.def_p);  //魔防
            if (attr != null && attr.Length > 1)
            {
                attrList.Add(RoleAttrFormat1(GoodsConst.ATTR_ID_ATT_DEF_P, "+" + attr[0]));
            }
            attr = StringUtils.SplitVoString(vo.def_m);  //物防
            if (attr != null && attr.Length > 1)
            {
                attrList.Add(RoleAttrFormat1(GoodsConst.ATTR_ID_ATT_DEF_M, "+" + attr[0]));
            }
            attr = StringUtils.SplitVoString(vo.hurt_re);  //减伤
            if (attr != null && attr.Length > 1)
            {
                attrList.Add(RoleAttrFormat1(GoodsConst.ATTR_ID_HURT_RE, "+" + attr[0]));
            }
            //最大攻击
            if(vo.att_max > 0)
                attrList.Add(RoleAttrFormat1(GoodsConst.ATTR_ID_ATT_MAX, "+" + vo.att_max));

        }
        return attrList;
    }
    public static float GetRefineRate(this SysRefineVo vo)
    {
        return vo.value/10000f ;
    }

    public static int GetLvl(this SysEquipVo vo)
    {
        return int.Parse(StringUtils.SplitVoString(vo.lvl)[0]);
    }
    /// <summary>
    /// 获取装备镶嵌类型
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    public static int GetGemType(this SysEquipVo vo)
    {
        return int.Parse(StringUtils.SplitVoString(vo.gem_type)[0]);
        //vo.GetGemType()调用
    }
    /// <summary>
    /// 返回装备的属性值 ：转换为整数
    /// </summary>
    /// <param name="vo"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<int> GetEquipAttrValue(this SysEquipVo vo, int type)
    {
        List<int> attrList = new List<int>();
        if (type == 1)   //整数的属性
            attrList.Add(vo.str);
        else if (type == 2)
            attrList.Add(vo.agi);
        else if (type == 3)
            attrList.Add(vo.phy);
        else if (type == 4)
            attrList.Add(vo.wit);
        else if (type == 6)
            attrList.Add(vo.mp);
        else if (type == 7)
            attrList.Add(vo.att_p_min);
        else if(type == 9)
            attrList.Add(vo.att_m_min);
        else if (type == 19)
            attrList.Add(vo.speed);
        else if (type == 22)
            attrList.Add(vo.att_max);
        else   //字符串的属性
        {
            string attr = string.Empty;
            if (type == 5)
                attr = vo.hp;
            else if (type == 8)
                attr = vo.att_p_max;
            else if (type == 10)
                attr = vo.att_m_max;
            else if (type == 11)
                attr = vo.def_p;
            else if (type == 12)
                attr = vo.def_m;
            else if (type == 13)
                attr = vo.hit;
            else if (type == 14)
                attr = vo.dodge;
            else if (type == 15)
                attr = vo.crit;
            else if (type == 16)
                attr = vo.crit_ratio;
            else if (type == 17)
                attr = vo.flex;
            else if (type == 18)
                attr = vo.hurt_re;
            else if (type == 20)
                attr = vo.luck;
            string[] attrSplit = StringUtils.SplitVoString(attr);
            foreach (string s in attrSplit)
            {
                if(!string.IsNullOrEmpty(s))
                    attrList.Add(int.Parse(s));
            }
        }
        return attrList;
    }
    /// <summary>
    /// 返回装备强化属性加成
    /// </summary>
    /// <param name="vo"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetEquipStrenType(this SysEquipVo vo,int type)
    {
        if (type == 1)
            return string.Empty + vo.str;
        else if (type == 2)
            return string.Empty + vo.agi;
        else if (type == 3)
            return string.Empty + vo.phy;
        else if (type == 4)
            return string.Empty + vo.wit;
        else if (type == 5)
            return string.Empty + vo.hp;
        else if (type == 6)
            return string.Empty + vo.mp;
        else if (type == 7)
            return string.Empty + vo.att_p_min;
        else if (type == 8)
            return string.Empty + vo.att_p_max;
        else if (type == 9)
            return string.Empty + vo.att_m_min;
        else if (type == 10)
            return string.Empty + vo.att_m_max;
        else if (type == 11)
            return string.Empty + vo.def_p;
        else if (type == 12)
            return string.Empty + vo.def_m;
        else if (type == 13)
            return string.Empty + vo.hit;
        else if (type == 14)
            return string.Empty + vo.dodge;
        else if (type == 15)
            return string.Empty + vo.crit;
        else if (type == 16)
            return string.Empty + vo.crit_ratio;
        else if (type == 17)
            return string.Empty + vo.flex;
        else if (type == 18)
            return string.Empty + vo.hurt_re;
        else if (type == 19)
            return string.Empty + vo.speed;
        else if (type == 20)
            return string.Empty + vo.luck;
        return null;

    }


    

    
    #endregion

    #region LanguageUtils
    /// <summary>
    /// 语言包 将 "\n"解析为 换行
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetWordWithLine(string key)
    {
        return  LanguageManager.GetWord(key).Replace(@"\n", "\n");
    }
    public static string GetRoleAttr(int type)
    {
        return LanguageManager.GetWord("Role.Attr" + type);
    }
    //{0}:{1}
    public static string RoleAttrFormat1(int type, string value)
    {
        return string.Format(LanguageManager.GetWord("Language.Format1"), GetRoleAttr(type), value);
    }
    //Lanaguage.Format2:{0}：{1}  ( {2} )
    public static string RoleAttrFormat2(int type, string value,string addValue)
    {
        return string.Format(LanguageManager.GetWord("Language.Format2"), GetRoleAttr(type),   value,addValue);
    }

    public static string RoleAttrFormat3(int type, string value, string addValue1, string addValue2)
    {
        return string.Format(LanguageManager.GetWord("Language.Format3"), GetRoleAttr(type), value, addValue1,addValue2);
    }
    public static string RoleAttrFormat4(int type, string value, string addValue1, string addValue2)
    {
        return string.Format(LanguageManager.GetWord("Language.Format4"), GetRoleAttr(type), value, addValue1, addValue2);
    }
    /// <summary>
    /// 语言包 format 
    /// </summary>
    /// <param name="language">语言包</param>
    /// <param name="value">填充参数</param>
    /// <returns></returns>
    public static string LanguagerFormat(string language, params object[] value)
    {
        string key = LanguageManager.GetWord(language);
        if (string.IsNullOrEmpty(key))
        {
            Log.info("VoUtils","没有找到语言包： " + language);
            return null;
        }
        return string.Format(key, value);
    }
    /// <summary>
    /// 装备镶嵌宝石类别提示
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetInlayTips(int type)
    {
        return string.Format(LanguageManager.GetWord("Smelt.InlayTips"),string.Format( ColorConst.YELLOW_FORMAT,LanguageManager.GetWord("Smelt.Type" + type)));
    }
    /// <summary>
    /// 宝石充灵类型提示
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetMergeTips(int type)
    {
        return string.Format(LanguageManager.GetWord("Smelt.MergeTips"), string.Format(ColorConst.YELLOW_FORMAT, LanguageManager.GetWord("Smelt.Type" + type)));
    }


   
    
    
    /// <summary>
    /// 装备附加属性默认字符串 ： 红色品质开启
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>

    public static string GetColorTips(int type)
    {
        string key = "Equip.ColorTips" + type;
        return LanguageManager.GetWord(key);
    }
    
    #endregion

}
