/**
 * 精英副本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PSuperDungeonInfo
  	{

    public uint id = 0;
    public byte grade = 0;
    public byte times = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        grade = proto_util.readUByte(msdata);
        times = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, grade);
        proto_util.writeUByte(msdata, times);
    }
    
    public static void readLoop(MemoryStream msdata, List<PSuperDungeonInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PSuperDungeonInfo _pm = new PSuperDungeonInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PSuperDungeonInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PSuperDungeonInfo ps in p) ps.write(msdata);
        }
    
    
   }
}