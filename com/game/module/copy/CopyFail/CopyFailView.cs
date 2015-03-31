using Com.Game.Module.Role;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.module.map;
using com.u3d.bases.debug;
using com.game;
using com.game.consts;
using System.IO;
using Proto;
using com.game.data;
using com.game.manager;
using com.game.utils;
using com.game.Public.Confirm;
using com.game.Public.Message;
using com.game.vo;
using com.game.module.battle;
using Com.Game.Module.GoGo;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  副本失败视图
 * *******************************************************/
using com.game.module.SystemData;


namespace Com.Game.Module.Copy
{
	public class CopyFailView : BaseView<CopyFailView>
	{
		private static int REMAIN_TIME = 5;   //副本时间到失败界面维持时间
		private static int REMAIN_TIME_DEATH = 15;   //人物死亡失败界面维持时间
		public override string url { get { return "UI/Copy/CopyFailView.assetbundle"; } }
		public override ViewLayer layerType
		{
			get { return ViewLayer.MiddleLayer; }
		}

		private UILabel failReason;
		private UILabel btnRightLabel;
		private UILabel btnLeftLabel;
		private UILabel reliveMoney;
		private Button btnRight;
		private Button btnLeft;
		private UILabel remainTimeValue;

		private int _failReason;
		public int FailReason{ set{_failReason = value;} }
		private float delTime = 0.0f;
		private int remainTime = REMAIN_TIME;
		protected override void Init()
		{
			InitLabel ();
			failReason = FindInChild<UILabel>("failReason");
			btnRightLabel = FindInChild<UILabel>("btn_right/label");
			btnLeftLabel = FindInChild<UILabel>("btn_left/label");
			btnRight = FindInChild<Button>("btn_right");
			btnLeft = FindInChild<Button>("btn_left");
			remainTimeValue = FindInChild<UILabel>("QuitCopy/value");
			reliveMoney = FindInChild<UILabel>("btn_right/money/value");

		}

		//静态语言处理
		private void InitLabel()
		{

		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<RoleMode>.Instance.dataUpdated += UpdateRoleState;
//			UPDATE_ROLE_RELIFE
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			delTime = 0.0f;
			switch (_failReason)
			{
			case (int)CopyFailReason.TIME_OVER:
				remainTime = REMAIN_TIME;
				break;
			case (int)CopyFailReason.DEATH:
				remainTime = REMAIN_TIME_DEATH;
				break;
			default:
				break;
			}
			remainTimeValue.text = remainTime.ToString ();
			this.ShowCopyFailView(this._failReason);

			Log.info(this, "通关失败UI打开，游戏暂停");
			if (_failReason != (int)CopyFailReason.TIME_OVER)
				this.GamePause ();
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<RoleMode>.Instance.dataUpdated -= UpdateRoleState;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
			if (_failReason != (int)CopyFailReason.TIME_OVER)
				this.GameResume ();
		}

		private void UpdateRoleState(object send, int code)
		{
			if (code == Singleton<RoleMode>.Instance.UPDATE_ROLE_RELIFE)
			{
				this.CloseView();
			}
		}

		public override void Update ()
		{
			base.Update ();
			delTime += Time.deltaTime;
			if (delTime > 1.0f)
			{
				delTime--;
				remainTime--;
				remainTimeValue.text = remainTime.ToString();
				if (remainTime <= 0)
				{
					switch (_failReason)
					{
					case (int)CopyFailReason.TIME_OVER:
						this.CloseView ();
						Singleton<CopyControl>.Instance.CopyEnd ();
						break;
					case (int)CopyFailReason.DEATH:
						this.ReviveReturnCity();
						break;
					}
				}
			}
		}

		//更新副本剩余时间
		private void ShowRemainTime()
		{
			string reaminTimeStr;
			int remainTime = MapMode.EndTimestamp - ServerTime.Instance.Timestamp;
			if (remainTime < 0) remainTime = 0;
			int min = remainTime / 60;
			int sec = remainTime % 60;
			reaminTimeStr = (min < 10? "0"+min: min.ToString()) + ":" + (sec < 10? "0"+sec: ""+sec.ToString());
			failReason.text = "[7EBEE3]少年不哭,要不要站起来继续撸？[-]" + "\n\n" + "[FFD800]副本通关时间剩余：" + reaminTimeStr +"[-]";
		}

		//展示副本失败界面UI效果
		public void ShowCopyFailView(int failId)
		{
			switch (failId)
			{
				case (int)CopyFailReason.TIME_OVER:
										
					failReason.text = "[7EBEE3]很遗憾，时间到了，你还是没通关[-]";
					btnLeftLabel.text = "回城";
					btnLeft.onClick = TimeOver;
					Singleton<GoGoView>.Instance.CloseView();//玩家在今日场景没有移动的情况
					btnRight.gameObject.SetActive(false);
					btnLeft.gameObject.SetActive(true);
					btnLeft.transform.localPosition = new Vector3(0, btnLeft.transform.localPosition.y, btnLeft.transform.localPosition.z);
					break;
				case (int)CopyFailReason.DEATH:
					ShowRemainTime();
					SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.TypeRelive);
					string diam = StringUtils.GetValueListFromString(priceVo.diam)[0];
					reliveMoney.text = diam;
					btnRightLabel.text = "复活";
					btnLeftLabel.text = "回城";
					btnLeft.onClick = ReviveReturnMainCity;
					btnRight.onClick = ReviveNow;
					
					btnRight.gameObject.SetActive(true);
					btnLeft.gameObject.SetActive(true);
					btnLeft.transform.localPosition = new Vector3(-120, btnLeft.transform.localPosition.y, btnLeft.transform.localPosition.z);
					btnRight.transform.localPosition = new Vector3(120, btnRight.transform.localPosition.y, btnRight.transform.localPosition.z);
					break;
				default:
					break;
			}
		}

		//游戏暂停
		private void GamePause()
		{
			Singleton<MapMode>.Instance.StopLeftTime ();
			Singleton<CopyMode>.Instance.PauseCopy ();
		}
		//游戏恢复
		private void GameResume()
		{
			Singleton<CopyMode>.Instance.ResumeCopy ();
		}

		//时间到按键触发
		private void TimeOver(GameObject go)
		{
			this.CloseView ();
			Singleton<CopyControl>.Instance.CopyEnd ();
		}

		// 原地复活
		private void ReviveNow(GameObject go)
		{
			SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.TypeRelive);
			string diam = StringUtils.GetValueListFromString(priceVo.diam)[0];
			if (uint.Parse(diam) > MeVo.instance.diamond)
			{
				MessageManager.Show(LanguageManager.GetWord("PlayerBattleControler.DiamNotEnough"));
			}
			else
			{
				RoleMode.Instance.ReLife(MapTypeConst.ROLE_REVIVE_USE_DIAM);
//				this.GameResume ();
//				this.CloseView();
			}
		}
		
		// 回城复活
		private void ReviveReturnMainCity(GameObject go)
		{
//			this.CloseView ();
			this.ReviveReturnCity();
		}

		
		// 回城复活
		public void ReviveReturnCity()
		{
			AppMap.Instance.me.Controller.ContCutMgr.StopAll();
			Singleton<RoleMode>.Instance.ReLife (MapTypeConst.ROLE_REVIVE_RETURN_CITY);
		}
	}
	
}
