/**
 * 当前品质和刷新次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class WoodsGradeInfoMsg_26_6
  	{

    public byte grade = 0;
    public byte refreshTimes = 0;

    public static int getCode()
    {
        // (26, 6)
        return 6662;
    }

    public void read(MemoryStream msdata)
    {
        grade = proto_util.readUByte(msdata);
        refreshTimes = proto_util.readUByte(msdata);
    }
   }
}