/**
 * 悬赏任务列表推送 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskRecruitListPushMsg_6_17
  	{

    public ushort code = 0;
    public List<PRecruit> tasklist = new List<PRecruit>();
    public uint time = 0;

    public static int getCode()
    {
        // (6, 17)
        return 1553;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PRecruit.readLoop(msdata, tasklist);
        time = proto_util.readUInt(msdata);
    }
   }
}