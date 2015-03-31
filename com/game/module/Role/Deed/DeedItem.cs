﻿﻿﻿using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using com.game.module.test;
using com.game.Public.Confirm;
using com.game.utils;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/31 04:00:00 
 * function: 契约的Item
 * *******************************************************/

namespace Com.Game.Module.Role.Deed
{
    public class DeedItem:MonoBehaviour
    {
        private Button _btnDeed;         //契约按钮
        private Button _btnSelect;       //契约选中按钮
        private UISprite _icon;          //契约Icon
        private UISprite _selectedIcon;  //被选中状态
        private uint _id;                //契约Id
        public int Value;                //契约值
        private SysDeedVo _deedVo;       //缓存Vo信息，减少查表次数
        private DeedMode _deedMode;      //数据中心

        public void Init()
        {
            _btnDeed = NGUITools.FindInChild<Button>(gameObject, "Button");
            _btnSelect = NGUITools.FindInChild<Button>(gameObject, "Item");
            _icon = NGUITools.FindInChild<UISprite>(gameObject, "Item/Icon");
            _selectedIcon = NGUITools.FindInChild<UISprite>(gameObject, "SelectedIcon");
            _btnDeed.onClick += OnDeedClick;
            _btnSelect.onClick += OnSelected;
            CancleSelected();
            _deedMode = Singleton<DeedMode>.Instance;
            _deedMode.dataUpdated += DeedModeDataUpdate;
        }

        private void OnDeedClick(GameObject buttonItem)
        {
            var cost = _deedVo.money;
            if (Singleton<GoodsMode>.Instance.GetPGoodsByGoodsId((uint)_deedVo.item_id) == null)
            {
                var typeString = Singleton<DeedMode>.Instance.TypeStrings[_deedMode.CurrentDeedType-1];
                var costAlter = LanguageManager.GetWord("DeedItem.CostAlter", new[] { cost + "", "[00ff00]" + Value + "[-]" + typeString});
                ConfirmMgr.Instance.ShowCommonAlert(costAlter, ConfirmCommands.DeedCost, BuyDeed, LanguageManager.GetWord("ConfirmView.Ok"), null, LanguageManager.GetWord("ConfirmView.Cancel"));
            }
            else
            {
                Singleton<DeedMode>.Instance.RequestDeed(_id, 1);
            }
            Singleton<DeedMode>.Instance.CurrentSelectedDeedItem = null;
        }

        private void BuyDeed()
        {
            Singleton<DeedMode>.Instance.RequestDeed(_id, 2);
        }

        private void OnSelected(GameObject selectedItem)
        {
            Singleton<DeedMode>.Instance.CurrentSelectedDeedItem = this;
        }

        public void SetDeedInfo(SysDeedVo deedVo,int index,bool hasDeed)
        {
            _deedVo = deedVo;
            _id = deedVo.unikey;
            Value = int.Parse(StringUtils.GetValueListFromString(deedVo.val)[index]);
            if (!hasDeed)
            {
                _btnDeed.gameObject.SetActive(true);
                _icon.gameObject.SetActive(false);
            }
            else
            {
                _btnDeed.gameObject.SetActive(false);
                _icon.gameObject.SetActive(true);
                var atlas = Singleton<AtlasManager>.Instance.GetAtlas("SmeltIcon");
                var itemId = BaseDataMgr.instance.GetSysDeedVo(_id).item_id;
                var vo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)itemId);
                var spriteName = string.Empty + vo.icon;
                _icon.atlas = atlas;
                _icon.spriteName = spriteName;
            }
        }

        public void Selected()
        {
            _selectedIcon.gameObject.SetActive(true);
        }

        public void CancleSelected()
        {
            _selectedIcon.gameObject.SetActive(false);
        }

        private void DeedModeDataUpdate(object sender, int code)
        {
            switch (code)
            {
                case DeedMode.EventUpdateSelectedDeedItem:
                    if (Singleton<DeedMode>.Instance.CurrentSelectedDeedItem == this)
                    {
                        Selected();
                    }
                    else
                    {
                        CancleSelected();
                    }
                    break;
            }
        }
    }
}