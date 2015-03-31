using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.vo;
using Com.Game.Module.Role;
using com.game.module.map;
using com.game.consts;
using com.game.Public.Confirm;
using com.game.ui;
using com.game.manager;
using Com.Game.Module.GoldSilverIsland;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.u3d.bases.debug;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/01/23 09:39:52 
 * function:  竞技场系统--战斗场景视图类
 * *******************************************************/
using com.game.module.SystemData;


namespace Com.Game.Module.Arena
{
	public class ArenaFightView : BaseView<ArenaFightView>
	{
		public override ViewLayer layerType {
			get { return ViewLayer.NoneLayer; }
		}

//		public override ViewType viewType
//		{
//			get { return ViewType.BattleView; }
//		}

		private Button btn_tx;
		private UILabel remainTimeLabel;

		private UILabel myHealthLabel;
		private UILabel myMagicLabel;
		private UISlider mySldHp;
		private UISlider mySldMagic;
		private UISprite myHeadSprite;
		private UILabel myLevel;
		private UILabel myName;

		private UILabel vserHealthLabel;
		private UILabel vserMagicLabel;
		private UISlider vserSldHp;
		private UISlider vserSldMagic;
		private UISprite vserHeadSprite;
		private UILabel vserLevel;
		private UILabel vserName;




		private uint remainTime;
				
		protected override void Init()
		{
			InitLabelLanguage ();
			btn_tx = FindInChild<Button>("btn_tg");
			remainTimeLabel = FindInChild<UILabel>("title/shijian");
			myHealthLabel = FindInChild<UILabel>("zuo/SldHp/Label");
			myMagicLabel = FindInChild<UILabel>("zuo/SldMagic/Label");
			mySldHp = FindInChild<UISlider>("zuo/SldHp");
			mySldMagic = FindInChild<UISlider>("zuo/SldMagic");
			myHeadSprite = FindInChild<UISprite>("zuo/PlayerInfo/PlayerHeadIcon");
			myLevel = FindInChild<UILabel>("zuo/PlayerInfo/PlayerLevelLabel");
			myName = FindInChild<UILabel>("zuo/PlayerInfo/PlayerNameLabel");

			vserHealthLabel = FindInChild<UILabel>("you/SldHp/Label");
			vserMagicLabel = FindInChild<UILabel>("you/SldMagic/Label");
			vserSldHp = FindInChild<UISlider>("you/SldHp");
			vserSldMagic = FindInChild<UISlider>("you/SldMagic");
			vserHeadSprite = FindInChild<UISprite>("you/PlayerInfo/PlayerHeadIcon");
			vserLevel = FindInChild<UILabel>("you/PlayerInfo/PlayerLevelLabel");
			vserName = FindInChild<UILabel>("you/PlayerInfo/PlayerNameLabel");


			btn_tx.onClick = LoseOnClick;
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("btn_tg/label").text = LanguageManager.GetWord("ArenaFightView.surrender");
		}
		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<MapMode>.Instance.dataUpdated += this.UpdateLeftTimeHandler;
			MeVo.instance.DataUpdated += UpdateMyHpMpHandler;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += this.UpdateRobbedPlayerHpMpHandler;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			Singleton<ArenaMode>.Instance.IsBattleEnd = false;
			UpdateLeftTime();
			InitPlayerInfo ();
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<MapMode>.Instance.dataUpdated -= this.UpdateLeftTimeHandler;
			MeVo.instance.DataUpdated -= UpdateMyHpMpHandler;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += this.UpdateRobbedPlayerHpMpHandler;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
			vp_Timer.CancelAll("UpdateTimeArena");
		}

		//投降按钮被点击
		private void LoseOnClick(GameObject go)
		{
			ConfirmMgr.Instance.ShowOkCancelAlert (LanguageManager.GetWord("ArenaFightView.surrenderEnsure"), ConfirmCommands.OK_CANCEL, 
			                                    Singleton<ArenaControl>.Instance.ChallengeFail);
		}

		//初始化战斗双方玩家状态
		private void InitPlayerInfo()
		{
			Singleton<ArenaMode>.Instance.vsPlayerAttr.CurHp = Singleton<ArenaMode>.Instance.vsPlayerAttr.Hp;
			Singleton<ArenaMode>.Instance.vsPlayerAttr.CurMp = Singleton<ArenaMode>.Instance.vsPlayerAttr.Mp;
			MeVo.instance.CurHp = MeVo.instance.Hp;
			MeVo.instance.CurMp = MeVo.instance.Mp;
			UpdateOtherInfo ();
			UpdateMyInfo ();
			UpdatePlayersInfo();
		}

		private void UpdatePlayersInfo()
		{
			//更新我的信息        
			myHeadSprite.spriteName = MeVo.instance.job + "001";
			myLevel.text = MeVo.instance.Level.ToString();
			myName.text = MeVo.instance.Name;

			//更新对方信息
			if (MapTypeConst.ARENA_MAP == MeVo.instance.mapId)
			{
				vserHeadSprite.spriteName =Singleton<ArenaMode>.Instance.vsPlayerAttr.job + "001";
				vserLevel.text = Singleton<ArenaMode>.Instance.vsPlayerAttr.Level.ToString();
				vserName.text = Singleton<ArenaMode>.Instance.vsPlayerAttr.Name;
			}
			else if (MapTypeConst.GoldSilverIsland_MAP == MeVo.instance.mapId)
			{			
				RobbedPlayer robbedPlayer = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayer;
				vserHeadSprite.spriteName = robbedPlayer.Vo.job + "001";
				vserLevel.text = robbedPlayer.Vo.Level.ToString();
				vserName.text = robbedPlayer.Vo.Name;
			}
		}

