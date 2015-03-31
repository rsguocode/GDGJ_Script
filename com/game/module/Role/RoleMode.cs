using System.Collections.Generic;
using com.u3d.bases.debug;
using UnityEngine;
using System.Collections;
using PCustomDataType;
using com.game.module.test;
using System.IO;
using Proto;
using com.game;
using com.game.consts;
using com.game.module.effect;
using com.game.module.map;

namespace Com.Game.Module.Role
{
    public class RoleMode : BaseMode<RoleMode>
    {
        //更新体力代号
        public const int UPDATE_VIGOUR = 1;
        //更新Buff列表
        public readonly int UPDATE_BUFF = 2;

        //更新财富信息
        public readonly int UPDATE_FORTUNE = 3;

        //更新他人角色信息
        public readonly int UPDATE_OTHER_INFO = 4;

        //玩家升级
        public readonly int PLAYER_UPGRADE_INFO = 5;

		//玩家属性更新
		public readonly int UPDATE_ROLE_ATTR = 6;

		//玩家复活
		public readonly int UPDATE_ROLE_RELIFE = 7;

        public override bool ShowTips
        {
            get { return GoodsMode.Instance.ShowTips || GrowMode.Instance.ShowTips; }
        }

        
		/// <summary>
		/// 其他玩家的信息
		/// </summary>
		/// <value>The other attr.</value>
		public List<PRoleAttr> otherAttr{get;set;}

        //未升级前等级，动态更新
        private int oldLevel = 0;
        //升级后等级，动态更新
        private int newLevel = 0;
        
		private string _roleName;
		public string roleName
		{
			set
			{
				_roleName = value;
			}

			get
			{
				return _roleName;
			}
		}
		
		//判断玩家是否刚升级过
		public bool Upgraded
		{
			get
			{
				return (newLevel > oldLevel);
			}
		}


        //buff列表
        public List<PBuff> buffList { get; set; }
        /**
        * 请求角色信息
         */
        //        public void RequestRoleInfo()
        //        {
        //            MemoryStream msdata = new MemoryStream();
        //            Module_3.write_3_1(msdata);
        //            AppNet.gameNet.send(msdata,3,1);
        //        }

        /**
         * buff列表
         */
        public void RequestBuffList()
        {
            MemoryStream msdata = new MemoryStream();
            Module_3.write_3_2(msdata);
            AppNet.gameNet.send(msdata, 3, 2);
        }

        /**
         * 其他人信息（角色属性面板）
         */
        public void RequestOtherRoleInfo(uint id)
        {
            MemoryStream msdata = new MemoryStream();
            Module_3.write_3_3(msdata, id);
            AppNet.gameNet.send(msdata, 3, 3);
        }

        /**
         * 财富更新
         */
        public void UpdateMoney()
        {
            MemoryStream msdata = new MemoryStream();
            Module_3.write_3_5(msdata);
            AppNet.gameNet.send(msdata, 3, 5);
        }

        /**
         * 复活  复活类型1使用钻石 2使用道具 3回城复活
         * 复活
         */
        public void ReLife(byte type)
        {
            Log.info(this, "-ReLife()发送3-20复活协议，参数：" + type);
            MemoryStream msdata = new MemoryStream();
            Module_3.write_3_20(msdata, type);
            AppNet.gameNet.send(msdata, 3, 20);
        }
		//直接使用钻石复活，给世界Boss调用
		public void ReLife()
		{
			ReLife(MapTypeConst.ROLE_REVIVE_USE_DIAM);
		}

        //购买体力
        public void BuyVigour(byte type)
        {
            MemoryStream msdata = new MemoryStream();
            Module_3.write_3_40(msdata, type);
            AppNet.gameNet.send(msdata, 3, 40);
        }

        /// <summary>
        /// 主角血蓝变化
        /// </summary>
        public void SendHeroHpAndMagic(uint hp, uint mp)
        {
            MemoryStream msdata = new MemoryStream();
            Module_3.write_3_6(msdata, hp, mp);
            AppNet.gameNet.send(msdata, 3, 6);
        }

        //更新体力
        public void UpdateVigour()
        {
            DataUpdate(UPDATE_VIGOUR);
        }
        //更新Buffer列表
        public void UpdateBuffList(List<PBuff> buffList)
        {
            this.buffList = buffList;
            DataUpdate(UPDATE_BUFF);
        }
        //更新财富信息
        public void UpdateFortune()
        {
            DataUpdate(UPDATE_FORTUNE);
        }
		//基础属性更新
		public void UpdateAttr()
		{
			DataUpdate (UPDATE_ROLE_ATTR);
		}
		//玩家复活
		public void UpdateReLife()
		{
			this.DataUpdate(UPDATE_ROLE_RELIFE);
		}
		                     
        //玩家升级提示
        public void PlayerUpgrade(int preLevel, int curLevel)
        {
            oldLevel = preLevel;
            newLevel = curLevel;

            DataUpdate(PLAYER_UPGRADE_INFO);
        }


		public void UpdateOtherInfo(List<PRoleAttr> roleAttr)
		{
			this.otherAttr = roleAttr;
			DataUpdate(UPDATE_OTHER_INFO);
		}

        /// <summary>
        /// 状态更新
        /// </summary>
        public void SendStatuChange()
        {
            var msdata = new MemoryStream();
            var statu = (byte)AppMap.Instance.me.Controller.StatuController.CurrentStatu;
            var x = (uint)(AppMap.Instance.me.Controller.transform.position.x*1000);
            var y = (uint)(AppMap.Instance.me.Controller.transform.position.y*1000);
            var dir = (byte) AppMap.Instance.me.CurDire;
            Module_3.write_3_44(msdata,statu,x,y,dir);
            //Log.info(this, "Statu:" + statu + "X" +  x + "Y" +  y + "Dir" + dir +"");
            AppNet.gameNet.send(msdata, 3, 44);
        }

		public string GetPlayerHeadSpriteName(byte job)
		{
            switch (job)
			{
			case 1:
				return "101";
			case 2:
				return "201";
			case 3:
				return "301";
            default:
                return "";
			}
		}
    }

	public enum AttrType : byte
	{
		ATTR_ID_STR = 1,     //力
		ATTR_ID_AGI,         //敏
		ATTR_ID_PHY,         //体
		ATTR_ID_WIT,         //智
		ATTR_ID_HP,          //生命
		ATTR_ID_MP,          //魔法
		ATTR_ID_ATT_P_MIN,   //最小物功
		ATTR_ID_ATT_P_MAX,   //最大物功
		ATTR_ID_ATT_M_MIN,   //最小魔功
		ATTR_ID_ATT_M_MAX,   //最大魔功
		ATTR_ID_ATT_DEF_P,   //物防
		ATTR_ID_ATT_DEF_M,   //魔防
		ATTR_ID_HIT,         //命中
		ATTR_ID_DODGE,       //闪避
		ATTR_ID_CRIT,        //暴击
		ATTR_ID_CRIT_RATIO,  //暴击伤害比例
		ATTR_ID_FLEX,        //韧性
		ATTR_ID_HURT_RE,     //格挡
		ATTR_ID_SPEED,       //速度
		ATTR_ID_LUCK,        //幸运值
		ATTR_ID_ATT_MIN,     //最小攻击
		ATTR_ID_ATT_MAX,     //最大攻击  (人物攻击)
	}
}

