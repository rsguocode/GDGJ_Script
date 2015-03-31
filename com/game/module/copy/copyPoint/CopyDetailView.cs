using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.module.map;
using com.game.vo;
using com.game.manager;
using com.game.Public.Confirm;
using Com.Game.Module.Role;
using com.game.consts;
using com.game.Public.LocalVar;
using System.Collections.Generic;
using com.game.utils;
using com.game.Public.Message;
using com.game.Public.UICommon;
using Com.Game.Module.Manager;
using com.game.sound;
using Com.Game.Module.Tips;
using com.game.sound;

namespace Com.Game.Module.Copy
{
	public class CopyDetailView : BaseView<CopyDetailView>
	{
		private readonly int NORMAL_COPY_POINT_DEPTH = 1009;
		private readonly int GRAY_COPY_POINT_DEPTH = 1509;

		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}
		public override bool playClosedSound { get { return false; } }

		//副本详细信息UI相关
		private Button btn_close;
		public Button btn_play;
		private Button btn_fast_fight1;
		private Button btn_fast_fight10;
		private Transform stars;
		private UILabel remainTimes;
		private UILabel needVigour;
		private UILabel vigour;
		private UISprite smallMap;
		private UIGrid monstersGrid;
		private UIGrid bossGrid;
		private UIGrid goodsGrid;
		private List<Transform> monsterList = new List<Transform> ();
		private List<Transform> goodsList = new List<Transform> ();
		private List<Transform> bossList = new List<Transform> ();

		private uint copyPointId;
		private int maxFightNum;
		private int needVigourValue;
		private List<string> awards = new List<string> ();
		private UIAtlas copyIconAtlas;
		private UIAtlas monsterHeadIconAtlas;
		protected override void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button> ("btn_guanbi");
			btn_play = FindInChild<Button> ("btn_play");
			btn_fast_fight1 = FindInChild<Button> ("btn_sd1");
			btn_fast_fight10 = FindInChild<Button> ("btn_sd10");
			stars = FindInChild<Transform> ("Info/Stars");
			remainTimes = FindInChild<UILabel> ("Info/RemainTimes/Value");
			needVigour = FindInChild<UILabel> ("Info/NeedVigour/Value");
			vigour = FindInChild<UILabel> ("Info/Vigour/Value");
			smallMap = FindInChild<UISprite> ("Info/ditu");
			monstersGrid = FindInChild<UIGrid> ("CopyMonsters/Monsters");
			bossGrid = FindInChild<UIGrid> ("CopyMonsters/Monsters/Boss");
			goodsGrid = FindInChild<UIGrid> ("CopyGoods/Grid");
			monsterList.Add (FindInChild<Transform> ("CopyMonsters/Monsters/1"));
			monsterList.Add (FindInChild<Transform> ("CopyMonsters/Monsters/2"));
			monsterList.Add (FindInChild<Transform> ("CopyMonsters/Monsters/3"));
			monsterList.Add (FindInChild<Transform> ("CopyMonsters/Monsters/4"));
			monsterList.Add (FindInChild<Transform> ("CopyMonsters/Monsters/5"));
			monsterList.Add (FindInChild<Transform> ("CopyMonsters/Monsters/6"));
			bossList.Add(FindInChild<Transform> ("CopyMonsters/Monsters/Boss/1"));
			bossList.Add(FindInChild<Transform> ("CopyMonsters/Monsters/Boss/2"));
			bossList.Add(FindInChild<Transform> ("CopyMonsters/Monsters/Boss/3"));
			bossList.Add(FindInChild<Transform> ("CopyMonsters/Monsters/Boss/4"));
			goodsList.Add(FindInChild<Transform> ("CopyGoods/Grid/1"));
			goodsList.Add(FindInChild<Transform> ("CopyGoods/Grid/2"));
			goodsList.Add(FindInChild<Transform> ("CopyGoods/Grid/3"));
			goodsList.Add(FindInChild<Transform> ("CopyGoods/Grid/4"));


			btn_close.onClick = CloseCopyDetailView;
			btn_play.onClick = PlayOnClick;
			btn_fast_fight1.onClick = FastFight1OnClick;
			btn_fast_fight10.onClick = FastFight10OnClick;

