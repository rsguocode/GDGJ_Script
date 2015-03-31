using UnityEngine;
using System.Collections;
using com.game.module.test;
using Proto;
using System.IO;
using com.game;
using com.u3d.bases.debug;
using com.game.Public.Message;
using com.game.manager;

namespace com.game.module.LoginAward
{
    public class LoginAwardMode : BaseMode<LoginAwardMode>
    {
        public readonly int UPDATE_AWARD_INFO = 1;             //7天登陆奖励
        public readonly int UPDATE_GET_AWARD = 2;              //领取奖励
		public readonly int UPDATE_CODE_SUCCESS = 3;           //成功领取激活码
		public readonly int UPDATE_TIPS = 4;                   //更新tips
		public readonly int UPDATE_SIGN = 5;                   //签到信息
		public readonly int UPDATE_GETSIGNREAWARD = 6;         //领取签到奖励

        private GiftLoginGiftStatusMsg_29_0 _dayInfo;
        public GiftLoginGiftStatusMsg_29_0 dayInfo { get { return _dayInfo; } }
		private string giftActivationCode;
		public string GiftActivationCode
		{
			get { return giftActivationCode ; }
			set { giftActivationCode = value ; }
		}
		private GiftSignInfoMsg_29_3 _signInfo;
		public GiftSignInfoMsg_29_3 signInfo { get { return _signInfo; } }

		private bool canShowTips;
       ///**************************协议请求**********************************
       
        //请求奖励的领取情况
        public void ApplyDayInfo()
        {
            MemoryStream msdata = new MemoryStream();
            Module_29.write_29_0(msdata);
            AppNet.gameNet.send(msdata, 29, 0);
        }

        //领取奖励的请求
        public void ApplyAward()
        {
            MemoryStream msdata = new MemoryStream();
            Module_29.write_29_1(msdata);
            AppNet.gameNet.send(msdata, 29, 1);
        }

		//领取激活码
		public void ApplyActivationCode(ushort type , string cardCode)
		{
			MemoryStream msdata = new MemoryStream();
			Module_29.write_29_2(msdata , type , cardCode);
			AppNet.gameNet.send(msdata, 29, 2);
		}

		//签到信息
		public void ApplySignInfo()
		{
			MemoryStream msdata = new MemoryStream();
			Module_29.write_29_3(msdata);
			AppNet.gameNet.send(msdata , 29 , 3);
		}

		//领取签到奖励
		public void ApplyGetSignAward()
		{
			MemoryStream msdata = new MemoryStream();
			Module_29.write_29_4(msdata);
			AppNet.gameNet.send(msdata , 29 , 4);
		}

        //*************************数据更新**********************************   
        //更新天数信息
        public void UpdateDayInfo(GiftLoginGiftStatusMsg_29_0 msg)
        {
            _dayInfo = msg;
            DataUpdate(UPDATE_AWARD_INFO);
        }

        //更新领取奖励信息
        public void UpdateAward(GiftGetLoginGiftMsg_29_1 msg)
        {
			if (msg.code != 0)
			{
				ErrorCodeManager.ShowError(msg.code);	
				return;
			}
			MessageManager.Show(LanguageManager.GetWord("LoginAward.AwardSuccess"));
			DataUpdate(UPDATE_GET_AWARD);
         }

		//更新领取激活码
		public void UpdateActivationCode(GiftGetMsg_29_2 msg)
		{
			if (msg.code != 0)
			{
				ErrorCodeManager.ShowError(msg.code);	
				return;
			}
			MessageManager.Show(LanguageManager.GetWord("LoginAwardMode.codeSuccess"));
			DataUpdate(UPDATE_CODE_SUCCESS);
		}

		//签到信息
		public void UpdateSignInfo(GiftSignInfoMsg_29_3 msg)
		{
			_signInfo = msg;
			DataUpdate(UPDATE_SIGN);
		}

		//是否显示小红点
		public override bool ShowTips
		{
			get {return canShowTips;}
		}

		public void StopTips()
		{
			canShowTips = false;
			DataUpdate(UPDATE_TIPS);
		}
    }
}
