﻿﻿﻿
using System.Collections.Generic;
using com.game;
using com.game.cmd;
using com.game.module.test;
/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/31 05:42:26 
 * function: 契约控制中心
 * *******************************************************/
using com.game.Public.Message;
using com.net.interfaces;
using Proto;

namespace Com.Game.Module.Role.Deed
{
    public class DeedControl : BaseControl<DeedControl>
    {

        private DeedMode _deedMode;

        protected override void NetListener()
        {
            _deedMode = Singleton<DeedMode>.Instance;
            AppNet.main.addCMD(CMD.CMD_18_1, Fun_18_1);				//契约信息
            AppNet.main.addCMD(CMD.CMD_18_2, Fun_18_2);				//契约签署结果
        }

        private void Fun_18_1(INetData data)
        {
            var deedInfo = new DeedInfoMsg_18_1();
            deedInfo.read(data.GetMemoryStream());
            if (deedInfo.code != 0)
            {
                ErrorCodeManager.ShowError(deedInfo.code);
                return;
            }
            _deedMode.UpdateDeedInfo(deedInfo.id,deedInfo.count);
        }

        private void Fun_18_2(INetData data)
        {
            var deedDo = new DeedDoMsg_18_2();
            deedDo.read(data.GetMemoryStream());
            if (deedDo.code != 0)
            {
                ErrorCodeManager.ShowError(deedDo.code);
                return;
            }
            _deedMode.UpdateDeedInfo(new List<uint>() { deedDo.id }, new List<uint>() { deedDo.count });
        }
    }
}