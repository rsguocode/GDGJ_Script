/**
 * 公会玩家信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGuildMember
  	{

    public uint id = 0;
    public string name = "";
    public byte vip = 0;
    public byte sex = 0;
    public byte lvl = 0;
    public byte position = 0;
    public byte contribution = 0;
    public uint lastLogoutTime = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        vip = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
        position = proto_util.readUByte(msdata);
        contribution = proto_util.readUByte(msdata);
        lastLogoutTime = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, vip);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, position);
        proto_util.writeUByte(msdata, contribution);
        proto_util.writeUInt(msdata, lastLogoutTime);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGuildMember> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGuildMember _pm = new PGuildMember();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGuildMember> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGuildMember ps in p) ps.write(msdata);
        }
    
    
   }
}