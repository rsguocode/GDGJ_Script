/**
 * 排行榜记录 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PHeroesHistory
  	{

    public byte type = 0;
    public string name = "";
    public uint time = 0;
    public byte result = 0;
    public uint pos = 0;

    public void read(MemoryStream msdata)
    {
        
        type = proto_util.readUByte(msdata);
        name = proto_util.readString(msdata);
        time = proto_util.readUInt(msdata);
        result = proto_util.readUByte(msdata);
        pos = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, type);
        proto_util.writeString(msdata, name);
        proto_util.writeUInt(msdata, time);
        proto_util.writeUByte(msdata, result);
        proto_util.writeUInt(msdata, pos);
    }
    
    public static void readLoop(MemoryStream msdata, List<PHeroesHistory> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PHeroesHistory _pm = new PHeroesHistory();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PHeroesHistory> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PHeroesHistory ps in p) ps.write(msdata);
        }
    
    
   }
}