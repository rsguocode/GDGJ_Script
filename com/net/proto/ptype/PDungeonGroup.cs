/**
 * 副本组信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDungeonGroup
  	{

    public uint mapid = 0;
    public byte grade = 0;
    public byte reward = 0;

    public void read(MemoryStream msdata)
    {
        
        mapid = proto_util.readUInt(msdata);
        grade = proto_util.readUByte(msdata);
        reward = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, mapid);
        proto_util.writeUByte(msdata, grade);
        proto_util.writeUByte(msdata, reward);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDungeonGroup> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDungeonGroup _pm = new PDungeonGroup();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDungeonGroup> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDungeonGroup ps in p) ps.write(msdata);
        }
    
    
   }
}