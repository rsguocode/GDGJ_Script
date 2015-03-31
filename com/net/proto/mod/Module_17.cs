/**
 * 角色属性培养 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_17
  	{

    /**
     * 培养信息
     */
    public static void write_17_1(MemoryStream msdata)
    {
    }

    /**
     * 培养
     */
    public static void write_17_2(MemoryStream msdata, byte type, bool isFree)
    {
        proto_util.writeUByte(msdata, type);
        proto_util.writeBool(msdata, isFree);
    }
   }
}