using System;
using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Chat;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.utils;
using PCustomDataType;
using UnityEngine;

namespace Com.Game.Module.Equip
{
    public class EquipLeftView : BaseView<EquipLeftView>
    {

        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }
        public delegate void ClickCallBack(uint uid);

        public ClickCallBack mItemCallBack;

        public Action NoEquipHandle;

        public int Repos = 0;
        private int pos = 0;//穿戴位置
        public uint Id;//当前选择装备唯一Id

        public bool IsOpenByBag = false;//是否从背包打开

        private GameObject tips;

        private ItemContainer currentIc;//当前选中的
        public ClickCallBack ItemClickCallBack
        {
            get { return mItemCallBack; }
            set
            {
                if (value != null && value != mItemCallBack)
                {
                    mItemCallBack = value;
                    InitItemClick();
                }
            }
        }

        private GameObject item;
        private List<ItemContainer> itemList = new List<ItemContainer>();
        private UIGrid grid;
        private UIScrollView scrollView;

        private Button lastButton;
        protected override void Init()
        {
            tips = FindChild("tips");
            ItemContainer ic;
            item = FindChild("content/grid/item1");
            item.name = "000";
            item.SetActive(false);
            ic = item.AddMissingComponent<ItemContainer>();
            ic.buttonType = Button.ButtonType.None;
            ic.onClick = ItemOnClick;
            itemList.Add(ic);

            grid = FindInChild<UIGrid>("content/grid");
            scrollView = FindInChild<UIScrollView>("content");
        }


        protected override void HandleBeforeCloseView()
        {
            IsOpenByBag = false;
        }
        private void InitItemClick()
        {
            foreach (ItemContainer ic in itemList)
            {
                ic.onClick = ItemOnClick;
            }
        }
		private void ItemOnClick(GameObject go)
		{
			ItemContainer ic = go.AddMissingComponent<ItemContainer>();
			if (mItemCallBack != null )
			{
			    if (ic.Id != 0)
			    {
                    this.Id = ic.Id;
                    if(!EquipDestroyView.Instance.gameObject.activeInHierarchy&& currentIc && currentIc != ic)
                        currentIc.SetHighLightState(false);
                    ic.SetHighLightState(true);
			        currentIc = ic;
			        mItemCallBack(ic.Id);
			        
			    }
			}
		}

        public void CancelCurrentSelect()
        {
            if (currentIc != null)
            {
                currentIc.SetHighLightState(false);
                currentIc = null;
            }
            this.Id = 0;
        }

        private bool isDataUpdate = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repos"></param>
        /// <param name="pos">备用参数，只提供给继承用</param>
        public void ReposChange(int repos,int pos = 0)
        {
            //CancelCurrentSelect();
            //if (repos != Repos || this.pos != pos)
            //{
                Repos = repos;
                this.pos = pos;
                InitItemData();
               
                
            //销毁和分解界面不默认选中
            if (EquipInheritView.Instance.gameObject.activeInHierarchy ||
                EquipDestroyView.Instance.gameObject.activeInHierarchy)
            {
                scrollView.ResetPosition();
                return;
            }
            bool isNoEquip = true;
            if(!isDataUpdate )
                scrollView.ResetPosition();
			if ((IsOpenByBag || isDataUpdate ) && GoodsMode.Instance.GetReposById(Id) == repos) //背包中打开，或数据更新，已经选中
            {
                //if (GoodsMode.Instance.GetReposById(Id) == repos) //已经选中
                //{
	            foreach (ItemContainer ic in itemList)
	            {
	                if (ic.Id == this.Id)
	                {
	                    //上一个ItemContainter不是当前

	                    if (mItemCallBack != null)
	                        mItemCallBack(Id);
	                    if (currentIc && currentIc != ic)
	                        currentIc.SetHighLightState(false);
	                    ic.SetHighLightState(true);
	                    currentIc = ic;
	                    isNoEquip = false;
	                    break;

	                }
	            }
                //}
                IsOpenByBag = false;
                isDataUpdate = false;
            }
            else   //默认选中第一个
            {
                foreach (ItemContainer ic in itemList)
                {
                    if (ic.Id != 0)
                    {
                        if (mItemCallBack != null)
                        {
                            this.Id = ic.Id;
                            //上一个ItemContainter不是当前
                            if (currentIc && currentIc != ic)
                                currentIc.SetHighLightState(false);
                            mItemCallBack(this.Id);
                            ic.SetHighLightState(true);
                            currentIc = ic;
                            isNoEquip = false;
                            break;
                        }
                    }
                }
            }
            if (isNoEquip == true && NoEquipHandle != null)
            {
                NoEquipHandle();
            }
        }

