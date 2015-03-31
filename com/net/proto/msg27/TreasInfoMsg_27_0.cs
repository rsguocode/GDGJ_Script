/**
 * 寻宝信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TreasInfoMsg_27_0
  	{

    public byte remainTimes = 0;
    public byte refresh1 = 0;
    public byte refresh2 = 0;
    public byte refresh3 = 0;
    public byte take1 = 0;
    public byte take2 = 0;
    public byte take3 = 0;

    public static int getCode()
    {
        // (27, 0)
        return 6912;
    }

    public void read(MemoryStream msdata)
    {
        remainTimes = proto_util.readUByte(msdata);
        refresh1 = proto_util.readUByte(msdata);
        refresh2 = proto_util.readUByte(msdata);
        refresh3 = proto_util.readUByte(msdata);
        take1 = proto_util.readUByte(msdata);
        take2 = proto_util.readUByte(msdata);
        take3 = proto_util.readUByte(msdata);
    }
   }
}