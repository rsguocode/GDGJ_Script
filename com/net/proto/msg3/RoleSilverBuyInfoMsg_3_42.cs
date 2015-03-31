/**
 * 购买银币信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleSilverBuyInfoMsg_3_42
  	{

    public ushort code = 0;
    public ushort times = 0;
    public ushort remainTimes = 0;
    public ushort gold = 0;
    public uint silver = 0;
    public ushort batchGold = 0;

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
        gold = proto_util.readUShort(msdata);
        silver = proto_util.readUInt(msdata);
        batchGold = proto_util.readUShort(msdata);
    }
   }
}