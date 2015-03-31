/**
 * 伤害 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDamage
  	{

    public uint id = 0;
    public byte type = 0;
    public ushort x = 0;
    public ushort y = 0;
    public uint dmg = 0;
    public uint hp = 0;
    public byte dmgType = 0;
    public byte stateType = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        type = proto_util.readUByte(msdata);
        x = proto_util.readUShort(msdata);
        y = proto_util.readUShort(msdata);
        dmg = proto_util.readUInt(msdata);
        hp = proto_util.readUInt(msdata);
        dmgType = proto_util.readUByte(msdata);
        stateType = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, type);
        proto_util.writeUShort(msdata, x);
        proto_util.writeUShort(msdata, y);
        proto_util.writeUInt(msdata, dmg);
        proto_util.writeUInt(msdata, hp);
        proto_util.writeUByte(msdata, dmgType);
        proto_util.writeUByte(msdata, stateType);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDamage> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDamage _pm = new PDamage();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDamage> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDamage ps in p) ps.write(msdata);
        }
    
    
   }
}