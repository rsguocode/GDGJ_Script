//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：PlayerDetailView
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
using PCustomDataType;
using Com.Game.Module.Role;
using com.game.manager;
using Com.Game.Module.Manager;
using com.game.data;

namespace Com.Game.Module.Tips
{
	public class PlayerDetailView : BaseView<PlayerDetailView> 
	{		
		public override string url { get { return "UI/Tips/RoleInfo/PlayerDetailView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}

		public Button btnClose;
		private UILabel labLevel;
		private UILabel labName;
		private UILabel labForce;

		private uint playerId;
		public PGoods currentGoods;
		private List<ItemContainer> equipList = new List<ItemContainer>();//左边面板装备
		private IDictionary<uint, RoleDisplay> roleModelDict = new Dictionary<uint, RoleDisplay>();
		private SpinWithMouse modelSpin;
		private GameObject roleBackground;
		private uint roleJob;
		private GameObject centerObj;

		private Vector3 centerPos = new Vector3(240f, 0f, 0f);
		private Vector3 leftPos = Vector3.zero;

		private RoleDisplay roleModel
		{
			get
			{
				if (roleModelDict.ContainsKey(roleJob))
				{
					return roleModelDict[roleJob];
				}
				else
				{
					return null;
				}
			}
		}

		protected override void Init()
		{
			btnClose = FindInChild<Button>("center/topright/btn_close");
			labLevel = FindInChild<UILabel>("center/info/title/lvl");
			labName = FindInChild<UILabel>("center/info/title/name");
			labForce = FindInChild<UILabel>("center/info/force/zl");
			centerObj = transform.Find("center").gameObject;

			roleBackground = FindChild("center/rolebackground");
			modelSpin = NGUITools.FindInChild<SpinWithMouse>(roleBackground, "rolebackgroundl");

			btnClose.onClick = CloseOnClick;

			GetLeftItemList();
		}

		private void CloseOnClick(GameObject go)
		{
			CloseView();
		}

		private void GetLeftItemList()
		{
			for (int i=1; i<11; i++)
			{
				ItemContainer temp = FindChild("center/equip/left/item"+i).AddMissingComponent<ItemContainer>();
				temp.onClick = ShowLeftTips;
				equipList.Add(temp);
			}

			InitDefalutEquipItem();			
		}

		private void AddTweenPositionComponent(Vector3 from, Vector3 to)
		{
			TweenPosition tweenPosition = centerObj.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}

			tweenPosition = centerObj.AddComponent<TweenPosition>();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.style = UITweener.Style.Once;
			tweenPosition.method = UITweener.Method.QuintEaseInOut;
			tweenPosition.duration = 0.5f;
		}

		//单击左边装备，显示 Tips 界面
		private void ShowLeftTips(GameObject go)
		{
			ItemContainer ic = go.GetComponent<ItemContainer>();
			if (ic.Id != 0)
			{
				currentGoods = GoodsMode.Instance.GetOtherPGoods(ic.Id);
				OpenTips();	
				AddTweenPositionComponent(centerPos, leftPos);
			}				
		}

		private void OpenTips()
		{
			Singleton<TipsManager>.Instance.OpenPlayerEquipTipsById(currentGoods.id, null, null, RestorePos, LanguageManager.GetWord("Goods.Forge"),
			                                             LanguageManager.GetWord("Goods.TakeOff"), TipsType.DEFAULT_TYPE);
		}

		private void RestorePos()
		{
			AddTweenPositionComponent(leftPos, centerPos);
		}

		private void InitDefalutEquipItem()
		{
			int index = 1;
			foreach (ItemContainer temp in equipList)
			{
				ItemManager.Instance.InitItem(temp,ItemManager.Instance.EMPTY_ICON,ItemType.Equip);
				temp.FindInChild<UILabel>("pos").text = LanguageManager.GetWord("Equip.Pos" + (index));
				temp.FindInChild<UILabel>("stren").text = string.Empty;
				temp.FindInChild<UISprite>("background").alpha = 0.3f;
				temp.FindInChild<UISprite>("icon").alpha = 0.3f;
				temp.isEnabled = false;
				temp.Id = 0;
				index ++;
			}
		}

		private void UpdateEquipInfo()
		{
			List<PGoods> equipInfoList = Singleton<GoodsMode>.Instance.otherList;
			InitDefalutEquipItem();
			SysEquipVo vo;
			ItemContainer ic;
			foreach (PGoods goods in equipInfoList)
			{
				vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
				ic = equipList[vo.pos - 1];
				ic.Id = goods.id;
				ic.FindInChild<UILabel>("pos").text = string.Empty;
				Singleton<ItemManager>.Instance.InitItem(ic, goods.goodsId, ItemType.Equip);
				ic.FindInChild<UILabel>("stren").text = "+" + goods.equip[0].stren;
				ic.FindInChild<UISprite>("background").alpha = 1f;
				ic.FindInChild<UISprite>("icon").alpha = 1f;
				ic.isEnabled = true;
				ic.buttonType = Button.ButtonType.Toggle;				
			}
		}

		private void UpdatePlayerInfo()
		{
			if (Singleton<RoleMode>.Instance.otherAttr.Count < 1)
			{
				return;
			}

			PRoleAttr playerAttr = Singleton<RoleMode>.Instance.otherAttr[0];
			roleJob = playerAttr.job;
			labLevel.text = "Lv." + playerAttr.level;
			labName.text = playerAttr.name;
			labForce.text = LanguageManager.GetWord("Goods.FightPoint") + ": " + playerAttr.attr.fightPoint;	

			ShowModel();
		}

		private void ShowModel()
		{
			if (null == roleModel)
			{
				roleModelDict[roleJob] = new RoleDisplay();
				roleModel.CreateRole((int)roleJob, LoadCallBack);
			}
			else
			{
				modelSpin.target = roleModel.GoBase.transform.GetChild(0);
				roleModel.GoBase.SetActive(true);
			}
		}

		private void HideModel()
		{
			if (null != roleModel)
			{
				roleModel.GoBase.SetActive(false);
			}
		}

		public void LoadCallBack(GameObject go)
		{
			modelSpin.target = roleModel.GoBase.transform.GetChild(0);
			modelSpin.speed = 3f;
			SetModelPosition();
		}

		private void SetModelPosition()
		{
			roleBackground.SetActive(true);
			if (roleModel != null && roleModel.GoBase != null)
			{
				NGUITools.SetActive(roleModel.GoBase, true);
				roleModel.GoBase.transform.localPosition = new Vector3(0f, -162f, 0);
				roleModel.GoBase.transform.parent = roleBackground.transform;
			}
			
		}

		public void ShowWindow(uint playerId)
		{
			this.playerId = playerId;
			OpenView();
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<GoodsMode>.Instance.dataUpdated += UpdateEquipHandler;
			Singleton<RoleMode>.Instance.dataUpdated += UpdatePlayerInfoHandler;
		}

		public override void CancelUpdateHandler()
		{
			Singleton<GoodsMode>.Instance.dataUpdated -= UpdateEquipHandler;
			Singleton<RoleMode>.Instance.dataUpdated -= UpdatePlayerInfoHandler;
		}

		private void UpdateEquipHandler(object sender, int code)
		{
			if (code == Singleton<GoodsMode>.Instance.UPDATE_OTHEE_EQUIP)
			{
				UpdateEquipInfo();
			}
		}

		private void UpdatePlayerInfoHandler(object sender, int code)
		{
			if (code == Singleton<RoleMode>.Instance.UPDATE_OTHER_INFO)
			{
				UpdatePlayerInfo();
			}
		}

		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();
			Singleton<RoleControl>.Instance.RequestOtherInfo(playerId);
		}

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
			HideModel();
			Singleton<EquipTips>.Instance.CloseView();
		}
		
	}
}
