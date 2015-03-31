/**
 * 怪物地图信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PMapMon
  	{

    public uint id = 0;
    public uint monid = 0;
    public ushort lvl = 0;
    public byte dir = 0;
    public uint hp = 0;
    public uint hpFull = 0;
    public ushort speed = 0;
    public byte born = 0;
    public byte state = 0;
    public List<uint> buffList = new List<uint>();

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        monid = proto_util.readUInt(msdata);
        lvl = proto_util.readUShort(msdata);
        dir = proto_util.readUByte(msdata);
        hp = proto_util.readUInt(msdata);
        hpFull = proto_util.readUInt(msdata);
        speed = proto_util.readUShort(msdata);
        born = proto_util.readUByte(msdata);
        state = proto_util.readUByte(msdata);
        proto_util.readLoopUInt(msdata, buffList);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, monid);
        proto_util.writeUShort(msdata, lvl);
        proto_util.writeUByte(msdata, dir);
        proto_util.writeUInt(msdata, hp);
        proto_util.writeUInt(msdata, hpFull);
        proto_util.writeUShort(msdata, speed);
        proto_util.writeUByte(msdata, born);
        proto_util.writeUByte(msdata, state);
        proto_util.writeLoopUInt(msdata, buffList);
    }
    
    public static void readLoop(MemoryStream msdata, List<PMapMon> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PMapMon _pm = new PMapMon();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PMapMon> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PMapMon ps in p) ps.write(msdata);
        }
    
    
   }
}