        public void SetItemActive(uint uid, bool state)
        {
            foreach (ItemContainer container in itemList)
            {
                if (container.Id == uid)
                {
                    container.SetHighLightState(state);
                    break;
                }
            }
        }
        public override void RegisterUpdateHandler()
        {
            Singleton<GoodsMode>.Instance.dataUpdated += GoodsUpdateHandle;
        }
        public override void CancelUpdateHandler()
        {
            Singleton<GoodsMode>.Instance.dataUpdated -= GoodsUpdateHandle;
        }

        private void GoodsUpdateHandle(object sender, int code)
        {
            if (code == Singleton<GoodsMode>.Instance.UPDATE_GOODS
                || code == Singleton<GoodsMode>.Instance.UPDATE_EQUIP)
            {
                int repos = this.Repos;
                this.Repos = 0;
                int tempPos = pos;
                this.pos = -1;
                this.isDataUpdate = true;
                ReposChange(repos, tempPos);
            }

        }
        public void InitItemData()
        {
            List<PGoods> goodsList = null;
            SysEquipVo vo;
            
            if (Repos == GoodsMode.Instance.GOODS_REPOS)
            {
                if(pos == 0)
                    goodsList = Singleton<GoodsMode>.Instance.GetPGoodsByType(GoodsMode.GoodsType.Equip);
                else
                {
                    goodsList = GoodsMode.Instance.GetEquipByPosInBag(this.pos);
                    if (goodsList.Count == 0)
                    {
                        if (grid.gameObject.activeInHierarchy)
                            grid.SetActive(false);
                        if (!tips.gameObject.activeInHierarchy)
                            tips.SetActive(true);
                        return;
                    }
                }

            }
            else if (Repos == GoodsMode.Instance.EQUIP_REPOS)
            {
                goodsList = Singleton<GoodsMode>.Instance.equipList;
            }
            if (!grid.gameObject.activeInHierarchy)
            {
                grid.SetActive(true); 
            }
            if (tips.gameObject.activeInHierarchy)
                tips.SetActive(false);
            int count = goodsList.Count;
            if (count < 10)
                count = 10;
            ItemContainer ic;
            while (itemList.Count< count)
            {
                ic = NGUITools.AddChild(grid.gameObject, item).AddMissingComponent<ItemContainer>();
                ic.gameObject.name = string.Format("{0:D3}", itemList.Count);
                ic.onClick = ItemOnClick;
                ic.buttonType = Button.ButtonType.None;
                itemList.Add(ic);
            }
            grid.Reposition();
            int index = 0;
            //默认值初始化
            foreach (ItemContainer container in itemList)
            {
                if (index >= count)
                    container.SetActive(false);
                else
                {
                    Singleton<ItemManager>.Instance.InitItem(container, 1, ItemType.Equip); //空白
                    container.FindInChild<UILabel>("stren").text = string.Empty;
                    container.FindInChild<UILabel>("name").text = string.Empty;
                    container.FindInChild<UISprite>("highlight").gameObject.SetActive(false);
                    
                    for (int i = 1; i < 6; i++)
                    {
                        ItemManager.Instance.InitItem(container.FindChild("bs" + i), 1, 0);
                    }
                    if(EquipInheritView.Instance.gameObject.activeInHierarchy && this.Repos == GoodsMode.Instance.GOODS_REPOS)
                        container.FindInChild<UILabel>("pos").text = LanguageManager.GetWord("Equip.Pos" +this.pos);
                    else if (this.Repos == GoodsMode.Instance.EQUIP_REPOS)
                        container.FindInChild<UILabel>("pos").text = LanguageManager.GetWord("Equip.Pos" + (index + 1));
                    else
                    {
                        container.FindInChild<UILabel>("pos").text = string.Empty;
                    }
                    container.Id = 0;
                    container.SetActive(true);
                    index++;
                }
                
            }
            index = 0;
            List<PGemInHole> gemList;
            foreach (PGoods goods in goodsList)
            {
                vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
                if (Repos == 2)
                    ic = itemList[vo.pos - 1];
                else
                    ic = itemList[index];
				ic.Id = goods.id;
                ic.FindInChild<UILabel>("pos").text = string.Empty;
                ic.enabled = true;
                Singleton<ItemManager>.Instance.InitItem(ic, goods.goodsId, ItemType.Equip);
                ic.FindInChild<UILabel>("stren").text = "+" + goods.equip[0].stren;
                UILabel label = ic.FindInChild<UILabel>("name");
                label.text = vo.name;
                ColorUtils.SetEqNameColor(label,vo.color);          
                
                gemList = goods.equip[0].gemList;
                uint gemId; //宝石
                SysItemVo itemVo;
                for (int i = 0; i < 5; i++)
                {
                    if (i < gemList.Count)
                    {
                        gemId = gemList[i].gemId;
                    }
                    else
                    {
                        gemId = 1;
                    }
                    ItemManager.Instance.InitItem(ic.FindChild("bs" + (i+1)), gemId, 0);
                    
                }
                index ++;

            }
            vo = null;
        }
        

    }

}
