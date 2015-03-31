/**
 * 提交任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskCommitMsg_6_4
  	{

    public ushort code = 0;
    public uint taskId = 0;
    public uint nextId = 0;
    public List<uint> visible = new List<uint>();

    public static int getCode()
    {
        // (6, 4)
        return 1540;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        taskId = proto_util.readUInt(msdata);
        nextId = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, visible);
    }
   }
}