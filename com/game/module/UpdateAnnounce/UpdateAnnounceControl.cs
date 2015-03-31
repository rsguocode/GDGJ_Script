//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：UpdateAnnounceControl
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game;

namespace Com.Game.Module.UpdateAnnounce
{
	public class UpdateAnnounceControl : BaseControl<UpdateAnnounceControl> 
	{	
		public void GetAnnounce()
		{
			AppNet.main.GetAnnounce(UpdateAnnounceMode.Instance.URL);
		}

		public void ShowAnnounce()
		{
			if (UpdateAnnounceMode.Instance.CanShowAnnounce)
			{
				UpdateAnnounceView.Instance.ShowWindow(UpdateAnnounceMode.Instance.Date, UpdateAnnounceMode.Instance.Content);
			}
		}
		
	}
}
