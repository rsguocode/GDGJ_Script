//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：EquipIdentificationView
//文件描述：装备图鉴
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using Com.Game.Module.Role;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.consts;
using com.game.data;
using com.game.module.EquipIdentification;
using Com.Game.Module.Manager;
using com.game.module.test;
using Com.Game.Module.Tips;
using com.game.manager;
using com.game.Public.Message;
using com.game.utils;
using com.game.vo;


using com.u3d.bases.debug;

namespace com.game.module.EquipIdentification
{
    public class EquipIdentificationView : BaseView<EquipIdentificationView>
    {
        public override string url { get { return "UI/EquipIdentification/EquipIdentificationView.assetbundle"; } }
        public override ViewLayer layerType { get { return ViewLayer.HighLayer; } }
        public override bool IsFullUI { get { return true; } }
		public override bool isDestroy { get { return true; } }

		public override ViewType viewType
		{
			get { return ViewType.CityView; }
		}

        private Button btn_close;
        private Button previousPageBtn;
        private Button nextBtnPageBtn;

        private UILabel allEquipLabel;
        private UILabel allBtnLabel;
        private UILabel weaponBtnLabel;
        private UILabel helmetBtnLabel;
        private UILabel cuirassBtnLabel;
        private UILabel trousersBtnLabel;
        private UILabel shoeBtnLabel;
        private UILabel recommendBtnLabel;
        private UILabel necklaceBtnLabel;
		private UILabel beltBtnLabel;
        private UILabel ringBtnLabel;
        private UILabel badgeBtnLabel;
        private UILabel jujuBtnLabel;
        private UILabel pageNumberLabel;

        private UIToggle allBtn;//全部
        private UIToggle weaponBtn;//武器
        private UIToggle helmetBtn;//头盔
        private UIToggle cuirassBtn;//衣服
        private UIToggle trousersBtn;//护腿
        private UIToggle shoeBtn;//靴子

        private UIToggle recommendBtn;//推荐
        private UIToggle necklaceBtn;//护肩
        private UIToggle beltBtn;//腰带
        private UIToggle ringBtn;//戒指
        private UIToggle badgeBtn;//护腕
        private UIToggle jujuBtn;//护符

        private List<SysEquipVo> allEquipList;//全部
        private List<SysEquipVo> filterEquipList;//过滤后物品列表
		private List<SysEquipVo> recommenEquipList;//推荐物品列表

        private List<Transform> equipItemList;
        private int curentPageNum = 1;
        private int totalPageNum = 1;
		private bool isClickAllEquipBtn;
		private bool isClickRecommenBtn;
		private SysEquipVo currentClickEquipVo;

        protected override void Init()
        {
			initFilterAllEquipData ();
            initView();
			initEquipList ();
            initClick();
            initTextLanguage();
        }

        private void initView()
        {
            btn_close       = FindInChild<Button>("btn_close");
            previousPageBtn = FindInChild<Button>("arrow/btn_left");
            nextBtnPageBtn  = FindInChild<Button>("arrow/btn_right");

            allEquipLabel     = FindInChild<UILabel>("allEquipment");
            allBtnLabel       = FindInChild<UILabel>("categoryBtn/allBtn/label");
            weaponBtnLabel    = FindInChild<UILabel>("categoryBtn/weaponBtn/label");
			helmetBtnLabel    = FindInChild<UILabel>("categoryBtn/helmetBtn/label");
            cuirassBtnLabel   = FindInChild<UILabel>("categoryBtn/cuirassBtn/label");
            trousersBtnLabel  = FindInChild<UILabel>("categoryBtn/trousersBtn/label");
            shoeBtnLabel      = FindInChild<UILabel>("categoryBtn/shoeBtn/label");
            recommendBtnLabel = FindInChild<UILabel>("categoryBtn/recommendBtn/label");
            necklaceBtnLabel  = FindInChild<UILabel>("categoryBtn/necklaceBtn/label");
			beltBtnLabel      = FindInChild<UILabel>("categoryBtn/beltBtn/label");
            ringBtnLabel  = FindInChild<UILabel>("categoryBtn/ringBtn/label");
            badgeBtnLabel     = FindInChild<UILabel>("categoryBtn/badgeBtn/label");
            jujuBtnLabel    = FindInChild<UILabel>("categoryBtn/jujuBtn/label");
            pageNumberLabel   = FindInChild<UILabel>("pageNumber");

            allBtn      = FindInChild<UIToggle>("categoryBtn/allBtn");
            weaponBtn   = FindInChild<UIToggle>("categoryBtn/weaponBtn");
            helmetBtn   = FindInChild<UIToggle>("categoryBtn/helmetBtn");
            cuirassBtn   = FindInChild<UIToggle>("categoryBtn/cuirassBtn");
            trousersBtn = FindInChild<UIToggle>("categoryBtn/trousersBtn");
            shoeBtn     = FindInChild<UIToggle>("categoryBtn/shoeBtn");

            recommendBtn = FindInChild<UIToggle>("categoryBtn/recommendBtn");
            necklaceBtn  = FindInChild<UIToggle>("categoryBtn/necklaceBtn");
            beltBtn      = FindInChild<UIToggle>("categoryBtn/beltBtn");
            ringBtn  = FindInChild<UIToggle>("categoryBtn/ringBtn");
            badgeBtn     = FindInChild<UIToggle>("categoryBtn/badgeBtn");
            jujuBtn    = FindInChild<UIToggle>("categoryBtn/jujuBtn");     
        }

