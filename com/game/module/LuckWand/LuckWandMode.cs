using System.Collections.Generic;
using System.IO;
using com.game;
using com.game.module.test;
using PCustomDataType;
using Proto;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.LuckWand
{
    public class LuckWandMode : BaseMode<LuckWandMode>
    {
        public readonly int UPDATE_WAND_OPEN = 0;//开启
        public readonly int UPDATE_WAND_INFO = 1;//活动信息
        public readonly int UPDATE_WAND_COMMON = 2;//开奖记录
        public readonly int UPDATE_WAND_DRAW = 4;//打开魔杖
        public readonly int UPDATE_WAND_GRAND = 3;//至尊大奖


        public bool IsOpen
        {
            get { return this.Free > 0 || this.CanBuyTimes > 0; }
        }
        public List<PWandType> WandTypeList = new List<PWandType>(); //精灵列表
        public byte Free;//今日可免费使用次数
        public ushort Diam;//下次钻石购买花费的钻石数，0表示购买次数已完
        public uint TotalGold;//奖池总奖金
        public byte CanBuyTimes;//今日可购买次数
        public byte CanUseTimes;//今日可用此时

        public List<PWandInfo> GrandInfoList = new List<PWandInfo>();//至尊大奖
        public List<PWandInfo> CommonInfoList = new List<PWandInfo>();//普通奖励列表
        /// <summary>
        /// 活动开启
        /// </summary>
        public void UpdateWandOpen(byte free,ushort diam,List<PWandType> wandTypes  )
        {
            this.Free = free;
            this.Diam = diam;
            this.WandTypeList = wandTypes;
           
            this.DataUpdate(UPDATE_WAND_OPEN);
        }
        /// <summary>
        /// 魔杖信息
        /// </summary>
        /// <param name="wandInfo"></param>
        public void UpdateWandInfo(byte free ,ushort diam, uint totalGold,byte canBuys,byte canUses,List<PWandType> wandTypes )
        {
            this.Free = free;
            this.Diam = diam;
            this.TotalGold = totalGold;
            this.CanBuyTimes = canBuys;
            this.CanUseTimes = canUses;
            this.WandTypeList = wandTypes;
            this.DataUpdate(this.UPDATE_WAND_INFO);
        }
        /// <summary>
        /// 至尊大奖
        /// </summary>
        /// <param name="drawInfoList"></param>
        public void UpdateWandGand(List<PWandInfo> drawInfoList)
        {
            this.GrandInfoList = drawInfoList;
            this.DataUpdate(this.UPDATE_WAND_GRAND);
        }
        /// <summary>
        /// 普通奖励
        /// </summary>
        /// <param name="commonInfoList"></param>
        public void UpdateWandCommon(List<PWandInfo> commonInfoList)
        {
            this.CommonInfoList = commonInfoList;
            this.DataUpdate(this.UPDATE_WAND_COMMON);
        }
        /// <summary>
        /// 点击魔杖幻兽
        /// </summary>
        public void UpdateWandDraw()
        {
            this.DataUpdate(this.UPDATE_WAND_DRAW);
        }
		/**
     * 开启魔杖活动信息
     */
		public void write_25_0()
		{
            MemoryStream msdata = new MemoryStream();
            Module_25.write_25_0(msdata);
            AppNet.gameNet.send(msdata, 25, 0);
		}
		
		/**
     * 魔杖活动信息
     */
		public void write_25_1()
		{
            MemoryStream msdata = new MemoryStream();
            Module_25.write_25_1(msdata);
            AppNet.gameNet.send(msdata, 25, 1);
		}
		
		/**
     * 魔杖开奖记录
     */
		public void write_25_2()
		{
            MemoryStream msdata = new MemoryStream();
            Module_25.write_25_1(msdata);
            AppNet.gameNet.send(msdata, 25, 1);
		}
		
		/**
     * 魔杖活动至尊大奖
     */
		public void write_25_3()
		{
            MemoryStream msdata = new MemoryStream();
            Module_25.write_25_3(msdata);
            AppNet.gameNet.send(msdata, 25, 3);
		}
		
		/**
     * 打开魔杖
     */
		public void write_25_4( byte index)
		{
            MemoryStream msdata = new MemoryStream();
            Module_25.write_25_4(msdata,index);
            AppNet.gameNet.send(msdata, 25, 4);
		}
    }

}

