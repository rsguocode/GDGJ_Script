/**
 * 契约 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_18
  	{

    /**
     * 信息
     */
    public static void write_18_1(MemoryStream msdata)
    {
    }

    /**
     * 培养
     */
    public static void write_18_2(MemoryStream msdata, uint id, uint type)
    {
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, type);
    }
   }
}