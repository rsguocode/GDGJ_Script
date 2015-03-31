/**
 * 循环任务信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskLoopInfoMsg_6_11
  	{

    public ushort code = 0;
    public byte type = 0;
    public uint taskId = 0;
    public ushort ratio = 0;

    public static int getCode()
    {
        // (6, 11)
        return 1547;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        taskId = proto_util.readUInt(msdata);
        ratio = proto_util.readUShort(msdata);
    }
   }
}