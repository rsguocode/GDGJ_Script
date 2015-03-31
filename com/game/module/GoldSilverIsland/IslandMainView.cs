//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：IslandMainView
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;
using com.game.module.effect;
using PCustomDataType;
using com.game.vo;
using com.game.data;
using com.game.Public.Confirm;
using com.game.Public.Message;
using com.game.module.map;
using com.game.consts;
using com.game.utils;
using com.game.module.effect;

namespace Com.Game.Module.GoldSilverIsland
{
	public class IslandMainView : BaseView<IslandMainView>  
	{		
		public override string url { get { return "UI/GoldSilverIsland/IslandMainView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}	

		public Button btnBack;
		private Button btnSearchDiam;
		private Button btnRefresh;
		private Button btnClearAdventCD;
		private Button btnClearRobCD;

		private const int maxShips = 20;
		private Button[] btnShips = new Button[maxShips];
		private IDictionary<uint, int> shipsRemainTime = new Dictionary<uint, int>();
		private uint[] playerIds = new uint[maxShips];
		private Vector3[] shipsPosition = new Vector3[maxShips];
		private int adventLeftTime = 0;
		private int robLeftTime = 0;
		private byte adventRemainCnt;

		private UILabel labAdventRemain;
		private UILabel labAdventRemainCnt;
		private UILabel labAdventRemainTime;

		private UILabel labRobRemain;
		private UILabel labRobRemainCnt;
		private UILabel labRobRemainTime;

		private UILabel labAssistRemain;
		private UILabel labAssistRemainCnt;

		private const byte iconOpen = 1;
		private const byte iconClosed = 0;

		private const float startX = -326f;
		private const float endX = 400f;
		private const int invalidSlot = -1;

		private GameObject seaWaterObj;
		private PWoodsPlayer me;
		private int myShipSlot = -1;
		private bool firstOpenFlag = false;
		private bool viewOpened = false;
		private Vector3 targetPos;
		private bool startBlast = false;
		private const float blastSpeed = 800;

		private float prevTime = Time.time;

		protected override void Init()
		{
			btnBack = FindInChild<Button>("center/btn_fanhui");
			btnSearchDiam = FindInChild<Button>("center/btn_cz");
			btnRefresh = FindInChild<Button>("center/btn_fs");

			btnClearAdventCD = FindInChild<Button>("center/cs/nr/xbcs/btn_js");
			btnClearRobCD = FindInChild<Button>("center/cs/nr/jccs/btn_js");

			labAdventRemain = FindInChild<UILabel>("center/cs/nr/xbcs/xunbao");
			labAdventRemainCnt = FindInChild<UILabel>("center/cs/nr/xbcs/shuzi");
			labAdventRemainTime = FindInChild<UILabel>("center/cs/nr/xbcs/shijian");

			labRobRemain = FindInChild<UILabel>("center/cs/nr/jccs/xunbao");
			labRobRemainCnt = FindInChild<UILabel>("center/cs/nr/jccs/shuzi");
			labRobRemainTime = FindInChild<UILabel>("center/cs/nr/jccs/shijian");

			labAssistRemain = FindInChild<UILabel>("center/cs/nr/xzcs/xzcs");
			labAssistRemainCnt = FindInChild<UILabel>("center/cs/nr/xzcs/shuzi");

			for (int i=1; i<=maxShips; i++)
			{
				btnShips[i-1] = FindInChild<Button>("center/tu/chuan" + i.ToString());
				btnShips[i-1].SetActive(false);
				btnShips[i-1].onClick = ShipOnClick;
				InitShipLocalY(i-1);
				playerIds[i-1] = 0;
				shipsPosition[i-1] = btnShips[i-1].transform.localPosition;
			}
			
			btnBack.onClick = BackOnClick;
			btnSearchDiam.onClick = SearchDiamOnClick;
			btnRefresh.onClick = RefreshOnClick;
			btnClearAdventCD.onClick = ClearAdventCDOnClick;
			btnClearRobCD.onClick = ClearRobCDOnClick;

			ShowSearchButton(false);			
			InitLabel();

			//延时设置Layer
			vp_Timer.In(0.5f, SetCenterToLayerMode, 1, 0);
		}

		private void InitLabel()
		{
			labAdventRemain.text = LanguageManager.GetWord("GoldSilverIsland.AdventRemain");
			labRobRemain.text = LanguageManager.GetWord("GoldSilverIsland.RobRemain");
			labAssistRemain.text = LanguageManager.GetWord("GoldSilverIsland.AssistRemain");
			btnSearchDiam.label.text = LanguageManager.GetWord("GoldSilverIsland.StartAdvent");
			btnRefresh.label.text = LanguageManager.GetWord("GoldSilverIsland.Refresh");
		}

		//船只Y坐标均匀分布
		private void InitShipLocalY(int index)
		{
			float centerY = -8f;
			float MaxH = 148f;
			float[] shipsYPosRatio = {0f, 0.5f, -0.5f, 1f, -1f, 0.25f, -0.25f, 0.75f, -0.75f, 0.35f, -0.35f, 0.85f, -0.85f, 0.15f, -0.15f, 0.65f, -0.65f, 0.45f, -0.45f, 0.92f};
			Vector3 pos = btnShips[index].transform.localPosition;
			pos.y = centerY + shipsYPosRatio[index]*MaxH;
			btnShips[index].transform.localPosition = pos;
		}

		//获得玩家的船槽号
		private int GetShipSlot(uint playerId)
		{
			//看玩家是否已分配船槽号
			for (int i=0; i<maxShips; i++)
			{
				if (playerIds[i] == playerId)
				{
					return i;
				}
			}

			//如果玩家还没有分配船槽号，分配一个最小的空闲号
			for (int i=0; i<maxShips; i++)
			{
				if (0 == playerIds[i])
				{
					playerIds[i] = playerId;
					return i;
				}
			}

			//如果船槽号已分配满，则不能再分配
			return invalidSlot;
		}

		//删除船槽号
		private void DelShipSlot(uint playerId)
		{
			for (int i=0; i<maxShips; i++)
			{
				if (playerIds[i] == playerId)
				{
					playerIds[i] = 0;
					return;
				}
			}
		}

		private void AddTweenPositionComponentForShip(int shipSlot)
		{
			GameObject go = btnShips[shipSlot].gameObject;

			if (null != go)
			{
				float[] floatY = {0f, 7f, 5f, 3f, 2f, 1f};
				float[] floatTime = {0f, 0.6f, 0.7f, 0.8f, 0.9f, 1f};
				uint playerId = playerIds[shipSlot];
				PWoodsPlayer player = Singleton<GoldSilverIslandMode>.Instance.GetPlayer(playerId);
				TweenPosition tweenPosition = NGUITools.AddMissingComponent<TweenPosition>(go);

				Vector3 fromPos = btnShips[shipSlot].transform.localPosition;
				fromPos.y = shipsPosition[shipSlot].y;

				tweenPosition.from = fromPos;
				tweenPosition.to = fromPos + new Vector3(0f, floatY[player.grade], 0f);
				tweenPosition.style = UITweener.Style.PingPong;
				tweenPosition.method = UITweener.Method.QuintEaseInOut;
				tweenPosition.duration = floatTime[player.grade];
			}
		}

		private void DelTweenPositionComponent(GameObject go)
		{
			if (null != go)
			{
				TweenPosition tweenPosition = go.GetComponent<TweenPosition>();
				if (null != tweenPosition)
				{
					GameObject.Destroy(tweenPosition);
				}	
			}
		}

		public override void OpenView()
		{
			if (null == EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_SeaWater))
			{
				CreateSeaWaterEffect();
			}

			base.OpenView();
		}

		private void SendCommandsToServerWhileOpen()
		{
			Singleton<GoldSilverIslandMode>.Instance.IconOpen(iconOpen);
			Singleton<GoldSilverIslandMode>.Instance.GetAdventureRemainCount();
			Singleton<GoldSilverIslandMode>.Instance.GetPlayersInfo();
			Singleton<GoldSilverIslandMode>.Instance.GetRobRemainCount();
			Singleton<GoldSilverIslandMode>.Instance.GetAssistRemainCount();
			Singleton<GoldSilverIslandMode>.Instance.GetRobCD();
			Singleton<GoldSilverIslandMode>.Instance.GetCurAssistInfo();
			Singleton<GoldSilverIslandMode>.Instance.StopTips();
		}

		//设置相关子对象层级
		private void SetCenterToLayerMode()
		{
			GameObject centerObj = NGUITools.FindChild(gameObject, "center");
			NGUITools.SetLayer(centerObj, LayerMask.NameToLayer("Mode")); 
		}

		//创建海水特效
		private void CreateSeaWaterEffect()
		{
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_SeaWater, Vector3.zero);
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();

			firstOpenFlag = true;

			seaWaterObj = EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_SeaWater);	
			if (null != seaWaterObj)
			{
				seaWaterObj.SetActive(true);
			}

