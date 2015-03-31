/**
 * 悬赏任务推送 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskRecruitPushMsg_6_16
  	{

    public ushort code = 0;
    public ushort count = 0;
    public List<PRecruit> tasklist = new List<PRecruit>();

    public static int getCode()
    {
        // (6, 16)
        return 1552;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        count = proto_util.readUShort(msdata);
        PRecruit.readLoop(msdata, tasklist);
    }
   }
}