			for (int i = 0; i < goodsList.Count; ++i)
			{
				goodsList[i].GetComponent<Button>().onClick = GoodsOnClick;
			}

			monsterHeadIconAtlas = Singleton<AtlasManager>.Instance.GetAtlas("MonsterHeadAtlas");
			for (int i = 0; i < monsterList.Count; ++i)
			{
				monsterList[i].FindChild("touxiang").GetComponent<UISprite>().atlas = monsterHeadIconAtlas;//
			}
			for (int i = 0; i < bossList.Count; ++i)
			{
				bossList[i].FindChild("touxiang").GetComponent<UISprite>().atlas = monsterHeadIconAtlas;//
			}
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("Info/Stars/Label").text = LanguageManager.GetWord("CopyDetailView.starLevel");
			FindInChild<UILabel>("Info/RemainTimes/Label").text = LanguageManager.GetWord("CopyDetailView.times");
			FindInChild<UILabel>("Info/Vigour/Label").text = LanguageManager.GetWord("CopyDetailView.remainVigour");
			FindInChild<UILabel>("Info/NeedVigour/Label").text = LanguageManager.GetWord("CopyDetailView.needVigour");
			FindInChild<UILabel>("CopyMonsters/Label").text = LanguageManager.GetWord("CopyDetailView.copy") + "\n" + 
															  LanguageManager.GetWord("CopyDetailView.monster");
			FindInChild<UILabel>("CopyGoods/Label").text = LanguageManager.GetWord("CopyDetailView.copy") + "\n" + 
														   LanguageManager.GetWord("CopyDetailView.drop");
			FindInChild<UILabel>("btn_play/Label").text = LanguageManager.GetWord("CopyDetailView.challenge");
			FindInChild<UILabel>("btn_sd1/label").text = LanguageManager.GetWord("CopyDetailView.fastFight");
		}
		
		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<CopyMode>.Instance.dataUpdated += UpdateCopyDetailView;
			Singleton<RoleMode>.Instance.dataUpdated += UpdateRoleMode;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
            //重新制定事件
            btn_close.onClick = CloseCopyDetailView;
            btn_play.onClick = PlayOnClick;
            btn_fast_fight1.onClick = FastFight1OnClick;
            btn_fast_fight10.onClick = FastFight10OnClick;
            //end desuo添加

			base.HandleAfterOpenView ();

			if (copyIconAtlas == null)
			{
				Singleton<AtlasManager>.Instance.LoadAtlasHold(AtlasUrl.CopyIconHold, AtlasUrl.CopyIconNormal, LoadCopyIconAtlasCallBack, true);
			}


			copyPointId = Singleton<CopyPointMode>.Instance.selectedCopyPoint;
			needVigourValue = BaseDataMgr.instance.GetMapVo (copyPointId).vigour;
			ShowCopyDetailView ();

			//播放选择具体的副本音效
			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_CopySelect);

		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<CopyMode>.Instance.dataUpdated -= UpdateCopyDetailView;
			Singleton<RoleMode>.Instance.dataUpdated -= UpdateRoleMode;
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
			}
		}

		//挑战次数更新回调
		private void UpdateCopyDetailView(object sender,int code)
		{
			if (code == Singleton<CopyMode>.Instance.LOADING_VIEW_OPEND)
			{
				this.CloseView ();             //在loading界面打开后再关闭
				Singleton<CopyPointView>.Instance.CloseView();
			}
		}

		//玩家属性更新回调
		private void UpdateRoleMode(object sender,int code)
		{
			if (code == RoleMode.UPDATE_VIGOUR)
			{
				vigour.text = MeVo.instance.vigour.ToString ();
				vigour.color = MeVo.instance.vigour < needVigourValue? Color.red: new Color(128.0f / 255, 224.0f / 255, 64.0f / 255, 1);
				UpdateMaxFightTimes();
			}
		}

		//显示选中副本详细信息
		private void ShowCopyDetailView()
		{
			needVigour.text = needVigourValue.ToString();   //显示选中副本需要消耗的体力点
			vigour.text = MeVo.instance.vigour.ToString ();
			vigour.color = MeVo.instance.vigour < needVigourValue? Color.red: new Color(128.0f / 255, 224.0f / 255, 64.0f / 255, 1);
//			smallMap.spriteName = BaseDataMgr.instance.GetSysDungeonTree (copyPointId).icon;
			smallMap.atlas = copyIconAtlas;
			smallMap.spriteName = BaseDataMgr.instance.GetMapVo (copyPointId).resource_id.ToString();;
			//显示选中副本星星获取数量
			int starNum = Singleton<CopyPointMode>.Instance.GetStarNum (copyPointId);
			for (int j = 1; j <= 3; ++j)  
			{
				stars.FindChild("xx" + j).GetComponent<UISprite>().spriteName   
					= j <= starNum?"xingxing1":"kongxing";
			}

			remainTimes.text = LanguageManager.GetWord("CopyDetailView.boundless");           //主线副本没有次数限制
			UpdateMaxFightTimes ();
			string[] monsters = StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (copyPointId).monster_icon);
			ShowMonsters (monsterList, monsters, monstersGrid);

			string[] bosses = StringUtils.GetValueListFromString (
				BaseDataMgr.instance.GetSysDungeonTree (copyPointId).boss_icon);
			ShowMonsters (bossList, bosses, bossGrid);

			ShowGoods (goodsList, goodsGrid);

			ShowFastFuncState ();
		}

		//更新最大扫荡次数
		private void UpdateMaxFightTimes()
		{
			int myVigour = MeVo.instance.vigour;
			int copyVigour = BaseDataMgr.instance.GetMapVo (copyPointId).vigour;
			maxFightNum = (myVigour / copyVigour) > 10? 10: (myVigour / copyVigour);
			btn_fast_fight10.FindInChild<UILabel>("label").text = maxFightNum > 0? 
				(LanguageManager.GetWord("CopyDetailView.fastFight") + maxFightNum + LanguageManager.GetWord("CopyDetailView.ci")): 
				LanguageManager.GetWord("CopyDetailView.noFastFight");
		}

		//更新副本扫荡功能状态
		private void ShowFastFuncState()
		{
			//扫荡功能状态
			if (Singleton<CopyPointMode>.Instance.GetStarNum(copyPointId) < 0)
			{
				btn_fast_fight1.SetActive(false);
				btn_fast_fight10.SetActive(false);
			}
			else
			{
				btn_fast_fight1.SetActive(true);
				btn_fast_fight10.SetActive(true);
			}
		}

		//显示物品
		private void ShowGoods(List<Transform> goodsList, UIGrid grid)
		{
			List<uint> goodIdList = new List<uint> ();
			string[] award1 = StringUtils.GetValueCost (BaseDataMgr.instance.GetSysDungeonReward (copyPointId).goods1);
			string[] award2 = StringUtils.GetValueCost (BaseDataMgr.instance.GetSysDungeonReward (copyPointId).goods2);
			string[] award3 = StringUtils.GetValueCost (BaseDataMgr.instance.GetSysDungeonReward (copyPointId).goods3);
			string[] award4 = StringUtils.GetValueCost (BaseDataMgr.instance.GetSysDungeonReward (copyPointId).goods4);
			string[] extAward = StringUtils.GetValueCost (BaseDataMgr.instance.GetSysDungeonReward (copyPointId).ext_goods);
			awards.Clear ();
			for (int i = 0; i < extAward.Length; ++i)
			{
				if (extAward[i].Split(',')[0] != "")
					awards.Add(extAward[i].Split(',')[0]);
			}
			for (int i = 0; i < award4.Length; ++i)
			{
				if (award4[i].Split(',')[0] != "")
					awards.Add(award4[i].Split(',')[0]);
			}
			for (int i = 0; i < award3.Length; ++i)
			{
				if (award3[i].Split(',')[0] != "")
					awards.Add(award3[i].Split(',')[0]);
			}
			for (int i = 0; i < award2.Length; ++i)
			{
				if (award2[i].Split(',')[0] != "")
					awards.Add(award2[i].Split(',')[0]);
			}
			for (int i = 0; i < award1.Length; ++i)
			{
				if (award1[i].Split(',')[0] != "")
					awards.Add(award1[i].Split(',')[0]);
			}
			
			for (int i = 0; i < goodsList.Count; ++i)
			{
				if (i < awards.Count)
				{
					ItemManager.Instance.InitItem (goodsList[i].gameObject, uint.Parse(awards[i]), 0);
					goodsList[i].gameObject.SetActive(true);
				}
				else
				{
					goodsList[i].gameObject.SetActive(false);
				}
			}
			grid.Reposition ();
		}

		//显示怪物头像
		private void ShowMonsters(List<Transform> monList, string[] sprNameStr, UIGrid grid)
		{
			for (int i = 0; i < monList.Count; ++i)
			{
				if (i < sprNameStr.Length)
				{
					monList[i].FindChild("touxiang").GetComponent<UISprite>().spriteName =
						BaseDataMgr.instance.getSysMonsterVo(uint.Parse(sprNameStr[i])).icon.ToString();
					monList[i].gameObject.SetActive(true);
				}
				else
				{
					monList[i].gameObject.SetActive(false);
				}
			}
			grid.Reposition ();
		}

		//关闭副本详细界面
		private void CloseCopyDetailView(GameObject go)
		{
			this.CloseView ();
		}

		//挑战按钮被点击
		private void PlayOnClick(GameObject go)
		{
			//播放副本出征音效
			SoundMgr.Instance.PlayUIAudio(SoundId.Sound_CopyChallenge);
			GoToScene (copyPointId);
		}

		private void GoToScene(uint mapId)
		{
			if (MeVo.instance.vigour >= BaseDataMgr.instance.GetMapVo(mapId).vigour)
			{
				LocalVarManager.SetInt (LocalVarManager.COPY_WORLD_ID, (int)(MeVo.instance.mapId));
				Singleton<MapMode>.Instance.changeScene(mapId, false, 5, 1.8f);
//				this.CloseView ();             //在loading界面打开后再关闭
//				Singleton<CopyView>.Instance.CloseView();
			}
			else
			{
				OpenBuyVigourTips();
			}
		}

		private void OpenBuyVigourTips()
		{
			Singleton<RoleControl>.Instance.OpenBuyVigourTips ();
		}
		
		//购买体力按钮被点击
		private void AddVigourOnClick(GameObject go)
		{
			Singleton<RoleControl>.Instance.OpenBuyVigourTips ();
		}
		


		//扫荡一次按钮被点击
		private void FastFight1OnClick(GameObject go)
		{
			if (MeVo.instance.vip >= BaseDataMgr.instance.GetVIPLowestGrade(17))
			{
				if (MeVo.instance.vigour >= BaseDataMgr.instance.GetMapVo(copyPointId).vigour)
				{
					Singleton<CopyPointMode>.Instance.fightNum = 1;
					Singleton<FastFightView>.Instance.OpenView();
				}
				else
				{
					OpenBuyVigourTips();
				}
			}
			else
			{
				MessageManager.Show(LanguageManager.GetWord("CopyDetailView.fastFightFunc") +
				                    "VIP" + BaseDataMgr.instance.GetVIPLowestGrade(17) +
				                    LanguageManager.GetWord("CopyDetailView.open"));
			}

		}

		//扫荡十次按钮被点击
		private void FastFight10OnClick(GameObject go)
		{
			if (MeVo.instance.vip >= BaseDataMgr.instance.GetVIPLowestGrade(17))
			{
				if (maxFightNum > 0)
				{
					Singleton<CopyPointMode>.Instance.fightNum = maxFightNum;
					Singleton<FastFightView>.Instance.OpenView();
				}
				else
				{
					if (MeVo.instance.vigour < BaseDataMgr.instance.GetMapVo(copyPointId).vigour)
					{
						OpenBuyVigourTips();
					}
				}
			}
			else
			{
				MessageManager.Show(LanguageManager.GetWord("CopyDetailView.fastFightFunc") +
				                    "VIP" + BaseDataMgr.instance.GetVIPLowestGrade(17) +
				                    LanguageManager.GetWord("CopyDetailView.open"));
			}
		}

		//物品被点击
		private void GoodsOnClick(GameObject go)
		{
			int sub = int.Parse (go.name) - 1;
			uint goodsId;
			if (sub < awards.Count)
			{
				goodsId = uint.Parse(awards[sub]);
				TipsManager.Instance.OpenTipsByGoodsId (goodsId, null, null, "", "");
			}
			
		}
	}
}
