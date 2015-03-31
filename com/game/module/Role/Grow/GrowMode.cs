using System.Collections.Generic;
using System.IO;
using com.game;
using com.game.data;
using com.game.manager;
using com.game.module.test;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using Proto;

namespace Com.Game.Module.Role
{
    public class GrowMode : BaseMode<GrowMode>
    {
        //培养类型 1普通培养 2加强培养 3白金培养 4钻石培养 5至尊培养3，4需要vip 
        /*  [1000,500,100000]
         *  金币计算公式  第一个是基础 第二个增值 第三个是上限
            目前是培养得到的属性总和 除以5
            然后乘以这个增值 再加上基础值 就是当前培养需要的费用*/

        //免费次数 5 ， 2， 1 
        // CD 时间 10分钟，12小时，24小时
        /// <summary>
        /// price配置表里面的培育Id
        /// </summary>
        public readonly uint GeneralGrowId = 1005;
        public readonly uint StrenGrowId = 1006;
        public readonly uint TopGrowId = 1007;

        public readonly int UPDATE_CD_TIME = 11;
        public readonly int UPDATE_PANNEL_INFO = 1010;
        public readonly int UPDATE_SHUXING = 1011;
        public readonly int UPDATE_TIPS = 12;

        public List<sbyte> AddList = new List<sbyte>();
        public GrowattrInfoMsg_17_1 GrowAttrInfo = new GrowattrInfoMsg_17_1();
        public int LeftTimes; //剩余付费次数次数

        private int[] growCost;

        public int[] GrowCost
        {
            get
            {
                if (growCost == null)
                    GetGrowCost();
                return growCost;
            }
        }

        private bool isInit = false;
        /// <summary>
        /// 是否显示小红点提示
        /// </summary>
        public override bool ShowTips
        {
            get { return IsShowTips(); }
        }

        private bool IsShowTips()
        {
            if (!isInit)
                return false;
            return GetGrowState(1) == 3 || GetGrowState(2) ==3 || GetGrowState(3) ==3;
        }
        /// <summary>
        ///     培育付费次数
        /// </summary>
        public int VipGrowCount
        {
            get
            {
                SysVipDroitVo vo;
                vo = BaseDataMgr.instance.GetDataById<SysVipDroitVo>((uint) (MeVo.instance.vip*100 + 2));
                return vo.value;
            }
        }

        //培养花费
        private void GetGrowCost()
        {
            growCost = new int[3];
            growCost[0] =
                int.Parse(StringUtils.SplitVoString(BaseDataMgr.instance.GetDataById<SysPriceVo>(GeneralGrowId).gold)[0]);
            growCost[1] =
                int.Parse(StringUtils.SplitVoString(BaseDataMgr.instance.GetDataById<SysPriceVo>(StrenGrowId).diam)[0]);
            growCost[2] =
                int.Parse(StringUtils.SplitVoString(BaseDataMgr.instance.GetDataById<SysPriceVo>(TopGrowId).diam)[0]);
        }

        /// <summary>
        ///     0 表示未开启，
        ///     1 表示CD没到，但有付费次数， 2 表示CD没到（有免费次数）没有付费次数， 3 表示CD时间到，有免费次数，
        /// </summary>
        /// 4 表示没有免费次数，有付费次数 5 表示没有免费次数，没有付费次数
        /// <param name="type">1表示普通培养 2 表示 加强培养 3 表示 至尊培养</param>
        /// <returns></returns>
        public int GetGrowState(int type)
        {
            int index = type - 1;
            int useFree = 0; //免费使用次数
            int useCost = 0; //付费使用次数
            int cdLeft = 0; //剩余CD时间
            if (type == 1)
            {
                useFree = (int) GrowAttrInfo.lv1Free;
                cdLeft = (int) GrowAttrInfo.lv1Cd;
            }
            else if (type == 2)
            {
                useFree = (int) GrowAttrInfo.lv2Free;
                cdLeft = (int) GrowAttrInfo.lv2Cd;
            }
            else if (type == 3)
            {
                useFree = (int) GrowAttrInfo.lv3Free;
                cdLeft = (int) GrowAttrInfo.lv3Cd;
            }
            int[] freeCount = {5, 2, 1}; //三种培育的免费次数
            int[] vipRequire = {0, 4, 6}; //vip开启

            int leftCost = LeftTimes; //剩余付费次数
            int leftFree = freeCount[index] - useFree; //剩余免费次数
            if (MeVo.instance.vip < vipRequire[index])
                return 0;
            if (leftFree > 0)
            {
                if (cdLeft > 0)
                {
                    if (leftCost > 0)
                        return 1;
                    return 2;
                }
                return 3;
            }
            if (leftCost > 0)
                return 4;
            return 5;
            return 0;
        }

        public void GrowInfoApply()
        {
            Log.debug(this, "发送17_1给服务器请求培养面板的信息");
            var msdata = new MemoryStream();
            Module_17.write_17_1(msdata);
            AppNet.gameNet.send(msdata, 17, 1);
        }

        /**
        * 保存培养属性
        */

        public void UpdateGrowShuxing(List<sbyte> addList)
        {
            Log.debug(this, "服务器返回培育的确认信息");
            AddList = addList;
            DataUpdate(UPDATE_SHUXING);
        }


        //发送17_2请求培养
        public void AppyGrow(int type, bool is_free)
        {
            Log.debug(this, "发送17_2给服务器请求培育");
            var msdata = new MemoryStream();
            Module_17.write_17_2(msdata, (byte) type, is_free);
            AppNet.gameNet.send(msdata, 17, 2);
        }


        //**************************************服务器返回信息****************************
        public void UpdateGrowPannelInfo(GrowattrInfoMsg_17_1 msg)
        {
            isInit = true;
            Log.debug(this, "服务器返回17_1的协议");
            GrowAttrInfo = msg;
            //付费剩余次数
            LeftTimes = VipGrowCount - (int) msg.payTimes;
            uint maxTime = GrowAttrInfo.lv1Cd;
            maxTime = maxTime < GrowAttrInfo.lv2Cd ? GrowAttrInfo.lv2Cd : maxTime;
            maxTime = maxTime < GrowAttrInfo.lv3Cd ? GrowAttrInfo.lv3Cd : maxTime;
            vp_Timer.CancelAll("UpdateCDTime");
            vp_Timer.In(0f, UpdateCDTime, (int) (maxTime + 1), 1f);
            DataUpdate(UPDATE_PANNEL_INFO);
            DataUpdate(this.UPDATE_TIPS);
        }

        private void UpdateCDTime()
        {
            DataUpdate(UPDATE_CD_TIME);
            if (GrowAttrInfo.lv1Cd == 0 && GrowAttrInfo.lv2Cd == 0 && GrowAttrInfo.lv3Cd == 0)
            {
                vp_Timer.CancelAll("UpdateCDTime");
                return;
            }
            if (GrowAttrInfo.lv1Cd == 0 || GrowAttrInfo.lv2Cd == 0 || GrowAttrInfo.lv3Cd == 0)
            {
                DataUpdate(this.UPDATE_TIPS);
            }
            GrowAttrInfo.lv1Cd = GrowAttrInfo.lv1Cd > 0 ? GrowAttrInfo.lv1Cd - 1 : 0;
            GrowAttrInfo.lv2Cd = GrowAttrInfo.lv2Cd > 0 ? GrowAttrInfo.lv2Cd - 1 : 0;
            GrowAttrInfo.lv3Cd = GrowAttrInfo.lv3Cd > 0 ? GrowAttrInfo.lv3Cd - 1 : 0;
            
            //DataUpdate(UPDATE_CD_TIME);
        }
    }
}