using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using com.game.cmd;
using com.net.interfaces;
using Proto;
using com.u3d.bases.debug;
using com.game.Public.Message;
using com.game.module.main;
using com.game.vo;

namespace Com.Game.Module.VIP
{
    public class VIPControl : BaseControl<VIPControl>
    {
        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_32_1, Fun_32_1);		//获取VIP信息
            AppNet.main.addCMD(CMD.CMD_32_2, Fun_32_2);    //领取VIP奖励
        }

        /****************************服务器返回信息********************/
        private void Fun_32_1(INetData data)
        {
            VipInfoMsg_32_1 vipInfoMsg_32_1 = new VipInfoMsg_32_1();
            vipInfoMsg_32_1.read(data.GetMemoryStream());
            Singleton<VIPMode>.Instance.UpdateVIPInfo(vipInfoMsg_32_1);
            if (Singleton<MainTopLeftView>.Instance.labVip)
            {
                MeVo.instance.vip = vipInfoMsg_32_1.vip;
                Singleton<MainTopLeftView>.Instance.labVip.text = vipInfoMsg_32_1.vip.ToString();
            }
        }

        //领取奖励的返回
        private void Fun_32_2(INetData data)
        {
            VipGetRewardMsg_32_2 vipGetRewardMsg_32_2 = new VipGetRewardMsg_32_2();
            vipGetRewardMsg_32_2.read(data.GetMemoryStream());
			if(vipGetRewardMsg_32_2.code != 0)
			{
				ErrorCodeManager.ShowError(vipGetRewardMsg_32_2.code);
				return;
			}
			Singleton<VIPMode>.Instance.UpdateVIPAward();//表示领取成功
        }
    }
}