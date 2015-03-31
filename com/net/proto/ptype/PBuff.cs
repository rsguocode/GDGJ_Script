/**
 * 角色buff (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PBuff
  	{

    public uint id = 0;
    public byte lvl = 0;
    public byte isDebuff = 0;
    public byte type = 0;
    public ushort val = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        lvl = proto_util.readUByte(msdata);
        isDebuff = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
        val = proto_util.readUShort(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, isDebuff);
        proto_util.writeUByte(msdata, type);
        proto_util.writeUShort(msdata, val);
    }
    
    public static void readLoop(MemoryStream msdata, List<PBuff> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PBuff _pm = new PBuff();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PBuff> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PBuff ps in p) ps.write(msdata);
        }
    
    
   }
}