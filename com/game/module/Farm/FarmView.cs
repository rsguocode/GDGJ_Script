/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/04 09:39:52 
 * function:  家园种植系统视图类
 * *******************************************************/

using UnityEngine;
using System.Collections;
using com.game.module.test;
using Com.Game.Module.Waiting;
using com.u3d.bases.debug;
using System.Collections.Generic;
using com.game.Public.UICommon;
using com.game.vo;


namespace Com.Game.Module.Farm
{
	public class FarmView : BaseView<FarmView> {

		public override string url { get { return "UI/Farm/FarmView.assetbundle"; } }

		private readonly Color LAND1_COLOR = new Color (1, 1, 1, 1);
		private readonly Color LAND2_COLOR = new Color (251.0f / 255.0f, 129.0f / 255.0f, 0, 0);
		private readonly Color LAND3_COLOR = new Color (141.0f / 255.0f, 26.0f / 142.0f, 0, 0);
		private readonly int LAND_MAX_NUM = 8;

		private Button btn_close;
		private Button btn_rz;


		private Button btn_land1;
		private Button btn_land2;
		private Button btn_land3;
		private Button btn_land4;
		private Button btn_land5;
		private Button btn_land6;
		private Button btn_land7;
		private Button btn_land8;
//		private Button btn_land9;
		private Button btn_kj1;
		private Button btn_kj2;
		private Button btn_kj3;
		private Button btn_kj4;
		private Button btn_kj5;
		private Button btn_kj6;
		private Button btn_kj7;
		private Button btn_kj8;
//		private Button btn_kj9;
		private Button btn_clearCD1;
		private Button btn_clearCD2;
		private Button btn_clearCD3;
		private Button btn_clearCD4;
		private Button btn_clearCD5;
		private Button btn_clearCD6;
		private Button btn_clearCD7;
		private Button btn_clearCD8;

		private UILabel farmLevel;
		private UISlider farmExp;
		private UILabel farmExpValue;
		private Transform lands;
		private Transform info;


		private Button btn_fhjy;
		private Button btn_zz;
		private Button btn_sd;
		private Button btn_sjtd;
		private Button btn_sh;
		private Button btn_clear_cd_ensure;
		private Button btn_clear_cd_cancle;

		private GameObject clearCDEnsureTips;
		private Transform friendsGroup;


		private List<FriendFarmBaseInfo> friendsFarmSimpleInfo;
		private Dictionary<UILabel, uint> remainTimeDic = new Dictionary<UILabel, uint> ();
//		private Dictionary<UILabel, uint> updateRemainTimeDic = new Dictionary<UILabel, uint>();
		private float curTime = 0;
		private uint durTime = 0;
		private byte clearCDPos;
//		private FarmInfo farmInfo;
//		private List<LandVo> landInfo;
		public override ViewLayer layerType
		{
			get { return ViewLayer.MiddleLayer; }
		}

