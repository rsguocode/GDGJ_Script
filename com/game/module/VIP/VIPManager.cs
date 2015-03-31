using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using com.game.vo;
using UnityEngine;
using System.Collections;

public class VipManager
{

    
    /// <summary>
    /// 购买体力
    /// </summary>
    public const int BuyVigour = 1;
    /// <summary>
    /// 普通培养
    /// </summary>
    public const int GeneralGrow = 2;
    /// <summary>
    /// 加强培养
    /// </summary>
    public const int StrenGrow = 3;
    /// <summary>
    /// 至尊培养
    /// </summary>
    public const int TopGrow = 4;
    /// <summary>
    /// 黄金宝箱开启
    /// </summary>
    public const int OpenGlowBox = 5;
    /// <summary>
    /// 一键采集功能
    /// </summary>
    public const int OneKeyCollection = 6;
    /// <summary>
    /// 种植升级土地
    /// </summary>
    public const int UpgradeFramLand = 7;
    /// <summary>
    /// 世界Boss金币鼓舞
    /// </summary>
    public const int WorldBossInspiation = 8;
    /// <summary>
    /// 幸运魔杖使用次数
    /// </summary>
    public const int LuckWandTime = 9;
    /// <summary>
    /// 商店赠送功能
    /// </summary>
    public const int OpenStoreGive = 10;
    /// <summary>
    /// 好友上限
    /// </summary>
    public const int FriendLimit = 12;
    /// <summary>
    /// 打怪经验加成
    /// </summary>
    public const int MonsterExpAdd = 13;
    /// <summary>
    /// 黄金宝箱批量使用
    /// </summary>
    public const int GoldBoxBatch = 16;

    /// <summary>
    /// 获取Vip特权的value值
    /// </summary>
    /// <param name="vipDroitType">Vip特权类型 </param>
    /// <returns></returns>
    public static int GetVipDroitValue(int vipDroitType)
    {
        SysVipDroitVo vo = BaseDataMgr.instance.GetDataById<SysVipDroitVo>((uint)(MeVo.instance.vip*100 + vipDroitType));
        if (vo != null)
            return vo.value;
        return 0;
    }
    /// <summary>
    /// Vip特权是否开启
    /// </summary>
    /// <param name="vipDroitType"></param>
    /// <returns></returns>
    public static bool IsVipDriotOpen(int vipDroitType)
    {
        SysVipDroitVo vo = BaseDataMgr.instance.GetDataById<SysVipDroitVo>((uint)(MeVo.instance.vip * 100 + vipDroitType));
        if (vo != null)
            return true;
        return false;
    }


}
