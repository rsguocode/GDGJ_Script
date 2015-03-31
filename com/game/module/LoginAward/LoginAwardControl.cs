using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using com.game.cmd;
using com.net.interfaces;
using Proto;
using com.u3d.bases.debug;
using com.game.module.main;


namespace com.game.module.LoginAward
{
    public class LoginAwardControl : BaseControl<LoginAwardControl>
    {
        protected override void NetListener ()
        {
			AppNet.main.addCMD(CMD.CMD_29_0, Fun_29_0);		//七天登陆礼包
			AppNet.main.addCMD(CMD.CMD_29_1, Fun_29_1);		//领取礼包
			AppNet.main.addCMD(CMD.CMD_29_2, Fun_29_2);     //领取激活码
			AppNet.main.addCMD(CMD.CMD_29_3, Fun_29_3);     //签到信息
			AppNet.main.addCMD(CMD.CMD_29_4, Fun_29_4);     //领取签到奖励
        }

        //***********************服务器信息返回********************************

        private void Fun_29_0(INetData data)
        {
            GiftLoginGiftStatusMsg_29_0 giftLoginGiftStatusMsg_29_0 = new GiftLoginGiftStatusMsg_29_0();
            giftLoginGiftStatusMsg_29_0.read(data.GetMemoryStream());
            Singleton<LoginAwardMode>.Instance.UpdateDayInfo(giftLoginGiftStatusMsg_29_0);
        }

        private void Fun_29_1(INetData data)
        {
            GiftGetLoginGiftMsg_29_1 giftGetLoginGiftMsg_29_1 = new GiftGetLoginGiftMsg_29_1();
            giftGetLoginGiftMsg_29_1.read(data.GetMemoryStream());
            Singleton<LoginAwardMode>.Instance.UpdateAward(giftGetLoginGiftMsg_29_1);
        }

		private void Fun_29_2(INetData data)
		{
			GiftGetMsg_29_2 giftGetMsg_29_2 = new GiftGetMsg_29_2();
			giftGetMsg_29_2.read(data.GetMemoryStream());
			Singleton<LoginAwardMode>.Instance.UpdateActivationCode(giftGetMsg_29_2);
		}

		private void Fun_29_3(INetData data)
		{
			GiftSignInfoMsg_29_3 giftSignMsg_29_3 = new GiftSignInfoMsg_29_3();
			giftSignMsg_29_3.read(data.GetMemoryStream());
			Singleton<LoginAwardMode>.Instance.UpdateSignInfo(giftSignMsg_29_3);
		}

		private void Fun_29_4(INetData data)
		{
			GiftGetSignRewardMsg_29_4 giftSignRewardMsg_29_4 = new GiftGetSignRewardMsg_29_4();
			giftSignRewardMsg_29_4.read(data.GetMemoryStream());
		}
	}
}
