/**
 * 悬赏任务信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskRecruitMsg_6_14
  	{

    public ushort code = 0;
    public List<PRecruit> tasklist = new List<PRecruit>();
    public ushort count = 0;
    public uint time = 0;

    public static int getCode()
    {
        // (6, 14)
        return 1550;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PRecruit.readLoop(msdata, tasklist);
        count = proto_util.readUShort(msdata);
        time = proto_util.readUInt(msdata);
    }
   }
}