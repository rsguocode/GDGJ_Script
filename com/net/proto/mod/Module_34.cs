/**
 * 每日悬赏任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_34
  	{

    /**
     * 悬赏任务信息
     */
    public static void write_34_1(MemoryStream msdata)
    {
    }

    /**
     * 领取奖励
     */
    public static void write_34_2(MemoryStream msdata, ushort type, uint subtype)
    {
        proto_util.writeUShort(msdata, type);
        proto_util.writeUInt(msdata, subtype);
    }
   }
}