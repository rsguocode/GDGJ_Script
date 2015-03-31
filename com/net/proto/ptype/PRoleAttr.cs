/**
 * 玩家属性信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRoleAttr
  	{

    public uint id = 0;
    public string name = "";
    public byte sex = 0;
    public byte level = 0;
    public byte job = 0;
    public UInt64 exp = 0;
    public UInt64 expFull = 0;
    public byte vip = 0;
    public byte nation = 0;
    public uint gold = 0;
    public uint diam = 0;
    public uint diamBind = 0;
    public uint repu = 0;
    public ushort vigour = 0;
    public ushort vigourFull = 0;
    public byte hasCombine = 0;
    public string customFace = "";
    public List<uint> titleList = new List<uint>();
    public PBaseAttr attr = new PBaseAttr();
    public uint guildId = 0;
    public string guildName = "";

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        sex = proto_util.readUByte(msdata);
        level = proto_util.readUByte(msdata);
        job = proto_util.readUByte(msdata);
        exp = proto_util.readULong(msdata);
        expFull = proto_util.readULong(msdata);
        vip = proto_util.readUByte(msdata);
        nation = proto_util.readUByte(msdata);
        gold = proto_util.readUInt(msdata);
        diam = proto_util.readUInt(msdata);
        diamBind = proto_util.readUInt(msdata);
        repu = proto_util.readUInt(msdata);
        vigour = proto_util.readUShort(msdata);
        vigourFull = proto_util.readUShort(msdata);
        hasCombine = proto_util.readUByte(msdata);
        customFace = proto_util.readString(msdata);
        proto_util.readLoopUInt(msdata, titleList);
        attr.read(msdata);
        guildId = proto_util.readUInt(msdata);
        guildName = proto_util.readString(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUByte(msdata, level);
        proto_util.writeUByte(msdata, job);
        proto_util.writeULong(msdata, exp);
        proto_util.writeULong(msdata, expFull);
        proto_util.writeUByte(msdata, vip);
        proto_util.writeUByte(msdata, nation);
        proto_util.writeUInt(msdata, gold);
        proto_util.writeUInt(msdata, diam);
        proto_util.writeUInt(msdata, diamBind);
        proto_util.writeUInt(msdata, repu);
        proto_util.writeUShort(msdata, vigour);
        proto_util.writeUShort(msdata, vigourFull);
        proto_util.writeUByte(msdata, hasCombine);
        proto_util.writeString(msdata, customFace);
        proto_util.writeLoopUInt(msdata, titleList);
        attr.write(msdata);
        proto_util.writeUInt(msdata, guildId);
        proto_util.writeString(msdata, guildName);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRoleAttr> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRoleAttr _pm = new PRoleAttr();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRoleAttr> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRoleAttr ps in p) ps.write(msdata);
        }
    
    
   }
}