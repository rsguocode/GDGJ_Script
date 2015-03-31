using com.game.cmd;
using com.game.module.test;
using com.game.Public.Message;
using com.net.interfaces;
using com.u3d.bases.debug;
using Proto;
using UnityEngine;
using System.Collections;
using com.game;

namespace Com.Game.Module.LuckWand
{
    public class LuckWandControl : BaseControl<LuckWandControl>
    {

        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_25_1, Fun_25_1); //开启通知
            AppNet.main.addCMD(CMD.CMD_25_2, Fun_25_2); //魔杖活动信息
            AppNet.main.addCMD(CMD.CMD_25_3, Fun_25_3); //魔杖开奖活动
            AppNet.main.addCMD(CMD.CMD_25_4, Fun_25_4); //打开魔杖
            AppNet.main.addCMD(CMD.CMD_25_0, Fun_25_0); //开启魔杖活动信息
        }

        public void StrikeWand(byte index)
        {
            Singleton<LuckWandMode>.Instance.write_25_4(index);
        }

        public void RequestWandInfo()
        {
            Singleton<LuckWandMode>.Instance.write_25_1();
        }

        public void RequestWandCommon()
        {
            Singleton<LuckWandMode>.Instance.write_25_2();
        }

        public void RequestWandGrand()
        {
            Singleton<LuckWandMode>.Instance.write_25_3();
        }

        private void Fun_25_1(INetData data)
        {
            WandInfoMsg_25_1 message = new WandInfoMsg_25_1();
            message.read(data.GetMemoryStream());
            Singleton<LuckWandMode>.Instance.UpdateWandInfo(message.free,message.diam,message.totalGold,
                message.canBuyTimes,message.canUseTimes,message.spriteList);
            Log.info(this,"Fun_25_1 ");
        }

        private void Fun_25_2(INetData data)
        {
            WandCommonPrizeMsg_25_2 message = new WandCommonPrizeMsg_25_2();
            message.read(data.GetMemoryStream());
            Singleton<LuckWandMode>.Instance.UpdateWandCommon(message.commonList);
            Log.info(this, "Fun_25_2 ");
        }
        private void Fun_25_3(INetData data)
        {
            WandGrandPrizeMsg_25_3 message = new WandGrandPrizeMsg_25_3();
            message.read(data.GetMemoryStream());
            Singleton<LuckWandMode>.Instance.UpdateWandGand(message.grandList);
            
            Log.info(this, "Fun_25_3 ");
        }
        private void Fun_25_4(INetData data)
        {
            WandDrawMsg_25_4 message = new WandDrawMsg_25_4();
            message.read(data.GetMemoryStream());
            if (message.code != 0)
            {
                ErrorCodeManager.ShowError(message.code);
                return;
            }
            Singleton<LuckWandMode>.Instance.UpdateWandDraw();
            Log.info(this, "Fun_25_4 ");
        }
        private void Fun_25_0(INetData data)
        {
            WandOpenMsg_25_0 message = new WandOpenMsg_25_0();
            message.read(data.GetMemoryStream());
            Singleton<LuckWandMode>.Instance.UpdateWandOpen(message.free,message.diam,message.spriteList);
            Log.info(this, "Fun_25_0 ");
        }
    }
}
