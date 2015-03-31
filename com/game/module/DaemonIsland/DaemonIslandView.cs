using com.game.module.Guide;
using com.game.module.Task;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using System;
using com.u3d.bases.debug;
using com.game;
using Com.Game.Module.Manager;
using com.game.vo;
using com.game.manager;
using com.game.data;
using com.game.utils;
using com.game.consts;
using Com.Game.Module.Waiting;
using com.game.module.map;
using Com.Game.Module.Role;
using com.game.Public.Confirm;
using com.game.module.main;
using com.game.sound;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   
 * function:  恶魔岛系统视图管理
 * *******************************************************/
using com.game.Public.UICommon;
using Com.Game.Module.Copy;


namespace Com.Game.Module.DaemonIsland
{
	public class DaemonIslandView : BaseView<DaemonIslandView>
	{
		private readonly int COPY_POINT_NUM_PER_GROUP = 6;
		private readonly int NORMAL_COPY_POINT_DEPTH = 1003;
		private readonly int GRAY_COPY_POINT_DEPTH = 1503;

		//public override string url { get { return "UI/DaemonIsland/DaemonIslandView.assetbundle"; } }
		//public override ViewLayer layerType
		//{
			//get { return ViewLayer.HighLayer; }
		//}


		private Button btn_close;
		private Button btn_next_city;
		private Button btn_up_city;
		
		private List<Transform> copyPoints = new List<Transform>();
		private UILabel worldName;
		private Button btn_box1;
		private Button btn_box2;
		private Button btn_box3;
		private Transform Box1;
		private Transform Box2;
		private Transform Box3;


	
		private uint selectWorldId;
		private uint selectCopyPointId;
		private string[] worldIdList;
		private int getStarsNum = 0;
		private UIAtlas copyIconAtlas; //副本缩略图图集
		private UIAtlas copyIconGrayAtlas; //副本缩略图图集
		protected override void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("btn_guanbi");
			btn_next_city = FindInChild<Button>("btn_right_jiantou");
			btn_up_city = FindInChild<Button>("btn_left_jiantou");
			worldName = FindInChild<UILabel>("IslandName");
			Box1 = FindInChild<Transform>("Box1");
			Box2 = FindInChild<Transform>("Box2");
			Box3 = FindInChild<Transform>("Box3");
			btn_box1 = FindInChild<Button>("Box1/icon");
			btn_box2 = FindInChild<Button>("Box2/icon");
			btn_box3 = FindInChild<Button>("Box3/icon");
			
			for (int i = 1; i <= COPY_POINT_NUM_PER_GROUP; ++i)
			{
				copyPoints.Add(FindInChild<Transform>("Copys/" + i) );
				FindInChild<Button> ("Copys/" + i).onClick = CopyPointOnClick;
			}

			btn_close.onClick = CloseOnClick;
			btn_next_city.onClick = NextCityOnClick;
			btn_up_city.onClick = UpCityOnClick;
			btn_box1.onClick = Box1OnClick;
			btn_box2.onClick = Box2OnClick;
			btn_box3.onClick = Box3OnClick;