		private void initEquipList()
		{
			equipItemList = new List<Transform>();
			for (int i = 0; i < EquipIdentificationConst.EQUIPMAX_12; i++)
			{
				equipItemList.Add(FindInChild<Transform>("equipItemContainer/" + i));
				FindInChild<Button>("equipItemContainer/" + i).onClick = equipItemOnClick;
			}
		}
		
		private void initClick()
		{
			btn_close.onClick       = closeOnClick;
            previousPageBtn.onClick = previousePageOnClick;
            nextBtnPageBtn.onClick  = nextPageOnClick;

            EventDelegate.Add(allBtn.onChange, allBtnChange);
            EventDelegate.Add(weaponBtn.onChange, weaponBtnChange);
            EventDelegate.Add(helmetBtn.onChange, helmetBtnChange);
            EventDelegate.Add(cuirassBtn.onChange, clotheBtnChange);
            EventDelegate.Add(trousersBtn.onChange, trousersBtnChange);
            EventDelegate.Add(shoeBtn.onChange, shoeBtnChange);

            EventDelegate.Add(recommendBtn.onChange, recommendBtnChange);
            EventDelegate.Add(necklaceBtn.onChange, necklaceBtnChange);
            EventDelegate.Add(beltBtn.onChange, beltBtnChange);
            EventDelegate.Add(ringBtn.onChange, ringBtnChange);
            EventDelegate.Add(badgeBtn.onChange, badgeBtnChange);
            EventDelegate.Add(jujuBtn.onChange, jujuBtnChange);
        }

        private void initTextLanguage()
        {
            allEquipLabel.text     = LanguageManager.GetWord("EquipIdentificationView.title");
			allBtnLabel.text       = LanguageManager.GetWord("EquipIdentificationView.allBtn");//全部
			weaponBtnLabel.text    = LanguageManager.GetWord("EquipIdentificationView.weaponBtn");//武器
			helmetBtnLabel.text    = LanguageManager.GetWord("EquipIdentificationView.helmetBtn");//头盔
			cuirassBtnLabel.text   = LanguageManager.GetWord("EquipIdentificationView.clotheBtn");//胸甲
			trousersBtnLabel.text  = LanguageManager.GetWord("EquipIdentificationView.trousersBtn");//护腿
			shoeBtnLabel.text      = LanguageManager.GetWord("EquipIdentificationView.shoeBtn");//靴子
			recommendBtnLabel.text = LanguageManager.GetWord("EquipIdentificationView.recommendBtn");//推荐
			necklaceBtnLabel.text  = LanguageManager.GetWord("EquipIdentificationView.necklaceBtn");//护肩
			beltBtnLabel.text      = LanguageManager.GetWord("EquipIdentificationView.ringBtn");//腰带
			ringBtnLabel.text      = LanguageManager.GetWord("EquipIdentificationView.braceletBtn");//戒指
			badgeBtnLabel.text     = LanguageManager.GetWord("EquipIdentificationView.gloveBtn");//护腕
			jujuBtnLabel.text      = LanguageManager.GetWord("EquipIdentificationView.baggesBtn");//护符
        }

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			recommendBtn.value = true;
			setRecommendBtnClick();
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
			recommendBtn.value = isClickAllEquipBtn = isClickRecommenBtn = false;
		}

