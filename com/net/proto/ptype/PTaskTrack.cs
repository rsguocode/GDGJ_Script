/**
 * 任务进度 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PTaskTrack
  	{

    public ushort id = 0;
    public byte type = 0;
    public ushort n = 0;
    public ushort sum = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUShort(msdata);
        type = proto_util.readUByte(msdata);
        n = proto_util.readUShort(msdata);
        sum = proto_util.readUShort(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUShort(msdata, id);
        proto_util.writeUByte(msdata, type);
        proto_util.writeUShort(msdata, n);
        proto_util.writeUShort(msdata, sum);
    }
    
    public static void readLoop(MemoryStream msdata, List<PTaskTrack> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PTaskTrack _pm = new PTaskTrack();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PTaskTrack> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PTaskTrack ps in p) ps.write(msdata);
        }
    
    
   }
}