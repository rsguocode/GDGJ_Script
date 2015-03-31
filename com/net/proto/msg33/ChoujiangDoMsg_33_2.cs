/**
 * 抽奖吧，骚年 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChoujiangDoMsg_33_2
  	{

    public ushort code = 0;
    public byte type = 0;
    public List<PGift> reward = new List<PGift>();

    public static int getCode()
    {
        // (33, 2)
        return 8450;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        PGift.readLoop(msdata, reward);
    }
   }
}