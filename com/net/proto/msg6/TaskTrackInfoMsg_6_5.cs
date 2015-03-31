/**
 * 进度信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class TaskTrackInfoMsg_6_5
  	{

    public uint taskId = 0;
    public byte state = 0;
    public List<PTaskTrack> track = new List<PTaskTrack>();

    public static int getCode()
    {
        // (6, 5)
        return 1541;
    }

    public void read(MemoryStream msdata)
    {
        taskId = proto_util.readUInt(msdata);
        state = proto_util.readUByte(msdata);
        PTaskTrack.readLoop(msdata, track);
    }
   }
}