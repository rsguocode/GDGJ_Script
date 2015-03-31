//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：NPCBustMgr
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.game.manager;
using com.game.utils;
using com.game.module;

namespace Com.Game.Module.NPCBust
{
	public class NPCBustMgr 
	{	
		public delegate void BustCreatedCallback(GameObject bustGo);
		public const float ZoomFactor = 100f;
		public const string SpecialNPCId = "10001";

		private GameObject bustRoot;
		private string blackNPCId = "90000";

		public static NPCBustMgr Instance = (Instance == null ? new NPCBustMgr() : Instance);	

		private IDictionary<string, GameObject> bustDictionary = new Dictionary<string, GameObject>(); 
		private BustCreatedCallback bustCallBack;
		private string bustUrl;

		//确保不能外部实例化
		private NPCBustMgr()
		{
			bustRoot = NGUITools.AddChild(ViewTree.go);
			bustRoot.name = "BustRoot";
			NGUITools.SetLayer(bustRoot, LayerMask.NameToLayer("UI"));
			GameObject.DontDestroyOnLoad(bustRoot);
		}

		//黑底半身像
		private string blackBustUrl
		{
			get {return UrlUtils.npcBustUrl(blackNPCId);}
		}

		//获得半身像
		public void GetBust(string url, BustCreatedCallback callBack)
		{
			bustUrl = url;
			bustCallBack = callBack;

			if (bustDictionary.ContainsKey(bustUrl))
			{
				bustCallBack(bustDictionary[bustUrl]);
			}
			else
			{
				AssetManager.Instance.LoadAsset<GameObject>(url, BustLoaded);
			}
		}

		//半身像加载回调
		private void BustLoaded(GameObject obj)
		{
			if (null == obj)
			{
				AssetManager.Instance.LoadAsset<GameObject>(blackBustUrl, BustLoaded);
			}
			else
			{			
				GameObject go = GameObject.Instantiate(obj) as GameObject;
				GameObject bustGo = new GameObject("bust");
				go.transform.parent = bustGo.transform;	
				bustGo.SetActive(false);
				NGUITools.SetLayer(bustGo, LayerMask.NameToLayer("UI"));
				
				go.transform.localPosition = Vector3.zero;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localScale = Vector3.one;
				
				bustGo.transform.parent = bustRoot.transform;	
				bustGo.transform.position = Vector3.zero;
				bustGo.transform.localRotation = Quaternion.identity;
				bustGo.transform.localScale = new Vector3(ZoomFactor, ZoomFactor, ZoomFactor);
				
				bustDictionary[bustUrl] = bustGo;
				bustCallBack(bustGo);
			}
		}
		
	}
}
