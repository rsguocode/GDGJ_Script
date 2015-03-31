/**
 * 粗略信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PFarmSimple
  	{

    public uint id = 0;
    public string name = "";
    public byte lvl = 0;
    public uint exp = 0;
    public uint fullExp = 0;
    public byte status = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        lvl = proto_util.readUByte(msdata);
        exp = proto_util.readUInt(msdata);
        fullExp = proto_util.readUInt(msdata);
        status = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUInt(msdata, exp);
        proto_util.writeUInt(msdata, fullExp);
        proto_util.writeUByte(msdata, status);
    }
    
    public static void readLoop(MemoryStream msdata, List<PFarmSimple> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PFarmSimple _pm = new PFarmSimple();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PFarmSimple> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PFarmSimple ps in p) ps.write(msdata);
        }
    
    
   }
}