			UpdateViewInfo();
			SendCommandsToServerWhileOpen();
			viewOpened = true;
		}

		private void SortViewOpened()
		{
			//如果当前正在冒险，则不需要打开船只刷新界面
			if ((null == me) && (adventRemainCnt > 0))
			{
				OpenRefreshBoatView();
			}
			else
			{
				if (null != me)
				{
					ShowAutoSearchGoldEffect();
				}

				ShowSearchButton(false);
			}

			firstOpenFlag = false;
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();

			viewOpened = false;

			seaWaterObj = EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_SeaWater);
			if (null != seaWaterObj)
			{
				seaWaterObj.SetActive(false);
			}

			HideAutoSearchGoldEffect();

			SendCommandsToServerWhileClose();
		}

		private void SendCommandsToServerWhileClose()
		{
			Singleton<GoldSilverIslandMode>.Instance.IconOpen(iconClosed);
			Singleton<GoldSilverIslandMode>.Instance.StartTips();
		}

		private void ShipOnClick(GameObject go)
		{
			string shipName = go.name;
			string shipNO = shipName.Replace("chuan", "");
			int shipSlot = int.Parse(shipNO) - 1;
			uint playerId = playerIds[shipSlot];

			if (playerId == MeVo.instance.Id)
			{
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.CannotRobSelf"));
			}
			else
			{
				Singleton<RobPlayerView>.Instance.ShowWindow(playerId);
			}
		}
		
		private void BackOnClick(GameObject go)
		{
			//疾风速时不能关闭界面
			if (startBlast)
			{
				return;
			}

			CloseView();
		}

		private void SearchDiamOnClick(GameObject go)
		{
			OpenRefreshBoatView();
		}

		private void OpenRefreshBoatView()
		{
			Singleton<BoatRefreshView>.Instance.ShowWindow(btnRefresh.background.atlas);
			ShowSearchRefreshButtons(false);
		}

		public void ShowSearchRefreshButtons(bool visible)
		{
			ShowSearchButton(visible);
			ShowRefreshButton(visible);
		}

		public void ShowSearchButton(bool visible)
		{
			btnSearchDiam.SetActive(visible);
		}

		public void ShowRefreshButton(bool visible)
		{
			btnRefresh.SetActive(visible);
		}

		private void ClearAdventCDOnClick(GameObject go)
		{
			if (null == me)
			{
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.NotNeedClearAdventCD"));
			}
			else
			{
				SysPriceVo priceVo = BaseDataMgr.instance.GetSysPriceVos(PriceConst.TypeBlast);
				string diam = StringUtils.GetValueListFromString(priceVo.diam)[0];
				string[] param = {diam};
				ConfirmMgr.Instance.ShowSelectOneAlert(LanguageManager.GetWord("GoldSilverIsland.BlastDiamNeed", param), ConfirmCommands.SELECT_ONE, ClearAdventCD);	
			}
		}

		private void ClearRobCDOnClick(GameObject go)
		{
			if (0 == robLeftTime)
			{
				MessageManager.Show(LanguageManager.GetWord("GoldSilverIsland.NotNeedClearRobCD"));
			}
			else
			{
				int diamNeed = (robLeftTime+59)/60 * 2;
				string[] param = {diamNeed.ToString()};
				ConfirmMgr.Instance.ShowSelectOneAlert(LanguageManager.GetWord("GoldSilverIsland.ClearRobCDDiamNeed", param), ConfirmCommands.SELECT_ONE, ClearRobCD);
			}
		}		

		private void ClearRobCD()
		{
			Singleton<GoldSilverIslandMode>.Instance.ClearRobCD();
		}

		private void ClearAdventCD()
		{
			Singleton<GoldSilverIslandMode>.Instance.Blast();
		}

		private void RefreshOnClick(GameObject go)
		{
			Singleton<GoldSilverIslandMode>.Instance.GetPlayersInfo();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateRobRemainHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateAssistRemainHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateAdventRemainHandle;	
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdatePlayerListHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateRobCDTimeHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateBlastHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateAdventFinishedHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdatePlayerDelHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated += UpdateStartRobPlayerHandle;
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateRobRemainHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateAssistRemainHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateAdventRemainHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdatePlayerListHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateRobCDTimeHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateBlastHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateAdventFinishedHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdatePlayerDelHandle;
			Singleton<GoldSilverIslandMode>.Instance.dataUpdated -= UpdateStartRobPlayerHandle;
		}

		private void UpdateStartRobPlayerHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_START_ROB == type)
			{
				UpdateStartRobPlayer();
			}
		}

		private void UpdateRobRemainHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ROB_REMAIN == type)
			{
				UpdateRobRemain();
			}
		}

		private void UpdateAssistRemainHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ASSIST_REMAIN == type)
			{
				UpdateAssistRemain();
			}
		}

		private void UpdateAdventRemainHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ADVENT_REMAIN == type)
			{
				UpdateAdventRemain();
			}
		}

		private void UpdatePlayerListHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_PLAYER_LIST == type)
			{
				UpdatePlayerList();

				if (firstOpenFlag)
				{
					SortViewOpened();
				}
			}
		}

		private void UpdatePlayerDelHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_PLAYER_DEL == type)
			{
				UpdatePlayerDel();	
			}
		}

		private void UpdateRobCDTimeHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ROB_CDTIME == type)
			{
				UpdateRobCDTime();
			}
		}

		private void UpdateBlastHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ADVENT_BLAST == type)
			{
				UpdateBlast();
			}
		}

		private void UpdateAdventFinishedHandle(object sender, int type)
		{
			if (Singleton<GoldSilverIslandMode>.Instance.UPDATE_ADVENT_FINISHED == type)
			{
				UpdateAdventFinished();
			}
		}

		//开始打劫玩家
		private void UpdateStartRobPlayer()
		{
			//Singleton<MapMode>.Instance.changeScene(MapTypeConst.GoldSilverIsland_MAP, false, 5, 1.5f);
			CloseView();
		}

		//疾风速
		private void UpdateBlast()
		{
			adventLeftTime = 0;
			startBlast = true;
		}

		//冒险完成
		private void UpdateAdventFinished()
		{
			vp_Timer.In(1f, ShowAdventReward, 1, 1f);
		}

		private void ShowAdventReward()
		{
			uint repu = Singleton<GoldSilverIslandMode>.Instance.RepuGets;
			uint gold = Singleton<GoldSilverIslandMode>.Instance.GoldGets;

			HideAutoSearchGoldEffect();
			Singleton<RewardView>.Instance.ShowWindow((int)gold, (int)repu, AdventFinishedCallback);
		}

		private void AdventFinishedCallback()
		{
			if (adventRemainCnt > 0)
			{
				ShowSearchButton(true);
			}
		}

		private void UpdateRobRemain()
		{
			labRobRemainCnt.text = Singleton<GoldSilverIslandMode>.Instance.RobRemainTimes.ToString();
		}

		private void UpdateAssistRemain()
		{
			labAssistRemainCnt.text = Singleton<GoldSilverIslandMode>.Instance.AssistRemainTimes.ToString();
		}

		private void UpdateAdventRemain()
		{
			adventRemainCnt = Singleton<GoldSilverIslandMode>.Instance.AdventRemainTimes;
			labAdventRemainCnt.text = adventRemainCnt.ToString();
		}

		private void UpdateRobCDTime()
		{
			robLeftTime = Singleton<GoldSilverIslandMode>.Instance.RobCDTime;
			SetLabelLeftTime(labRobRemainTime, robLeftTime);
		}

		private void UpdatePlayerList()
		{
			foreach (Button item in btnShips)
			{
				item.SetActive(false);
			}

			me = Singleton<GoldSilverIslandMode>.Instance.GetPlayer(MeVo.instance.Id);
			if (null != me)
			{
				adventLeftTime = (int)me.remainTime;
				myShipSlot = GetShipSlot(me.id);
				targetPos = btnShips[myShipSlot].transform.localPosition;
				targetPos.x = endX;
			}

			SetLabelLeftTime(labAdventRemainTime, adventLeftTime);

			UpdatePlayersShow();
		}

		private void UpdatePlayerDel()
		{
			uint delPlayerId = Singleton<GoldSilverIslandMode>.Instance.DelPlayerId;

			//如果是主角疾风速，暂不删除船槽号
			if (myShipSlot>=0 && (playerIds[myShipSlot]==delPlayerId))
			{
				return;
			}

			//隐藏船只并删除船槽号
			int shipSlot = GetShipSlot(delPlayerId);
			btnShips[shipSlot].SetActive(false);
			DelShipSlot(delPlayerId);
		}

		private void UpdatePlayersShow()
		{
			int curShipCnt = Singleton<GoldSilverIslandMode>.Instance.PlayerList.Count;
			int needShowCnt = (curShipCnt <= maxShips) ? curShipCnt : maxShips;

			for (int i=0; i<needShowCnt; i++)
			{
				PWoodsPlayer player = Singleton<GoldSilverIslandMode>.Instance.PlayerList[i];
				int shipSlot = GetShipSlot(player.id);
				btnShips[shipSlot].background.spriteName = GetShipSpriteName(player.grade);
				shipsRemainTime[player.id] = (int)player.remainTime;

				UpdatePlayerPos(i);

				btnShips[shipSlot].SetActive(true);
			}
		}

		private void UpdatePlayerPos(int index)
		{
			PWoodsPlayer player = Singleton<GoldSilverIslandMode>.Instance.PlayerList[index];
			SysForest shipInfo = BaseDataMgr.instance.GetSysForestVo((uint)player.grade);
			int shipSlot = GetShipSlot(player.id);
			int totalTime = shipInfo.time;
			Vector3 pos = btnShips[shipSlot].transform.localPosition;
			pos.x = startX + (((float)(totalTime-shipsRemainTime[player.id])/totalTime) * (endX-startX));
			btnShips[shipSlot].transform.localPosition = pos;
			AddTweenPositionComponentForShip(shipSlot);
		}

		private void AutoUpdatePlayersPos()
		{
			int curShipCnt = Singleton<GoldSilverIslandMode>.Instance.PlayerList.Count;
			int needShowCnt = (curShipCnt <= maxShips) ? curShipCnt : maxShips;

			for (int i=0; i<needShowCnt; i++)
			{
				PWoodsPlayer player = Singleton<GoldSilverIslandMode>.Instance.PlayerList[i];
				int shipSlot = GetShipSlot(player.id);
				if (startBlast && (myShipSlot==shipSlot))
				{
					continue;
				}

				shipsRemainTime[player.id]--;

				if (shipsRemainTime[player.id] < 0)
				{
					shipsRemainTime[player.id] = 0;
				}

				UpdatePlayerPos(i);
			}
		}

		private void AutoUpdateRobCDTime()
		{
			robLeftTime--;

			if (robLeftTime < 0)
			{
				robLeftTime = 0;
			}

			SetLabelLeftTime(labRobRemainTime, robLeftTime);
		}

		private void AutoUpdateAdventCDTime()
		{
			adventLeftTime--;
			
			if (adventLeftTime < 0)
			{
				adventLeftTime = 0;
			}
			
			SetLabelLeftTime(labAdventRemainTime, adventLeftTime);
		}

		public override void Update()
		{
			if (!viewOpened)
			{
				return;
			}

			float curTime = Time.time;

			//每秒自动更新
			if (curTime - prevTime >= 1f)
			{
				prevTime = curTime;

				AutoUpdateAdventCDTime();
				AutoUpdateRobCDTime();
				AutoUpdatePlayersPos();
			}

			//疾风速度效果处理
			if (startBlast)
			{
				SortBlastEffect();
			}
		}

		private void SortBlastEffect()
		{
			DelTweenPositionComponent(btnShips[myShipSlot].gameObject);

			if (btnShips[myShipSlot].transform.localPosition == targetPos)
			{
				btnShips[myShipSlot].SetActive(false);
				startBlast = false;
				DelShipSlot(playerIds[myShipSlot]);
				myShipSlot = -1;
				return;
			}

			btnShips[myShipSlot].SetActive(true);
			btnShips[myShipSlot].transform.localPosition = Vector3.MoveTowards(btnShips[myShipSlot].transform.localPosition, targetPos, Time.deltaTime * blastSpeed);
		}

		private string GetShipSpriteName(byte grade)
		{
			return "tu_chuan" + grade.ToString();
		}

		private void SetLabelLeftTime(UILabel label, int leftTime)
		{
			label.text = TimeUtil.GetTimeHhmmss(leftTime);
		}
		
		private void UpdateViewInfo()
		{
			UpdateRobRemain();
			UpdateAssistRemain();
			UpdateAdventRemain();
			UpdatePlayerList();
			UpdateRobCDTime();
		}

		public void ShowAutoSearchGoldEffect()
		{
			GameObject effectObj = EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_AutoSearchGold);
			if (null != effectObj)
			{
				effectObj.SetActive(true);
			}
			else
			{
				EffectMgr.Instance.CreateUIEffect(EffectId.UI_AutoSearchGold, Vector3.zero, null, true, EffectCreatedCallback);
			}
		}

		private void EffectCreatedCallback(GameObject effectObj)
		{
			Vector3 localPos = effectObj.transform.localPosition;
			localPos.z = -10000;
			effectObj.transform.localPosition = localPos;
		}

		private void HideAutoSearchGoldEffect()
		{
			GameObject effectObj = EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_AutoSearchGold);
			if (null != effectObj)
			{
				effectObj.SetActive(false);
			}
		}
		
	}
}
