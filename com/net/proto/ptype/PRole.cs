/**
 * 角色信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRole
  	{

    public uint id = 0;
    public string name = "";
    public byte sex = 0;
    public byte level = 0;
    public byte job = 0;
    public uint mapId = 0;
    public ushort x = 0;
    public ushort y = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        sex = proto_util.readUByte(msdata);
        level = proto_util.readUByte(msdata);
        job = proto_util.readUByte(msdata);
        mapId = proto_util.readUInt(msdata);
        x = proto_util.readUShort(msdata);
        y = proto_util.readUShort(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUByte(msdata, level);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUInt(msdata, mapId);
        proto_util.writeUShort(msdata, x);
        proto_util.writeUShort(msdata, y);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRole> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRole _pm = new PRole();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRole> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRole ps in p) ps.write(msdata);
        }
    
    
   }
}