		protected override void Init()
		{
			Singleton<MySeedsView>.Instance.gameObject = FindChild("MySeedsView");
			Singleton<MySeedsView>.Instance.Init();
			Singleton<SeedsStoreView>.Instance.gameObject = FindChild("SeedsStoreView");
			Singleton<SeedsStoreView>.Instance.Init();
			Singleton<FarmLogView>.Instance.gameObject = FindChild("FarmLogView");
			Singleton<FarmLogView>.Instance.Init();


			btn_close = FindInChild<Button>("zz/topright/btn_close");
			btn_rz = FindInChild<Button>("zz/Left/btn_rz");
			btn_zz = FindInChild<Button>("zz/Left/btn_zz");
			btn_sd = FindInChild<Button>("zz/Left/btn_sd");
			btn_sjtd = FindInChild<Button>("zz/Left/btn_sjtd");
			btn_sh = FindInChild<Button>("zz/Left/btn_sh");
			btn_clear_cd_ensure = FindInChild<Button>("zz/Left/ClearCDEnsurePannel/btn_ensure");
			btn_clear_cd_cancle = FindInChild<Button>("zz/Left/ClearCDEnsurePannel/btn_quxiao");
			clearCDEnsureTips = FindInChild<Transform>("zz/Left/ClearCDEnsurePannel").gameObject;

			farmLevel = FindInChild<UILabel>("zz/Left/Info/Level/Value");
			farmExp = FindInChild<UISlider>("zz/Left/Info/Exp");
			farmExpValue = FindInChild<UILabel>("zz/Left/Info/Exp/Value");
			info = FindInChild<Transform>("zz/Left/Info");
			btn_fhjy = FindInChild<Button>("zz/Left/btn_fhjy");
			lands = FindInChild<Transform>("zz/Left/tudi");

			btn_land1 = FindInChild<Button>("zz/Left/tudi/1/colider-1");
			btn_land2 = FindInChild<Button>("zz/Left/tudi/2/colider-2");
			btn_land3 = FindInChild<Button>("zz/Left/tudi/3/colider-3");
			btn_land4 = FindInChild<Button>("zz/Left/tudi/4/colider-4");
			btn_land5 = FindInChild<Button>("zz/Left/tudi/5/colider-5");
			btn_land6 = FindInChild<Button>("zz/Left/tudi/6/colider-6");
			btn_land7 = FindInChild<Button>("zz/Left/tudi/7/colider-7");
			btn_land8 = FindInChild<Button>("zz/Left/tudi/8/colider-8");
//			btn_land9 = FindInChild<Button>("zz/Left/tudi/9/colider-9");
			btn_kj1 = FindInChild<Button>("zz/Left/tudi/1/close/btn_kj-1");
			btn_kj2 = FindInChild<Button>("zz/Left/tudi/2/close/btn_kj-2");
			btn_kj3 = FindInChild<Button>("zz/Left/tudi/3/close/btn_kj-3");
			btn_kj4 = FindInChild<Button>("zz/Left/tudi/4/close/btn_kj-4");
			btn_kj5 = FindInChild<Button>("zz/Left/tudi/5/close/btn_kj-5");
			btn_kj6 = FindInChild<Button>("zz/Left/tudi/6/close/btn_kj-6");
			btn_kj7 = FindInChild<Button>("zz/Left/tudi/7/close/btn_kj-7");
			btn_kj8 = FindInChild<Button>("zz/Left/tudi/8/close/btn_kj-8");
//			btn_kj9 = FindInChild<Button>("zz/Left/tudi/9/close/btn_kj-9");

			btn_clearCD1 = FindInChild<Button>("zz/Left/tudi/1/CD/btn_clear_cd-1");
			btn_clearCD2 = FindInChild<Button>("zz/Left/tudi/2/CD/btn_clear_cd-2");
			btn_clearCD3 = FindInChild<Button>("zz/Left/tudi/3/CD/btn_clear_cd-3");
			btn_clearCD4 = FindInChild<Button>("zz/Left/tudi/4/CD/btn_clear_cd-4");
			btn_clearCD5 = FindInChild<Button>("zz/Left/tudi/5/CD/btn_clear_cd-5");
			btn_clearCD6 = FindInChild<Button>("zz/Left/tudi/6/CD/btn_clear_cd-6");
			btn_clearCD7 = FindInChild<Button>("zz/Left/tudi/7/CD/btn_clear_cd-7");
			btn_clearCD8 = FindInChild<Button>("zz/Left/tudi/8/CD/btn_clear_cd-8");


			friendsGroup = FindInChild<Transform>("zz/right/Friends/Grid");


			btn_close.onClick = CloseFarmView;
			btn_rz.onClick = OpenFarmLog;
			btn_land1.onClick = LandOnClick;
			btn_land2.onClick = LandOnClick;
			btn_land3.onClick = LandOnClick;
			btn_land4.onClick = LandOnClick;
			btn_land5.onClick = LandOnClick;
			btn_land6.onClick = LandOnClick;
			btn_land7.onClick = LandOnClick;
			btn_land8.onClick = LandOnClick;
//			btn_land9.onClick = LandOnClick;
			btn_kj1.onClick = ExpandLandOnClick;
			btn_kj2.onClick = ExpandLandOnClick;
			btn_kj3.onClick = ExpandLandOnClick;
			btn_kj4.onClick = ExpandLandOnClick;
			btn_kj5.onClick = ExpandLandOnClick;
			btn_kj6.onClick = ExpandLandOnClick;
			btn_kj7.onClick = ExpandLandOnClick;
			btn_kj8.onClick = ExpandLandOnClick;
//			btn_kj9.onClick = ExpandLandOnClick;
			btn_clearCD1.onClick = ClearCDOnClick;
			btn_clearCD2.onClick = ClearCDOnClick;
			btn_clearCD3.onClick = ClearCDOnClick;
			btn_clearCD4.onClick = ClearCDOnClick;
			btn_clearCD5.onClick = ClearCDOnClick;
			btn_clearCD6.onClick = ClearCDOnClick;
			btn_clearCD7.onClick = ClearCDOnClick;
			btn_clearCD8.onClick = ClearCDOnClick;

			btn_fhjy.onClick = BackToMyFarm;
			btn_zz.onClick = MySeedsOnClick;
			btn_sd.onClick = SeedsStoreOnClick;
			btn_sjtd.onClick = UpLevelLandsOnClick;
			btn_sh.onClick = AllGetOnClick;
			btn_clear_cd_ensure.onClick = EnsureClearCDOnClick;
			btn_clear_cd_cancle.onClick = CancleClearCDOnClick;
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated += UpdateFarmData;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			remainTimeDic.Clear ();
			this.UpdateFarmView (MeVo.instance.Id);
//			Singleton<FarmMode>.Instance.ApplyFriendsFarmBaseInfo ();  //请求好友农场简略信息
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated -= UpdateFarmData;
			
		}

		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
		}

