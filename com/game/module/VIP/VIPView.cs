//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：VIPView
//文件描述：VIP界面
//创建者：张燕茹
//创建日期：2014-3-7
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.debug;
using com.game.Public.Message;
using com.game.data;
using com.game.manager;
using System.Collections.Generic;
using com.game.utils;
using System;
using com.game.module.main;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using Com.Game.Module.Tips;
using com.game.start;
using com.game.SDK;
using com.game.module.login;

namespace Com.Game.Module.VIP
{
    public class VIPView : BaseView<VIPView>
    {
        public override string url { get { return "UI/VIP/VIPView.assetbundle"; } }

        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }

        private Button btnClose;   //关闭 
        private Button leftVIP;    //点击左键查看上一级VIP信息
        private Button rightVIP;   //查看下一级VIP信息
        private Button btnPay;     //充值的按钮
        private Button btnGetAward;  //；领取奖励的按钮

        private UILabel vipLevel1;
        private UILabel vipLevel2;
        private UILabel payTip;  //提示再充值金额
        private UILabel payTotal;
        private UILabel vipTitle1;   //v1特权
        private UILabel vipGiftTitle;  //礼包标题
        private UILabel processDes;
        private UILabel awardWord;  //领取按钮的文字

        private UISlider payProcess;
        private UISprite sliderBg;  //进度条前景图
        private UISprite btn_bg;
        private Vector3 vec;

        private GameObject desItem; //描述的第一条
        private GameObject desObj;
        private GameObject objNextVIP;  //下一级VIP对象，到顶时隐藏
        private Vector3 desItemLocalPosition;
        private Transform showArea;
        private UIPlayTween playTween;
        private UICenterOnChild centerOnChild;       //控制节点居中的组件
        private PageIndex pageIndex;                     //翻页的组件
        private UIScrollView scrollView;            //滚动的组件

        private ITEM [] ShowPannel = new ITEM[13];  //展示奖励的面板数组，共12个
        private int ShowVIP = 0;

        protected override void Init()
        {
			initView();
			initClick();
        }

		private void initView()
		{
			initViewLayout();
			initDesItem();
			initAwardItem();
			initShowArea();
		}

		private void initViewLayout()
		{
			btnClose = FindInChild<Button>("btn_guanbi");
			leftVIP = FindInChild<Button>("jiantou/left");
			rightVIP = FindInChild<Button>("jiantou/right");
			btnPay = FindInChild<Button>("sm/btn_zz");
			btnPay.SetActive(false);
			vipLevel1 = FindInChild<UILabel>("sm/1/1/level");
			vipLevel2 = FindInChild<UILabel>("sm/2/2/level");
			payTip = FindInChild<UILabel>("sm/2/cz");
			payTotal = FindInChild<UILabel>("sm/total_money");
			payTotal.SetActive(false);
			
			processDes = FindInChild<UILabel>("sm/1/process/shuzi");
			payProcess = FindInChild<UISlider>("sm/1/process");
			sliderBg = FindInChild<UISprite>("sm/1/process/Foreground");
			objNextVIP = FindInChild<Transform>("sm/2").gameObject;
			//第一个面板的信息
			btnGetAward = FindInChild<Button>("center/"+"1"+"/right/btn_lq");
			btn_bg = FindInChild<UISprite>("center/" + "1" + "/right/btn_lq/background");
			awardWord = FindInChild<UILabel>("center/" + "1" + "/right/btn_lq/lk");
			vipTitle1 = FindInChild<UILabel>("center/" + "1" + "/left/title");
			vipGiftTitle = FindInChild<UILabel>("center/" + "1" + "/right/gift");
			showArea = FindInChild<Transform>("center/" + "1");
			
			scrollView = FindInChild<UIScrollView>("center");  //滚动的组件
			centerOnChild = FindInChild<UICenterOnChild>("center");
			pageIndex = FindChild("fanye").AddMissingComponent<PageIndex>();
			pageIndex.RegisterOnCenter(centerOnChild);
			pageIndex.InitPage(1, 13);
			
			desItem = FindInChild<UILabel>("center/" + "1" + "/left/des/1").gameObject;
			desObj = FindInChild<Transform>("center/" + "1" + "/left/des").gameObject;
			desItemLocalPosition = desItem.transform.localPosition;

			ShowPannel[0] = new ITEM();
			ShowPannel[0].DesList.Add(desItem.GetComponent<UILabel>());
			ShowPannel[0].vipTitle = vipTitle1;   //等级标题
			ShowPannel[0].vipTequan = vipGiftTitle;   //特权标题
			ShowPannel[0].btnAward = btnGetAward;
			ShowPannel[0].btnBg = btn_bg;
			ShowPannel[0].btnWord = awardWord;  //按钮上的文字
			ShowPannel[0].desItem = desItem;
			ShowPannel[0].desObj = desObj;
			ShowPannel[0].pannel = showArea.gameObject;

			btnGetAward.gameObject.SetActive(false);  //这是VIP的按钮，但是因为没有奖励，就不显示它了
		}

