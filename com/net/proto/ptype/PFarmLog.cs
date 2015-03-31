/**
 * 种植日志 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PFarmLog
  	{

    public uint time = 0;
    public uint id = 0;
    public string name = "";
    public byte type = 0;
    public uint goodsid = 0;
    public uint num = 0;

    public void read(MemoryStream msdata)
    {
        
        time = proto_util.readUInt(msdata);
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        type = proto_util.readUByte(msdata);
        goodsid = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, time);
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, type);
        proto_util.writeUInt(msdata, goodsid);
        proto_util.writeUInt(msdata, num);
    }
    
    public static void readLoop(MemoryStream msdata, List<PFarmLog> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PFarmLog _pm = new PFarmLog();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PFarmLog> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PFarmLog ps in p) ps.write(msdata);
        }
    
    
   }
}