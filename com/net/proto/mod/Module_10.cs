/**
 * 聊天 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_10
  	{

    /**
     * 聊天请求，包含世界，公会
     */
    public static void write_10_1(MemoryStream msdata, byte chatType, string content, List<PChatGoods> goodsList)
    {
        proto_util.writeUByte(msdata, chatType);
        proto_util.writeString(msdata, content);
        PChatGoods.writeLoop(msdata, goodsList);
    }

    /**
     * 私聊
     */
    public static void write_10_2(MemoryStream msdata, uint roleId, string content, List<PChatGoods> goodsList)
    {
        proto_util.writeUInt(msdata, roleId);
        proto_util.writeString(msdata, content);
        PChatGoods.writeLoop(msdata, goodsList);
    }

    /**
     * 查看玩家展示信息
     */
    public static void write_10_4(MemoryStream msdata, uint id, byte type)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 请求小喇叭队列长度
     */
    public static void write_10_5(MemoryStream msdata)
    {
    }

    /**
     * GM反馈
     */
    public static void write_10_8(MemoryStream msdata, string title, string content)
    {
        proto_util.writeString(msdata, title);
        proto_util.writeString(msdata, content);
    }

    /**
     * 客户端提交错误
     */
    public static void write_10_9(MemoryStream msdata, string flashver, byte errType, string desc)
    {
        proto_util.writeString(msdata, flashver);
        proto_util.writeUByte(msdata, errType);
        proto_util.writeString(msdata, desc);
    }

    /**
     * 请求私聊基本信息
     */
    public static void write_10_10(MemoryStream msdata, uint roleId)
    {
        proto_util.writeUInt(msdata, roleId);
    }
   }
}