			worldIdList = StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDaemonIslandTree (0).list);
		}

		private void InitLabelLanguage()
		{

		}
		
		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<DaemonIslandMode>.Instance.dataUpdated += UpdateDaemonIslandView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();

			if (copyIconAtlas == null)
			{
				Singleton<AtlasManager>.Instance.LoadAtlasHold(AtlasUrl.CopyIconHold, AtlasUrl.CopyIconNormal, LoadCopyIconAtlasCallBack, true);
			}

			if (Singleton<CopyMode>.Instance.openCopyById)
			{
				Singleton<DaemonCopyDetailView>.Instance.OpenView ();
			}
			Singleton<WaitingView>.Instance.OpenView ();
			Singleton<DaemonIslandMode>.Instance.ApplyDaemonCopyInfo (0);

			//播放副本备战音乐
			SoundMgr.Instance.StopAll();
			SoundMgr.Instance.PlaySceneAudio(SoundId.Music_PrepareBattle);
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<DaemonIslandMode>.Instance.dataUpdated -= UpdateDaemonIslandView;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
		}

		//副本缩略图图集加载回调
		private void LoadCopyIconAtlasCallBack(UIAtlas atlas)
		{
			if (copyIconAtlas == null) 
			{
				copyIconAtlas = atlas;
				copyIconGrayAtlas = Singleton<AtlasManager>.Instance.GetAtlas (AtlasUrl.CopyIconGray);
				copyIconAtlas = atlas;
			}
		}

		//副本信息已更新回调
		private void UpdateDaemonIslandView(object sender,int code)
		{
			
			if (code == Singleton<DaemonIslandMode>.Instance.UPDATE_DAEMONISLAND_INFO)
			{
				Log.info (this, "8-16返回的恶魔岛信息已更新");
				selectWorldId = GetSelectedWorldId();
				Singleton<DaemonIslandMode>.Instance.ApplyAwardInfo (0);
			}
			else if (code == Singleton<DaemonIslandMode>.Instance.INIT_AWARD_INFO)
			{
				Singleton<WaitingView>.Instance.CloseView ();
				UpdateCopyInfoByCity();

			}
		}

		//获取刚打开副本UI时应该打开的章节
		private uint GetSelectedWorldId()
		{
			if (Singleton<CopyMode>.Instance.openCopyById)
			{
				return (uint)BaseDataMgr.instance.GetSysDaemonIslandTree(Singleton<DaemonIslandMode>.Instance.selectedCopyPoint).parentId;
			}

			if (Singleton<DaemonIslandMode>.Instance.NewOpenMapId > 0)
			{
				return (uint)BaseDataMgr.instance.GetSysDaemonIslandTree(Singleton<DaemonIslandMode>.Instance.NewOpenMapId).parentId;
			}
			else
			{
				uint firstCopyId;
				for (int i = worldIdList.Length - 1 ; i >= 0; --i)
				{
					firstCopyId = uint.Parse(StringUtils.GetValueListFromString( 
					                                 BaseDataMgr.instance.GetSysDaemonIslandTree(uint.Parse(worldIdList[i])).list)[0]);
					if (Singleton<DaemonIslandMode>.Instance.GetDaemonCopyInfo(firstCopyId).grade >= 0)
					{
						return uint.Parse(worldIdList[i]);
					}
				}
			}
			return 1;
		}


		//切换章节后调用刷新
		private void UpdateCopyInfoByCity()
		{
			worldName.text = BaseDataMgr.instance.GetSysDaemonIslandTree (selectWorldId).name;
			worldName.SetActive (true);
			UpDateCityChangeButtonState ();

			//基本信息更新
			string[] copypointId;
			copypointId = StringUtils.GetValueListFromString (BaseDataMgr.instance.GetSysDaemonIslandTree (selectWorldId).list);
			SysMapVo copyPointInfo;
			for (int i = 0; i < copyPoints.Count; ++i)
			{
				copyPointInfo = BaseDataMgr.instance.GetMapVo ( uint.Parse(copypointId[i]) );
				copyPoints[i].FindChild("Name").GetComponent<UILabel>().text = copyPointInfo.name;
				copyPoints[i].FindChild("ditu").GetComponent<UISprite>().spriteName = 
					BaseDataMgr.instance.GetMapVo (uint.Parse(copypointId[i])).resource_id.ToString();
			}

			//根据后端数据更新副本点信息
			DaemonCopyVo copyVo = new DaemonCopyVo ();
			int grade;
			int usedTimes;
			uint newMapId;
			getStarsNum = 0;
			for (int i = 0; i  < copypointId.Length; ++i)
			{
				copyVo = Singleton<DaemonIslandMode>.Instance.GetDaemonCopyInfo(uint.Parse(copypointId[i]));
				grade = copyVo.grade;
				usedTimes = copyVo.usedTimes;

				//更新可玩次数
				copyPoints[i].FindChild("Times").GetComponent<UILabel>().text = usedTimes + "/" 
																				+ BaseDataMgr.instance.GetMapVo(uint.Parse(copypointId[i])).enter_count;
				copyPoints[i].FindChild("Times").GetComponent<UILabel>().color = 
					usedTimes < (BaseDataMgr.instance.GetMapVo(uint.Parse(copypointId[i])).enter_count)? Color.white: Color.red;

				getStarsNum += (grade > 0?grade:0);
				for (int j = 1; j <= 3; ++j)  //更新副本星星获取数量
				{
					copyPoints [i].FindChild("xx" + j).GetComponent<UISprite>().spriteName   
						= j <= grade?"xingxing1":"kongxing";
				}
				
				//更新副本开启状态
				newMapId = Singleton<DaemonIslandMode>.Instance.NewOpenMapId;
				if (grade >= 0 || uint.Parse(copypointId[i]) == newMapId)  //开启该副本点
				{
					copyPoints[i].GetComponent<BoxCollider>().enabled = true;
					copyPoints[i].FindChild("suo").gameObject.SetActive(false);
					copyPoints[i].FindChild("zhezao").gameObject.SetActive(false);
					copyPoints[i].FindChild("new").gameObject.SetActive(false);
//					UIUtils.ChangeNormalShader( copyPoints[i].FindChild("ditu").GetComponent<UISprite>(), NORMAL_COPY_POINT_DEPTH);
					copyPoints[i].FindChild("ditu").GetComponent<UISprite>().atlas = copyIconAtlas;
					copyPoints[i].FindChild("ditu").GetComponent<UISprite>().depth = NORMAL_COPY_POINT_DEPTH;
					
					if (grade < 0 && uint.Parse(copypointId[i]) == newMapId)
					{
						copyPoints[i].FindChild("new").gameObject.SetActive(true);
					}
				}
				else
				{
					copyPoints[i].GetComponent<BoxCollider>().enabled = false;
					copyPoints[i].FindChild("new").gameObject.SetActive(false);
					copyPoints[i].FindChild("suo").gameObject.SetActive(true);
					copyPoints[i].FindChild("zhezao").gameObject.SetActive(true);
//					UIUtils.ChangeGrayShader( copyPoints[i].FindChild("ditu").GetComponent<UISprite>(), GRAY_COPY_POINT_DEPTH);
					copyPoints[i].FindChild("ditu").GetComponent<UISprite>().atlas = copyIconGrayAtlas;
					copyPoints[i].FindChild("ditu").GetComponent<UISprite>().depth = GRAY_COPY_POINT_DEPTH;
				}
				copyPoints[i].gameObject.SetActive(true);
			}

			UpdateBoxInfo(); // 更新宝箱状态
		}

		//更新翻页按钮状态
		private void UpDateCityChangeButtonState()
		{
			int worldSub = GetWorldSub (selectWorldId);
			//更新向上翻页状态
			if (worldSub == 0)
			{
				UIUtils.SetButtonState(btn_up_city.transform, false, GRAY_COPY_POINT_DEPTH);
			}
			else
			{
				UIUtils.SetButtonState(btn_up_city.transform, true, NORMAL_COPY_POINT_DEPTH);
			}
			//更新向下翻页状态
			if (worldSub + 1 < worldIdList.Length)
			{
				string[] copysInNextWorld = StringUtils.GetValueListFromString( BaseDataMgr.instance.GetSysDaemonIslandTree(uint.Parse(worldIdList[worldSub + 1])).list);
				if (MeVo.instance.Level < BaseDataMgr.instance.GetMapVo(uint.Parse(copysInNextWorld[0])).lvl)
				{
					UIUtils.SetButtonState(btn_next_city.transform, false, GRAY_COPY_POINT_DEPTH);
				}
				else
				{
					UIUtils.SetButtonState(btn_next_city.transform, true, NORMAL_COPY_POINT_DEPTH);
				}
			}
			else
			{
				UIUtils.SetButtonState(btn_next_city.transform, false, GRAY_COPY_POINT_DEPTH);
			}
		}

		//刷新星星收集信息
		private void UpdateBoxInfo()
		{	
			DaemonCopyAwardVo awardInfo = Singleton<DaemonIslandMode>.Instance.GetAdditionalAwardInfo (selectWorldId);
			SetBoxState (getStarsNum, 6, Box1, awardInfo.isGettedAward1);
			SetBoxState (getStarsNum, 12, Box2, awardInfo.isGettedAward2);
			SetBoxState (getStarsNum, 18, Box3, awardInfo.isGettedAward3);
		}

		//设置宝箱状态
		private void SetBoxState(int getStarNum, int needStarNum, Transform box, bool isAwardGetted)
		{
			if (getStarNum < needStarNum)
			{
				box.FindChild("Num").GetComponent<UILabel>().text = getStarNum + "/" + needStarNum;
				box.FindChild("Num").GetComponent<UILabel>().color = Color.white;
				box.FindChild("State").gameObject.SetActive(false);
				box.FindChild("icon").GetComponent<BoxCollider>().enabled = false;
			}
			else
			{
				box.FindChild("Num").GetComponent<UILabel>().text = needStarNum + "/" + needStarNum;
				box.FindChild("Num").GetComponent<UILabel>().color = Color.green;
				box.FindChild("State").gameObject.SetActive(true);
				if (isAwardGetted)
				{
					box.FindChild("icon").GetComponent<BoxCollider>().enabled = false;
					box.FindChild("State/Label").GetComponent<UILabel>().text = "已领";
					box.FindChild("icon").GetComponent<BoxCollider>().enabled = false;
				}
				else
				{
					box.FindChild("icon").GetComponent<BoxCollider>().enabled = true;
					box.FindChild("State/Label").GetComponent<UILabel>().text = "可领";
					box.FindChild("icon").GetComponent<BoxCollider>().enabled = true;
				}
			}

		}

		
		
		//副本点地图的返回键被点击
		private void CloseOnClick(GameObject go)
		{
			//this.CloseView ();
            CopyView.Instance.CloseView();
		}

		//获取指定世界地图ID在世界ID list中的下标
		private int GetWorldSub(uint worldId)
		{
			int sub = 0;
			for (int i  = 0; i < worldIdList.Length; ++i)
			{
				if (uint.Parse(worldIdList[i]) == worldId)
				{
					sub = i;
					break;
				}
			}
			return sub;
		}
		//切换到下一个城市副本的按键被点击
		private void NextCityOnClick(GameObject go)
		{
//			selectWorldId = selectWorldId + 1;
			int sub;
			sub = this.GetWorldSub (selectWorldId);

			sub++;
			selectWorldId = uint.Parse(worldIdList [sub]);
			UpdateCopyInfoByCity ();

		}

		//切换到下一个城市副本的按键被点击
		private void UpCityOnClick(GameObject go)
		{
//			selectWorldId = selectWorldId - 1;
			int sub;
			sub = this.GetWorldSub (selectWorldId);

			sub--;
			selectWorldId = uint.Parse(worldIdList [sub]);
			UpdateCopyInfoByCity ();
		}
		
		//点击副本点
		private void CopyPointOnClick(GameObject go)
		{
			selectCopyPointId = uint.Parse (StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDaemonIslandTree (selectWorldId).list)[uint.Parse(go.name) - 1]);
			//弹出副本选中后的UI
			Singleton<DaemonIslandMode>.Instance.selectedCopyPoint = selectCopyPointId;
			Singleton<DaemonCopyDetailView>.Instance.OpenView ();

		}

		//宝箱被点击
		private void Box1OnClick(GameObject go)
		{
			Singleton<DaemonIslandMode>.Instance.ApplyGetAward (selectWorldId, 1);
		}
		private void Box2OnClick(GameObject go)
		{
			Singleton<DaemonIslandMode>.Instance.ApplyGetAward (selectWorldId, 2);
		}
		private void Box3OnClick(GameObject go)
		{
			Singleton<DaemonIslandMode>.Instance.ApplyGetAward (selectWorldId, 3);
		}

	}
}