/**
 * 角色地图信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PMapRole
  	{

    public uint id = 0;
    public string name = "";
    public byte sex = 0;
    public byte level = 0;
    public byte job = 0;
    public uint mapId = 0;
    public ushort x = 0;
    public ushort y = 0;
    public uint hp = 0;
    public uint hpFull = 0;
    public uint petId = 0;
    public List<uint> buffList = new List<uint>();

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
        hp = proto_util.readUInt(msdata);
        hpFull = proto_util.readUInt(msdata);
        petId = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, buffList);
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
        proto_util.writeUInt(msdata, hp);
        proto_util.writeUInt(msdata, hpFull);
        proto_util.writeUInt(msdata, petId);
        proto_util.writeLoopUInt(msdata, buffList);
    }
    
    public static void readLoop(MemoryStream msdata, List<PMapRole> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PMapRole _pm = new PMapRole();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PMapRole> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PMapRole ps in p) ps.write(msdata);
        }
    
    
   }
}