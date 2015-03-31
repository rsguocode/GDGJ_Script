//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：SystemSettingControl
//文件描述：
//创建者：黄江军
//创建日期：2013-12-17
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using Com.Game.Module.Role;
using UnityEngine;
using System.Collections;
using System;

using com.game.module.test;
using com.game.cmd;
using Proto;
using com.net.interfaces;
using com.game;
using com.u3d.bases.debug;
using com.game.consts;
using com.game.Public.Message;
using com.game.manager;

namespace Com.Game.Module.SystemSetting
{
	public class SystemSettingControl : BaseControl<SystemSettingControl> 
	{	
		public SystemSettingControl() 
		{	
		}	

		//注册Socket数据返回监听
		override protected void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_11_1, Fun_11_1);      //获取系统设置
			AppNet.main.addCMD(CMD.CMD_11_2, Fun_11_2);      //系统设置返回
			AppNet.main.addCMD(CMD.CMD_11_3, Fun_11_3);      //协议禁止返回
		}

		//获取系统设置
		private void Fun_11_1(INetData data)
		{
			SettingGetAllMsg_11_1 getAllMsg = new SettingGetAllMsg_11_1();
			getAllMsg.read(data.GetMemoryStream());		

			Log.info(this, "key count: " + getAllMsg.key.Count + " value count: " + getAllMsg.val.Count);

            uint[] skillpos = new uint[5];
			for (int i=0; i<getAllMsg.key.Count; i++)
			{
				switch (getAllMsg.key[i])
				{
				case GameConst.CritShakeKey:
					Singleton<SystemSettingMode>.Instance.CritShake = Convert.ToBoolean(getAllMsg.val[i]);
					break;

				case GameConst.HidePlayerKey:
					Singleton<SystemSettingMode>.Instance.HidePlayer = Convert.ToBoolean(getAllMsg.val[i]);
					break;

				case GameConst.MuteKey:
					Singleton<SystemSettingMode>.Instance.Mute = Convert.ToBoolean(getAllMsg.val[i]);
					break;

				case GameConst.SceneVolumnKey:
					Singleton<SystemSettingMode>.Instance.SceneVolumn = getAllMsg.val[i];
					break;

				case GameConst.SkillPos1:
                    skillpos[0]=getAllMsg.val[i];
					break;

                case GameConst.SkillPos2:
                    skillpos[1] = getAllMsg.val[i];
                    break;
                case GameConst.SkillPos3:
                    skillpos[2] = getAllMsg.val[i];
                    break;
                case GameConst.SkillPos4:
                    skillpos[3] = getAllMsg.val[i];
                    break;
                case GameConst.SkillPos5:
                    skillpos[4] = getAllMsg.val[i];
                    break;  
				default:
					break;
				}
			}
            Singleton<SkillMode>.Instance.SetSkillPos(skillpos);
		}

		//系统设置返回
		private void Fun_11_2(INetData data)
		{
			SettingSetOneSettingMsg_11_2 setMsg = new SettingSetOneSettingMsg_11_2();
			setMsg.read(data.GetMemoryStream());

			Log.info(this, "key: " + setMsg.key + " code: " + setMsg.code);
		}

		//协议禁止返回
		private void Fun_11_3(INetData data)
		{
			SettingProtoForbidMsg_11_3 forbidMsg = new SettingProtoForbidMsg_11_3();
			forbidMsg.read(data.GetMemoryStream());

			string[] param = {forbidMsg.section.ToString(), forbidMsg.method.ToString()};
			MessageManager.Show(LanguageManager.GetWord("SystemSettingControl.ForbidMessage", param));
			
			Log.info(this, "section: " + forbidMsg.section + " method: " + forbidMsg.method);
		}		
	}
}
