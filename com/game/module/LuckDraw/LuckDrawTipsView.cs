//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：LuckDrawTipsView
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
using com.game.sound;
using com.game.consts;
using com.game.utils;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Tips;
using com.game.module.effect;
using com.game.Public.Confirm;
using Com.Game.Speech;
using Com.Game.Module.Pet;

namespace Com.Game.Module.LuckDraw
{
	public struct GoodsEffectParam
	{
		public Color EffectColor;
		public Vector3 EffectPos;
		public string EffectId;
	}

	public class LuckDrawTipsView : BaseView<LuckDrawTipsView> 
	{		
		public override string url { get { return "UI/LuckDraw/LuckDrawTipsView.assetbundle"; } }
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; }}
		public override bool IsFullUI { get { return true; }}	

		private const int tenthIconsNum = 10; 
		private const float iconEffectDuration = 0.18f;
		private int iconIndex;
		private IList<GoodsEffectParam> goodsEffctParamList = new List<GoodsEffectParam>();

		private GameObject onceObj;
		private GameObject tenthObj;
		private GameObject btnsObj;
		private GameObject petsObj;

		private GameObject petAnimationObj;
		private GameObject petLightObj;

		private Button btnBuyAgain;
		private Button btnOk;
		private UILabel labOnecePrice;

		private UILabel labBuyAgain;
		private UILabel labOk;

		private UISprite sprOnce;
		private UILabel labOnce;
		private UISprite[] sprTenthArr = new UISprite[tenthIconsNum];
		private UILabel[] labTenthArr = new UILabel[tenthIconsNum];
		private UISprite sprPetCloseEye;

		private Vector3 petsFromPos = Vector3.zero;
		private Vector3 petsToPos = new Vector3(0f, 175f, 0f);

		protected override void Init()
		{
			onceObj = gameObject.transform.Find("center/reward/once").gameObject;
			tenthObj = gameObject.transform.Find("center/reward/tenth").gameObject;
			btnsObj = gameObject.transform.Find("center/anniu").gameObject;
			petsObj = gameObject.transform.Find("center/pets").gameObject;

			sprPetCloseEye = FindInChild<UISprite>("center/pets/peteyeclose");

			btnBuyAgain = FindInChild<Button>("center/anniu/btn_anniu1");
			btnOk = FindInChild<Button>("center/anniu/btn_anniu2");
			labOnecePrice = FindInChild<UILabel>("center/anniu/btn_anniu1/shuzi");
			labBuyAgain = FindInChild<UILabel>("center/anniu/btn_anniu1/zi");
			labOk = FindInChild<UILabel>("center/anniu/btn_anniu2/zi");

			sprOnce = FindInChild<UISprite>("center/reward/once/item/tubiao");
			sprOnce.transform.parent.gameObject.GetComponent<UIWidgetContainer>().onClick = ShowRewardTips;
			labOnce = FindInChild<UILabel>("center/reward/once/item/num");

			for (int i=0; i<tenthIconsNum; i++)
			{
				sprTenthArr[i] = FindInChild<UISprite>("center/reward/tenth/item" + (i+1) + "/tubiao");
				sprTenthArr[i].transform.parent.gameObject.GetComponent<UIWidgetContainer>().onClick = ShowRewardTips;
				labTenthArr[i] = FindInChild<UILabel>("center/reward/tenth/item" + (i+1) + "/num");
			}

			btnBuyAgain.onClick = BuyAgainOnClick;
			btnOk.onClick = OkOnClick;

			HideItemIcons();
			InitLabel();
			PreloadResource();
		}

		private void InitLabel()
		{
			labOk.text = LanguageManager.GetWord("LuckDraw.Ok");
		}

		private void PreloadResource()
		{
			EffectMgr.Instance.PreloadUIEffect(EffectId.UI_PetBurst);
			EffectMgr.Instance.PreloadUIEffect(EffectId.UI_PetAnimation);
			EffectMgr.Instance.PreloadUIEffect(EffectId.UI_PetLight);
		}

		private void HideItemIcons()
		{
			sprOnce.transform.parent.gameObject.SetActive(false);
			for (int i=0; i<tenthIconsNum; i++)
			{
				sprTenthArr[i].transform.parent.gameObject.SetActive(false);
			}
		}

		//显示奖品说明
		private void ShowRewardTips(GameObject go)
		{
			string strName = go.name;
			string strNO = strName.Replace("item", "");
			int itemIndex = ("" == strNO) ? 0 : (int.Parse(strNO) - 1);
			uint goodsId = Singleton<LuckDrawMode>.Instance.RewardList[itemIndex].id;
			Singleton<TipsManager>.Instance.OpenTipsByGoodsId(goodsId, null, null, string.Empty, string.Empty);
		}

		private bool IsPetGoods(int itemIndex)
		{
			uint goodsId = Singleton<LuckDrawMode>.Instance.RewardList[itemIndex].id;

			if (goodsId > 100000)  //物品道具
			{
				SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);
				if ((null != vo) && (5 == vo.type) && (2 == vo.subtype))
				{
					return true;
				}
			}
			else if(goodsId > 10000) //装备
			{
				SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
				if ((null != vo) && (5 == vo.type) && (2 == vo.subtype))
				{
					return true;
				}
			}

			return false;
		}

		private void SetIconSpriteName(UISprite itemIcon, UILabel labNum, int itemIndex)
		{
			string spriteName = "100000";
			UIAtlas atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
			string borderSpriteName = "pz_1";
			UIAtlas borderAtlas = Singleton<AtlasManager>.Instance.GetAtlas("common");

			uint goodsId = Singleton<LuckDrawMode>.Instance.RewardList[itemIndex].id;
			byte num = Singleton<LuckDrawMode>.Instance.RewardList[itemIndex].num;

			if (goodsId > 100000)  //物品道具
			{
				borderAtlas = Singleton<AtlasManager>.Instance.GetAtlas("common"); 
				SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);

				if (null != vo)
				{
					spriteName = string.Empty +vo.icon;
					atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
					borderSpriteName = "pz_" + vo.color;

					if (atlas.GetSprite(spriteName) == null)
					{
						if(vo.type == GoodsConst.SMELT_GOODS)
						{
							atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
						}
						else
						{
							atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
						}

						if (atlas == null || atlas.GetSprite(spriteName) == null)
						{
							spriteName = "100000";
						}
					}
				}
			}
			else  //装备
			{
				borderAtlas = Singleton<AtlasManager>.Instance.GetAtlas("EquipIcon"); 
				SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);

				if (null != vo)
				{
					spriteName = string.Empty + vo.icon;
					borderSpriteName = "pz_" + vo.color;

					atlas = Singleton<AtlasManager>.Instance.GetAtlas("EquipIcon");
					if (atlas.GetSprite(spriteName) == null)
					{
						if(vo.type == GoodsConst.SMELT_GOODS)
						{
							atlas = Singleton<AtlasManager>.Instance.GetAtlas("GemIcon");
						}
						else
						{
							atlas = Singleton<AtlasManager>.Instance.GetAtlas("PropIcon");
						}

						if (atlas == null || atlas.GetSprite(spriteName) == null)
						{
							spriteName = "100000";
						}
					}
				}
			}

			itemIcon.atlas = atlas;
			itemIcon.spriteName = spriteName;

			UISprite sprBorder = itemIcon.transform.parent.FindChild("border").GetComponent<UISprite>();
			sprBorder.atlas = borderAtlas;
			sprBorder.spriteName = borderSpriteName;
			labNum.text = num.ToString();
		}

		private int GetHighGoodsColorLevel(int itemIndex)
		{
			int color = 0;
			uint goodsId = Singleton<LuckDrawMode>.Instance.RewardList[itemIndex].id;
			
			if (goodsId > 100000)  //物品道具
			{
				SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goodsId);	
				color = vo.color;
			}
			else if(goodsId > 10000) //装备
			{
				SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goodsId);
				color = vo.color;
			}
			
			return color;
		}

		private bool IsHighGoods(int color)
		{
			return (color >= 4);
		}

		private Color GetHighGoodsColor(int colorLevel)
		{
			switch (colorLevel)
			{
			case 4:
				return ColorConst.orange;

			case 5:
				return ColorConst.purple;

			case 6:
				return ColorConst.red;

			default:
				return Color.gray;
			}
		}

		private string GetHighGoodsEffectId(int colorLevel)
		{
			switch (colorLevel)
			{
			case 4:
				return EffectId.UI_TopPropOrange;
				
			case 5:
				return EffectId.UI_TopPropPurple;
				
			case 6:
				return EffectId.UI_TopPropRed;
				
			default:
				return string.Empty;
			}
		}
		
		private void BuyAgainOnClick(GameObject go)
		{
			SetButtonsEnabled(false);

			if (LockDrawTypeEnum.Ten != Singleton<LuckDrawMode>.Instance.LuckDrawType)
			{
				Singleton<LuckDrawMode>.Instance.LuckDrawOnce();
			}
			else
			{
				Singleton<LuckDrawMode>.Instance.LuckDrawTen();
			}
		}

		private void SetButtonsEnabled(bool enabled)
		{
			btnsObj.SetActive(enabled);
		}

		private void OkOnClick(GameObject go)
		{
			CloseView();
		}

		//每次打开界面后回调
		protected override void HandleAfterOpenView()
		{
			base.HandleAfterOpenView();	
			SetButtonsEnabled(false);
			UpdateLuckDrawInfo();
		}

		//每次关闭界面前回调
		protected override void HandleBeforeCloseView()
		{
			RemoveAllUIEffect();
			base.HandleBeforeCloseView();
		}
		
		public override void RegisterUpdateHandler()
		{
			Singleton<LuckDrawMode>.Instance.dataUpdated += UpdateLuckDrawTypeHandle;	
			Singleton<LuckDrawMode>.Instance.dataUpdated += UpdateLuckDrawBuyErrorHandle;	
		}
		
		public override void CancelUpdateHandler()
		{
			Singleton<LuckDrawMode>.Instance.dataUpdated -= UpdateLuckDrawTypeHandle;
			Singleton<LuckDrawMode>.Instance.dataUpdated -= UpdateLuckDrawBuyErrorHandle;	
		}

		//抽奖成功后回调
		private void UpdateLuckDrawTypeHandle(object sender, int type)
		{
			if (Singleton<LuckDrawMode>.Instance.UPDATE_UPDATE_LUCKDRAW_TYPE == type)
			{
				FirstStep();
			}
		}

		//抽奖失败后回调
		private void UpdateLuckDrawBuyErrorHandle(object sender, int type)
		{
			if (Singleton<LuckDrawMode>.Instance.UPDATE_UPDATE_BUY_ERROR == type)
			{
				SetButtonsEnabled(true);
			}
		}

		private void UpdateLuckDrawInfo()
		{
			FirstStep();
		}

		private void StartIconsEffect()
		{
			HideItemIcons();

			if (LockDrawTypeEnum.Ten != Singleton<LuckDrawMode>.Instance.LuckDrawType)
			{
				labBuyAgain.text = LanguageManager.GetWord("LuckDraw.BuyAgainOnce");
				labOnecePrice.text = Singleton<LuckDrawMode>.Instance.DiamondNeedsForOnce.ToString();

				onceObj.gameObject.SetActive(true);
				tenthObj.gameObject.SetActive(false);

				SetIconSpriteName(sprOnce, labOnce, 0);
				ShowIconEffect(sprOnce.transform.parent.gameObject, 0);

				//当抽到的物品大类为5，小类为2，数量大于10时，插播一个召唤宠物的表现
				if (IsPetGoods(0))
				{
					SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicGetBigReward);
					uint goodsId = Singleton<LuckDrawMode>.Instance.RewardList[0].id;
					Singleton<PetView>.Instance.OpenViewForStone(goodsId, StartButtonsDelayActive);
				}
				else
				{
					StartButtonsDelayActive();
				}
			}
			else
			{
				labBuyAgain.text = LanguageManager.GetWord("LuckDraw.BuyAgainTenth");
				labOnecePrice.text = Singleton<LuckDrawMode>.Instance.DiamondNeedsForTenth.ToString();

				onceObj.gameObject.SetActive(false);
				tenthObj.gameObject.SetActive(true);

				iconIndex = 0;
				StartTenthIconEffect();
			}
		}

		private void StartTenthIconEffect()
		{
			SetIconSpriteName(sprTenthArr[iconIndex], labTenthArr[iconIndex], iconIndex);
			ShowIconEffect(sprTenthArr[iconIndex].transform.parent.gameObject, iconIndex);

			//当抽到的物品大类为5，小类为2，数量大于10时，插播一个召唤宠物的表现
			if (IsPetGoods(iconIndex))
			{
				SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicGetBigReward);
				uint goodsId = Singleton<LuckDrawMode>.Instance.RewardList[iconIndex].id;
				Singleton<PetView>.Instance.OpenViewForStone(goodsId, ContinueTenthIconEffect);
			}
			else
			{
				ContinueTenthIconEffect();
			}
		}

		private void ContinueTenthIconEffect()
		{
			iconIndex++;

			RestorePetEffectShow();
			
			if (iconIndex >= tenthIconsNum)
			{
				StartButtonsDelayActive();
			}
			else
			{
				vp_Timer.In(iconEffectDuration, StartTenthIconEffect, 1, 0);
			}
		}

		private void RestorePetEffectShow()
		{
			//恢复萌宠献礼相关特效显示
			EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_PetAnimation).SetActive(true);
			EffectMgr.Instance.GetUIEffectGameObject(EffectId.UI_PetLight).SetActive(true);
		}

		private void StartButtonsDelayActive()
		{
			RestorePetEffectShow();
			vp_Timer.In(iconEffectDuration + 0.2f, SetButtonsActiveCallback, 1, 0);
		}

		private void SetButtonsActiveCallback()
		{
			SetButtonsEnabled(true);
		}

		private void ShowIconEffect(GameObject iconObj, int index)
		{
			iconObj.gameObject.SetActive(true);

			//高级物品需要播放特效
			int colorLevel = GetHighGoodsColorLevel(index);
			if (IsHighGoods(colorLevel))
			{
				GoodsEffectParam param = new GoodsEffectParam();
				param.EffectColor = GetHighGoodsColor(colorLevel);
				param.EffectPos = iconObj.transform.position;
				param.EffectId = GetHighGoodsEffectId(colorLevel);
				goodsEffctParamList.Add(param);
				vp_Timer.In(iconEffectDuration, CreateHightGoodsEffect, 1, 0);
			}

			Vector3 toPos = iconObj.transform.localPosition;
			Vector3 fromPos = petAnimationObj.transform.localPosition;
			fromPos.x -= 38f;

			//位置处理
			TweenPosition tweenPosition = iconObj.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}

			tweenPosition = iconObj.AddComponent<TweenPosition>();
			tweenPosition.from = fromPos;
			tweenPosition.to = toPos;
			tweenPosition.style = UITweener.Style.Once;
			tweenPosition.method = UITweener.Method.QuintEaseInOut;
			tweenPosition.duration = iconEffectDuration;

			//缩放处理
			TweenScale tweenScale = iconObj.GetComponent<TweenScale>();
			if (null != tweenScale)
			{
				GameObject.Destroy(tweenScale);
			}
			
			tweenScale = iconObj.AddComponent<TweenScale>();
			tweenScale.from = Vector3.zero;
			tweenScale.to = Vector3.one;
			tweenScale.style = UITweener.Style.Once;
			tweenScale.method = UITweener.Method.QuintEaseInOut;
			tweenScale.duration = iconEffectDuration;
		}

		private void CreateHightGoodsEffect()
		{
			GoodsEffectParam effectParam = goodsEffctParamList[0];
			goodsEffctParamList.RemoveAt(0);
			Vector3 effectPos = effectParam.EffectPos;
			string effectId = effectParam.EffectId;
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_PropBurst, effectPos);
			EffectMgr.Instance.CreateUIEffect(effectId, effectPos);
		}

		private void PropLightCreatedCallBack(GameObject go)
		{
			Color effectColor = goodsEffctParamList[0].EffectColor;
			goodsEffctParamList.RemoveAt(0);
			SetPropLightColor(go, effectColor);
		}

		public void SetPropLightColor(GameObject effectObj, Color effectColor)
		{
			SpriteRenderer[] renders = effectObj.GetComponentsInChildren<SpriteRenderer>();
			
			foreach (SpriteRenderer item in renders)
			{
				item.color = effectColor;
			}
		}

		//宠物颤抖
		private void TremblePet()
		{
			Vector3 offset1 = new Vector3(-1f, 1f, 0f);
			Vector3 offset2 = new Vector3(1f, -1f, 0f);

			TweenPosition tweenPosition = petsObj.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}
			
			tweenPosition = petsObj.AddComponent<TweenPosition>();
			tweenPosition.from = petsFromPos + offset1;
			tweenPosition.to = petsFromPos + offset2;
			tweenPosition.style = UITweener.Style.Loop;
			tweenPosition.method = UITweener.Method.QuintEaseInOut;
			tweenPosition.duration = 0.3f;
		}

		//删除所有UI特效
		private void RemoveAllUIEffect()
		{
			EffectMgr.Instance.RemoveAllUIEffect();
			petAnimationObj = null;
			goodsEffctParamList.Clear();
		}

		//1.点击抽奖后，闭眼的宠物身体颤抖施法动画
		private void FirstStep()
		{
			RemoveAllUIEffect();
			SetButtonsEnabled(false);
			onceObj.gameObject.SetActive(false);
			tenthObj.gameObject.SetActive(false);

			petsObj.transform.localPosition = Vector3.zero;
			petsObj.SetActive(false);

			EffectMgr.Instance.CreateUIEffect(EffectId.UI_PetBurst, Vector3.zero, OpenEyeStep);
		}

		//2.睁眼出现魔法特效
		private void OpenEyeStep()
		{
			//这里开始播放魔法特效（先保留）
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_PetAnimation, Vector3.zero, null, true, CreatePetAninationCallback);
			EffectMgr.Instance.CreateUIEffect(EffectId.UI_PetLight, Vector3.zero, null, true, CreatePetLightCallback);

			vp_Timer.In(0.1f, ScaleToTopStep, 1, 0);
		}

		private void CreatePetLightCallback(GameObject go)
		{
			petLightObj = go;
		}

		private void CreatePetAninationCallback(GameObject go)
		{
			petAnimationObj = go;
		}

		//宠物缩小
		private void ScalePet()
		{
			Vector3 to = new Vector3(0.5f, 0.5f, 0.5f);
			ScalePetEffect(petAnimationObj, to); 
			ScalePetEffect(petLightObj, to); 
		}

		private void ScalePetEffect(GameObject effectObj, Vector3 to)
		{
			TweenScale tweenScale = effectObj.GetComponent<TweenScale>();
			if (null != tweenScale)
			{
				GameObject.Destroy(tweenScale);
			}
			
			tweenScale = effectObj.AddComponent<TweenScale>();
			tweenScale.from = new Vector3(1f, 1f, 1f);
			tweenScale.to = to;
			tweenScale.style = UITweener.Style.Once;
			tweenScale.method = UITweener.Method.QuintEaseInOut;
			tweenScale.duration = 0.5f;
		}

		//宠物移动到上方
		private void PetMoveToTop()
		{
			PetEffectMoveToTop(petAnimationObj);
			PetEffectMoveToTop(petLightObj);
		}

		private void PetEffectMoveToTop(GameObject effectObj)
		{
			TweenPosition tweenPosition = effectObj.GetComponent<TweenPosition>();
			if (null != tweenPosition)
			{
				GameObject.Destroy(tweenPosition);
			}
			
			tweenPosition = effectObj.AddComponent<TweenPosition>();
			tweenPosition.from = petsFromPos;
			tweenPosition.to = petsToPos;
			tweenPosition.style = UITweener.Style.Once;
			tweenPosition.method = UITweener.Method.QuintEaseInOut;
			tweenPosition.duration = 0.5f;
		}

		//3.缩小到上方
		private void ScaleToTopStep()
		{
			ScalePet();
			PetMoveToTop();
			vp_Timer.In(0.5f, ShowPropsStep, 1, 0);
		}

		//4.开始出现道具
		private void ShowPropsStep()
		{
			StartIconsEffect();
		}
	}
}
