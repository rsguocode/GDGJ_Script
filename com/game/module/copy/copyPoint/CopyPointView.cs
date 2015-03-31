using com.game.module.Guide;
using com.game.module.Guide.GuideLogic;
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
 * function:  副本点选择ui
 * *******************************************************/
using com.game.Public.UICommon;
using com.game.Public.Message;


namespace Com.Game.Module.Copy
{
	public class CopyPointView : BaseView<CopyPointView>
	{
		private readonly int COPY_POINT_NUM_PER_GROUP = 6;
		private readonly int NORMAL_COPY_POINT_DEPTH = 1019;
		private readonly int GRAY_COPY_POINT_DEPTH = 1519;

		//public override string url { get { return "UI/Copy/CopyPointView.assetbundle"; } }
		//public override ViewLayer layerType
		//{
			//get { return ViewLayer.HighLayer; }
		//}
		public override bool playClosedSound { get { return false; } }

	    public override bool IsFullUI
	    {
	        get { return false; }
	    }

	    private Button btn_close;
		private Button btn_next_city;
		private Button btn_up_city;

		private UIToggle ckb_common;
		private UIToggle ckb_hard;
		private UIToggle ckb_death;
		private List<Transform> copyPoints = new List<Transform>();
		private UIGrid grid;
		private UILabel worldName;
		private List<UILabel> attrAddList = new List<UILabel> ();
		private UILabel ActivedStarNum;
		private UISprite ActivedStar;

	    public Button GuideCopyButton;

		private UISprite ns;
		private Button btn_star;
		private UILabel nsRemark;

		private bool isCollectAllStars = false;
		private uint selectWorldId;
		private uint selectSubWorldId;
		private uint selectCopyPointId;
		private string[] worldIdList;
		private UIAtlas copyIconAtlas; //副本缩略图图集
		private UIAtlas copyIconGrayAtlas; //副本缩略图图集
		protected override void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("btn_close");
			btn_next_city = FindInChild<Button>("Title/btn_right_jt");
			btn_up_city = FindInChild<Button>("Title/btn_left_jt");
			worldName = FindInChild<UILabel>("Title/Name");
			ckb_common = FindInChild<UIToggle>("CopyType/ckb_pt");
			ckb_hard = FindInChild<UIToggle>("CopyType/ckb_jy");
			ckb_death = FindInChild<UIToggle>("CopyType/ckb_dy");
			ActivedStarNum = FindInChild<UILabel>("Left/CollectStars/percent");
			ActivedStar = FindInChild<UISprite>("Left/CollectStars/ActiveStar");
			ns = FindInChild<UISprite>("ns/ns");
			btn_star = FindInChild<Button>("Left/CollectStars");
			nsRemark = FindInChild<UILabel>("Left/describe");

			attrAddList.Add (FindInChild<UILabel> ("Left/AttrAdd/1"));
			attrAddList.Add (FindInChild<UILabel> ("Left/AttrAdd/2"));
			attrAddList.Add (FindInChild<UILabel> ("Left/AttrAdd/3"));
			attrAddList.Add (FindInChild<UILabel> ("Left/AttrAdd/4"));
			
			for (int i = 1; i <= COPY_POINT_NUM_PER_GROUP; ++i)
			{
				copyPoints.Add(FindInChild<Transform>("CopyPoints/Grid/" + i) );
				FindInChild<Button> ("CopyPoints/Grid/" + i).onClick = CopyPointOnClick;
			}
			grid = FindInChild<UIGrid> ("CopyPoints/Grid");
			btn_close.onClick = CloseOnClick;
			btn_next_city.onClick = NextCityOnClick;
			btn_up_city.onClick = UpCityOnClick;
			ckb_common.onStateChange = CommonCopyChoose;
			ckb_hard.onStateChange = HardCopyChoose;
			ckb_death.onStateChange = DeathCopyChoose;

			btn_star.onClick = StartOnClick;

