//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SystemSettingMode
//文件描述：
//创建者：黄江军
//创建日期：2013-12-17
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.IO;
using System;

using Proto;
using com.game;
using com.game.sound;
using com.game.consts;
using com.game.module.test;

namespace Com.Game.Module.SystemSetting
{
	public class SystemSettingMode : BaseMode<SystemSettingMode> 
	{	
		private bool criteShake = true;
		private bool hidePlayer = false;
		private bool mute = false;
		private uint sceneVolumn = GameConst.DefaultSceneVolumn;
		private uint effectVolumn = GameConst.DefaultEffectVolumn;
	    private bool _showButton;
	    public const int ShowButtonUpdate = 1;

		//暴击震动
		public bool CritShake 
		{
			get
			{
				return criteShake;
			}

			set
			{
				criteShake = value;
			}
		} 

		//隐藏玩家
		public bool HidePlayer 
		{
			get
			{
				return hidePlayer;
			}
			
			set
			{
				hidePlayer = value;

				if (value)
				{
					AppMap.Instance.HideOtherPlayer();
				}
				else
				{
					AppMap.Instance.ShowOtherPlayer();
				}
			}
		}   

		//静音
		public bool Mute 
		{
			get
			{
				return mute;
			}
			
			set
			{
				mute = value;
				SoundMgr.Instance.Mute = value;
			}
		}  

		//场景音量大小
		public uint SceneVolumn 
		{
			get
			{
				return sceneVolumn;
			}
			
			set
			{
				sceneVolumn = value;
				SoundMgr.Instance.SceneVolumn = (float)value/100;
			}
		}   

		//音效音量大小
        public uint EffectVolumn 
		{
			get
			{
				return effectVolumn;
			}
			
			set
			{
				effectVolumn = value;
				SoundMgr.Instance.EffectVolumn = (float)value/100;
			}
		}

	    public bool ShowButton
	    {
	        get { return _showButton; }
	        set
	        {
	            _showButton = value;
                DataUpdate(ShowButtonUpdate);
	        }
	    }

	    public SystemSettingMode() 
		{	
		}	

		//获得系统所有设置
		public void GetAllSystemSetting()
		{
			MemoryStream msdata = new MemoryStream();
			Module_11.write_11_1(msdata);
			AppNet.gameNet.send (msdata, 11, 1); 
		}

		//保存更改的系统设置到服务器
        public void SaveChangedSystemSettingToServer(bool lastCrit, bool lastHide, bool lastMute, uint lastSceneVol, uint lastEffectVol)
		{
			if (lastCrit != criteShake)
			{
				SetSystemValue(GameConst.CritShakeKey, Convert.ToUInt16(criteShake));
			}

			if (lastHide != hidePlayer)
			{
				//隐藏玩家暂时不保存到服务器
				//setSystemValue(GameConst.HidePlayerKey, Convert.ToUInt16(hidePlayer));
			}

			if (lastMute != mute)
			{
				SetSystemValue(GameConst.MuteKey, Convert.ToUInt16(mute));
			}

			if (lastSceneVol != sceneVolumn)
			{
				SetSystemValue(GameConst.SceneVolumnKey, sceneVolumn);
			}

			if (lastEffectVol != effectVolumn)
			{
				SetSystemValue(GameConst.EffectVolumnKey, effectVolumn);
			}
		}

		//设置系统变量
        private void SetSystemValue(uint key, uint value)
		{
			MemoryStream msdata = new MemoryStream();
			Module_11.write_11_2(msdata, key, value);
			AppNet.gameNet.send (msdata, 11, 2); 
		}		
	}
}
