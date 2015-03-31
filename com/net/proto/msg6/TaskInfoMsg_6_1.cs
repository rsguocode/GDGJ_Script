/**
 * 任务信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskInfoMsg_6_1
  	{

    public ushort code = 0;
    public uint nextId = 0;
    public List<PTaskDoing> taskDoing = new List<PTaskDoing>();
    public List<uint> visible = new List<uint>();

    public static int getCode()
    {
        // (6, 1)
        return 1537;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        nextId = proto_util.readUInt(msdata);
        PTaskDoing.readLoop(msdata, taskDoing);
        proto_util.readLoopUInt(msdata, visible);
    }
   }
}