using com.game.data;
using com.game.manager;
using com.game.vo;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using Proto;
using com.u3d.bases.debug;
using System.IO;
using com.game;

namespace Com.Game.Module.VIP
{
    public class VIPMode : BaseMode<VIPMode>
    {
        public readonly int UPDATE_VIP_INFO = 1; //玩家的VIP信息
        public readonly int UPDATE_VIP_AWARD = 2;   //更新VIP奖励领取
        
        private VipInfoMsg_32_1 _vipInfoMsg;
        public VipInfoMsg_32_1 vipInfoMsg { get {return _vipInfoMsg;}}
        /*************************协议请求*******************************/

        //请求玩家的VIP情况
        public void ApplyVIPInfo()
        {
            MemoryStream msdata = new MemoryStream();
            Module_32.write_32_1(msdata);
            AppNet.gameNet.send(msdata, 32, 1);
        }

        //向服务器发送领取奖励
        public void ApplyGetAward()
        {
            MemoryStream msdata = new MemoryStream();
            Module_32.write_32_2(msdata);
            AppNet.gameNet.send(msdata, 32, 2);
        }

        /**************************数据更新************************/
        public void UpdateVIPInfo(VipInfoMsg_32_1 msg)
        {
            _vipInfoMsg = msg;
            int lastVip = MeVo.instance.vip;

            if (lastVip != msg.vip)
            {
                MeVo.instance.vip = msg.vip;
            }
            DataUpdate(UPDATE_VIP_INFO);
        }
        
        //更新VIP奖励
        public void UpdateVIPAward()
        {
            DataUpdate(UPDATE_VIP_AWARD);
        }
    }
}