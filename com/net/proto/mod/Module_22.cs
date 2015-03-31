/**
 * 世界BOSS (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_22
  	{

    /**
     * 鼓舞
     */
    public static void write_22_4(MemoryStream msdata, byte type)
    {
        proto_util.writeUByte(msdata, type);
    }

    /**
     * 角色攻击
     */
    public static void write_22_10(MemoryStream msdata, uint hurt)
    {
        proto_util.writeUInt(msdata, hurt);
    }
   }
}