		public override void Destroy()
		{
			base.Destroy();
		}

		private void closeOnClick(GameObject go)
        {
            CloseView();
        }

		private void initFilterAllEquipData()
		{
			allEquipList = new List<SysEquipVo>();
			allEquipList = EquipIndentificationUtil.getFilterAllEquipData ();
			allEquipList.Sort(EquipIndentificationUtil.setSortCompareList);
		}

        //全部
        private void allBtnChange()
        {
			if (UIToggle.current.Equals(allBtn) && UIToggle.current.value)
			{
				isClickAllEquipBtn = true;
				isClickRecommenBtn = false;
				setCurrentPageNum(allEquipList);
				setCurrentPageData(allEquipList, curentPageNum, totalPageNum);
			}
        }

        //武器
        private void weaponBtnChange()
        {
            if (UIToggle.current.Equals(weaponBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_WEAPON);
            }
        }

        //头盔
        private void helmetBtnChange()
        {
            if (UIToggle.current.Equals(helmetBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_HELMET);
            }
        }

        //胸甲
        private void clotheBtnChange()
        {
            if (UIToggle.current.Equals(cuirassBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_CLOTHE);
            }
        }

        //护腿
        private void trousersBtnChange()
        {
            if (UIToggle.current.Equals(trousersBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_TROUSERS);
            }
        }

        //靴子
        private void shoeBtnChange()
        {
            if (UIToggle.current.Equals(shoeBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_SHOE);
            }
        }

        //推荐
        private void recommendBtnChange()
        {
            //红色、橙色不推荐
			if (UIToggle.current.Equals(recommendBtn) && UIToggle.current.value)
			{
				setRecommendBtnClick();
			}
        }

		private void setRecommendBtnClick()
		{
			isClickAllEquipBtn = false;
			isClickRecommenBtn = true;
			recommenEquipList = EquipIndentificationUtil.getRecommendEquipData(allEquipList);
			setCurrentPageNum (recommenEquipList);
			setCurrentPageData (recommenEquipList, curentPageNum, totalPageNum);
		}

        //护肩
        private void necklaceBtnChange()
        {
            if (UIToggle.current.Equals(necklaceBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_NECKLACE);
            }
		}

        //腰带
        private void beltBtnChange()
        {
            if (UIToggle.current.Equals(beltBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_RING);
            }
        }

        //戒指
        private void ringBtnChange()
        {
            if (UIToggle.current.Equals(ringBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_BRACELET);
            }
        }

        //护腕
        private void badgeBtnChange()
        {
            if (UIToggle.current.Equals(badgeBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_GLOVE);
            }
        }

        //护符
        private void jujuBtnChange()
        {
            if (UIToggle.current.Equals(jujuBtn) && UIToggle.current.value)
            {
                setEquipListData(GoodsConst.POS_BAGGES);
            }
        }

        //设置分类数据
        private void setEquipListData(int type)
        {
			isClickAllEquipBtn = isClickRecommenBtn = false;
            filterEquipList = new List<SysEquipVo>();
			filterEquipList = EquipIndentificationUtil.getEquipListData (type , allEquipList);
			setCurrentPageNum(filterEquipList);
            setCurrentPageData(filterEquipList, curentPageNum, totalPageNum);
        }

        //设置当前分页页码
        private void setCurrentPageNum(List<SysEquipVo> dataList)
        {
            totalPageNum = dataList.Count / EquipIdentificationConst.EQUIPMAX_12;
            if ((dataList.Count % EquipIdentificationConst.EQUIPMAX_12) > 0)
            {
                curentPageNum = EquipIdentificationConst.PAGEMIN_1;
                totalPageNum += 1;
            }else
            {
                curentPageNum = EquipIdentificationConst.PAGEMIN_1;
            }
        }

        //设置当前页数据
        private void setCurrentPageData(List<SysEquipVo> dataList, int curNum, int totalNum)
        {
			if(dataList.Count == 0)
			{
				return;
			}
            List<SysEquipVo> curPageDataList = new List<SysEquipVo>();
            if (curNum == totalNum)
            {
				curPageDataList = EquipIndentificationUtil.getBetweenData(
					dataList , (curNum - 1) * EquipIdentificationConst.EQUIPMAX_12 ,(dataList.Count));
            }
            else
            {
				curPageDataList = EquipIndentificationUtil.getBetweenData(
					dataList, (curNum - 1) * EquipIdentificationConst.EQUIPMAX_12, (curNum * EquipIdentificationConst.EQUIPMAX_12));
            }
            setCurrentPageView(curPageDataList, curNum, totalNum);
            pageNumberLabel.text = curentPageNum + "/" + totalPageNum;
        }

