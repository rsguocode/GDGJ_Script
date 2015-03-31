using System;
using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Chat;
using com.game.module.effect;
using Com.Game.Module.Manager;
using com.game.module.test;
using Com.Game.Module.Tips;
using com.game.Public.Confirm;
using com.game.utils;
using PCustomDataType;
using Proto;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.LuckWand
{
    public class LuckWandView : BaseView<LuckWandView>
    {
        public override string url
        {
            get { return "UI/LuckWand/LuckWandView.assetbundle"; }
        }

        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }

        private UILabel djName;
        private UILabel djValue; //至尊大奖

        private UITextList jlList; //普通奖励信息

        private UILabel syLabel; //所有奖励文本
        private List<ItemContainer> wpList = new List<ItemContainer>(); //奖励物品列表 

        private List<ItemContainer> hsList = new List<ItemContainer>(); //幻兽列表

        private UILabel mfLabel; //免费次数文本
        private UILabel mfValue; //免费次数值
        private UILabel leftLabel; //剩余次数
        private UILabel leftValue;

        private UILabel dqjjLabel; //当前奖金

        private Button closeButton; //关闭按钮

        private TweenPosition centerTween;

        private int updateRate = 10;
        private int animationRate = 4;
        private int updateIndex = 0;
        private int animationIndex = 0; //切换随机动画
        private int[] randomSequence;

        protected override void Init()
        {
            djName = FindInChild<UILabel>("zc/dejiang/dezhu/name");
            djValue = FindInChild<UILabel>("zc/dejiang/dezhu/value");

            jlList = FindInChild<UITextList>("zc/dejiang/huode");

            syLabel = FindInChild<UILabel>("zc/wupin/jiangpin");
            for (int i = 1; i < 9; i++)
            {
                wpList.Add(FindChild("zc/wupin/wupin/item" + i).AddMissingComponent<ItemContainer>());
            }

            for (int i = 1; i < 13; i++)
            {
                ItemContainer ic = FindChild("zc/yuan/item" + i).AddMissingComponent<ItemContainer>();
                ic.Id = 0;
                ic.onClick = WandClick;
                hsList.Add(ic);
            }

            mfLabel = FindInChild<UILabel>("zc/mianfu/mflabel");
            mfLabel.text = LanguageManager.GetWord("Wand.UseFree");
            mfValue = FindInChild<UILabel>("zc/mianfu/mfvalue");
            leftLabel = FindInChild<UILabel>("zc/mianfu/leftlabel");
            leftLabel.text = LanguageManager.GetWord("Wand.LeftBuy");
            leftValue = FindInChild<UILabel>("zc/mianfu/leftvalue");

            FindInChild<UILabel>("zc/wupin/jiangpin").text = LanguageManager.GetWord("Wand.AllAward");

            dqjjLabel = FindInChild<UILabel>("zc/jiangchi/shuzi");
            closeButton = FindInChild<Button>("zc/topright/btn_close");

            closeButton.onClick = CloseOnClick;
            centerTween = FindInChild<TweenPosition>("zc");
        }


        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            Singleton<LuckWandControl>.Instance.RequestWandInfo();
            Singleton<LuckWandControl>.Instance.RequestWandCommon();
            Singleton<LuckWandControl>.Instance.RequestWandGrand();
            centerTween.PlayForward(); //播放动画
            InitAwardGoods();
        }

        protected override void HandleBeforeCloseView()
        {
            base.HandleBeforeCloseView();
            centerTween.ResetToBeginning();
            
        }

        private void InitAwardGoods()
        {
            ItemContainer ic;
            uint goodsId = 0;
            for (int i = 0; i < 8; i++)
            {
                ic = wpList[i];
                goodsId = (uint) (100010 + (i + 1)*100);
                Singleton<ItemManager>.Instance.InitItem(ic,goodsId,ItemType.BaseGoods);
                ic.onClick = GoodsOnClick;
                ic.Id = goodsId;
            }
            
        }

        private void GoodsOnClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            TipsManager.Instance.OpenTipsByGoodsId(ic.Id, null, null, string.Empty, string.Empty, 0);
        }

    //注册数据更新回调函数
        public override void RegisterUpdateHandler()
        {
            Singleton<LuckWandMode>.Instance.dataUpdated += UpdateWandHandle;
        }

        private void UpdateWandHandle(object sender, int code)
        {
            if (code == Singleton<LuckWandMode>.Instance.UPDATE_WAND_COMMON)
            {
                UpdateWandCommon();
            }
            else if (code == Singleton<LuckWandMode>.Instance.UPDATE_WAND_INFO)
            {
                UpdateWandInfo();
            }
            else if (code == Singleton<LuckWandMode>.Instance.UPDATE_WAND_OPEN)
            {
                
            }
            else if (code == Singleton<LuckWandMode>.Instance.UPDATE_WAND_GRAND)
            {

                UpdateWandGrand();
            }
            else if (code == Singleton<LuckWandMode>.Instance.UPDATE_WAND_DRAW)
            {
                UpdateWandDraw();
            }
        }

        private void UpdateWandCommon()
        {
            List<PWandInfo> wandInfos = Singleton<LuckWandMode>.Instance.CommonInfoList;
            string template = LanguageManager.GetWord("Wand.Common");
            string get1 =string.Empty;
            string money = string.Empty;
            foreach (PWandInfo temp in wandInfos)
            {
                if (temp.prize == 1)
                {
                    get1 = string.Empty + temp.num;
                    money =LanguageManager.GetWord("Money.Gold") ;
                }
                else if (temp.prize == 2)
                {
                    get1 = string.Empty + temp.num;
                    money = LanguageManager.GetWord("Money.Diam");
                }
                else if (temp.prize == 3)
                {
                    get1 = LanguageManager.GetWord("Wand.OnceAgain");
                    money = string.Empty;
                }
                else
                {
                    SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint) temp.prize);
                    if (vo != null)
                    {
                        get1 = string.Empty +  temp.num;
                        money =  vo.name;
                    }
                }
                this.jlList.Add(string.Format(template,temp.str ,get1,money));
            }
        }
        private void UpdateWandInfo()
        {
            mfValue.text = string.Empty + Singleton<LuckWandMode>.Instance.Free;
            leftValue.text = string.Empty + Singleton<LuckWandMode>.Instance.CanBuyTimes+"/"+
                Singleton<LuckWandMode>.Instance.CanUseTimes;
            dqjjLabel.text = string.Empty + Singleton<LuckWandMode>.Instance.TotalGold;
			foreach (ItemContainer ic in hsList)
			{
				ic.FindInChild<UISprite>("icon").spriteName = null;
                ic.FindInChild<UISprite>("icon1").spriteName = null;
				NGUITools.AddWidgetCollider(ic.gameObject);
				ic.Id = 0;
			}
			ItemContainer temp;
			foreach (PWandType wandType in Singleton<LuckWandMode>.Instance.WandTypeList)
			{
				temp = hsList[wandType.index - 1];
				temp.FindInChild<UISprite>("icon").spriteName = "xy"+wandType.type+ "-1";
				temp.FindInChild<UISprite>("icon").MakePixelPerfect();
                temp.FindInChild<UISprite>("icon1").spriteName = "xy" + wandType.type + "-2";
			    //temp.FindInChild<UISprite>("icon1").alpha = 0f;
                temp.FindInChild<UISprite>("icon1").gameObject.SetActive(false);
                temp.FindInChild<UISprite>("icon1").gameObject.SetActive(true);
                temp.FindInChild<UISprite>("icon").MakePixelPerfect();
				temp.Id = wandType.index;
				NGUITools.AddWidgetCollider(temp.gameObject);
				
			}

        }
        private void UpdateWandGrand()
        {
            if (Singleton<LuckWandMode>.Instance.GrandInfoList.Count > 0)
            {
                PWandInfo wandInfo = Singleton<LuckWandMode>.Instance.GrandInfoList[0];
                djName.text = wandInfo.str;
                djValue.text = string.Format(LanguageManager.GetWord("Wand.Grand"), wandInfo.num);
                
                
            }
			else 
			{
				djName.text = string.Empty;
				djValue.text = string.Empty ;
			}
        }

        private void UpdateWandDraw()
        {
            //播放特效
        }
        public override void CancelUpdateHandler()
        {
            Singleton<LuckWandMode>.Instance.dataUpdated -= UpdateWandHandle;
        }

        private uint currentId = 0;
        //打地主了
        private void WandClick(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            currenClick = ic;
            if (ic.Id == 0)
                return;
            currentId = ic.Id ;
            if (Singleton<LuckWandMode>.Instance.CanUseTimes > 0 && Singleton<LuckWandMode>.Instance.Free <= 0)
            {
				ConfirmMgr.Instance.ShowCommonAlert(string.Format(LanguageManager.GetWord("Wand.BuyTips"), Singleton<LuckWandMode>.Instance.Diam ), ConfirmCommands.OK_CANCEL,
                                                    SendClickMessage, LanguageManager.GetWord("Wand.Buy"), null, LanguageManager.GetWord("Wand.Cancel"));
            }
            else
            {
                SendClickMessage();
            }
        }

        private ItemContainer currenClick;
        public void SendClickMessage()
        {
            EffectMgr.Instance.CreateUIEffect(EffectId.UI_LuckWand, currenClick.transform.position);
            Singleton<LuckWandControl>.Instance.StrikeWand((byte)currentId);
        }
        private void CloseOnClick(GameObject go)
        {
            CloseView();
        }

        public override void Update()
        {
            if (updateIndex%updateRate == 0)
            {
                updateIndex = 1;
                string spriteName;
                UISprite sprite;
                UISprite sprite1;
                string[] split;
                ItemContainer ic;
                if (animationIndex%animationRate == 0)
                {
                    randomSequence = MathUtils.RandomRsequence(0, 12, 12);
                    animationIndex = 1;
                }
                else
                {
                    animationIndex ++;
                }
                foreach (int index in randomSequence)
                {
                    ic = hsList[index];
                    sprite = ic.FindInChild<UISprite>("icon") ;
                    sprite1 = ic.FindInChild<UISprite>("icon1");
                    spriteName = sprite.spriteName;
                    if (string.IsNullOrEmpty(spriteName))
                    {
                        continue;
                    }
                    else
                    {
                        /*split = spriteName.Split('-');
                        if (spriteName.Contains("-2"))
                        {
                            spriteName = split[0] + "-1";
                        }
                        else
                        {
                            spriteName = split[0] + "-2";
                        }*/
                        /*if (sprite.alpha < 0.01f)
                        {
                            sprite1.alpha = 0f;
                            sprite.alpha = 1f;
                        }
                        else if (sprite1.alpha < 0.01f)
                        {
                            sprite.alpha = 0f;
                            sprite1.alpha = 1f;
                        }*/
                        if (sprite.gameObject.activeInHierarchy)
                        {
                            sprite.gameObject.SetActive(false);
                            sprite1.gameObject.SetActive(true);
                        }
                        else
                        {
                            sprite1.gameObject.SetActive(false);
                            sprite.gameObject.SetActive(true);
                        }
                    }
					//sprite.spriteName = spriteName;
                }
            }
            else
            {
                updateIndex++;
            }
        }
    }

}

