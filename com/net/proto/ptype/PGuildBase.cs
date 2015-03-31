/**
 * 公会的基本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGuildBase
  	{

    public uint id = 0;
    public string name = "";
    public byte lvl = 0;
    public uint rank = 0;
    public uint ownerId = 0;
    public string ownerName = "";
    public byte memberNum = 0;
    public byte memberMax = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        lvl = proto_util.readUByte(msdata);
        rank = proto_util.readUInt(msdata);
        ownerId = proto_util.readUInt(msdata);
        ownerName = proto_util.readString(msdata);
        memberNum = proto_util.readUByte(msdata);
        memberMax = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUInt(msdata, rank);
        proto_util.writeUInt(msdata, ownerId);
        proto_util.writeString(msdata, ownerName);
        proto_util.writeUByte(msdata, memberNum);
        proto_util.writeUByte(msdata, memberMax);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGuildBase> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGuildBase _pm = new PGuildBase();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGuildBase> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGuildBase ps in p) ps.write(msdata);
        }
    
    
   }
}