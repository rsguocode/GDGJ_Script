/**
 * 金银岛 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_26
  	{

    /**
     * 面板信息
     */
    public static void write_26_0(MemoryStream msdata)
    {
    }

    /**
     * 剩余打劫次数
     */
    public static void write_26_1(MemoryStream msdata)
    {
    }

    /**
     * 剩余协助次数
     */
    public static void write_26_2(MemoryStream msdata)
    {
    }

    /**
     * 剩余冒险次数
     */
    public static void write_26_3(MemoryStream msdata)
    {
    }

    /**
     * 打劫cd
     */
    public static void write_26_4(MemoryStream msdata)
    {
    }

    /**
     * 清除抢劫cd时间
     */
    public static void write_26_5(MemoryStream msdata)
    {
    }

    /**
     * 当前品质和刷新次数
     */
    public static void write_26_6(MemoryStream msdata)
    {
    }

    /**
     * 当前的协助玩家
     */
    public static void write_26_7(MemoryStream msdata)
    {
    }

    /**
     * 邀请协助者
     */
    public static void write_26_11(MemoryStream msdata, uint friendId)
    {
        proto_util.writeUInt(msdata, friendId);
    }

    /**
     * 推送邀请应答
     */
    public static void write_26_12(MemoryStream msdata, byte result, uint id)
    {
        proto_util.writeUByte(msdata, result);
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 随机品质
     */
    public static void write_26_13(MemoryStream msdata, byte type)
    {
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 开始冒险
     */
    public static void write_26_14(MemoryStream msdata)
    {
    }

    /**
     * 抢劫
     */
    public static void write_26_15(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }

    /**
     * 疾风术
     */
    public static void write_26_16(MemoryStream msdata)
    {
    }

    /**
     * 好友剩余协助次数列表
     */
    public static void write_26_18(MemoryStream msdata)
    {
    }

    /**
     * 打劫结果
     */
    public static void write_26_20(MemoryStream msdata, byte result, uint derid)
    {
        proto_util.writeUByte(msdata, result);
        proto_util.writeUInt(msdata, derid);
    }

    /**
     * 完成冒险
     */
    public static void write_26_21(MemoryStream msdata)
    {
    }

    /**
     * 图标打开状态
     */
    public static void write_26_24(MemoryStream msdata, byte status)
    {
        proto_util.writeUByte(msdata, status);
    }

    /**
     * 打劫预览
     */
    public static void write_26_26(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }
   }
}