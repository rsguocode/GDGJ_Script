using com.game.Public.Message;
using com.u3d.bases.debug;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using PCustomDataType;
using com.game.manager;
using com.game.consts;
using com.game.data;
using Com.Game.Module.Role;

namespace Com.Game.Module.Manager
{
    //皮质颜色：1为白色；2为绿色；3为蓝色；4为橙色；5为紫色；6为红色；
	// goodsId : 0 为锁， 1为空白，其他为具体的图标
	public enum ItemType
	{
		BaseGoods = 0,//background, icon ，普通物品 其中background 是品质框
		Equip//background , ,icon , 装备 其中 background 是品质框
	}
    //***********************************************
    //注意 注意 新版的 分为 background icon highlight
    //***********************************************
    
	/// <summary>
	/// 只负责初始化Icon，对于诸如装备强化等级和物品数量的信息没有处理
	/// </summary>
	public class ItemManager :Singleton<ItemManager>
	{
        /// <summary>
        /// 特殊图标类型
        /// </summary>
	    public readonly uint LOCK_ICON = 0;//锁
	    public readonly uint EMPTY_ICON = 1;//空白
	    public readonly uint ADD_ICON = 2;//加号
        
        /// <summary>
        /// 确保goodsId是存在的，0表示锁的icon ,1表示空白
        /// </summary>
        /// <param name="item"></param>
        /// <param name="goodsId"></param>
        /// <param name="type"></param>
        public void InitItemByUId(GameObject item, uint uid, ItemType type)
        {
            int color = 1;  //默认为 1 
            string spriteName = string.Empty;
            UIAtlas atlas = null;
            PGoods goods = GoodsMode.Instance.GetPGoodsById(uid);
            if (goods == null)
            {
                Log.info(this,"背包没有找到改唯一id的物品");
                return;
            }
            uint goodsId = goods.goodsId;
            if (goodsId > 100000)  //物品道具
            {
                SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);
                color = vo.color;
                spriteName = string.Empty + vo.icon;
				//atlas = Singleton<AtlasManager>.Instance.GetAtlas("SmeltIcon");   //后续可以根据物品类型Id来分流
                //string spriteName = string.Empty + vo.icon;
                //if (atlas.GetSprite(spriteName) == null)
                {
					if(vo.type == GoodsConst.SMELT_GOODS)
					{
						atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
					}else
					{
						atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
					}
                    if (atlas == null || atlas.GetSprite(spriteName) == null)  //没有图片 变成 设置为 问号图片
                    {
                        spriteName = "100000";
                    }
                }
                item.SetChildActive("color", false);
                InitSpriteHelp(item, "icon", atlas, spriteName);
                InitSpriteHelp(item, "background", "common", "pz_" + color);
            }
            else  //装备
            {
                SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
                color = vo.color;
                spriteName = string.Empty + vo.icon;
                atlas = Singleton<AtlasManager>.Instance.GetAtlas("EquipIcon");
                if (atlas.GetSprite(spriteName) == null)  //没有图片 变成 设置为 问号图片
                    InitSpriteHelp(item, "icon", "PropIcon", "100000");
                else
                    InitSpriteHelp(item, "icon", "EquipIcon", spriteName);
                InitSpriteHelp(item, "background", "EquipIcon", "pz_" + color);
                item.SetChildActive("color", false);
            }



        }

