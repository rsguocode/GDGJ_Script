using System.Collections.Generic;
using com.game.manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.net.interfaces;
using com.u3d.bases.debug;
using Proto;
using UnityEngine;
using System.Collections;
using com.game;
using com.game.cmd;
using Com.Game.Module.Tips;

namespace Com.Game.Module.Role
{
    public class GrowControl : BaseControl<GrowControl>
    {
        //培养类型 1普通培养 2加强培养 3白金培养 4钻石培养 3，4需要vip
        public readonly uint GEN_GROW = 1;
        public readonly uint STREN_GROW = 2;
        public readonly uint PLATINUM_GROW = 3; 
        public readonly uint DIAM_GROW = 4;

       

        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_17_1, Fun_17_1); //培养面板的信息
            AppNet.main.addCMD(CMD.CMD_17_2, Fun_17_2); //培养
        }

        public void ApplyGrow(int type )
        {
            int state = GrowMode.Instance.GetGrowState(type);
            bool isFree = false;
            if (state == 0 ) //没有开启
            {
                Log.info(this,"没有开启");
                return;
            }
            else if (state == 5)
            {
                Log.info(this, "免费次数和付费次数都用完了");
                return;
            }
            else if (state == 2)
            {
                Log.info(this, "CD时间没有到，请等待");
                return;
            }
            else if (state == 3)   //免费
            {
                isFree = true;
            }
            else if (state == 1 || state == 4)   //付费
            {
                isFree = false;
            }
            Log.info(this,"培养类型： " + type + " 是否付费： " + isFree);
            GrowMode.Instance.AppyGrow(type, isFree);
        }
        //兑换
        private void Fun_17_1(INetData data)
        {
            Log.debug(this, "服务器返回17_1培养面板的信息");
            GrowattrInfoMsg_17_1 message = new GrowattrInfoMsg_17_1();
            message.read(data.GetMemoryStream());
            Singleton<GrowMode>.Instance.UpdateGrowPannelInfo(message);
            Log.info(this, "Fun_17_1");
        }

        //培养
        private void Fun_17_2(INetData data)
        {
            Log.debug(this, "服务器返回17_2的培育确认");
            GrowattrDoMsg_17_2 message = new GrowattrDoMsg_17_2();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                //显示培育成功的提示
                MessageManager.Show(LanguageManager.GetWord("Grow.Success"));
                MessageManager.CurItem._warnIconSprite.gameObject.SetActive(false);
                MessageManager.CurItem._tipIconSprite.gameObject.SetActive(true);
                List<sbyte> addAttr = message.addAttr;
                Singleton<GrowMode>.Instance.UpdateGrowShuxing(addAttr);
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
            Log.info(this, "Fun_17_2");
        }
    }
}