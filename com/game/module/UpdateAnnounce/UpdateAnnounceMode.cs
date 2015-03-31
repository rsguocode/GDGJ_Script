//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 Haoyue studio
//All rights reserved
//文件名称：UpdateAnnounceMode
//文件描述：
//创建者：黄江军
//创建日期：
//版本号：0.1
//////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;

namespace Com.Game.Module.UpdateAnnounce
{
	public class UpdateAnnounceMode : BaseMode<UpdateAnnounceMode>  
	{	
		private string lastAnnounce;
		private string curAnnounce;
		private string date;
		private string content;

		private string pageName = "xml/updated_board.xml";

		public bool CanShowAnnounce
		{
			get {return !lastAnnounce.Equals(curAnnounce);}
		}

		public string Date
		{
			get {return date;}
		}

		public string Content
		{
			get {return content;}
		}

		private string serverHost
		{
			get
			{
				if (Application.platform == RuntimePlatform.WindowsEditor ||
				    Application.platform == RuntimePlatform.WindowsPlayer)
				{
					return "http://172.16.10.140/"; //内网
				}
				else
				{
					return "http://gateway.mxqy.4399sy.com/"; //外网
				}
			}
		}

		public string URL
		{
			get
			{
				int randNo = Random.Range(1, 99999999);
				return serverHost + pageName + "?" + randNo.ToString();
			}
		}

		public UpdateAnnounceMode()
		{
			lastAnnounce = PlayerPrefs.GetString("Announce");
		}

		public void SaveAnnounce(string announce)
		{
			curAnnounce = announce;
			ParseAnnounce();
			PlayerPrefs.SetString("Announce", announce);
		}

		private void ParseAnnounce()
		{
			XMLNode xn = XMLParser.Parse(curAnnounce);
			date = xn.GetValue("note>0>date>0>_text");
			content = xn.GetValue("note>0>content>0>_text");
		}
		
	}
}
