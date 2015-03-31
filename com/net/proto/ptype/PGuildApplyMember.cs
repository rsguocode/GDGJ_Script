/**
 * 申请公会的玩家信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGuildApplyMember
  	{

    public uint id = 0;
    public string name = "";
    public byte lvl = 0;
    public byte sex = 0;
    public byte vip = 0;
    public uint time = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        lvl = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        vip = proto_util.readUByte(msdata);
        time = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUByte(msdata, vip);
        proto_util.writeUInt(msdata, time);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGuildApplyMember> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGuildApplyMember _pm = new PGuildApplyMember();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGuildApplyMember> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGuildApplyMember ps in p) ps.write(msdata);
        }
    
    
   }
}