		//请求农场更新
		public void UpdateFarmView(uint farmId)
		{
//			Singleton<WaitingView>.Instance.OpenView ();
//			uint farmId = Singleton<FarmMode>.Instance.farmInfo.id;
			if (farmId == MeVo.instance.Id)
				Singleton<FarmMode>.Instance.ApplyFarmInfo ();  //请求农场信息
			else
				Singleton<FarmMode>.Instance.ApplyFriendFarmInfo(farmId);  //请求农场信息

			Singleton<FarmMode>.Instance.ApplyFriendsFarmBaseInfo ();  //请求好友农场简略信息
		}

		public override void Update ()
		{
			base.Update ();
			curTime += Time.deltaTime;
			if (curTime > 1.0f)
			{
				durTime = (uint)curTime / 1;
				curTime -= durTime;

				Dictionary<UILabel, uint> updateRemainTimeDic = new Dictionary<UILabel, uint>();
//				updateRemainTimeDic.Clear();
				foreach (UILabel key in remainTimeDic.Keys)
				{
//					Log.info(this, key.parent.name);
//					Log.info(this, remainTimeDic[key].ToString());
					if (remainTimeDic[key] > durTime)
					{
						updateRemainTimeDic.Add(key, remainTimeDic[key] - durTime);
						UIUtils.ShowTimeToHMS((int)updateRemainTimeDic[key], key);
					}
					else
					{
						Transform land = key.transform.parent;

						land.FindChild("CD").gameObject.SetActive(false);

						land.FindChild("plant").GetComponent<UISprite>().spriteName = "fruit";
						land.FindChild("plant").GetComponent<UISprite>().MakePixelPerfect();
						
						//显示可收获提示
						land.FindChild("state").gameObject.SetActive(true);
						land.FindChild("state").GetComponent<UISprite>().spriteName = "1_shouhuo";
					}

				}
				remainTimeDic = updateRemainTimeDic;
			}

		}

