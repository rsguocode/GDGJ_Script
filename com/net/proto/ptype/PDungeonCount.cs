/**
 * 副本组计数信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDungeonCount
  	{

    public ushort groupid = 0;
    public byte count = 0;

    public void read(MemoryStream msdata)
    {
        
        groupid = proto_util.readUShort(msdata);
        count = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUShort(msdata, groupid);
        proto_util.writeUByte(msdata, count);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDungeonCount> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDungeonCount _pm = new PDungeonCount();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDungeonCount> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDungeonCount ps in p) ps.write(msdata);
        }
    
    
   }
}