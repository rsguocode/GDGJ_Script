using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.cmd;
using com.game;
using com.net.interfaces;
using com.u3d.bases.debug;
using Proto;
using com.game.module.map;
using com.game.Public.Message;

namespace Com.Game.Module.GoldHit
{
    public class GoldHitControl : BaseControl<GoldHitControl>
    {
        protected override void NetListener ()
        {
            AppNet.main.addCMD(CMD.CMD_8_11, Fun_8_11);		//获取剩余次数、时间信息
            AppNet.main.addCMD(CMD.CMD_8_14, Fun_8_14);   //攻击伤害
            AppNet.main.addCMD(CMD.CMD_8_12, Fun_8_12);	 //清除挑战CD时间返回
        }



        /********************************服务器信息返回**************************/
        private void Fun_8_11(INetData data)
        {
            Log.debug(this, "服务器返回 8_11 剩余挑战次数、时间信息");
            DungeonGoldTreeInfoMsg_8_11 dungeonGoldTreeInfoMsg_8_11 = new DungeonGoldTreeInfoMsg_8_11();
            dungeonGoldTreeInfoMsg_8_11.read(data.GetMemoryStream());
            Singleton<GoldHitMode>.Instance.UpdateGoldHitInfo(dungeonGoldTreeInfoMsg_8_11);
        }


        private void Fun_8_14(INetData data)
        {    
            DungeonGoldTreeAddGoldMsg_8_14 msg = new DungeonGoldTreeAddGoldMsg_8_14();
            msg.read(data.GetMemoryStream());
            Log.debug(this, "怪物死亡，服务器返回金币：" + msg.gold.ToString());
            Singleton<GoldHitMode>.Instance.UpdateGold(msg);
        }

        //服务器返回清除CD
        private void Fun_8_12(INetData data)
        {
            DungeonGoldTreeClearCdMsg_8_12 dungeonGoldTreeClearCdMsg_8_12 = new DungeonGoldTreeClearCdMsg_8_12();
            dungeonGoldTreeClearCdMsg_8_12.read(data.GetMemoryStream());
            Log.debug(this, "服务器返回CD请求：" + dungeonGoldTreeClearCdMsg_8_12.code);
            if (dungeonGoldTreeClearCdMsg_8_12.code == 0)
            {

                Singleton<GoldHitMode>.Instance.UpdateCD(0);
            }
            else
            {
                ErrorCodeManager.ShowError(dungeonGoldTreeClearCdMsg_8_12.code);
                return;
            }
        }
        

        //服务器返回石像被打掉的消息
        //private void Fun_4_12(INetData data)
        //{
        //    var mapMonDeadMsg = new MapMonDeadMsg_4_12();
        //    mapMonDeadMsg.read(data.GetMemoryStream());
        //    if(mapMonDeadMsg.code == 0) //表示怪物死亡
        //    {
        //        Singleton<GoldHitMode>.Instance.UpdateStoneMonster();
        //    }
        //}

        ////获取副本结束时间
        //private  void Fun_8_8(INetData data)
        //{
        //    DungeonInfoMsg_8_8 dungeonInfoMsg = new DungeonInfoMsg_8_8();
        //    dungeonInfoMsg.read(data.GetMemoryStream());
        //}


        //玩家对怪物的伤害计算
        public  void Attack(uint hurt)
        {
            Singleton<GoldHitMode>.Instance.Attack(hurt);
        }

       public void OpenMainView()
        {
            Singleton<GoldHitLogView>.Instance.CloseView();
            Singleton<GoldHitMainView>.Instance.OpenView();
        }
    }

}
