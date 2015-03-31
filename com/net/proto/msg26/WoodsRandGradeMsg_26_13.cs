/**
 * 随机品质 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsRandGradeMsg_26_13
  	{

    public ushort code = 0;
    public byte grade = 0;
    public byte refreshTimes = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (26, 13)
        return 6669;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        grade = proto_util.readUByte(msdata);
        refreshTimes = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}