		//农场信息更新回调
		private void UpdateFarmData(object sender, int code)
		{
			if (code == Singleton<FarmMode>.Instance.UPDATE_FARM_INFO)
			{
				Singleton<WaitingView>.Instance.CloseView();
				this.UpdateFarmInfo();
			}
			else if (code == Singleton<FarmMode>.Instance.UPDATE_FRIENDS_FARM_BASE_INFO)
			{
				this.UpdateFriendsFarmSimpleInfo();
			}
			else if (code == Singleton<FarmMode>.Instance.UPDATE_EXPAND_NEW_LAND)
			{
				this.ActiveLand(Singleton<FarmMode>.Instance.newExpandLand);
			}
			else if (code == Singleton<FarmMode>.Instance.UPDATE_FARM_ATTR)
			{
				this.UpdateFarmAttr ();
			}
		}

		//农场基础属性更新
		private void UpdateFarmAttr()
		{
			FarmInfo farmInfo = Singleton<FarmMode>.Instance.farmInfo;
			
			farmLevel.text = farmInfo.level.ToString();
			farmExpValue.text = farmInfo.exp + "/" + farmInfo.expful;
			farmExp.value = (float)farmInfo.exp / farmInfo.expful;
		}
		//农场信息更新
		private void UpdateFarmInfo()
		{
			remainTimeDic.Clear ();

			FarmInfo farmInfo = Singleton<FarmMode>.Instance.farmInfo;
			Dictionary<byte, LandVo> landInfo = farmInfo.landInfo;

			//判断是否为自己的农场
			if (farmInfo.id == MeVo.instance.Id)
			{
				btn_sh.gameObject.SetActive(true);
				btn_sjtd.gameObject.SetActive (landInfo.Count >= LAND_MAX_NUM?true:false);
				this.UpdateFarmAttr ();
				btn_fhjy.gameObject.SetActive(false);
				info.gameObject.SetActive(true);

				btn_kj1.gameObject.SetActive(true);
				btn_kj2.gameObject.SetActive(true);
				btn_kj3.gameObject.SetActive(true);
				btn_kj4.gameObject.SetActive(true);
				btn_kj5.gameObject.SetActive(true);
				btn_kj6.gameObject.SetActive(true);
				btn_kj7.gameObject.SetActive(true);
				btn_kj8.gameObject.SetActive(true);
//				btn_kj9.gameObject.SetActive(true);
				
			}
			else
			{
				btn_sh.gameObject.SetActive(false);
				btn_sjtd.gameObject.SetActive(false);
				btn_fhjy.gameObject.SetActive(true);
				info.gameObject.SetActive(false);

				btn_kj1.gameObject.SetActive(false);
				btn_kj2.gameObject.SetActive(false);
				btn_kj3.gameObject.SetActive(false);
				btn_kj4.gameObject.SetActive(false);
				btn_kj5.gameObject.SetActive(false);
				btn_kj6.gameObject.SetActive(false);
				btn_kj7.gameObject.SetActive(false);
				btn_kj8.gameObject.SetActive(false);
//				btn_kj9.gameObject.SetActive(false);
			}

			//显示土地
			byte pos;
			foreach (Transform child in lands)
			{
				pos = byte.Parse(child.name);
				if (landInfo.ContainsKey(pos))
					this.ShowOpenedLand(landInfo[pos]);
				else
				{
					this.ShowClosedLand(pos);
				}
			}
//			foreach (byte key in landInfo.Keys)
//			{
//				this.ShowOpenedLand(landInfo[key]);
//				Log.info(this, "土地编号:" + landInfo[key].pos); 
//				Log.info(this, "土地颜色:" + landInfo[key].color);
//				Log.info(this, "种子ID:" + landInfo[key].seedId);
//				Log.info(this, "收获剩余时间:" + landInfo[key].remainTime);
//				Log.info(this, "数量:" + landInfo[key].num);
//				Log.info(this, "是否可偷:" + landInfo[key].canSteal);
//				Log.info(this, "田地状态:" + landInfo[key].status);
//			}
		}

		//展示未开垦土地
		private void ShowClosedLand(byte pos)
		{
			Transform land;
			land = lands.FindChild (pos.ToString ());
			land.GetComponent<UISprite>().color = LAND1_COLOR;
			land.FindChild ("close").gameObject.SetActive (true);
			land.FindChild ("colider-" + land.name).gameObject.SetActive (false);
			land.FindChild("state").gameObject.SetActive(false);
			land.FindChild("plant").gameObject.SetActive(false);
			land.FindChild("CD").gameObject.SetActive(false);
//			land.FindChild ("close/btn_kj-" + pos).gameObject.SetActive (true);
		}

