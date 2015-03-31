using System.Collections.Generic;
using System.Linq;
using com.game.consts;
using Com.Game.Module.Medal;
using Com.Game.Module.Role;
using com.game.vo;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using PCustomDataType;
using com.game.manager;
using com.game.data;
using Com.Game.Module.Manager;
using Com.Game.Module.Equip;
using com.game.utils;
using com.u3d.bases.debug;

namespace Com.Game.Module.Tips
{
	public class EquipTips : BaseView<EquipTips>{
		public override string url {get {return "UI/Tips/EquipTips/EquipTipsView1.assetbundle";}}
        public override ViewLayer layerType { get { return ViewLayer.HighLayer; } }
		private Button btn_left;  // 左边按钮  使用
		public Button btn_right;   //右边按钮  
		private Button btn_close;   //关闭按钮

	    private UIWidgetContainer background;
        public UIWidgetContainer.ClickDelegate mCloseOnClick;
        public UIWidgetContainer.ClickDelegate mLeftOnClick;
        public UIWidgetContainer.ClickDelegate mRightOnClick;
        public UIWidgetContainer.ClickDelegate mOpenHandle;


		private UILabel jiBenSX;//基本属性
		private List<UILabel> fjList = new List<UILabel>();//附加属性List
        private List<UIWidgetContainer> smeltList = new List<UIWidgetContainer>();//宝石属性List
        private List<UILabel> jcList = new List<UILabel>();//基础属性List
        private List<UISprite> refineList = new List<UISprite>();
		private UILabel itemName;    //装备名字
		private UILabel itemLevel;   //装备等级

		private UIWidgetContainer goodsIcon;//物品图标
	    private UILabel baseLabel;//基本属性标签
	    private UILabel smeltLabel;//宝石属性标签
	    private UILabel descLabel;//描述标签
	    private UILabel jobLabel;//职业
	    private UILabel fightpoint;//战斗力

		private string leftBtnLabel;//左边按钮文本
		private string rightBtnLabel;//右边按钮文本
        
		public uint id;//装备唯一id
		private PGoods goods ;//当前装备

		public UIPlayTween tweenPlay;
		private TweenPlay equipTipsTP;

        private TipsType tipsType;

		private Vector3 fromOld;
		private Vector3 fromNew = new Vector3(260f, 0f, 0f);
		private UISprite sprMask;
		private TweenPosition tweenPos;
		private bool showOtherPlayerEquip = false;

		protected override void Init()
		{
			btn_left = FindInChild<Button>("view/btn_left");
			btn_right = FindInChild<Button>("view/btn_right");
			tweenPlay = FindInChild<UIPlayTween>();
			equipTipsTP = FindInChild<TweenPlay>("view");
			btn_close = FindInChild<Button>("view/btn_close");
			background = FindInChild<UIWidgetContainer>("background");
		    background.clickDelegate = CloseView;
			//jiBenSX = FindInChild<UILabel>("view/jbsx/item/shuzi");
			itemName = FindInChild<UILabel>("view/name");
			itemLevel = FindInChild<UILabel>("view/level");
			jobLabel = FindInChild<UILabel>("view/job");
			goodsIcon = FindInChild<UIWidgetContainer>("view/item");
			GetItemList();
			btn_close.clickDelegate = CloseView;

			smeltLabel = FindInChild<UILabel>("view/smelttile");
		    smeltLabel.text = LanguageManager.GetWord("Tips.Smelt");
			descLabel = FindInChild<UILabel>("view/ms/title");
		    fightpoint = FindInChild<UILabel>("view/fightpointvalue");

			sprMask = FindInChild<UISprite>("background");
			tweenPos = FindInChild<TweenPosition>("view");
			fromOld = FindInChild<TweenPosition>("view").from;
		}

		private void SetViewForOtherPlayer()
		{
			sprMask.color = new Color(1f, 1f, 1f, 0f);
			tweenPos.from = fromNew;
		}

		private void RestoreView()
		{
			showOtherPlayerEquip = false;
			sprMask.color = new Color(1f, 1f, 1f, 1f);
			tweenPos.from = fromOld;
		}

