using System.Collections.Generic;
using System.IO;
using com.game;
using com.game.cmd;
using Com.Game.Module.Role;
using com.game.module.Store;
using com.game.module.test;
using com.game.Public.Message;
using com.net.interfaces;
using com.u3d.bases.debug;
using Proto;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Equip
{
    public class Equip1Control : BaseControl<Equip1Control>
    {

        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_14_1, Fun_14_1);  // 强化 : SmeltStrenMsg_14_1             
            AppNet.main.addCMD(CMD.CMD_14_2, Fun_14_2);	 // 升级 : SmeltUpMsg_14_2		
            AppNet.main.addCMD(CMD.CMD_14_3, Fun_14_3);	 // 镶嵌宝石 : SmeltInlayMsg_14_3		
            AppNet.main.addCMD(CMD.CMD_14_4, Fun_14_4);	 // 摘除宝石 : SmeltRemoveMsg_14_4
            AppNet.main.addCMD(CMD.CMD_14_5, Fun_14_5);  // 合成宝石
            AppNet.main.addCMD(CMD.CMD_14_11, Fun_14_11);  // 合成宝石
            AppNet.main.addCMD(CMD.CMD_14_12, Fun_14_12);  // 合成宝石
            AppNet.main.addCMD(CMD.CMD_14_13, Fun_14_13);  // 合成宝石
        }
        //装备强化
        public void EquipStren(uint uid, int repos)
        {
            int i = GoodsMode.Instance.IsStren(uid);
            if( i == 3)
                MessageManager.Show("已经强化至最高等级");
            else if(i == 2)
                MessageManager.Show("强化等级不能超过角色等级");
            else if (i == 1)
                MessageManager.Show("金币不足");
            else if(i ==0)
            {
                MemoryStream msdata = new MemoryStream();
                Module_14.write_14_1(msdata, uid, (byte) repos);
                AppNet.gameNet.send(msdata, 14, 1);
                Log.info(this, "装备强化 Send 14_1");
            }
        }
        //获得装备成功率
        public void EquipRate(uint uid, int repos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_14.write_14_2(msdata, uid, (byte)repos);
            AppNet.gameNet.send(msdata, 14, 2);
            Log.info(this, "获得装备成功率 Send 14_2");
        }
        //装备精炼
        public void EquipRefine(uint uid, int repos)
        {
            int i = GoodsMode.Instance.IsRefine(uid);
            if ( i == 0)
            {
                MemoryStream msdata = new MemoryStream();
                Module_14.write_14_3(msdata, uid, (byte) repos);
                AppNet.gameNet.send(msdata, 14, 3);
                Log.info(this, "装备精炼 Send 14_3");
            }
            else if (i == 1)
            {
                MessageManager.Show("金币不足");
            }
            else if (i == 2)
            {
                MessageManager.Show("精炼石不足");
            }
            
        }
        //装备继承
        public void EquipInherit(uint preId, uint nextId)
        {
            if (!GoodsMode.Instance.IsInherit(preId))
                MessageManager.Show("金币不足");
            else
            {

                MemoryStream msdata = new MemoryStream();
                Module_14.write_14_4(msdata, preId, nextId);
                AppNet.gameNet.send(msdata, 14, 4);
                Log.info(this, "装备继承 Send 14_4");
            }
        }
        //装备分解
        public void EquipDestroy(List<uint> idList )
        {
            MemoryStream msdata = new MemoryStream();
            Module_14.write_14_5(msdata, idList);
            AppNet.gameNet.send(msdata, 14, 5);
            Log.info(this, "装备分解 Send 14_5");
        }
        //装备镶嵌
        public void EquipInlay(uint uid, int repos,uint stoneid)
        {
            MemoryStream msdata = new MemoryStream();
            Module_14.write_14_11(msdata, uid,(byte)repos,stoneid);
            AppNet.gameNet.send(msdata, 14, 11);
            Log.info(this, "装备镶嵌 Send 14_11");
        }
        //宝石摘除
        public void EquipRemove(uint uid, int repos, int pos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_14.write_14_12(msdata, uid, (byte)repos, (byte)pos);
            AppNet.gameNet.send(msdata, 14, 12);
            Log.info(this, "宝石摘除 Send 14_12");
        }
        //宝石充灵
        public void EquipMerge(uint id,int repos, int pos, List<uint> idList)
        {
            MemoryStream msdata = new MemoryStream();
            Module_14.write_14_13(msdata, id, (byte)repos,(byte)pos,idList);
            AppNet.gameNet.send(msdata, 14, 13);
            Log.info(this, "宝石充灵 Send 14_13");
        }
        private void Fun_14_1(INetData data)
        {
            SmeltStrenMsg_14_1 message = new SmeltStrenMsg_14_1();
            message.read(data.GetMemoryStream());

            if (message.code == 0)
            {
                if(message.result == 0)
                    MessageManager.Show("强化成功");
                else
                {
                    MessageManager.Show("强化失败");
                }
                Singleton<Equip1Mode>.Instance.UpdateEquipStren(message.result);
                
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
                //失败直接请求 
                
            }
           
            Log.info(this,"Fun_14_1");
        }

        private void Fun_14_2(INetData data)
        {
            SmeltGetEquipRateMsg_14_2 message = new SmeltGetEquipRateMsg_14_2();
            message.read(data.GetMemoryStream());
            Singleton<Equip1Mode>.Instance.UpdateStrenProp(message.id,message.rate);
            Log.info(this, "Fun_14_2");
        }

        private void Fun_14_3(INetData data)
        {
            SmeltRefineMsg_14_3 message = new SmeltRefineMsg_14_3();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                if(message.result == 0)
                    MessageManager.Show("精炼成功");
                else
                {
                    MessageManager.Show("精炼失败");
                }
                Singleton<Equip1Mode>.Instance.UpdateEquipRefine();
            }
            else
                ErrorCodeManager.ShowError(message.code);
            Log.info(this, "Fun_14_3");
        }

        private void Fun_14_4(INetData data)
        {
            SmeltInheritMsg_14_4 message = new SmeltInheritMsg_14_4();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                MessageManager.Show("继承成功");
                Singleton<Equip1Mode>.Instance.UpdateEquipInherit();
            }
            else
                ErrorCodeManager.ShowError(message.code);
            Log.info(this, "Fun_14_4");
        }
        private void Fun_14_5(INetData data)
        {
            SmeltDestroyMsg_14_5 message = new SmeltDestroyMsg_14_5();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                MessageManager.Show("分解成功");
                Singleton<Equip1Mode>.Instance.UpdateEquipDestroy();
            }
            else
                ErrorCodeManager.ShowError(message.code);
            Log.info(this, "Fun_14_5");
        }

        private void Fun_14_11(INetData data)
        {
            SmeltInlayMsg_14_11 message = new SmeltInlayMsg_14_11();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                MessageManager.Show("镶嵌成功");
                Singleton<Equip1Mode>.Instance.UpdateEquipInlay();
            }
            else
                ErrorCodeManager.ShowError(message.code);
            Log.info(this, "Fun_14_11");
        }
        private void Fun_14_12(INetData data)
        {
            SmeltRemoveMsg_14_12 message = new SmeltRemoveMsg_14_12();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
                MessageManager.Show("摘除成功");
			else
                ErrorCodeManager.ShowError(message.code);
            Log.info(this, "Fun_14_12");
        }
        private void Fun_14_13(INetData data)
        {
            SmeltGemUpgradeMsg_14_13 message = new SmeltGemUpgradeMsg_14_13();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
                MessageManager.Show("充灵成功");
			else
                ErrorCodeManager.ShowError(message.code);
            Log.info(this, "Fun_14_13");
        }
       
    }

}

