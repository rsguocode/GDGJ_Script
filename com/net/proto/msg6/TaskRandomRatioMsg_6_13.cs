/**
 * 刷新循环任务系数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskRandomRatioMsg_6_13
  	{

    public ushort code = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (6, 13)
        return 1549;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}