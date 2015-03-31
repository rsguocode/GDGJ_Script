/**
 * 更新任务接取完成次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskUpdateCountMsg_6_12
  	{

    public byte state = 0;
    public byte type = 0;
    public ushort count = 0;

    public static int getCode()
    {
        // (6, 12)
        return 1548;
    }

    public void read(MemoryStream msdata)
    {
        state = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
        count = proto_util.readUShort(msdata);
    }
   }
}