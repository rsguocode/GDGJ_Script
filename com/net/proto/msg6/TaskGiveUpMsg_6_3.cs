/**
 * 放弃任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskGiveUpMsg_6_3
  	{

    public ushort code = 0;
    public uint taskId = 0;

    public static int getCode()
    {
        // (6, 3)
        return 1539;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        taskId = proto_util.readUInt(msdata);
    }
   }
}