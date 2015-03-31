/**
 * 任务次数 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskTaskCountMsg_6_9
  	{

    public ushort dLoopAccept = 0;
    public ushort dLoopFinish = 0;
    public ushort wLoopAccept = 0;
    public ushort wLoopFinish = 0;

    public static int getCode()
    {
        // (6, 9)
        return 1545;
    }

    public void read(MemoryStream msdata)
    {
        dLoopAccept = proto_util.readUShort(msdata);
        dLoopFinish = proto_util.readUShort(msdata);
        wLoopAccept = proto_util.readUShort(msdata);
        wLoopFinish = proto_util.readUShort(msdata);
    }
   }
}