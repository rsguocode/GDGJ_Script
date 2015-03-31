/**
 * 最后完成主线任务id (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskLastMainIdMsg_6_7
  	{

    public uint taskId = 0;

    public static int getCode()
    {
        // (6, 7)
        return 1543;
    }

    public void read(MemoryStream msdata)
    {
        taskId = proto_util.readUInt(msdata);
    }
   }
}