		//展示已开垦土地
		private void ShowOpenedLand(LandVo landVo)
		{
			Transform land;
			land = lands.FindChild(landVo.pos.ToString());

			//关闭未开垦土地效果，开启土地点击检测
			land.FindChild ("close").gameObject.SetActive (false);
			land.FindChild ("colider-" + land.name).gameObject.SetActive (true);

			//显示土地
			switch (landVo.color)
			{
				case 1:
					land.GetComponent<UISprite>().color = LAND1_COLOR;
					break;
				case 2:
					land.GetComponent<UISprite>().color = LAND2_COLOR;
					break;
				case 3:
					land.GetComponent<UISprite>().color = LAND3_COLOR;
					break;
				default:
					Log.error(this, "土地颜色参数错误");
					break;
			}



			//根据田地状态显示效果.0普通状态 1除虫 2除草 3浇水
			switch (landVo.status)
			{
				case (byte)LandStatu.NORMAL:
					land.FindChild("state").gameObject.SetActive(false);
					break;
				case (byte)LandStatu.KILL_WORM:
					land.FindChild("state").gameObject.SetActive(true);
					land.FindChild("state").GetComponent<UISprite>().spriteName = "1_chuchong";
					break;
				case (byte)LandStatu.CUT_GRASS:
					land.FindChild("state").gameObject.SetActive(true);
					land.FindChild("state").GetComponent<UISprite>().spriteName = "1_gecao";
					break;
				case (byte)LandStatu.WATER:
					land.FindChild("state").gameObject.SetActive(true);
					land.FindChild("state").GetComponent<UISprite>().spriteName = "1_jiaoshui";
					break;
			}

			//显示农作物
			if (landVo.seedId != 0)
			{
//				land.FindChild("plant").gameObject.SetActive(false);
				//农作物未成熟
				if (landVo.remainTime > 0)
				{
					//显示剩余时间
					land.FindChild("CD").gameObject.SetActive(true);
					UIUtils.ShowTimeToHMS((int)landVo.remainTime, land.FindChild("CD/Value").GetComponent<UILabel>());

					UILabel remainTime = land.FindChild("CD/Value").GetComponent<UILabel>();
					if (remainTimeDic.ContainsKey(remainTime))
						remainTimeDic[remainTime] = landVo.remainTime;
					else
						remainTimeDic.Add(remainTime, landVo.remainTime);

					//显示未成熟的农作物
					land.FindChild("plant").GetComponent<UISprite>().spriteName = "tree";
					land.FindChild("plant").GetComponent<UISprite>().MakePixelPerfect();
				}
				else
				{
					land.FindChild("CD").gameObject.SetActive(false);
					//显示成熟的农作物
					land.FindChild("plant").GetComponent<UISprite>().spriteName = "fruit";
					land.FindChild("plant").GetComponent<UISprite>().MakePixelPerfect();

					//显示可收获提示
					land.FindChild("state").gameObject.SetActive(true);
					land.FindChild("state").GetComponent<UISprite>().spriteName = "1_shouhuo";
				}
				land.FindChild("plant").gameObject.SetActive(true);
			}
			else
			{
				land.FindChild("plant").gameObject.SetActive(false);
				land.FindChild("CD").gameObject.SetActive(false);
			}

			//显示是否可偷
			if (landVo.canSteal)
			{
				land.FindChild("state").gameObject.SetActive(true);
				land.FindChild("state").GetComponent<UISprite>().spriteName = "1_shouhuo";
			}

		}

//		private void CloneSomeObj(int num, )