        /// <summary>
        /// 确保goodsId是存在的，0表示锁的icon ,1表示空白
        /// </summary>
        /// <param name="item"></param>
        /// <param name="goodsId"></param>
        /// <param name="type"></param>
        public void InitItemByUId(UIWidgetContainer item, uint uid, ItemType type)
        {
            UISprite sprite;
            int color = 1;   //默认为 空白 1 
            string spriteName = string.Empty;
            UIAtlas atlas = null;
            PGoods goods = GoodsMode.Instance.GetPGoodsById(uid);
            if (goods == null)
            {
                Log.info(this, "背包没有找到改唯一id的物品");
                return;
            }
            uint goodsId = goods.goodsId; 
            if (goodsId > 100000)  //物品道具
            {
                SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);
                color = vo.color;
                spriteName = string.Empty + vo.icon;
                if (vo.type == GoodsConst.SMELT_GOODS)
                {
                    atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
                }
                else
                {
                    atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
                }
                if (atlas == null || atlas.GetSprite(spriteName) == null)
                {
                    spriteName = "100000";
                }
                
                item.gameObject.SetChildActive("color", false);
                InitSpriteHelp(item, "icon", atlas, spriteName);
                InitSpriteHelp(item, "background", "common", "pz_" + color);
            }
            else //装备
            {
                SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
                color = vo.color;
                spriteName = string.Empty + vo.icon;
                atlas = Singleton<AtlasManager>.Instance.GetAtlas("EquipIcon");
                if (atlas.GetSprite(spriteName) == null)
                    InitSpriteHelp(item, "icon", "PropIcon", "100000");
                else
                    InitSpriteHelp(item, "icon", "EquipIcon", spriteName);
                InitSpriteHelp(item, "background", "EquipIcon", "pz_" + color);
                item.gameObject.SetChildActive("color", false);
            }
        }
        
        /// <summary>
		/// 确保goodsId是存在的，0表示锁的icon ,1表示空白
		/// </summary>
		/// <param name="item"></param>
		/// <param name="goodsId"></param>
		/// <param name="type"></param>
		public void InitItem(GameObject item,uint goodsId,ItemType type)
		{
			int color = 1;  //默认为 1 
			string spriteName = string.Empty;
			UIAtlas atlas = null;

            if (goodsId == this.LOCK_ICON)    //icon 锁
            {
                InitSpriteHelp(item, "background", "common", "pz_1");
                InitSpriteHelp(item, "icon", "common", "suo23");
            }
            else if (goodsId == this.EMPTY_ICON) //空格子
            {
                InitSpriteHelp(item, "background", "common", "pz_1");
                InitSpriteHelp(item, "icon", "common", "pzkong");
            }
            else if (goodsId == this.ADD_ICON)
            {
                InitSpriteHelp(item, "background", "common", "pz_1");
                InitSpriteHelp(item, "icon", "common", "add");
            }
			else if (goodsId > 100000)  //物品道具
			{
				SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);
				color = vo.color;
				spriteName = string.Empty +vo.icon;
				if(vo.type == GoodsConst.SMELT_GOODS)
				{
					atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
				}
                else
				{
					atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
				}
                if (atlas == null || atlas.GetSprite(spriteName) == null)  //没有图片 变成 设置为 问号图片
				{
                    spriteName = "100000";
				}
                item.SetChildActive("color", false);
				InitSpriteHelp(item, "icon", atlas, spriteName);
                InitSpriteHelp(item, "background", "common", "pz_" + color);
					
			}
			else  //装备
			{
				SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
				color = vo.color;
				spriteName = string.Empty + vo.icon;
				atlas = Singleton<AtlasManager>.Instance.GetAtlas("EquipIcon");
                if(atlas.GetSprite(spriteName) == null)  //没有图片 变成 设置为 问号图片
                    InitSpriteHelp(item, "icon", "PropIcon", "100000");
                else
                    InitSpriteHelp(item, "icon", "EquipIcon", spriteName);
                InitSpriteHelp(item, "background", "EquipIcon", "pz_" + color);
                item.SetChildActive("color", false);
			}
			
			
			
		}

		/// <summary>
        /// 确保goodsId是存在的，0表示锁的icon ,1表示空白
        /// </summary>
        /// <param name="item"></param>
        /// <param name="goodsId"></param>
        /// <param name="type"></param>
        public void InitItem(UIWidgetContainer item,uint goodsId, ItemType type)  
        {
			UISprite sprite ;
			int color = 1;   //默认为 空白 1 
			string spriteName = string.Empty;
			UIAtlas atlas = null;
			//背景
			
			//高光
			//InitSpriteHelp(item, "gaoguang", "common", "gaoguang");
			InitSpriteHelp(item, "border", string.Empty, string.Empty);
			if (goodsId == this.LOCK_ICON)    //icon 锁
			{
                InitSpriteHelp(item, "background", "common", "pz_1");
				InitSpriteHelp(item, "icon", "common", "suo23" );
			}
			else if (goodsId == this.EMPTY_ICON) //空格子
			{
                InitSpriteHelp(item, "background", "common", "pz_1");
                InitSpriteHelp(item, "icon", "common", "pzkong");
			}
            else if (goodsId == this.ADD_ICON)
            {
                InitSpriteHelp(item, "background", "common", "pz_1");
                InitSpriteHelp(item, "icon", "common", "add");
            }
          
	        else if (goodsId > 100000)  //物品道具
	        {
	            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);
	            color = vo.color;
	            spriteName = string.Empty +vo.icon;
                if (vo.type == GoodsConst.SMELT_GOODS)
                {
                    atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
                }
                else
                {
                    atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
                }
                if (atlas == null || atlas.GetSprite(spriteName) == null)
                {
                    spriteName = "100000";
                }
                item.gameObject.SetChildActive("color",false);
	            InitSpriteHelp(item, "icon", atlas, spriteName);
                InitSpriteHelp(item, "background", "common", "pz_" + color);
	        }
			else //装备
	        {
	            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
	            color = vo.color;
	            spriteName = string.Empty + vo.icon;
	            atlas = Singleton<AtlasManager>.Instance.GetAtlas("EquipIcon");
                if(atlas.GetSprite(spriteName) == null)
                    InitSpriteHelp(item, "icon", "PropIcon", "100000");
                else
	                InitSpriteHelp(item, "icon", "EquipIcon", spriteName);
                InitSpriteHelp(item, "background", "EquipIcon", "pz_" + color);
                item.gameObject.SetChildActive("color", false);
	        }
	        

        }

	   
	    private void InitSpriteHelp(UIWidgetContainer item, string path, UIAtlas atlas, string spriteName)
	    {
            UISprite sprite = item.FindInChild<UISprite>(path);
            if(string.IsNullOrEmpty((spriteName)))
	            atlas = null;
            if (sprite != null)
            {
                sprite.atlas = atlas;
                sprite.spriteName = spriteName;
            }
	    }

	    private void InitSpriteHelp(UIWidgetContainer item, string path, string atlas, string spriteName)
	    {
            UISprite sprite = item.FindInChild<UISprite>(path);
            if (string.IsNullOrEmpty((spriteName)))
                atlas = null;
	        if (sprite != null)
	        {
	            sprite.atlas = Singleton<AtlasManager>.Instance.GetAtlas(atlas);
	            sprite.spriteName = spriteName;
	        }
	    }

		private void InitSpriteHelp(GameObject item,string path, string atlas, string spriteName)
		{
		    
			UISprite sprite = NGUITools.FindInChild<UISprite>(item,path);
			if (string.IsNullOrEmpty((spriteName)))
				atlas = null;
			if (sprite != null)
			{
				sprite.atlas = Singleton<AtlasManager>.Instance.GetAtlas(atlas);
				sprite.spriteName = spriteName;
			}
		}
		private void InitSpriteHelp(GameObject item,string path, UIAtlas atlas, string spriteName)
		{
            
			UISprite sprite = NGUITools.FindInChild<UISprite>(item,path);
			if (string.IsNullOrEmpty((spriteName)))
				atlas = null;
			if (sprite != null)
			{
				sprite.atlas = atlas;
				sprite.spriteName = spriteName;
			}
		}
	}
}