		private void initDesItem()
		{
			for (int desItemIndex = 0; desItemIndex < 10; desItemIndex ++)
			{
				desItemLocalPosition -= new Vector3(0, 30, 0);
				GameObject go1 = GameObject.Instantiate(desItem, desItem.transform.localPosition, desItem.transform.rotation) as GameObject;
				go1.transform.parent = desItem.transform.parent;
				go1.transform.localPosition = desItemLocalPosition;
				go1.transform.localScale = new Vector3(1, 1, 1);
				UILabel lab = go1.GetComponent<UILabel>();
				ShowPannel[0].DesList.Add(lab);
			}
		}

		private void initAwardItem()
		{
			for (int itemIndex = 1; itemIndex < 7; itemIndex++)
			{
				AwardItem item = new AwardItem();
				item.obj = FindInChild<Transform>("center/" + "1" + "/right/wp/" + itemIndex.ToString()).gameObject;
				item.ItemBtn = FindInChild<Button>("center/" + "1" + "/right/wp/" + itemIndex.ToString());
				item.name = FindInChild<UILabel>("center/" + "1" + "/right/wp/" + itemIndex.ToString() + "/mz");
				item.count = FindInChild<UILabel>("center/" + "1" + "/right/wp/" + itemIndex.ToString() + "/geshu");
				item.icon = FindInChild<UISprite>("center/" + "1" + "/right/wp/" + itemIndex.ToString() + "/icon");
				item.icon.GetComponent<TweenPosition>().enabled = false;
				ShowPannel[0].AwardList.Add(item);
			}
		}

