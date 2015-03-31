/**
 * 技能 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_13
  	{

    /**
     * 技能使用
     */
    public static void write_13_0(MemoryStream msdata, uint skillId)
    {
        proto_util.writeUInt(msdata, skillId);
    }

    /**
     * 技能确认
     */
    public static void write_13_1(MemoryStream msdata, byte seq, uint skillId, byte dir, List<PTarget> target)
    {
        proto_util.writeUByte(msdata, seq);
        proto_util.writeUInt(msdata, skillId);
        proto_util.writeUByte(msdata, dir);
        PTarget.writeLoop(msdata, target);
    }

    /**
     * 技能信息
     */
    public static void write_13_4(MemoryStream msdata)
    {
    }

    /**
     * 学习技能
     */
    public static void write_13_5(MemoryStream msdata, uint skillId, uint nextId)
    {
        proto_util.writeUInt(msdata, skillId);
        proto_util.writeUInt(msdata, nextId);
    }

    /**
     * 一键学习
     */
    public static void write_13_6(MemoryStream msdata, List<PSkill> skillids)
    {
        PSkill.writeLoop(msdata, skillids);
    }

    /**
     * 重置
     */
    public static void write_13_7(MemoryStream msdata, List<uint> skillids)
    {
        proto_util.writeLoopUInt(msdata, skillids);
    }

    /**
     * 技能使用同步,客户端发起(暂时)
     */
    public static void write_13_11(MemoryStream msdata, uint id, byte type, uint skillId, byte dir)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, type);
        proto_util.writeUInt(msdata, skillId);
        proto_util.writeUByte(msdata, dir);
    }

    /**
     * 技能s伤害同步,客户端发起(暂时)
     */
    public static void write_13_12(MemoryStream msdata, uint skillId, List<PDamage> damage)
    {
        proto_util.writeUInt(msdata, skillId);
        PDamage.writeLoop(msdata, damage);
    }
   }
}