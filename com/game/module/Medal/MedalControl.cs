using System.IO;
using com.game;
using com.game.cmd;
using com.game.Public.Message;
using com.net.interfaces;
using Proto;
using com.game.module.test;

namespace Com.Game.Module.Medal
{
    public class MedalControl : BaseControl<MedalControl>
    {
        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_28_1, Fun_28_1);
            AppNet.main.addCMD(CMD.CMD_28_2, Fun_28_2);
        }

        public void RequestMedalInfo()
        {
            MemoryStream msdata = new MemoryStream();
            Module_28.write_28_1(msdata);
            AppNet.gameNet.send(msdata, 28, 1);
        }

        public void MedalUp()
        {
            MemoryStream msdata = new MemoryStream();
            Module_28.write_28_2(msdata);
            AppNet.gameNet.send(msdata, 28, 2);
           
        }

        private void Fun_28_1(INetData data)
        {
            MedalMaxIdMsg_28_1 message = new MedalMaxIdMsg_28_1();
            message.read(data.GetMemoryStream());
            MedalMode.Instance.UpdateMdealInfo(message.id);
        }
        private void Fun_28_2(INetData data)
        {
            MedalUpgradeMsg_28_2 message = new MedalUpgradeMsg_28_2();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                MedalMode.Instance.UpdateMedalUp(message.id);
            }
            else 
                ErrorCodeManager.ShowError(message.code);
        }
    }
}
