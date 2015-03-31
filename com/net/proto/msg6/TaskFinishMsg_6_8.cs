/**
 * 立即完成任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskFinishMsg_6_8
  	{

    public ushort code = 0;
    public byte type = 0;

    public static int getCode()
    {
        // (6, 8)
        return 1544;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
    }
   }
}