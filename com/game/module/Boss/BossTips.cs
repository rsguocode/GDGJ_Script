using Com.Game.Module.Role;
using UnityEngine;
using System.Collections;

using com.u3d.bases.debug;
using com.game.consts;
using com.game.module.test;
using com.game.manager;
using com.game.Public.Confirm;
using com.game.utils;

namespace Com.Game.Module.Boss
{
	public class BossTips : BaseView<BossTips> 
	{
		public override ViewLayer layerType 
		{
			get {return ViewLayer.HighLayer;}
		}

		//private GameObject pm;//排名
		//private Button btn_pm;//
		//private UILabel pmTips1;//
		//private UILabel pmTips2;//

		private GameObject win;// 战胜
		private Button btn_win;//
		private UILabel timeTips1;
		private UILabel winTips;//

		private GameObject ydfh;//原地复活
		private Button btn_yd;//
		private UILabel timeTips2;//
		private UILabel ydTips;

		private int tipsId;//1:排名 2:战胜 3：原地复活

		private int leftTime;
		private EventDelegate.Callback callBack;

		protected override void Init()
		{
			//pm = FindChild("pm");
			//btn_pm = FindInChild<Button>("pm/btn_zlbb");
			//btn_pm.onClick = CommitOnClick;
			//btn_pm.FindInChild<UILabel>("label").text = LanguageManager.GetWord("Boss.Commit");

			//pmTips1 = FindInChild<UILabel>("pm/zi1");
			//pmTips2 = FindInChild<UILabel>("pm/zi2");

			win = FindChild("win");
			btn_win = FindInChild<Button>("win/btn_zlbb");
			btn_win.onClick = CommitOnClick;
			btn_win.FindInChild<UILabel>("label").text = LanguageManager.GetWord("Boss.Commit");
			timeTips1 = FindInChild<UILabel>("win/zi2");
			winTips = FindInChild<UILabel>("win/zi1");

			ydfh = FindChild("ydfh");
			btn_yd = FindInChild<Button>("ydfh/btn_zlbb");
			btn_yd.onClick = CommitOnClick;
			btn_yd.FindInChild<UILabel>("label").text = LanguageManager.GetWord("Boss.Survive");
			timeTips2 = FindInChild<UILabel>("ydfh/zi2");
			ydTips = FindInChild<UILabel>("ydfh/zi1");
		}

		/*public void OpenPMView(int rank,int yb,int sw,EventDelegate.Callback callBack)
		{
			this.callBack = callBack;
			pmTips1.text = string.Format(LanguageManager.GetWord("Boss.RankAward"),
			                             ColorUtils.GetColorText(ColorConst.YELLOW,string.Empty+rank));
			pmTips2.text = string.Format(LanguageManager.GetWord("Boss.AwardTotal"),
			                             ColorUtils.GetColorText(ColorConst.YELLOW,string.Empty+yb),ColorUtils.GetColorText(ColorConst.YELLOW,string.Empty+sw));
			tipsId = 1;
			OpenView();

		}*/
        /// <summary>
        /// 排名
        /// </summary>
		public void OpenPMView()
		{
			int repu = (int)Singleton<BossMode>.Instance.winRepu;
			int sliver = (int)Singleton<BossMode>.Instance.winSiler;
			ConfirmMgr.Instance.ShowOkAlert(LanguageManager.GetWord("Boss.RankAward")+"\n"+ string.Format(LanguageManager.GetWord("Boss.AwardTotal"),
			                                     ColorUtils.GetColorText(ColorConst.YELLOW,string.Empty+ sliver),ColorUtils.GetColorText(ColorConst.YELLOW,string.Empty+ repu)));
			Singleton<BossMode>.Instance.isDie = false;
		}
        /// <summary>
        /// 世界Boss死亡
        /// </summary>
        /// <param name="name"></param>
        /// <param name="left"></param>
        /// <param name="callBack"></param>
		public void OpenWNView(string name , int left,EventDelegate.Callback callBack)
		{
			this.callBack = callBack;
			winTips.text = string.Format(LanguageManager.GetWord("Boss.Win"),
			                             ColorUtils.GetColorText(ColorConst.YELLOW,string.Empty+name));
			timeTips1.text = string.Format(LanguageManager.GetWord("Boss.TimeTips"),
			                               ColorUtils.GetColorText(ColorConst.RED,string.Empty+left));
			leftTime = left;
			tipsId = 2;
			OpenView();
		}
        /// <summary>
        /// 原地复活
        /// </summary>
        /// <param name="name"></param>
        /// <param name="left"></param>
        /// <param name="callBack"></param>
		public void OpenYDView(string name , int left,EventDelegate.Callback callBack)
		{
			this.callBack = callBack;
			ydTips.text = string.Format(LanguageManager.GetWord("Boss.Defeated"),
			                            ColorUtils.GetColorText(ColorConst.YELLOW,name + "\n"));
			timeTips1.text = string.Format(LanguageManager.GetWord("Boss.TimeTips"),
			                               ColorUtils.GetColorText(ColorConst.RED,string.Empty+left));
			leftTime = left;
			tipsId = 3;
			OpenView();
		}

