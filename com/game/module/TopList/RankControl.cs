

using com.game.cmd;
using com.game.module.test;
using System.IO;
using com.game;
using com.game.Public.Message;
using com.u3d.bases.debug;
using Proto;
using com.net.interfaces;
namespace Com.Game.Module.TopList
{
	class RankControl : BaseControl<RankControl>
	{
        //注册排行榜相关的协议
        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_20_1, ReceiveRankInfo);
        }


        /// <summary>
        /// 发送请求获取排行榜信息
        /// </summary>
        public void SendRequestForRankInfo(ushort type)
        {
            MemoryStream mem = new MemoryStream();
            Module_20.write_20_1(mem,(ushort)(type+20000));
            AppNet.gameNet.send(mem, 20, 1);
        }

        //20001 等级, 20002 战力, 20003 金币
        public void ReceiveRankInfo(INetData data)
	    {

            RankInfoMsg_20_1 msg = new RankInfoMsg_20_1();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
                Singleton<RankMode>.Instance.SetRankInfo(msg.type - 20000, msg);
            }
	    }
            

	}
}