        //设置当前页面数据
        private void setCurrentPageView(List<SysEquipVo> dataList, int curNum, int totalNum)
        {
            for (int i = 0; i < equipItemList.Count; i++)
            {
				Transform equipItem = equipItemList[i];
				UILabel equipItemName  = equipItem.FindChild("nameLabel").GetComponent<UILabel>();
				UISprite Icon          = equipItem.FindChild("icon").GetComponent<UISprite>();
				ItemContainer equipItemContainer = FindChild("equipItemContainer/"+i).AddMissingComponent<ItemContainer>();
				equipItem.gameObject.SetActive(true);

                if(i < dataList.Count)
                {
					setEveryIcon(equipItem , dataList[i] , equipItemContainer ,equipItemName);
                }else
                {
					equipItem.gameObject.SetActive(false);
                }
			}
        }

		//设置图标
		private void setEveryIcon(Transform item , SysEquipVo equipVo ,
		                          ItemContainer equipItemContainer , UILabel equipItemName)
		{
			int[] lvl = StringUtils.GetStringToInt(equipVo.lvl);
			if(EquipIndentificationUtil.getCurrentEquipIsGreaterTen(lvl[0]))
			{
				item.FindChild("icon").GetComponent<UISprite>().atlas = Singleton<AtlasManager>.Instance.GetAtlas("common");
				item.FindChild("icon").GetComponent<UISprite>().spriteName = "suo23";
				item.FindChild("background").GetComponent<UISprite>().atlas = Singleton<AtlasManager>.Instance.GetAtlas("common");
				item.FindChild("background").GetComponent<UISprite>().spriteName = "epz_" + equipVo.color;
			}else
			{
				Singleton<ItemManager>.Instance.InitItem(item.gameObject, 
			                                         (uint)equipVo.id, ItemType.Equip);
			}
			equipItemContainer.Id = (uint) equipVo.id;
			equipItemName.text = "Lv." + lvl[0] + "  " + equipVo.name;
		}
		
		//翻页调用
        private void clickTurnThePage()
        {
			if (isClickAllEquipBtn)
			{
				setCurrentPageData(allEquipList, curentPageNum, totalPageNum);
			}else if(isClickRecommenBtn)
			{
				setCurrentPageData(recommenEquipList, curentPageNum, totalPageNum);
			}else
			{
				setCurrentPageData(filterEquipList, curentPageNum, totalPageNum);
			}
        }

        //上一页
        private void previousePageOnClick(GameObject go)
        {
            if (curentPageNum == EquipIdentificationConst.PAGEMIN_1)
            {
                return;
            }
            curentPageNum--;
            clickTurnThePage();
        }

        //下一页
        private void nextPageOnClick(GameObject go)
        {
            if (curentPageNum == totalPageNum)
            {
                return;
            }
            curentPageNum++;
            clickTurnThePage();
        }

        //点击装备
        private void equipItemOnClick(GameObject go)
        {
			ItemContainer currentClickEquipItem = go.GetComponent<ItemContainer>();
			currentClickEquipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>(currentClickEquipItem.Id);
			int[] lvl = StringUtils.GetStringToInt(currentClickEquipVo.lvl);
			int canUseLevel = lvl[0] - EquipIdentificationConst.POOR_GRADES_10;
			if(EquipIndentificationUtil.getCurrentEquipIsGreaterTen(lvl[0]))
			{
				string tips = LanguageManager.GetWord("EquipIdentificationView.isGreaterTen" , string.Empty + canUseLevel);
				MessageManager.Show(tips);
				return;
			}
			Singleton<TipsManager>.Instance.OpenTipsByGoodsId((uint)currentClickEquipVo.id, onGetEquipWayHandler, 
			                                                  null, LanguageManager.GetWord("EquipIdentificationWayTipsView.way"), 
			                                                  string.Empty, TipsType.DELEGATENOCLOSE);
        }

		//获取途径
		private void onGetEquipWayHandler()
		{
			Singleton<EquipTips>.Instance.setEquipTipsPlayForward();
			Singleton<EquipIdentificationWayTipsView>.Instance.setCurrentEquipVo(currentClickEquipVo);
		}
    }
}