		//初始化显示的组件
		private void GetItemList()
		{
			for(int i =1;i<6;i++)  //描述的UILabel
			{
				fjList.Add(FindInChild<UILabel>("view/ms/zi"+i));
				smeltList.Add(FindInChild<UIWidgetContainer>("view/bs" + i));
			}
			jcList.Add(FindInChild<UILabel>("view/attr1"));
			jcList.Add(FindInChild<UILabel>("view/attr2"));
            for (int i = 1; i < 16; i++)   //宝石属性List
            {
				refineList.Add(FindInChild<UISprite>("view/refine/xx" + i));
            }
		}
        public void OpenViewByGoodsId(uint goodsId, string leftText, string rightText, UIWidgetContainer.ClickDelegate mLeftOnClick = null,
                                 UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, TipsType tipsType = TipsType.DEFAULT_TYPE)
        {
            this.id = goodsId;
            goods = new PGoods();
            goods.goodsId = goodsId;
            this.mLeftOnClick = mLeftOnClick;
            this.mRightOnClick = mRightOnClick;

            this.mCloseOnClick = closeOnClick;
            this.leftBtnLabel = leftText;
            this.rightBtnLabel = rightText;
            this.tipsType = tipsType;
            OpenView();
        }
		public void OpenViewById(uint id,string leftText,string rightText,UIWidgetContainer.ClickDelegate mLeftOnClick = null,
                                 UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, TipsType tipsType = TipsType.DEFAULT_TYPE)
		{
			this.id = id;
			goods = Singleton<GoodsMode>.Instance.GetPGoodsById(id);
			this.mLeftOnClick =mLeftOnClick;
			this.mRightOnClick = mRightOnClick;

			this.mCloseOnClick = closeOnClick;
			this.leftBtnLabel = leftText;
			this.rightBtnLabel = rightText;
            this.tipsType = tipsType;
			OpenView();
		}

		public void OpenPlayerEquipViewById(uint id,string leftText,string rightText,UIWidgetContainer.ClickDelegate mLeftOnClick = null,
		                         UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, TipsType tipsType = TipsType.DEFAULT_TYPE)
		{
			showOtherPlayerEquip = true;
			this.id = id;
			goods = Singleton<GoodsMode>.Instance.GetOtherPGoods(id);
			this.mLeftOnClick =mLeftOnClick;
			this.mRightOnClick = mRightOnClick;
			
			this.mCloseOnClick = closeOnClick;
			this.leftBtnLabel = leftText;
			this.rightBtnLabel = rightText;
			this.tipsType = tipsType;
			OpenView();
		}


        public void OpenPlayerEquipViewByPGoods(PGoods other,string leftText, string rightText, UIWidgetContainer.ClickDelegate mLeftOnClick = null,
                         UIWidgetContainer.ClickDelegate mRightOnClick = null, UIWidgetContainer.ClickDelegate closeOnClick = null, TipsType tipsType = TipsType.DEFAULT_TYPE)
        {
            showOtherPlayerEquip = true;
            this.id = other.id;
            goods = other;
            this.mLeftOnClick = mLeftOnClick;
            this.mRightOnClick = mRightOnClick;
            this.mCloseOnClick = closeOnClick;
            this.leftBtnLabel = leftText;
            this.rightBtnLabel = rightText;
            this.tipsType = tipsType;
            OpenView();
        }


		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();

			if (null != mCloseOnClick)
			{
				mCloseOnClick();
			}

			RestoreView();
		}

