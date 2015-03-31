/**
 * 刷新悬赏任务信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskRecruitRefreshMsg_6_15
  	{

    public ushort code = 0;
    public List<PRecruit> tasklist = new List<PRecruit>();

    public static int getCode()
    {
        // (6, 15)
        return 1551;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PRecruit.readLoop(msdata, tasklist);
    }
   }
}