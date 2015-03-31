using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Role;
using PCustomDataType;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;

namespace Com.Game.Module.Tips
{
	public enum TipsRepos
	{
		GOODS,//背包
		EQUIP,//装备栏

	}
	public enum IdType : byte
	{
		GOODSID,//物品id
		UNIQUEID,//唯一id,从GoodsMods中获取
	}

    public enum TipsType
    {
        DEFAULT_TYPE = 0,//默认样式
        DELEGATECLOSE =1,//按钮回调之后关闭
        DELEGATENOCLOSE = 2//按钮回调之后不关闭
    }
    /// <summary>
    /// 只有装备才有玩家信息，没有玩家信息的tips完全可以走goodsId的接口
    /// </summary>
	public class TipsManager : Singleton<TipsManager>
	{
        public void CloseAllTips()
        {
            GoodsTips.Instance.CloseView();
            EquipTips.Instance.CloseView();
            PetEquipTips.Instance.CloseView();
        }
        /// <summary>
        /// 关闭tips统一接口
        /// </summary>
        /// <param name="id"></param>
        public void CloseTipsById(uint id)
        {
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsById(id);
            if (goods.goodsId > GoodsMode.Instance.GoodsId)
            {
                GoodsTips.Instance.CloseView();
            }
            else
            {
                SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
                if (vo == null)
                {
                    Log.info(this, "没有找到Vo");
                    return;
                }
                else if (vo.type == GoodsConst.PET_EQUIP)
                {
                    PetEquipTips.Instance.CloseView();
                }
                else if (vo.type == GoodsConst.ROLE_EQUIP)
                    EquipTips.Instance.CloseView();
            }
        }
        /// <summary>
        /// 关闭tips统一接口
        /// </summary>
        /// <param name="goodsId"></param>
        public void CloseTipsByGoodsId(uint goodsId)
        {
            if (goodsId > GoodsMode.Instance.GoodsId)
            {
                GoodsTips.Instance.CloseView();
            }
            else
            {
                SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
                if (vo == null)
                {
                    Log.info(this, "没有找到Vo");
                    return;
                }
                else if (vo.type == GoodsConst.PET_EQUIP)
                {
                    PetEquipTips.Instance.CloseView();
                }
                else if (vo.type == GoodsConst.ROLE_EQUIP)
                    EquipTips.Instance.CloseView();
            }
        }
        /// <summary>
        /// 玩家的物品Tips
        /// </summary>
        /// <param name="id">唯一Id</param>
        /// <param name="leftClick">按钮响应函数，如果为null 则隐藏</param>
        /// <param name="rightClick">按钮响应函数，如果为null 则隐藏</param>
        /// <param name="leftText">按钮文本</param>
        /// <param name="rightText">按钮文本</param>
        /// <param name="tipsType">Tips的具体样式</param>
	    public void OpenTipsById(uint id, UIWidgetContainer.ClickDelegate leftClick,
	        UIWidgetContainer.ClickDelegate rightClick,
	        string leftText, string rightText, TipsType tipsType = TipsType.DEFAULT_TYPE)
        {
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsById(id);
            if (goods.goodsId > GoodsMode.Instance.GoodsId)
            {
                Singleton<GoodsTips>.Instance.OpenViewById(id, leftText, rightText, leftClick, rightClick,tipsType);
            }
            else
            {
                SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
                if (vo == null)
                {
                    Log.info(this,"没有找到Vo");
                    return;
                }
                else if (vo.type == GoodsConst.PET_EQUIP)
                {
                    Singleton<PetEquipTips>.Instance.OpenViewById(id, leftText, rightText, leftClick, rightClick);
                }
                else if(vo.type == GoodsConst.ROLE_EQUIP)
                    Singleton<EquipTips>.Instance.OpenViewById(id, leftText, rightText, leftClick, rightClick, null,tipsType);
            }
	    }

		public void OpenPlayerEquipTipsById(uint id, UIWidgetContainer.ClickDelegate leftClick,
		                         UIWidgetContainer.ClickDelegate rightClick,
		                         UIWidgetContainer.ClickDelegate closeClick,
		                         string leftText, string rightText, TipsType tipsType = TipsType.DEFAULT_TYPE)
		{
			PGoods goods = Singleton<GoodsMode>.Instance.GetOtherPGoods(id);

			SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
			if (vo == null)
			{
				Log.info(this,"没有找到Vo");
				return;
			}
			else if (vo.type == GoodsConst.PET_EQUIP)
			{
				Singleton<PetEquipTips>.Instance.OpenPlayerEquipViewById(id, leftText, rightText, leftClick, rightClick, closeClick);
			}
			else if(vo.type == GoodsConst.ROLE_EQUIP)
			{
				Singleton<EquipTips>.Instance.OpenPlayerEquipViewById(id, leftText, rightText, leftClick, rightClick, closeClick,tipsType);
			}
		}

        public void OpenPlayerEquipTipsByGoods(	PGoods goods )
        {
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            if (vo == null)
            {
                Log.info(this, "没有找到Vo");
                return;
            }
            else if (vo.type == GoodsConst.ROLE_EQUIP)
            {
                Singleton<EquipTips>.Instance.OpenPlayerEquipViewByPGoods(goods, "", "", null, null, null);
            }
        }

        /// <summary>
        /// 不在背包的物品，如商店等
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="leftClick">按钮响应函数，如果为null 则隐藏</param>
        /// <param name="rightClick">按钮响应函数，如果为null 则隐藏</param>
        /// <param name="leftText">按钮文本</param>
        /// <param name="rightText">按钮文本</param>
        /// <param name="tipsType">Tips的具体样式</param>
        public void OpenTipsByGoodsId(uint goodsId, UIWidgetContainer.ClickDelegate leftClick,
            UIWidgetContainer.ClickDelegate rightClick,
            string leftText, string rightText, TipsType tipsType = TipsType.DEFAULT_TYPE)
        {
            if (goodsId > GoodsMode.Instance.GoodsId)
            {
                Singleton<GoodsTips>.Instance.OpenViewByGoodsId(goodsId, leftText, rightText, leftClick, rightClick,tipsType);
            }
            else
            {
                SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
                if (vo == null)
                {
                    Log.info(this,"没有找到Vo");
                    return;
                }
                else if (vo.type == GoodsConst.PET_EQUIP)
                {
                    Singleton<PetEquipTips>.Instance.OpenViewByGoodsId(goodsId, leftText, rightText, leftClick, rightClick);
                }
                else if(vo.type == GoodsConst.ROLE_EQUIP)
                    Singleton<EquipTips>.Instance.OpenViewByGoodsId(goodsId, leftText, rightText, leftClick, rightClick,null,tipsType);
            }
        }
        

       
	}
}
