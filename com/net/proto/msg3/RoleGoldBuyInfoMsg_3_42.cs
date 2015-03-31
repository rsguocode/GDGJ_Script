/**
 * 购买金币信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleGoldBuyInfoMsg_3_42
  	{

    public ushort code = 0;
    public ushort times = 0;
    public ushort remainTimes = 0;
    public ushort diam = 0;
    public uint gold = 0;
    public ushort batchDiam = 0;

    public static int getCode()
    {
        // (3, 42)
        return 810;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        times = proto_util.readUShort(msdata);
        remainTimes = proto_util.readUShort(msdata);
        diam = proto_util.readUShort(msdata);
        gold = proto_util.readUInt(msdata);
        batchDiam = proto_util.readUShort(msdata);
    }
   }
}