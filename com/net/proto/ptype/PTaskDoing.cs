/**
 * 进行中任务 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PTaskDoing
  	{

    public uint taskId = 0;
    public byte state = 0;
    public List<PTaskTrack> track = new List<PTaskTrack>();

    public void read(MemoryStream msdata)
    {
        
        taskId = proto_util.readUInt(msdata);
        state = proto_util.readUByte(msdata);
        PTaskTrack.readLoop(msdata, track);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, taskId);
        proto_util.writeUByte(msdata, state);
        PTaskTrack.writeLoop(msdata, track);
    }
    
    public static void readLoop(MemoryStream msdata, List<PTaskDoing> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PTaskDoing _pm = new PTaskDoing();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PTaskDoing> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PTaskDoing ps in p) ps.write(msdata);
        }
    
    
   }
}