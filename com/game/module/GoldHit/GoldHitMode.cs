using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using System.IO;
using Proto;
using com.game;
using com.u3d.bases.consts;
using com.game.module.map;
using com.game.consts;
using com.game.module.main;

namespace Com.Game.Module.GoldHit
{
    public class GoldHitMode : BaseMode<GoldHitMode>
    {
        public readonly int UPDATE_CHALLENGE_TIMES = 1;
        //public readonly int UPDATE_CHANGE_SENCE = 2;
        public readonly int UPDATE_GOLD_COUNT = 3;  //金币数，暂未用到
        public readonly int UPDATE_CLEAR_CD = 4;   //清除CD
        public readonly int UPDATE_MONSTER_DEATH = 5;   //石像被打死了
		public readonly byte UPDATE_TIPS = 6;


        private DungeonGoldTreeInfoMsg_8_11 _info = new DungeonGoldTreeInfoMsg_8_11();
        private DungeonGoldTreeAddGoldMsg_8_14 _goldNum = new DungeonGoldTreeAddGoldMsg_8_14();
        
        public DungeonGoldTreeInfoMsg_8_11 info { get { return _info; } }
        public DungeonGoldTreeAddGoldMsg_8_14 goldNum { get { return _goldNum; } }
        public int useTime = 0;
        public int nGold = 0;
        public bool mainOpen = false;
        public Vector3 Monster;

        public bool CanPlay { get { return (_info.times < 6); } }

		private bool canShowTips = false;
		
		public override bool ShowTips
		{
			get {return canShowTips && (CanPlay && 0==_info.remainTime);}
		}

		public void StartTips()
		{
			canShowTips = true;
			ApplyGoldHitInfo();
		}
		
		public void StopTips()
		{
			canShowTips = false;
			DataUpdate(UPDATE_TIPS);
		}
		
		private void NotifyTips()
		{
			if (canShowTips)
			{
				DataUpdate(UPDATE_TIPS);
			}
		}
		
		private void GoldHitInfoCountDown()
		{
			if (_info.remainTime>0)
			{
				vp_Timer.CancelAll("GoldHitCountDownCallback");
				vp_Timer.In(_info.remainTime, GoldHitCountDownCallback, 1, 0);
			}
		}
		
		private void GoldHitCountDownCallback()
		{
			if (canShowTips)
			{
				ApplyGoldHitInfo();
			}
		}

        //-------------------------------------  协议请求  -----------------------------------------//
        //客户端请求点石成金的面板信息
        public void ApplyGoldHitInfo()
        {
            Log.info(this, "发送8-11给服务器请求点石成金信息");
			MemoryStream msdata = new MemoryStream ();
            Module_8.write_8_11(msdata);
			AppNet.gameNet.send(msdata, 8, 11);
        }

        //请求切换场景到击石成金的副本
        public void ApplyChangeSence()
        {
            Log.info(this, "发送4-1给服务器请求切换到点石成金副本");
            Singleton<MapMode>.Instance.changeScene(MapTypeConst.GoldHit_MAP, true, 5, 1.8f);
        }

        //请求切换回主城
        public void AppyBakctoMain()
        {
            Log.info(this, "发送4-1给服务器请求切换回主城");
            Singleton<MapMode>.Instance.changeScene(MapTypeConst.MajorCity, true, 5, 1.8f);
        }

        //向服务器发送伤害
        public void Attack(uint hurt)
        {
            //MemoryStream msdata = new MemoryStream();
            //Module_8.write_8_14(msdata, hurt);
            //AppNet.gameNet.send(msdata, 8,  14);
        }


        //向服务器发送清除CD
        public void ApplyClearCD()
        {
            MemoryStream msdata = new MemoryStream();
            Module_8.write_8_12(msdata);
            AppNet.gameNet.send(msdata, 8, 12);
            Log.debug(this, "向服务器请求清除CD！");
        }

        //----------------------------------------  更新数据  ----------------------------------------------//
        //更新面板信息. 剩余挑战次数
        public void UpdateGoldHitInfo(DungeonGoldTreeInfoMsg_8_11 recInfo)
        {
            Log.debug(this, "8_11 客户端收到数据了");
            _info.times = recInfo.times;
            _info.remainTime = recInfo.remainTime;
			_info.diam = recInfo.diam;
            DataUpdate(UPDATE_CHALLENGE_TIMES);
			NotifyTips();
			GoldHitInfoCountDown();
        }

        public void UpdateGold(DungeonGoldTreeAddGoldMsg_8_14 gold)
        {
            goldNum.gold = gold.gold;
            goldNum.monid = gold.monid;
            DataUpdate(UPDATE_GOLD_COUNT);
        }
        
        public void UpdateCD(byte cd)
        {
            _info.remainTime = cd;  //更新时间
            DataUpdate(UPDATE_CLEAR_CD);
        }

    }
}
