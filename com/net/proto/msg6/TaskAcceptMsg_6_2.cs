/**
 * 接任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskAcceptMsg_6_2
  	{

    public ushort code = 0;
    public uint taskId = 0;
    public byte state = 0;

    public static int getCode()
    {
        // (6, 2)
        return 1538;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        taskId = proto_util.readUInt(msdata);
        state = proto_util.readUByte(msdata);
    }
   }
}