/**
 * 奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonRewardMsg_8_5
  	{

    public byte attack = 0;
    public byte hp = 0;
    public byte time = 0;
    public uint exp = 0;
    public uint gold = 0;
    public uint diam = 0;
    public byte idType = 0;
    public List<uint> box = new List<uint>();
    public byte isFirstFinish = 0;

    public static int getCode()
    {
        // (8, 5)
        return 2053;
    }

    public void read(MemoryStream msdata)
    {
        attack = proto_util.readUByte(msdata);
        hp = proto_util.readUByte(msdata);
        time = proto_util.readUByte(msdata);
        exp = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
        diam = proto_util.readUInt(msdata);
        idType = proto_util.readUByte(msdata);
        proto_util.readLoopUInt(msdata, box);
        isFirstFinish = proto_util.readUByte(msdata);
    }
   }
}