        // 126 ，190 ，227
		protected override void HandleAfterOpenView()
		{
			if (showOtherPlayerEquip)
			{
				SetViewForOtherPlayer();
			}

			SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
            fightpoint.text = string.Empty + GoodsMode.Instance.CalculateFightPoint(goods);
			Singleton<ItemManager>.Instance.InitItem(goodsIcon,goods.goodsId,0);
            List<PSuitAttr> suitAttr  = new List<PSuitAttr>();
			if(vo !=null)  //装备描
			{
			    
				itemName.text = ColorUtils.GetEquipColor(vo.color,vo.name);
			    jobLabel.text = LanguageManager.GetWord("Role.TipsJob" + vo.job);
			    string lvl = StringUtils.SplitVoString(vo.lvl)[0];
                int refine =( goods.equip.Count ==0 )? 0 : (int)(goods.equip[0].refine);
                int stren = (goods.equip.Count == 0) ? 0 : (int)(goods.equip[0].stren);
                suitAttr = goods.equip.Count ==0?null: goods.equip[0].suitList;
			    goodsIcon.FindInChild<UILabel>("stren").text = "+" + stren;

                UILabel label;
			    string suitStr = null;
                for (int i = 0; i < 5; i++)
                {
                    label = fjList[i];
                    if (suitAttr != null && i < suitAttr.Count)
                    {
                        suitStr = string.Format(LanguageManager.GetWord("Language.Format1"),
                            LanguageManager.GetWord("Role.Attr" + suitAttr[i].id), suitAttr[i].attr) +
                                  VoUtils.LanguagerFormat("EquipTips.SuitStar", suitAttr[i].pos);
                        label.text = ColorUtils.GetEquipColor(3, suitStr);   //已经开启使用蓝色
                    }
                    else
                    {
                        suitStr = VoUtils.GetColorTips(i + 1);
                        label.text = ColorUtils.GetEquipColor(i + 2, suitStr);
                    }
                    
                }
                for (int i = 0; i < 15; i++)
			    {
                    if(refine <=i)
			            refineList[i].spriteName = "kongxing";
                    else
                        refineList[i].spriteName = "xingxing1";
			    }

                string[] strenType = StringUtils.SplitVoString(vo.stren_type);
                List<string> strs = new List<string>();
                foreach (string s in strenType)
                {
                    strs.AddRange(StringUtils.GetValueListFromString(vo.GetEquipStrenType(int.Parse(s))));
                }
			    int type;
			    SysRefineVo refineVo;
                //基础属性计算
                for (int i = 0; i < 2; i++)
                {
                    if (i < strenType.Length)
                    {
                        type = int.Parse(strenType[i]);
                        
                        int addValue = int.Parse(strs[i * 2 + 1]);
                        int strenValue = int.Parse(strs[i * 2]) + addValue * stren;
                        refineVo = BaseDataMgr.instance.GetDataById<SysRefineVo>((uint) refine);
                        
                        float rate = refineVo.GetRefineRate();
                        int addValue1 = Mathf.RoundToInt(strenValue*rate);
                        string format = LanguageManager.GetWord("Language.Format4");
                        string key = VoUtils.GetRoleAttr(type);  //属性文本
                        if (addValue1 == 0)  //没有精炼加成值
                        {
                            format = LanguageManager.GetWord("Language.Format1");
                            if (type != 10 && type != 8)   //不是物攻和魔攻
                            {
                                jcList[i].text = string.Format(format, key, strenValue);
                            }
                            else
                            {
                                string minValue = vo.GetEquipStrenType(type - 1);
                                jcList[i].text = string.Format(format, string.Format(ColorConst.EQUIP_BLUE_FORMAT, key), minValue + "-" + strenValue);
                            }
                        }
                        else
                        {
                            if (type != 10 && type != 8)
                            {
                                jcList[i].text = string.Format(format, string.Format(ColorConst.EQUIP_BLUE_FORMAT,key), strenValue, string.Format(ColorConst.GREEN_FORMAT, "+" + addValue1+"( "+
                                    string.Format("{0:0%}", rate)+" )"));

                            }
                            else
                            {
                                string minValue = vo.GetEquipStrenType( type - 1);
                                jcList[i].text = string.Format(format, string.Format(ColorConst.EQUIP_BLUE_FORMAT, key), minValue + "-" + strenValue, string.Format(ColorConst.GREEN_FORMAT, "+" + addValue1 + "( " +
                                    string.Format("{0:0%}", rate) + " )"));
                            }
                        }
                    }
                    else
                    {
                        jcList[i].text = string.Empty;
                    }
                }
			    int repos = Singleton<GoodsMode>.Instance.GetReposById(goods.id);
			    string lvlString = MeVo.instance.Level < vo.GetLvl() ? string.Format(ColorConst.RED_FORMAT, lvl):lvl + string.Empty;
                if (repos == GoodsMode.Instance.GOODS_REPOS)
                {
                    itemLevel.text ="等级： "+ lvlString + " (未装备)";
                }
                else if (repos == GoodsMode.Instance.EQUIP_REPOS)
                {
                    itemLevel.text = "等级： " + lvlString + " (已装备)";
                }else 
				{
					itemLevel.text = "等级： " + lvlString;
				}

                PEquip equip = (goods.equip.Count == 0) ? new PEquip() : goods.equip[0];
			    int count = equip.gemList.Count;
			    UIWidgetContainer ic;
			    PGemInHole gem;
			    SysItemVo smeltVo;
			    for (int i = 0; i < 5; i++)   //宝石镶嵌信息
			    {
			        ic = smeltList[i];
                    UILabel label1 = ic.FindInChild<UILabel>("label");
			        if (i < count)   //已经镶嵌
			        {
			            gem = equip.gemList[i];
                        ItemManager.Instance.InitItem(ic, gem.gemId, 0);
			            smeltVo = BaseDataMgr.instance.GetDataById<SysItemVo>(gem.gemId);
                        label1.text = VoUtils.GetRoleAttr(smeltVo.subtype) + ": +" + smeltVo.value;
                        label1.color = Color.green;
			        }
			        else
			        {
                        List<PGoods> goodsList = GoodsMode.Instance.GetSmeltGemListByGoodsId(this.goods.goodsId);
			            if (goodsList.Count != 0)
			            {
			                ItemManager.Instance.InitItem(ic, ItemManager.Instance.ADD_ICON, 0);
			                
                            label1.text = "可镶嵌";

			            }
			            else
			            {
                            ItemManager.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);
                            label1.text = "未镶嵌";
			            }
                        label1.color = new Color(169 / 255f, 169 / 255f, 169 / 255f);
			        }
			    }
			}