		//好友农场简略信息更新
		private void UpdateFriendsFarmSimpleInfo()
		{
			friendsFarmSimpleInfo = Singleton<FarmMode>.Instance.friendsFarmBaseInfo;

			int friendsNum = friendsFarmSimpleInfo.Count;
			int objNum = friendsGroup.childCount;

			//确保有足够多的对象
			Transform ObjMode = friendsGroup.GetChild (0);
			while (objNum < friendsNum)
			{
				UIUtils.CloneObj(ObjMode);
				objNum++;
			}

			//更新所有好友对象
			FriendFarmBaseInfo farmBaseInfo;
			int sub = 0;
			int rank;
			foreach (Transform child in friendsGroup)
			{
				if (sub < friendsNum)
				{
					farmBaseInfo = friendsFarmSimpleInfo[sub];
					child.FindChild("level/value").GetComponent<UILabel>().text = farmBaseInfo.level.ToString();
					child.FindChild("name").GetComponent<UILabel>().text = farmBaseInfo.name;

					//显示排名
					rank = sub + 1;
					child.FindChild("rank/value").GetComponent<UILabel>().text = rank.ToString();
					if (rank > 3)
					{
						child.FindChild("rank/background").GetComponent<UISprite>().spriteName = "rank4";
						child.FindChild("rank/value").localPosition = new Vector3(0, 0, 0);
					}
					else
					{
						child.FindChild("rank/background").GetComponent<UISprite>().spriteName = "rank" + rank;
						if (rank == 1)
						{
							child.FindChild("rank/value").localPosition = new Vector3(0, -8, 0);
						}
						else
						{
							child.FindChild("rank/value").localPosition = new Vector3(0, 0, 0);
						}
					}

					//显示状态（二进制表示 第一位可浇水 第二位可除草 第三位可除虫 第四位可收获（偷取））
					byte state = farmBaseInfo.status;
					int stateSub = 1;
//					Log.info(this, "Statu: " + state);
					if ((state >> 3 & 1) == 1)
					{
						child.FindChild("status-" + stateSub).GetComponent<UISprite>().spriteName = "1_jiaoshui";
						child.FindChild("status-" + stateSub).gameObject.SetActive(true);
						stateSub++;
					}
					if ((state >> 2 & 1) == 1)
					{
						child.FindChild("status-" + stateSub).GetComponent<UISprite>().spriteName = "1_gecao";
						child.FindChild("status-" + stateSub).gameObject.SetActive(true);
						stateSub++;
					}
					if ((state >> 1 & 1) == 1)
					{
						child.FindChild("status-" + stateSub).GetComponent<UISprite>().spriteName = "1_chuchong";
						child.FindChild("status-" + stateSub).gameObject.SetActive(true);
						stateSub++;
					}
					if ((state & 1) == 1)
					{
						child.FindChild("status-" + stateSub).GetComponent<UISprite>().spriteName = "1_shouhuo";
						child.FindChild("status-" + stateSub).gameObject.SetActive(true);
						stateSub++;
					}
					for (; stateSub <= 4; ++stateSub)
					{
						child.FindChild("status-" + stateSub).gameObject.SetActive(false);
					}

					//排序
					child.GetComponent<UIToggle>().onSelect = FriendFarmOnClick;

					child.name = sub + "-" + farmBaseInfo.id;
					child.gameObject.SetActive(true);
				}
				else
				{
					child.gameObject.SetActive(false);
				}
				sub++;
			}

//			for (int i = 0; i < friendsNum; ++i)
//			{
//				Log.info(this, "好友ID:" + friendsFarmSimpleInfo[i].id);
//				Log.info(this, "角色名:" + friendsFarmSimpleInfo[i].name);
//				Log.info(this, "农场等级:" + friendsFarmSimpleInfo[i].level);
//				Log.info(this, "农场经验:" + friendsFarmSimpleInfo[i].exp);
//				Log.info(this, "农场状态:" + friendsFarmSimpleInfo[i].status);//各种状态 二进制表示 第一位可浇水 第二位可除草 第三位可除虫 第四位可收获（偷取）
//			}

			friendsGroup.GetComponent<UIGrid>().Reposition ();
		}