		protected override void HandleAfterOpenView()
		{
			//pm.SetActive(false);
			win.SetActive(false);
			ydfh.SetActive(false);
		    this._isClick = false;
            Singleton<RoleMode>.Instance.dataUpdated += UpdateRoleHandle;
			//if(tipsId == 1)
			//{
				//pm.SetActive(true);
			//}
			if (tipsId == 2)
			{
				win.SetActive(true);
				vp_Timer.In(0f,LeftTimeSchedule1,(int)this.leftTime + 1,1f);
			}
			else 
			{
				ydfh.SetActive(true);
				vp_Timer.In(0f,LeftTimeSchedule1,(int)this.leftTime + 1 ,1f);
			}
		}

		private void LeftTimeSchedule1()
		{
			
			if(tipsId == 2)
				timeTips1.text = string.Format(LanguageManager.GetWord("Boss.TimeTips"),
				                               ColorUtils.GetColorText(ColorConst.RED,string.Empty+leftTime));
			if(tipsId == 3)
				timeTips2.text = string.Format(LanguageManager.GetWord("Boss.TimeTips"),
				                               ColorUtils.GetColorText(ColorConst.RED,string.Empty+leftTime));
			if(leftTime ==0)
			{
			    if (tipsId == 2)
			    {
                    CloseView();
			    }
			    if (tipsId == 3)
			    {
			        callBack = null;
                    RoleMode.Instance.ReLife(MapTypeConst.ROLE_REVIVE_NO_MONEY);  //时间到不用钻石原地复活
                    Log.info(this, "时间到不用钻石原地复活");
			    }
			}
            leftTime--;
		}

		protected override void HandleBeforeCloseView()
		{
			vp_Timer.CancelAll("LeftTimeSchedule1");
			Log.info(this," call Back quit :   sdssdsdsdsds");
			if(callBack !=null && !_isClick)
			{
				Log.info(this," call Back quit :  ");
				callBack();
			}
            Singleton<RoleMode>.Instance.dataUpdated -= UpdateRoleHandle;
		}

		public void ForceCloseView()
		{
			callBack = null;
			vp_Timer.CancelAll("LeftTimeSchedule1");
			CloseView ();
		}

	    private bool _isClick = false;
		private void CommitOnClick(GameObject go)
		{
            _isClick = true;
		    if (callBack != null)
		    {
		        callBack();
		    }
		}


        private void UpdateRoleHandle(object sender, int code)
        {
            if (code == Singleton<RoleMode>.Instance.UPDATE_ROLE_RELIFE)  //复活之后推出
            {
               CloseView();
            }
        }


	}
}
