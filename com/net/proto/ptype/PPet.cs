/**
 * 宠物信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PPet
  	{

    public uint id = 0;
    public uint petId = 0;
    public byte state = 0;
    public uint lvl = 0;
    public uint exp = 0;
    public uint star = 0;
    public byte grade = 0;
    public List<uint> skills = new List<uint>();
    public List<uint> equip = new List<uint>();

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        petId = proto_util.readUInt(msdata);
        state = proto_util.readUByte(msdata);
        lvl = proto_util.readUInt(msdata);
        exp = proto_util.readUInt(msdata);
        star = proto_util.readUInt(msdata);
        grade = proto_util.readUByte(msdata);
        proto_util.readLoopUInt(msdata, skills);
        proto_util.readLoopUInt(msdata, equip);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, petId);
        proto_util.writeUByte(msdata, state);
        proto_util.writeUInt(msdata, lvl);
        proto_util.writeUInt(msdata, exp);
        proto_util.writeUInt(msdata, star);
        proto_util.writeUByte(msdata, grade);
        proto_util.writeLoopUInt(msdata, skills);
        proto_util.writeLoopUInt(msdata, equip);
    }
    
    public static void readLoop(MemoryStream msdata, List<PPet> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PPet _pm = new PPet();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PPet> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PPet ps in p) ps.write(msdata);
        }
    
    
   }
}