		//激活新开垦土地
		private void ActiveLand(byte pos)
		{
			Transform newLand = lands.FindChild (pos.ToString ());
			newLand.FindChild ("close").gameObject.SetActive (false);
			newLand.FindChild ("colider-" + pos).gameObject.SetActive (true);
		}

		//---------------------------------------------  按钮点击触发 ---------------------------------------------------------
		//土地被点击
		private void LandOnClick(GameObject go)
		{
			byte landPos = byte.Parse (go.name.Split ('-') [1]);

			LandVo landVo = Singleton<FarmMode>.Instance.farmInfo.landInfo [landPos];
			FarmInfo farmInfo = Singleton<FarmMode>.Instance.farmInfo;



			//判断是否可以收获果实
			if (landVo.seedId !=0 && landVo.remainTime == 0)
			{
				Singleton<FarmMode>.Instance.ApplyFarmOpe(farmInfo.id, landPos, 0);  //获得果实
//				this.UpdateFarmInfo (farmInfo.id);
			}
			else if (landVo.status != 0)
			{
				Singleton<FarmMode>.Instance.ApplyFarmOpe(farmInfo.id, landPos, landVo.status); //操作农场
//				this.UpdateFarmInfo (farmInfo.id);

			}
			else if (landVo.seedId == 0)
			{
				if (farmInfo.id == MeVo.instance.Id)
				{
					Singleton<FarmMode>.Instance.selectedLandPos = landPos;
					Singleton<MySeedsView>.Instance.OpenView();//打开我的种子背包
				}
			}
		}



		//点击好友农场
		private void FriendFarmOnClick(GameObject go, bool state)
		{
			if (state)
			{
				uint friendId = uint.Parse (go.name.Split ('-') [1]);
//				Singleton<FarmMode>.Instance.ApplyFriendFarmInfo (friendId);
				if (friendId != Singleton<FarmMode>.Instance.farmInfo.id)
				{
					Singleton<WaitingView>.Instance.OpenView();
					this.UpdateFarmView(friendId);
				}
			}
		}

		//扩建土地按钮被点击
		private void ExpandLandOnClick(GameObject go)
		{
			//			byte landPos = byte.Parse (go.name.Split ('-') [1]);
			Singleton<FarmMode>.Instance.ApplyExpandLand ();
		}

		//关闭家园种植界面
		private void CloseFarmView(GameObject go)
		{
			base.CloseView ();
		}



		//打开家园种植日志
		private void OpenFarmLog(GameObject go)
		{
			Singleton<FarmLogView>.Instance.OpenView ();
		}

		//返回家园按钮点击
		private void BackToMyFarm(GameObject go)
		{
			Singleton<WaitingView>.Instance.OpenView ();
			this.UpdateFarmView (MeVo.instance.Id);
		}

		//我的种子按钮点击
		private void MySeedsOnClick(GameObject go)
		{
			Singleton<FarmMode>.Instance.selectedLandPos = 0;
			Singleton<MySeedsView>.Instance.OpenView ();
		}

		//种子商店按钮点击
		private void SeedsStoreOnClick(GameObject go)
		{
			Singleton<SeedsStoreView>.Instance.OpenView ();
		}

		//升级土地按钮点击
		private void UpLevelLandsOnClick(GameObject go)
		{
			Singleton<FarmMode>.Instance.ApplyUpdateLand ();
		}

		//一键收获按钮点击
		private void AllGetOnClick(GameObject go)
		{
			Singleton<FarmMode>.Instance.ApplyGainAll ();
		}

		//清除CD按钮被点击
		private void ClearCDOnClick(GameObject go)
		{
			clearCDPos = byte.Parse (go.name.Split ('-') [1]);
			clearCDEnsureTips.SetActive (true);
		}

		//确认清除CD按钮被点击
		private void EnsureClearCDOnClick(GameObject go)
		{
			clearCDEnsureTips.SetActive (false);
			Singleton<FarmMode>.Instance.ApplyFastGrowUp (clearCDPos, 0);
		}

		//取消清除CD选择按钮被点击
		private void CancleClearCDOnClick(GameObject go)
		{
			clearCDEnsureTips.SetActive (false);
		}


	}
}