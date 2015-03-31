//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：EffectCameraMgr
//文件描述：
//创建者：黄江军
//创建日期：2014-01-02
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace Com.Game.Public.EffectCameraManage
{
	public class EffectCameraMgr 
	{	
		private Camera effectCamera;
		private Camera uiCamera;

		public Vector3 CameraPosition
		{
			get
			{
				return effectCamera.transform.position;
			}
		}

		public bool CameraEnabled
		{
			get
			{
				return effectCamera.enabled;
			}
		}

		public void Init() 
		{	
			effectCamera = GameObject.FindGameObjectWithTag("EffectCamera").GetComponent<Camera>();
			uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();

			if (null != uiCamera)
			{
				//特效相机与UI相机位置保持一致
				Vector3 pos = uiCamera.transform.position;
				pos.z = effectCamera.transform.position.z; 
				effectCamera.transform.position = pos;

				//特效相机与UI相机大小保持一致
				effectCamera.orthographicSize = uiCamera.orthographicSize;
			}

			CloseCamera();
		}	

		public void OpenCamera()
		{
			effectCamera.enabled = true;
		}

		public void CloseCamera()
		{
			effectCamera.enabled = false;
		}	

		//实际高度换算为特效相机中高度
		public float ToEffectCameraSizeH(float orgH)
		{
			float CAMERAH = effectCamera.orthographicSize * 2;
			return orgH / Screen.height * CAMERAH;
		}

		//实际宽度换算为特效相机中宽度
		public float ToEffectCameraSizeW(float orgW)
		{
			float CAMERAW = effectCamera.orthographicSize * effectCamera.aspect * 2;
			return orgW / Screen.width * CAMERAW;
		}

		//实际高度换算为主相机中高度
		public float ToMainCameraSizeH(float orgH)
		{			
			float CAMERAH = Camera.main.orthographicSize * 2;
			return orgH / Screen.height * CAMERAH;
		}

		//实际宽度换算为主相机中宽度
		public float ToMainCameraSizeW(float orgW)
		{			
			float CAMERAW = Camera.main.orthographicSize * Camera.main.aspect * 2;
			return orgW / Screen.width * CAMERAW;
		}
	}
}
