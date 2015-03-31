/**
 * vip信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class VipInfoMsg_32_1
  	{

    public byte vip = 0;
    public uint totalDiam = 0;
    public byte rewardStatus = 0;

    public static int getCode()
    {
        // (32, 1)
        return 8193;
    }

    public void read(MemoryStream msdata)
    {
        vip = proto_util.readUByte(msdata);
        totalDiam = proto_util.readUInt(msdata);
        rewardStatus = proto_util.readUByte(msdata);
    }
   }
}