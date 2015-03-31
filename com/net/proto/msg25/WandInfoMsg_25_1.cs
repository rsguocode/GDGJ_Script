/**
 * 魔杖活动信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WandInfoMsg_25_1
  	{

    public byte free = 0;
    public ushort diam = 0;
    public uint totalGold = 0;
    public byte canBuyTimes = 0;
    public byte canUseTimes = 0;
    public List<PWandType> spriteList = new List<PWandType>();

    public static int getCode()
    {
        // (25, 1)
        return 6401;
    }

    public void read(MemoryStream msdata)
    {
        free = proto_util.readUByte(msdata);
        diam = proto_util.readUShort(msdata);
        totalGold = proto_util.readUInt(msdata);
        canBuyTimes = proto_util.readUByte(msdata);
        canUseTimes = proto_util.readUByte(msdata);
        PWandType.readLoop(msdata, spriteList);
    }
   }
}