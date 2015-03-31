/**
 * 任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_6
  	{

    /**
     * 任务信息
     */
    public static void write_6_1(MemoryStream msdata)
    {
    }

    /**
     * 接任务
     */
    public static void write_6_2(MemoryStream msdata, uint taskId, uint npcId)
    {
        proto_util.writeUInt(msdata, taskId);
        proto_util.writeUInt(msdata, npcId);
    }

    /**
     * 放弃任务
     */
    public static void write_6_3(MemoryStream msdata, uint taskId)
    {
        proto_util.writeUInt(msdata, taskId);
    }

    /**
     * 提交任务
     */
    public static void write_6_4(MemoryStream msdata, uint taskId, uint npcId)
    {
        proto_util.writeUInt(msdata, taskId);
        proto_util.writeUInt(msdata, npcId);
    }

    /**
     * 可接任务列表
     */
    public static void write_6_6(MemoryStream msdata)
    {
    }

    /**
     * 最后完成主线任务id
     */
    public static void write_6_7(MemoryStream msdata)
    {
    }

    /**
     * 立即完成任务
     */
    public static void write_6_8(MemoryStream msdata, ushort taskId)
    {
        proto_util.writeUShort(msdata, taskId);
    }

    /**
     * 任务次数
     */
    public static void write_6_9(MemoryStream msdata)
    {
    }

    /**
     * Npc对话
     */
    public static void write_6_10(MemoryStream msdata, uint taskId, uint npcId)
    {
        proto_util.writeUInt(msdata, taskId);
        proto_util.writeUInt(msdata, npcId);
    }

    /**
     * 循环任务信息
     */
    public static void write_6_11(MemoryStream msdata, ushort type)
    {
        proto_util.writeUShort(msdata, type);
    }

    /**
     * 刷新循环任务系数
     */
    public static void write_6_13(MemoryStream msdata, ushort type)
    {
        proto_util.writeUShort(msdata, type);
    }

    /**
     * 悬赏任务信息
     */
    public static void write_6_14(MemoryStream msdata)
    {
    }

    /**
     * 刷新悬赏任务信息
     */
    public static void write_6_15(MemoryStream msdata)
    {
    }
   }
}