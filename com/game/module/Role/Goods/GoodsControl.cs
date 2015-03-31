using System.IO;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.net.interfaces;
using com.game.cmd;
using com.game;
using com.u3d.bases.debug;
using Proto;
using PCustomDataType;
using System.Collections.Generic;
using Com.Game.Module.Waiting;
using com.game.Public.Message;
using com.game.manager;

namespace Com.Game.Module.Role
{
    public class GoodsControl : BaseControl<GoodsControl>
    {

        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_5_1, Fun_5_1); // 物品信息 : GoodsInfoMsg_5_1
            AppNet.main.addCMD(CMD.CMD_5_2, Fun_5_2);// 删除物品(支持多个) : GoodsDeleteMsg_5_2		
            AppNet.main.addCMD(CMD.CMD_5_5, Fun_5_5);// 整理包裹 : GoodsPackTidyMsg_5_5	
            AppNet.main.addCMD(CMD.CMD_5_6, Fun_5_6); // 交换位置 : GoodsSwapMsg_5_6
            AppNet.main.addCMD(CMD.CMD_5_7, Fun_5_7); // 拆分 : GoodsSplitMsg_5_7 
            AppNet.main.addCMD(CMD.CMD_5_9, Fun_5_9);// 使用物品 : GoodsUseMsg_5_9	
            AppNet.main.addCMD(CMD.CMD_5_10, Fun_5_10); // 销毁物品 : GoodsDestroyMsg_5_10;
            AppNet.main.addCMD(CMD.CMD_5_11, Fun_5_11); // 穿装备 : GoodsPutonMsg_5_11;
            AppNet.main.addCMD(CMD.CMD_5_12, Fun_5_12); // 卸装备 : GoodsTakeoffMsg_5_12;
            AppNet.main.addCMD(CMD.CMD_5_13, Fun_5_13); // 批量使用 : GoodsBatchUseMsg_5_13;
        }

        /**
         * 包裹信息
         */
        public void RequestWrapInfo(byte repos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_1(msdata, repos);
            AppNet.gameNet.send(msdata, 5, 1);
        }

        /**
         * 整理包裹
         */
        public void SortWrap(byte repos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_5(msdata, repos);
            AppNet.gameNet.send(msdata, 5, 5);
        }

        /**
         * 交换位置
         */
        public void SwapGrid(byte repos, ushort from, ushort to)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_6(msdata, repos, from, to);
            AppNet.gameNet.send(msdata, 5, 6);
        }

        /**
         * 拆分
         */
        public void SplitWrap(byte repos, uint id, ushort count)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_7(msdata, repos, id, count);
            AppNet.gameNet.send(msdata, 5, 7);
        }

        /**
         * 使用物品
         */
        public void UseGoods(uint id, string name)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_9(msdata, id, name);
            AppNet.gameNet.send(msdata, 5, 9);
        }

        /**
         * 销毁物品
         */
        public void DestroyGoods(uint id, byte repos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_10(msdata, id, repos);
            AppNet.gameNet.send(msdata, 5, 10);
        }

        /**
         * 批量使用
         */
        public void UseGoodsMany(uint id, ushort count)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_13(msdata, id, count);
            AppNet.gameNet.send(msdata, 5, 13);
        }

        /**
         * 仓库背包交换位置
         */
        public void SwapStorage(byte repos, uint id)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_14(msdata, repos, id);
            AppNet.gameNet.send(msdata, 5, 14);
        }

        /**
         * npc商店出售
         */
        public void SellGoods(uint id)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_15(msdata, id);
            AppNet.gameNet.send(msdata, 5, 15);
        }

        /**
         * npc商店购买
         */
        public void BuyGoods(uint npcId, uint shopId, uint goodsId, ushort count)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_16(msdata, npcId, shopId, goodsId, count);
            AppNet.gameNet.send(msdata, 5, 16);
        }

        /// <summary>
        ///穿装备
        /// </summary>
        /// <param name="msdata"></param>
        /// <param name="id">装备id</param>
        /// <param name="pos">位置</param>
        public void WearEquipment(uint id, byte pos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_11(msdata, id, pos);
            AppNet.gameNet.send(msdata, 5, 11);
        }

        /// <summary>
        /// 卸装备 
        /// </summary>
        /// <param name="msdata"></param>
        /// <param name="id">装备id</param>
        /// <param name="pos">位置</param>

        public void UnWearEquipment(uint id, byte pos)
        {
            MemoryStream msdata = new MemoryStream();
            Module_5.write_5_12(msdata, id, pos);
            AppNet.gameNet.send(msdata, 5, 12);
        }
        private void Fun_5_1(INetData data)
        {
            GoodsInfoMsg_5_1 message = new GoodsInfoMsg_5_1();
            message.read(data.GetMemoryStream());
            List<PGoods> goodsInfo = message.goodsInfo;
            Singleton<GoodsMode>.Instance.UpdateGoodsInfo(goodsInfo,message.repos);
            Log.info(this, "Fun_5_1 repos :"+ message.repos);
        }
        private void Fun_5_2(INetData data)
        {
            GoodsDeleteMsg_5_2 message = new GoodsDeleteMsg_5_2();
            message.read(data.GetMemoryStream());
            Singleton<GoodsMode>.Instance.DeleteGoods(message.id, message.repos);
            Log.info(this, "Fun_5_2");
        }
       
        private void Fun_5_5(INetData data)
        {
            GoodsPackTidyMsg_5_5 message = new GoodsPackTidyMsg_5_5();
            message.read(data.GetMemoryStream());
            if (message.code == 0) //
            {
                //MessageManager.Show(LanguageManager.GetWord("Goods.ArrangeSuccess"));
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
            Log.info(this, "Fun_5_5");
        }
        private void Fun_5_6(INetData data)
        {
            Log.info(this, "Fun_5_6");

        }

        private void Fun_5_7(INetData data)
        {
            Log.info(this, "Fun_5_7");
        }

        
        private void Fun_5_9(INetData data)
        {
            GoodsUseMsg_5_9 message = new GoodsUseMsg_5_9();
            message.read(data.GetMemoryStream());
            if (message.code == 0)
            {
                MessageManager.Show(LanguageManager.GetWord("Goods.UseSuccess"));
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
            Log.info(this, "Fun_5_9");
        }
        private void Fun_5_10(INetData data)
        {
            GoodsDestroyMsg_5_10 message = new GoodsDestroyMsg_5_10();
            message.read(data.GetMemoryStream());
            if (message.code == 0) //删除成功
            {
                MessageManager.Show(LanguageManager.GetWord("Goods.DeleteSuccess"));
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
            Log.info(this, "Fun_5_10");
        }
        private void Fun_5_11(INetData data)
        {
			GoodsPutonMsg_5_11 message = new GoodsPutonMsg_5_11();
			message.read(data.GetMemoryStream());
            if (message.code == 0) //
            {
                MessageManager.Show(LanguageManager.GetWord("Goods.EquipSuccess"));
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
            Log.info(this, "Fun_5_11 code: "+ message.code );
        }
        private void Fun_5_12(INetData data)
        {
			GoodsTakeoffMsg_5_12 message = new GoodsTakeoffMsg_5_12();
			message.read(data.GetMemoryStream());
            if (message.code == 0) //
            {
                MessageManager.Show(LanguageManager.GetWord("Goods.UnEquipSuccess"));
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
			Log.info(this, "Fun_5_12 code: "+ message.code );

        }
        private void Fun_5_13(INetData data)
        {
            GoodsBatchUseMsg_5_13 message = new GoodsBatchUseMsg_5_13();
            message.read(data.GetMemoryStream());
            if (message.code == 0) //
            {
                MessageManager.Show(LanguageManager.GetWord("Goods.QuickUseSucess"));
            }
            else
            {
                ErrorCodeManager.ShowError(message.code);
            }
            Log.info(this, "Fun_5_13");
        }
        
    }

}
