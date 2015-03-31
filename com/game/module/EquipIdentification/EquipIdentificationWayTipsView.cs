//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：EquipIdentificationWayTipsView
//文件描述：装备图鉴获得途径
//创建者：张永明
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Copy;
using Com.Game.Module.Manager;
using Com.Game.Module.Pet;
using com.game.module.test;
using Com.Game.Module.Tips;
using com.game.Public.Message;
using com.game.utils;


namespace com.game.module.EquipIdentification
{
	public class EquipIdentificationWayTipsView : BaseView<EquipIdentificationWayTipsView>
	{
		public override string url { get { return "UI/Tips/EquipIdentificationWayTips/EquipIdentificationWayTipsView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.HighLayer; } }

		public override ViewType viewType
		{
			get { return ViewType.CityView; }
		}

		private Button btn_close;
		private Button btn_back;

		private UILabel nameLabel;
		private UILabel wayLabel;
		private UILabel chapaterOneLabel;
		private UILabel chapaterOneCopyNameLabel;
		private UILabel chapaterTwoLabel;
		private UILabel chapaterTwoCopyNameLabel;
		private UILabel chapaterThreeLabel;
		private UILabel chapaterThreeCopyNameLabel;

		private List<GameObject> copyTitleList;
		private Transform Icon;

		private SysEquipVo currentEquipVo;

		private const int POSITION_X = - 92;
		private const int POSITION_Y = - 65;


		protected override void Init ()
		{
			initPosition();
			initView ();
			initClick ();
			initTextLanguage ();
		}

		private void initPosition()
		{
			var position = this.transform.position;
			position.x = POSITION_X;
			position.y = POSITION_Y;
			this.transform.localPosition = position;
		}

		private void initView()
		{
			btn_close = FindInChild<Button> ("btn_close");
			btn_back  = FindInChild<Button> ("btn_back");
			nameLabel = FindInChild<UILabel> ("nameLabel");
			wayLabel  = FindInChild<UILabel> ("wayLabel");

			Icon      = FindInChild<Transform> ("content/Icon");

			copyTitleList = new List<GameObject>();
			for (int i = 0; i < EquipIdentificationConst.COPYSOURCE_3; i++)
			{
				copyTitleList.Add(FindChild("content/copyTitle_" + i));
				NGUITools.FindInChild<Button>(copyTitleList[i], "").onClick = copyItemOnClick;
			}
		}

		private void initClick()
		{
			btn_close.onClick = closeOnClick;
			btn_back.onClick  = closeOnClick;
		}

		private void initTextLanguage()
		{
			wayLabel.text  = LanguageManager.GetWord ("EquipIdentificationWayTipsView.way");
		}

		private void closeOnClick(GameObject go)
		{
			Singleton<EquipTips>.Instance.setEquipTipsPlayReverse();
			CloseView ();
		}

		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView();
			nameLabel.text = currentEquipVo.name;
			setIcon();
			setCopyTitleData();
		}

		public void setCurrentEquipVo(SysEquipVo equipVo)
		{
			currentEquipVo = equipVo;
			OpenView();
		}

		private void setIcon()
		{
			Singleton<ItemManager>.Instance.InitItem(Icon.gameObject, 
			                                         (uint)currentEquipVo.id, ItemType.BaseGoods);
			Icon.FindChild("background").GetComponent<UISprite>().atlas = Singleton<AtlasManager>.Instance.GetAtlas("common");
			Icon.FindChild("background").GetComponent<UISprite>().spriteName = "epz_" + currentEquipVo.color;
		}

		private void setCopyTitleData()
		{
			if(currentEquipVo.source.Length <= 2)
			{
				return;
			}
			int[] copySource = StringUtils.GetStringToInt(currentEquipVo.source);
			for (int i = 0; i < copyTitleList.Count; i++)
			{
				ItemContainer copyItemContainer = copyTitleList[i].AddMissingComponent<ItemContainer>();
				if(i < copySource.Length)
				{
					copyItemContainer.Id = (uint)copySource[i];
				}
			}
			PetLogic.SetFBInfo(copyTitleList,copySource);
		}

		private void copyItemOnClick(GameObject go)
		{
			ItemContainer currentClickCopyItem = go.GetComponent<ItemContainer>();
			if (Singleton<CopyControl>.Instance.IsCopyOpened(currentClickCopyItem.Id))
			{
				Singleton<CopyControl>.Instance.OpenCopyById(currentClickCopyItem.Id);
			}else
			{
				MessageManager.Show(LanguageManager.GetWord("EquipIdentificationWayTipsView.copynoopen"));
			}
		}
	}
}