			worldIdList = StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (0).list);
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("CopyType/ckb_pt/label").text = LanguageManager.GetWord("CopyPointView.partOne");
			FindInChild<UILabel>("CopyType/ckb_jy/label").text = LanguageManager.GetWord("CopyPointView.partTwo");
			FindInChild<UILabel>("CopyType/ckb_dy/label").text = LanguageManager.GetWord("CopyPointView.partThree");
			FindInChild<UILabel>("Left/AttrAdd/Label").text = LanguageManager.GetWord("CopyPointView.AttrAdd");
		}
		
		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<CopyPointMode>.Instance.dataUpdated += UpdateCopyView;
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
				Singleton<CopyDetailView>.Instance.OpenView ();
				selectWorldId = (uint)BaseDataMgr.instance.GetSysDungeonTree(
					(uint)BaseDataMgr.instance.GetSysDungeonTree(Singleton<CopyPointMode>.Instance.selectedCopyPoint).parentId).parentId;
			}
			else
			{
				selectWorldId = MeVo.instance.mapId;
			}
			Singleton<WaitingView>.Instance.OpenView ();
			Singleton<CopyPointMode>.Instance.ApplyCopyInfo (0);

			//播放副本备战音乐
			SoundMgr.Instance.StopAll();
			SoundMgr.Instance.PlaySceneAudio(SoundId.Music_PrepareBattle);
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<CopyPointMode>.Instance.dataUpdated -= UpdateCopyView;
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
		private void UpdateCopyView(object sender,int code)
		{
			if (code == Singleton<CopyPointMode>.Instance.UPDATE_COPY_INFO)
			{
				Log.info (this, "8-1返回的副本信息已更新");
				Singleton<WaitingView>.Instance.CloseView ();
				UpdateCopyInfoByCity(GetFirstOpenSubWorldType());
			}
			else if (code == Singleton<CopyPointMode>.Instance.UPDATE_ACTIVED_SUBWORLDID)
			{
				Log.info (this, "已激活的女神列表有更新");
				ShowLoveGirl();
			}
		}

		//获取刚打开副本UI时应该打开的子章节
		private byte GetFirstOpenSubWorldType()
		{
			byte subWorldType;
			if (Singleton<CopyMode>.Instance.openCopyById)
			{
				subWorldType = GetTypeByCopyId(Singleton<CopyPointMode>.Instance.selectedCopyPoint);
			}
			else
			{
				uint curTaskMapId = Singleton<TaskModel>.Instance.TaskCopyMapId; //默认开启任务所在地图的子地图
				subWorldType = GetTypeByCopyId(Singleton<TaskModel>.Instance.TaskCopyMapId);
			}
			return subWorldType;
		}

		//根据副本ID获取副本子章节
		private byte GetTypeByCopyId(uint copyId)
		{
			byte subWorldType = (byte)CopyType.Common;
			if (copyId == 0)
				return subWorldType;
			uint subWorldId = (uint)BaseDataMgr.instance.GetSysDungeonTree (copyId).parentId;
			uint worldId = (uint)BaseDataMgr.instance.GetSysDungeonTree(subWorldId).parentId;
			string[] subWorldList = StringUtils.GetValueListFromString(BaseDataMgr.instance.GetSysDungeonTree (worldId).list);
			for (int i = 0; i < subWorldList.Length; ++i)
			{
				if (subWorldList[i] == subWorldId.ToString())
				{
					subWorldType = (byte)i;
					break;
				}
			}
			return subWorldType;
		}


		//切换章节后调用刷新
		private void UpdateCopyInfoByCity(byte copyType)
		{
			worldName.text = BaseDataMgr.instance.GetMapVo (selectWorldId).name;  //显示章节名字
			worldName.SetActive (true);

			selectSubWorldId = uint.Parse (StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (selectWorldId).list)[copyType]);
			switch (copyType)
			{
				case (byte)CopyType.Common:
					ckb_common.value = true;
					break;
				case (byte)CopyType.Hard:
					ckb_hard.value = true;
					break;
				case (byte)CopyType.Death:
					ckb_death.value = true;
					break;
			}

			UpdateCitySwitchingButton ();  //更新章节切换按钮的状态
			UpdateStageSwitchingButton ();    //更新阶段切换按钮的状态
			UpdateCopyPointByType ();      

		}

		//更新阶段切换按钮的状态
		private void UpdateStageSwitchingButton()
		{

			uint part2Id = uint.Parse (StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (selectWorldId).list)[(int)CopyType.Hard]);
			uint firstCopyPointIdInPart2 = uint.Parse (StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (part2Id).list)[0]);
			if (Singleton<CopyPointMode>.Instance.IsMainCopyOpend(firstCopyPointIdInPart2))
			{
				SetUIToggleState(ckb_hard, true);

				uint part3Id = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (selectWorldId).list)[(int)CopyType.Death]);
				uint firstCopyPointIdInPart3 = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (part3Id).list)[0]);
				if (Singleton<CopyPointMode>.Instance.IsMainCopyOpend(firstCopyPointIdInPart3))
				{
					SetUIToggleState(ckb_death, true);
				}
				else
				{
					SetUIToggleState(ckb_death, false);
				}
			}
			else
			{
				SetUIToggleState(ckb_hard, false);
				SetUIToggleState(ckb_death, false);
			}
		}

		//设置阶段切换按钮状态
		private void SetUIToggleState(UIToggle stageButton, bool state)
		{
			if (state)
			{
				stageButton.GetComponent<BoxCollider>().enabled = true;
				UIUtils.ChangeNormalShader(stageButton.FindInChild<UISprite>("background"), NORMAL_COPY_POINT_DEPTH);
			}
			else
			{
				stageButton.GetComponent<BoxCollider>().enabled = false;
				UIUtils.ChangeGrayShader(stageButton.FindInChild<UISprite>("background"), GRAY_COPY_POINT_DEPTH);
			}
		}

		//更新章节切换按钮的状态
		private void UpdateCitySwitchingButton()
		{
			//判断是否需要激活上一个城市的翻页按钮
			if (uint.Parse (worldIdList[0]) == selectWorldId)
			{
				UIUtils.SetButtonState(btn_up_city.transform, false, GRAY_COPY_POINT_DEPTH);
			}
			else
			{
				UIUtils.SetButtonState(btn_up_city.transform, true, NORMAL_COPY_POINT_DEPTH);
			}
			//判断是否需要激活下一个城市的翻页按钮
			if (this.GetWorldSub(selectWorldId) == worldIdList.Length - 1)
			{
				UIUtils.SetButtonState(btn_next_city.transform, false, GRAY_COPY_POINT_DEPTH);
			}
			else
			{
				uint nextWorldId = uint.Parse(worldIdList[this.GetWorldSub(selectWorldId) + 1]);
				uint firstSubWorldIdInNextWorld = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (nextWorldId).list)[(int)CopyType.Common]);
				uint firstCopyPointIdInNextWorld = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (firstSubWorldIdInNextWorld).list)[0]);
				
				if (Singleton<CopyPointMode>.Instance.IsMainCopyOpend (firstCopyPointIdInNextWorld))
				{
					UIUtils.SetButtonState(btn_next_city.transform, true, NORMAL_COPY_POINT_DEPTH);
				}
				else
				{
					UIUtils.SetButtonState(btn_next_city.transform, false, GRAY_COPY_POINT_DEPTH);
				}
				
			}
		}

		//切换副本类型后调用刷新
		private void UpdateCopyPointByType()
		{
			//基本信息更新
			string[] copypointId;
			copypointId = StringUtils.GetValueListFromString (BaseDataMgr.instance.GetSysDungeonTree (selectSubWorldId).list);
			SysMapVo copyPointInfo;
			for (int i = 0; i < copyPoints.Count; ++i)
			{
				copyPointInfo = BaseDataMgr.instance.GetMapVo ( uint.Parse(copypointId[i]) );
				copyPoints[i].FindChild("Name").GetComponent<UILabel>().text = copyPointInfo.name;
				copyPoints[i].FindChild("ditu/tu").GetComponent<UISprite>().spriteName = 
					BaseDataMgr.instance.GetMapVo (uint.Parse(copypointId[i])).resource_id.ToString();
			}

			//根据后端数据更新副本点信息
			int grade, getStarsNum = 0;
			uint curTaskMapId;
			bool needGuide = false;
			for (int i = 0; i  < copypointId.Length; ++i)
			{
				grade = Singleton<CopyPointMode>.Instance.GetStarNum(uint.Parse(copypointId[i]));
				getStarsNum += (grade > 0?grade:0);
				for (int j = 1; j <= 3; ++j)  //更新副本星星获取数量
				{
					copyPoints [i].FindChild("xx" + j).GetComponent<UISprite>().spriteName   
						= j <= grade?"xingxing1":"kongxing";
				}

				//更新副本开启状态
				curTaskMapId = Singleton<TaskModel>.Instance.TaskCopyMapId;
				if (grade >= 0 || uint.Parse(copypointId[i]) == curTaskMapId)  //开启该副本点
				{
					copyPoints[i].GetComponent<BoxCollider>().enabled = true;
					copyPoints[i].FindChild("ditu/suo").gameObject.SetActive(false);
					copyPoints[i].FindChild("ditu/zhezao").gameObject.SetActive(false);
					copyPoints[i].FindChild("ditu/new").gameObject.SetActive(false);
					copyPoints[i].FindChild("ditu/tu").GetComponent<UISprite>().atlas = copyIconAtlas;
					copyPoints[i].FindChild("ditu/tu").GetComponent<UISprite>().depth = NORMAL_COPY_POINT_DEPTH;
					if (grade < 0 && uint.Parse(copypointId[i]) == curTaskMapId)
					{
						copyPoints[i].FindChild("ditu/new").gameObject.SetActive(true);
					}
                    if (curTaskMapId <= 21002 && uint.Parse(copypointId[i]) == curTaskMapId&&!GuideModel.Instance.IsShowGuide)
                    {
                        GuideCopyButton = copyPoints[i].GetComponent<Button>();
						needGuide = true;
                    }
				}
				else
				{
					copyPoints[i].GetComponent<BoxCollider>().enabled = false;
					copyPoints[i].FindChild("ditu/new").gameObject.SetActive(false);
					copyPoints[i].FindChild("ditu/suo").gameObject.SetActive(true);
					copyPoints[i].FindChild("ditu/zhezao").gameObject.SetActive(true);
//					UIUtils.ChangeGrayShader( copyPoints[i].FindChild("ditu/tu").GetComponent<UISprite>(), GRAY_COPY_POINT_DEPTH);
					copyPoints[i].FindChild("ditu/tu").GetComponent<UISprite>().atlas = copyIconGrayAtlas;
					copyPoints[i].FindChild("ditu/tu").GetComponent<UISprite>().depth = GRAY_COPY_POINT_DEPTH;
				}
				copyPoints[i].gameObject.SetActive(true);
			}
			grid.Reposition ();
			if (needGuide)
			{
				GuideBase copyGuide = GuideManager.Instance.GetGuideLogic(GuideType.GuideCopy);
				copyGuide.BeginGuide();
				GuideManager.Instance.CurrentGuideType = GuideType.GuideCopy;
			}
			ActivedStarNum.text = getStarsNum + "/" + (copypointId.Length * 3);     //更新该副本组已收集的星星数量
			ActivedStar.fillAmount = (float)getStarsNum / (copypointId.Length * 3);

			isCollectAllStars = getStarsNum >= copypointId.Length * 3;
			ShowLoveGirl ();

		}

		private void ShowLoveGirl()
		{
			bool isGirlAtcived = Singleton<CopyPointMode>.Instance.IsGirlActived (selectSubWorldId);
			nsRemark.SetActive(isGirlAtcived? false: true);
			nsRemark.text = isCollectAllStars? 
							LanguageManager.GetWord("CopyPointView.activeGoddessRemark2"):
							LanguageManager.GetWord("CopyPointView.activeGoddessRemark1");
			if (isGirlAtcived)
			{
				UIUtils.ChangeNormalShader(ns, NORMAL_COPY_POINT_DEPTH);
			}
			else
			{
				UIUtils.ChangeGrayShader(ns, GRAY_COPY_POINT_DEPTH);
			}
			//女神属性加成
			string[] attr = StringUtils.GetValueCost (BaseDataMgr.instance.GetSysDungeonTree (selectSubWorldId).attrAdd);
			for (int i = 0; i  < attrAddList.Count; ++i)
			{
				attrAddList [i].text = UIUtils.ChangeAttrTypeToString (byte.Parse (attr [i].Split (',') [0]));
				attrAddList [i].text += " +" + attr[i].Split(',')[1];
				attrAddList [i].color = isGirlAtcived? ColorConst.GREEN_YES: ColorConst.GRAY;
			}
			ns.SetActive (true);

		}
		
		
		//副本点地图的返回键被点击
		private void CloseOnClick(GameObject go)
		{
			//this.CloseView ();
            CopyView.Instance.CloseView();
			Singleton<MainBottomLeftView>.Instance.HitMapPoint = true;
			AppMap.Instance.me.Pos(AppMap.Instance.mapParser.TransPosX, AppMap.Instance.mapParser.TransPosY - GameConst.HitPointEffectOffH);
			Singleton<MainBottomLeftView>.Instance.RoleLocked = false;

			//播放场景背景音乐
			SoundMgr.Instance.StopAll();
			string bgMusicId = Singleton<MapControl>.Instance.GetMapBackMusicName(AppMap.Instance.mapParser.MapId);
			SoundMgr.Instance.PlaySceneAudio(bgMusicId);
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

			UpdateCopyInfoByCity ((byte)CopyType.Common);
		}

		//切换到下一个城市副本的按键被点击
		private void UpCityOnClick(GameObject go)
		{
//			selectWorldId = selectWorldId - 1;
			int sub;
			sub = this.GetWorldSub (selectWorldId);
			sub--;
			selectWorldId = uint.Parse(worldIdList [sub]);
			UpdateCopyInfoByCity ((byte)CopyType.Common);
		}

		//选择普通副本类型
		private void CommonCopyChoose(bool state)
		{
			if (state)
			{
				Log.info(this, "切换到普通副本");
				selectSubWorldId = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (selectWorldId).list)[(int)CopyType.Common]);
				UpdateCopyPointByType();
			}
		}
		//选择精英副本类型
		private void HardCopyChoose(bool state)
		{
			if (state)
			{
				Log.info(this, "切换到精英副本");
				selectSubWorldId = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (selectWorldId).list)[(int)CopyType.Hard]);
				UpdateCopyPointByType();
			}
		}
		//选择地狱副本类型
		private void DeathCopyChoose(bool state)
		{
			if (state)
			{
				Log.info(this, "切换到地狱副本");
				selectSubWorldId = uint.Parse (StringUtils.GetValueListFromString (
					BaseDataMgr.instance.GetSysDungeonTree (selectWorldId).list)[(int)CopyType.Death]);
				UpdateCopyPointByType();
			}
		}

		//点击副本点
		private void CopyPointOnClick(GameObject go)
		{
			selectCopyPointId = uint.Parse (StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (selectSubWorldId).list)[uint.Parse(go.name) - 1]);
			//弹出副本选中后的UI
//			this.CloseView ();
			Singleton<CopyPointMode>.Instance.selectedCopyPoint = selectCopyPointId;
			Singleton<CopyDetailView>.Instance.OpenView ();

		}

		//星星点击
		private void StartOnClick(GameObject go)
		{
			if (isCollectAllStars)
			{
				if (!Singleton<CopyPointMode>.Instance.IsGirlActived(selectSubWorldId))
				{
					Singleton<CopyPointMode>.Instance.ActiveGirl(selectSubWorldId);
				}
			}
			else
			{
				MessageManager.Show(LanguageManager.GetWord("CopyPointView.activeGoddessRemark3"));
			}
		}


	}
}