            //按钮回调和位置调整 
			btn_right.FindInChild<UILabel>("label").text = rightBtnLabel;  //文本显示
			btn_left.FindInChild<UILabel>("label").text = leftBtnLabel;
			btn_left.clickDelegate = mLeftOnClick;   //响应事件指定
		    bool isCenter = false;
		    if (mLeftOnClick == null)
		    {
		        btn_left.SetActive(false);
		        isCenter = true;
		    }
		    else
		    {
		        btn_left.SetActive(true);
                if(tipsType != TipsType.DELEGATENOCLOSE)
                    btn_left.clickDelegate += CloseView;
		    }
		    btn_right.clickDelegate = mRightOnClick;
		    
		    if (mRightOnClick == null)
		    {
		        btn_right.SetActive(false);
		        isCenter = true;
		    }
		    else
		    {
                btn_right.SetActive(true);
                if (tipsType != TipsType.DELEGATENOCLOSE)
                    btn_right.clickDelegate += CloseView;
		    }
		    if (isCenter)  //调整位置
		    {
                btn_left.transform.localPosition = new Vector3(-3f, -238f, 0f);
		        btn_left.FindInChild<UISprite>("background").width = 280;
                btn_left.FindInChild<UISprite>("highlight").width = 298;
                btn_right.transform.localPosition = new Vector3(-3f, -238f, 0f);
                btn_right.FindInChild<UISprite>("background").width = 280;
                btn_right.FindInChild<UISprite>("highlight").width = 298;
		    }
		    else
		    {
                btn_left.transform.localPosition = new Vector3(-98.5f,-238f,0f);
                btn_left.FindInChild<UISprite>("background").width = 167;
                btn_left.FindInChild<UISprite>("highlight").width = 179;
                btn_right.transform.localPosition = new Vector3(78.5f, -238f, 0f);
                btn_right.FindInChild<UISprite>("background").width = 167;
                btn_right.FindInChild<UISprite>("highlight").width = 179;
		    }
			if(mOpenHandle != null)
			{
				mOpenHandle();
			}
			setEquipTipsPlayReverse();
		}

		public void setEquipTipsPlayForward()
		{
			equipTipsTP.PlayForward();
		}

		public void setEquipTipsPlayReverse()
		{
			equipTipsTP.PlayReverse();
		}
	}
}