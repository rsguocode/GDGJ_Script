/**
 * 英雄榜角色信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PHeroes
  	{

    public ushort pos = 0;
    public uint userid = 0;
    public string name = "";
    public byte job = 0;
    public byte lvl = 0;
    public byte sex = 0;
    public uint fightpoint = 0;

    public void read(MemoryStream msdata)
    {
        
        pos = proto_util.readUShort(msdata);
        userid = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        job = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        fightpoint = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUShort(msdata, pos);
        proto_util.writeUInt(msdata, userid);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUInt(msdata, fightpoint);
    }
    
    public static void readLoop(MemoryStream msdata, List<PHeroes> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PHeroes _pm = new PHeroes();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PHeroes> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PHeroes ps in p) ps.write(msdata);
        }
    
    
   }
}