		private void initShowArea()
		{
			vec = showArea.transform.position;
			for (int i = 2; i < 14; i ++ )
			{
				GameObject go = GameObject.Instantiate(showArea.gameObject, showArea.transform.position, showArea.transform.rotation) as GameObject;
				vec += new Vector3(3, 0, 0);
				go.transform.position = vec;
				go.transform.parent = showArea.parent;
				go.transform.localScale = showArea.transform.localScale;
				go.name = (i).ToString();   //修改对象的名字
				vipTitle1 = FindInChild<UILabel>("center/" + i + "/left/title");
				vipGiftTitle = FindInChild<UILabel>("center/" + i + "/right/gift");
				btnGetAward = FindInChild<Button>("center/" + i + "/right/btn_lq");
				btn_bg = FindInChild<UISprite>("center/" + i + "/right/btn_lq/background");
				awardWord = FindInChild<UILabel>("center/" + i  + "/right/btn_lq/lk");
				desObj = FindInChild<Transform>("center/" + i + "/left/des").gameObject;
				ShowPannel[i-1] = new ITEM();
				ShowPannel[i - 1].pannel = go.gameObject;
				ShowPannel[i - 1].vipTitle = vipTitle1;   //等级标题
				ShowPannel[i - 1].vipTequan = vipGiftTitle;   //特权标题
				ShowPannel[i - 1].btnAward = btnGetAward;
				ShowPannel[i - 1].btnBg = btn_bg;
				ShowPannel[i - 1].btnWord = awardWord;  //按钮上的文字
				//为领取按钮添加事件
				btnGetAward.onClick = BtnbtnGetAward;
				btn_bg = FindInChild<UISprite>("center/" + i + "/right/btn_lq/background");
				vipTitle1 = FindInChild<UILabel>("center/" + i  + "/left/title");
				vipGiftTitle = FindInChild<UILabel>("center/" + i  + "/right/gift");
				awardWord = FindInChild<UILabel>("center/" + i  + "/right/btn_lq/lk");
				showArea = FindInChild<Transform>("center/" + i);
				desItem = FindInChild<UILabel>("center/" + i +  "/left/des/1").gameObject;
				ShowPannel[i - 1].desItem = desItem;
				ShowPannel[i-1].desObj = desObj;
				ShowPannel[i - 1].AwardList.Clear();
				
				Transform [] trans = ShowPannel[i-1].desObj.GetComponentsInChildren<Transform>();
				foreach(Transform tran in trans)
				{
					if (tran.name.Contains("1"))
					{
						ShowPannel[i - 1].DesList.Add(tran.gameObject.GetComponent<UILabel>());
					}
				}
				for (int index2 = 1; index2 < 7; index2++)
				{
					AwardItem item = new AwardItem();
					item.obj = FindInChild<Transform>("center/" + i + "/right/wp/" + index2.ToString()).gameObject;
					item.ItemBtn = FindInChild<Button>("center/" + i + "/right/wp/" + index2.ToString());
					item.name = FindInChild<UILabel>("center/" + i + "/right/wp/" + index2.ToString() + "/mz");
					item.count = FindInChild<UILabel>("center/" + i + "/right/wp/" + index2.ToString() + "/geshu");
					item.icon = FindInChild<UISprite>("center/" + i + "/right/wp/" + index2.ToString() + "/icon");
					item.icon.GetComponent<TweenPosition>().enabled = false;
					ShowPannel[i - 1].AwardList.Add(item);
				}
			}
		}

