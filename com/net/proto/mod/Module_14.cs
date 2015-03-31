/**
 * 装备 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_14
  	{

    /**
     * 强化
     */
    public static void write_14_1(MemoryStream msdata, uint id, byte repos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
    }

    /**
     * 获得装备的失败加成成功率
     */
    public static void write_14_2(MemoryStream msdata, uint id, byte repos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
    }

    /**
     * 精炼
     */
    public static void write_14_3(MemoryStream msdata, uint id, byte repos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
    }

    /**
     * 装备继承
     */
    public static void write_14_4(MemoryStream msdata, uint prevId, uint nextId)
    {
        proto_util.writeUInt(msdata, prevId);
        proto_util.writeUInt(msdata, nextId);
    }

    /**
     * 装备分解
     */
    public static void write_14_5(MemoryStream msdata, List<uint> list)
    {
        proto_util.writeLoopUInt(msdata, list);
    }

    /**
     * 镶嵌宝石
     */
    public static void write_14_11(MemoryStream msdata, uint id, byte repos, uint stoneid)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
        proto_util.writeUInt(msdata, stoneid);
    }

    /**
     * 摘除宝石
     */
    public static void write_14_12(MemoryStream msdata, uint id, byte repos, byte pos)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
        proto_util.writeUByte(msdata, pos);
    }

    /**
     * 宝石升级(冲灵)
     */
    public static void write_14_13(MemoryStream msdata, uint id, byte repos, byte pos, List<uint> list)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
        proto_util.writeUByte(msdata, pos);
        proto_util.writeLoopUInt(msdata, list);
    }
   }
}