		private void UpdateOtherInfo()
		{
			if (MapTypeConst.ARENA_MAP == MeVo.instance.mapId)
			{
				UpdateVserInfo();
			}
			else if (MapTypeConst.GoldSilverIsland_MAP == MeVo.instance.mapId)
			{
				UpdateRobbedPlayerInfo();
			}
		}

		//--------------- 更新剩余时间相关代码--------------------//
		private void UpdateLeftTimeHandler(object sender, int code)
		{
			if (MapMode.EVENT_CODE_UPDATE_LEFTTIME == code)
			{
				UpdateLeftTime();
			}
		}

		// 更新副本剩余时间
		public void UpdateLeftTime()
		{
			if (MapMode.expire == 999999)
			{
				remainTimeLabel.gameObject.SetActive(false);
			} 
			else
			{
				remainTimeLabel.gameObject.SetActive(true);
				SetLeftTimeValue();
			}
			vp_Timer.CancelAll("UpdateBattleViewLeftTime");
			int leftSecond = MapMode.EndTimestamp - ServerTime.Instance.Timestamp;
			vp_Timer.In(1f, UpdateArenaFightViewLeftTime, leftSecond, 1f);
		}
		/// <summary>
		///     战斗UI剩余时间更新
		/// </summary>
		private void UpdateArenaFightViewLeftTime()
		{
			SetLeftTimeValue();
		}
		
		private void SetLeftTimeValue()
		{
			int leftSecond = MapMode.EndTimestamp - ServerTime.Instance.Timestamp;
			if (leftSecond < 0) leftSecond = 0;
			int min = leftSecond / 60;
			string minstr = min < 10 ? "0" + min : min + "";
			int sec = leftSecond % 60;
			string secstr = sec < 10 ? "0" + sec : sec + "";
			remainTimeLabel.text = minstr + ":" + secstr;
		}
		

		//---------------------- 更新本方血蓝相关代码 ---------------------//
		/// <summary>
		/// 更新信息 
		/// </summary>
		public void UpdateMyInfo()
		{
			UpdateMyHealth((int)MeVo.instance.CurHp, (int)MeVo.instance.Hp);
			UpdateMyMagic((int)MeVo.instance.CurMp, (int)MeVo.instance.Mp);
		}
		/// <summary>
		/// 更新玩家血量
		/// </summary>
		/// <param name="curHp">当前血量</param>
		/// <param name="hp">总血量</param>
		public void UpdateMyHealth(int curHp, int hp)
		{
			if (curHp < 0) curHp = 0;
			myHealthLabel.text = curHp + "/" + hp;
			TweenSlider.Begin(mySldHp.gameObject, 0.5f, (float)curHp /hp);
		}

		/// <summary>
		/// 更新玩家魔法
		/// </summary>
		/// <param name="curMp">当前魔法</param>
		/// <param name="mp">总魔法</param>
		public void UpdateMyMagic(int curMp, int mp)
		{
			if (curMp < 0) curMp = 0;
			myMagicLabel.text = curMp + "/" + mp;
			TweenSlider.Begin(mySldMagic.gameObject, 0.5f, (float)curMp /mp);
		}

		//蓝血更新回调
		private void UpdateMyHpMpHandler(object sender, int code)
		{
			if (MeVo.DataHpMpUpdate == code)
			{
				UpdateMyInfo();
			}
		}

		//---------------------- 更新敌方血蓝相关代码 ---------------------//
		/// <summary>
		/// 更新信息 
		/// </summary>
		public void UpdateVserInfo()
		{
			UpdateVserHealth((int)Singleton<ArenaMode>.Instance.vsPlayerAttr.CurHp, (int)Singleton<ArenaMode>.Instance.vsPlayerAttr.Hp);
			UpdateVserMagic((int)Singleton<ArenaMode>.Instance.vsPlayerAttr.CurMp, (int)Singleton<ArenaMode>.Instance.vsPlayerAttr.Mp);
		}

		/// <summary>
		/// 更新玩家血量
		/// </summary>
		/// <param name="curHp">当前血量</param>
		/// <param name="hp">总血量</param>
		public void UpdateVserHealth(int curHp, int hp)
		{
			if (curHp < 0) curHp = 0;
			vserHealthLabel.text = curHp + "/" + hp;
			TweenSlider.Begin(vserSldHp.gameObject, 0.5f, (float)curHp /hp);
		}
		
		/// <summary>
		/// 更新玩家魔法
		/// </summary>
		/// <param name="curMp">当前魔法</param>
		/// <param name="mp">总魔法</param>
		public void UpdateVserMagic(int curMp, int mp)
		{
			if (curMp < 0) curMp = 0;
			vserMagicLabel.text = curMp + "/" + mp;
			TweenSlider.Begin(vserSldMagic.gameObject, 0.5f, (float)curMp /mp);
		}

		//////////////////////////////////////////////////////////////////////////// 
		private void UpdateRobbedPlayerHpMpHandler(object sender, int code)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_PLAYER_ROB_HPMP == code)
			{
				UpdateRobbedPlayerInfo();
			}
		}

		public void UpdateRobbedPlayerInfo()
		{
			RobbedPlayer robbedPlayer = Singleton<GoldSilverIslandMode>.Instance.RobbedPlayer;
			UpdateVserHealth((int)robbedPlayer.Vo.CurHp, (int)robbedPlayer.Vo.Hp);
			UpdateVserMagic((int)robbedPlayer.Vo.CurMp, (int)robbedPlayer.Vo.Mp);
		}


		public override void Update ()
		{
			base.Update ();

			if (MapTypeConst.ARENA_MAP == MeVo.instance.mapId)
			{
				UpdateVserInfo();
			}
		}
	}
}