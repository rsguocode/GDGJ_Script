//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：VIPLogic
//文件描述：VIP逻辑处理
//////////////////////////////////////////////////////////////////////////////////////////////


using System.Collections.Generic;
using com.game.data;
using com.game.manager;

namespace Com.Game.Module.VIP
{
    public class VIPLogic 
    {
        public static Dictionary<int, int[]> VipLevelNumConfig = new Dictionary<int, int[]>();  //存储了某个功能对应的vip等级和数量

        public static void InitConfig()
        {
            foreach(SysVipDroitVo vip in BaseDataMgr.instance.GetDicByType<SysVipDroitVo>().Values)
            {
                int[] config;
                if (!VipLevelNumConfig.ContainsKey(vip.type))
                {
                    config = new int[13];
                    VipLevelNumConfig[vip.type] = config; //vip数量配置最多12级
                }
                else
                {
                    config = VipLevelNumConfig[vip.type];
                }
                config[vip.vip] = vip.value;
            }
        }

        /// <summary>
        /// 传入vip权限类型和自己vip等级，获取对应的vip权限次数值，返回-1表示无限制，0表示vip等级不足，>0表示对应的vip的等级最大次数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vipLevel"></param>
        /// <returns></returns>
        public static int GetVipNum(int type ,int vipLevel)
        {

            if (VipLevelNumConfig.ContainsKey(type))
            {
                int[] config = VipLevelNumConfig[type];
                int num = 0;
                for (int i = 0; i < config.Length;i++)
                {
                    if (i <= vipLevel&&config[i] != 0)
                    {
                        num = config[i];
                    }
                }

                return num;
            }
            //无此配置项 无限制
            return -1;
        }


        /// <summary>
        /// 获取功能的开启需要的vip等级
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetOpenVipLevel(int type)
        {
            if (VipLevelNumConfig.ContainsKey(type))
            {
                int[] config = VipLevelNumConfig[type];
                for (int i = 0; i < config.Length; i++)
                {
                    if (config[i] != 0)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取当前vip等级的下一级增加次数的vip等级
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vip"></param>
        /// <returns></returns>
        public static int GetNextVipLevel(int type, int vip)
        {
            if (VipLevelNumConfig.ContainsKey(type))
            {
                int[] config = VipLevelNumConfig[type];
                for (int i = vip + 1; i < config.Length; i++)
                {
                    if (config[i] != 0)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
    }
}