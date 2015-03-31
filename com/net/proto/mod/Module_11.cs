/**
 * 设置 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_11
  	{

    /**
     * 获取设置
     */
    public static void write_11_1(MemoryStream msdata)
    {
    }

    /**
     * 设置内容
     */
    public static void write_11_2(MemoryStream msdata, uint key, uint value)
    {
        proto_util.writeUInt(msdata, key);
        proto_util.writeUInt(msdata, value);
    }
   }
}