		private void initClick()
		{
			//添加点击事件
			btnClose.onClick = BtnClose;
			leftVIP.onClick  = BtnleftVIP;
			rightVIP.onClick = BtnrightVIP;
			btnPay.onClick   = ChongZhiBtn;
		}

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            Singleton<VIPMode>.Instance.ApplyVIPInfo();  //请求VIP信息
        }
 
        public override void RegisterUpdateHandler()
        {
            base.RegisterUpdateHandler();
            Singleton<VIPMode>.Instance.dataUpdated += UpdateVipInfo;
            Singleton<VIPMode>.Instance.dataUpdated += UpdateVipAward; //更新VIP奖励
        }
        
        //服务器返回VIP的基本信息
        private void UpdateVipInfo(object sender, int code)
        {
            if (code == Singleton<VIPMode>.Instance.UPDATE_VIP_INFO)
            {
                SetBtnWord(Singleton<VIPMode>.Instance.vipInfoMsg.vip);
                UpdateVIPInfo();
				for (int i = 0; i < (int)VIPConsts.PAGEBOUNDARY_13; i++)
                {
                    ShowPannel[i].btnAward.gameObject.SetActive(false);
                    ShowVIPDescribe(i);   //依次显示每一个面板上的详细信息
                }
                //定位到当前的VIP等级页面
                LocateVIPPage();
            }
        }

        public override void Update()
        {
            HindLeftOrRightBtn(pageIndex.current-1);
            //进度条数字
            if (payProcess && Singleton<VIPMode>.Instance.vipInfoMsg != null)
            {
				payProcess.value = ((float)Singleton<VIPMode>.Instance.vipInfoMsg.totalDiam) / (float)(VIPUtil.GetNextVIPPrice(Singleton<VIPMode>.Instance.vipInfoMsg.vip));

                if (payProcess.value != 0f)
				{
                    sliderBg.gameObject.SetActive(true);
				}
            }
        }

        /// <summary>
        /// 定位到当前VIP的那一页
        /// </summary>
        private  void LocateVIPPage()
        {
			int dstPage = Singleton<VIPMode>.Instance.vipInfoMsg.vip;
			centerOnChild.CenterOn(centerOnChild.transform.GetChild(dstPage));
        }

        private void SetBtnWord(int vipLevel)
        {
            if (vipLevel != Singleton<VIPMode>.Instance.vipInfoMsg.vip|| Singleton<VIPMode>.Instance.vipInfoMsg.vip ==0)
            {
                ShowPannel[vipLevel].btnAward.gameObject.SetActive(false);
            }
            if (Singleton<VIPMode>.Instance.vipInfoMsg.rewardStatus == (int)VIPConsts.GetStatus.UuReceive)
            {
                ShowPannel[vipLevel].btnWord.text = LanguageManager.GetWord("VIPView.NoGet");
                ShowPannel[vipLevel].btnAward.GetComponent<BoxCollider>().enabled = false;
                ShowPannel[vipLevel].btnBg.color = Color.gray;
            }else if (Singleton<VIPMode>.Instance.vipInfoMsg.rewardStatus == (int)VIPConsts.GetStatus.HaveReveive)
            {
                ShowPannel[vipLevel].btnWord.text = LanguageManager.GetWord("VIPView.Got");//"已领取";
                ShowPannel[vipLevel].btnAward.GetComponent<BoxCollider>().enabled = false;
                ShowPannel[vipLevel].btnAward.gameObject.SetActive(true);
            }else if (Singleton<VIPMode>.Instance.vipInfoMsg.rewardStatus == (int)VIPConsts.GetStatus.Receive)  //可领取
            {
                ShowPannel[vipLevel].btnWord.text = LanguageManager.GetWord("VIPView.Get1"); // "领取";
                ShowPannel[vipLevel].btnAward.GetComponent<BoxCollider>().enabled = true;
                ShowPannel[vipLevel].btnAward.gameObject.SetActive(true);
            }
        }

        //更新VIP奖励
        private void UpdateVipAward(object sender, int code)
        {
            if (code == Singleton<VIPMode>.Instance.UPDATE_VIP_AWARD)
            {
                MessageManager.Show(LanguageManager.GetWord("LoginAward.AwardSuccess"));
                MessageManager.SetWarmFalse();   //将前面的叉叉改为对勾
                UpdateVIPInfo();   //更新当前VIP状态
            }
        }
        
        //更新自己的VIP基本信息
        private void UpdateVIPInfo()
        {
			setCurrentNextVipText();
			setVipAmountDifference();
			setPayProcess();
        }

		//设置当前和下一级VIP
		private void setCurrentNextVipText()
		{
			vipLevel1.text = Singleton<VIPMode>.Instance.vipInfoMsg.vip.ToString();
			vipLevel2.text = (Singleton<VIPMode>.Instance.vipInfoMsg.vip+1).ToString();
		}

		//相邻两个VIP的金额差
		private void setVipAmountDifference()
		{
			int priceCha = VIPUtil.GetNextVIPPrice(Singleton<VIPMode>.Instance.vipInfoMsg.vip) - (int)Singleton<VIPMode>.Instance.vipInfoMsg.totalDiam;
			if (priceCha < 0)
			{
				priceCha = 0;
			}
			payTip.text = LanguageManager.GetWord("VIPView.PayTip", priceCha.ToString());
			payTotal.text = LanguageManager.GetWord("VIPView.PayTotal", Singleton<VIPMode>.Instance.vipInfoMsg.totalDiam.ToString());
		}

		//修改进度条
		private void setPayProcess()
		{
			if (Singleton<VIPMode>.Instance.vipInfoMsg.vip >= 0)
			{
				if (Singleton<VIPMode>.Instance.vipInfoMsg.totalDiam >= VIPUtil.GetNextVIPPrice(Singleton<VIPMode>.Instance.vipInfoMsg.vip))
				{
					payProcess.value = 0.999f;
				}else
				{
					payProcess.value = ((float)Singleton<VIPMode>.Instance.vipInfoMsg.totalDiam+0.9f) / (float)(VIPUtil.GetNextVIPPrice(Singleton<VIPMode>.Instance.vipInfoMsg.vip));
				}
			}
			if (Singleton<VIPMode>.Instance.vipInfoMsg.vip == VIPConsts.PAGEBOUNDARY_13)
			{
				payProcess.value = 1f;
				objNextVIP.gameObject.SetActive(false);  //不显示下一级VIP的信息
			}
			processDes.text = Singleton<VIPMode>.Instance.vipInfoMsg.totalDiam.ToString() + "/" + VIPUtil.GetNextVIPPrice(Singleton<VIPMode>.Instance.vipInfoMsg.vip).ToString();
		}
        
        
        //显示VIP描述信息
        private void ShowVIPDescribe(int vipLevel)
        {
			setVipTitle(vipLevel);
			setVipDescribe(vipLevel);
			setVipAwardList(vipLevel);
		}

		//呈现的VIP的等级
		private void setVipTitle(int vipLevel)
		{
            ShowVIP = vipLevel;    
            ShowPannel[vipLevel].vipTitle.text = LanguageManager.GetWord("VIPView.Right", vipLevel.ToString());
            ShowPannel[vipLevel].vipTequan.text = LanguageManager.GetWord("VIPView.RightGift", vipLevel.ToString());
            //更新按钮上的信息
            if (Singleton<VIPMode>.Instance.vipInfoMsg.vip != vipLevel)   //显示的大于我的VIP等级
            {
                ShowPannel[vipLevel].btnAward.GetComponent<BoxCollider>().enabled = false;
            }else if (Singleton<VIPMode>.Instance.vipInfoMsg.vip == vipLevel && Singleton<VIPMode>.Instance.vipInfoMsg.vip != 0)
            {
                ShowPannel[vipLevel].btnAward.GetComponent<BoxCollider>().enabled = true;
                ShowPannel[vipLevel].btnAward.gameObject.SetActive(true);
                SetBtnWord(vipLevel);
            }
		}
                 
        //显示该VIP的描述信息
		private void setVipDescribe(int vipLevel)
		{
            SysVipInfoVo vipinfo = BaseDataMgr.instance.GetVIPDescribe((uint)vipLevel);
            string[] s = vipinfo.describe.Split(new char[] { '\r', '\n' });
            List<string> s1 = new List<string>();
            int m = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if(s[i] != "")
                {
                    s1.Add(s[i]);
                    m++;
                }
            }
            for (int i = 0; i < s1.Count && i < ShowPannel[vipLevel].DesList.Count; i++)
            {
                ShowPannel[vipLevel].DesList[i].text = s1[i];
            }
            for (int i = s1.Count; i < ShowPannel[vipLevel].DesList.Count; i++)
            {
                ShowPannel[vipLevel].DesList[i].text = "";
            }
		}

        //显示VIP的礼包信息
		private void setVipAwardList(int vipLevel)
		{
			setVipAwardGetBtn(vipLevel);
			setVipAwardItem(vipLevel);
		}

		private void setVipAwardGetBtn(int vipLevel)
		{
            if (vipLevel == 0)
            {
                foreach (AwardItem item in ShowPannel[vipLevel].AwardList)
                {
                    item.obj.SetActive(false);
                }
                btnGetAward.gameObject.SetActive(false);
            }else if (vipLevel != Singleton<VIPMode>.Instance.vipInfoMsg.vip)
            {
                btnGetAward.gameObject.SetActive(false);
            }else
            {
                btnGetAward.gameObject.SetActive(true);
            }
		}

		private void setVipAwardItem(int vipLevel)
		{
			if(vipLevel != 0)
			{
				SysGiftVo gift = BaseDataMgr.instance.GetGiftPack(1000002, vipLevel); //(uint)day);
	            int num = 0;
	            gift.goods = StringUtils.GetValueString(gift.goods);
	            string[] goods = StringUtils.GetValueListFromString(gift.goods);
	            for (int i = 0; i < goods.Length; i++)
	            {
	                goods[i] = goods[i].TrimStart('[');
	                goods[i] = goods[i].TrimEnd(']');
	                if (int.Parse(goods[i]) > 100)  //为ID
	                {
	                    ShowPannel[vipLevel].AwardList[num].obj.SetActive(true);
	                    ShowPannel[vipLevel].AwardList[num].icon.gameObject.SetActive(true);
	                    SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)int.Parse(goods[i]));
	                    ShowPannel[vipLevel].AwardList[num].name.text = vo.name;
	                    Singleton<ItemManager>.Instance.InitItem(ShowPannel[vipLevel].AwardList[num].obj.gameObject, (uint)int.Parse(goods[i]), 0);
	                    ShowPannel[vipLevel].AwardList[num].ItemBtn.onClick = BtnItem;  //点击了具体的物品
	                    ShowPannel[vipLevel].AwardList[num].itemID = vo.id;
	                }else
	                {
	                    ShowPannel[vipLevel].AwardList[num].count.text = "x" + goods[i].ToString();  //这个时候是物品的个数
	                    num++;
	                }
	            }
			}
        }

        //关闭界面
        public void BtnClose(GameObject go)
        {
            this.CloseView();
        }

        /// <summary>
        /// 根据当前页的数目是否要要隐藏左右的提示按钮
        /// </summary>
        private void HindLeftOrRightBtn(int presentPage)
        {
            if(presentPage == VIPConsts.PAGEMIN_0)
            {
                leftVIP.gameObject.SetActive(false);
            }else
            {
                leftVIP.gameObject.SetActive(true);
            }
            if(presentPage == VIPConsts.PAGEMAX_12)
            {
                rightVIP.gameObject.SetActive(false);
            }else
            {
                rightVIP.gameObject.SetActive(true);
            }
        }

        //向左翻页
        private void BtnleftVIP(GameObject go)
        {
            if (pageIndex.current != 1)
            {
                scrollView.MoveRelative(new Vector3(1000, 0, 0));
                centerOnChild.Recenter();
				pageIndex.InitPage(pageIndex.current, (int)VIPConsts.PAGEBOUNDARY_13);
            }
        }

        //向右翻页
        private void BtnrightVIP(GameObject go)
        {
			if (pageIndex.current != (int)VIPConsts.PAGEBOUNDARY_13)
            {
                scrollView.MoveRelative(new Vector3(-1000, 0, 0));
                centerOnChild.Recenter();
				pageIndex.InitPage(pageIndex.current, (int)VIPConsts.PAGEBOUNDARY_13);
            }
        }

		//充值的按钮
		private void ChongZhiBtn(GameObject go)
		{
			if (AppStart.RunMode == 1)
			{
				SDKManager.SDKPay(Singleton<LoginView>.Instance.Info.Id.ToString(), 0, "");
			}else if (AppStart.RunMode == 2)
			{
				SDKManager.SDK91Pay("10", "20", "123", "100钻石", "1",Singleton<LoginView>.Instance.Info.Id.ToString() ,"");
			}else if (AppStart.RunMode == 0)
			{
				MessageManager.Show(LanguageManager.GetWord("VIPView.topUp"));
			}
		}

        //领取奖励的按钮
        private void BtnbtnGetAward(GameObject go)
        {
            Singleton<VIPMode>.Instance.ApplyGetAward(); //发送领取奖励的请求
        }

        //点击了具体的物品
        private void BtnItem(GameObject go)
        {
			Singleton<TipsManager>.Instance.OpenTipsByGoodsId((uint)VIPUtil.GetItemID(go.name , ShowPannel , pageIndex), null, null, "", "", 0);
        }
    }
}