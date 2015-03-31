/**
 * 宠物装备模块 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_24
  	{

    /**
     * 穿装备
     */
    public static void write_24_1(MemoryStream msdata, uint petId, uint equipId, byte pos)
    {
        proto_util.writeUInt(msdata, petId);
        proto_util.writeUInt(msdata, equipId);
        proto_util.writeUByte(msdata, pos);
    }

    /**
     * 宠物装备合成
     */
    public static void write_24_2(MemoryStream msdata, uint id)
    {
        proto_util.writeUInt(msdata, id);
    }
   }
}