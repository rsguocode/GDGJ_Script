/**
 * 副本组信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDungeonMapInfo
  	{

    public uint mapid = 0;
    public byte grade = 0;

    public void read(MemoryStream msdata)
    {
        
        mapid = proto_util.readUInt(msdata);
        grade = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, mapid);
        proto_util.writeUByte(msdata, grade);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDungeonMapInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDungeonMapInfo _pm = new PDungeonMapInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDungeonMapInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDungeonMapInfo ps in p) ps.write(msdata);
        }
    
    
   }
}