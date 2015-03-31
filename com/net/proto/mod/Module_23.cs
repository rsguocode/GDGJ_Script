/**
 * 宠物图鉴 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_23
  	{

    /**
     * 宠物图鉴信息
     */
    public static void write_23_1(MemoryStream msdata)
    {
    }

    /**
     * 幻化
     */
    public static void write_23_3(MemoryStream msdata, uint id, List<uint> autobuy)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeLoopUInt(msdata, autobuy);
    }

    /**
     * 技能图鉴信息
     */
    public static void write_23_4(MemoryStream msdata)
    {
    }

    /**
     * 技能图鉴收集奖励
     */
    public static void write_23_5(MemoryStream msdata, uint num)
    {
        proto_util.writeUInt(msdata, num);
    }
   }
}