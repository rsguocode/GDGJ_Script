/**
 * 可接任务列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskVisibleMsg_6_6
  	{

    public List<uint> visible = new List<uint>();

    public static int getCode()
    {
        // (6, 6)
        return 1542;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, visible);
    }
   }
}