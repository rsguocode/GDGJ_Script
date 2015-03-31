/**
 * 魔杖普通开奖玩家信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PWandCommonInfo
  	{

    public uint id = 0;
    public string str = "";
    public byte sex = 0;
    public byte job = 0;
    public byte lvl = 0;
    public byte vip = 0;
    public uint prize = 0;
    public uint num = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        str = proto_util.readString(msdata);
        sex = proto_util.readUByte(msdata);
        job = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
        vip = proto_util.readUByte(msdata);
        prize = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, str);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, vip);
        proto_util.writeUInt(msdata, prize);
        proto_util.writeUInt(msdata, num);
    }
    
    public static void readLoop(MemoryStream msdata, List<PWandCommonInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PWandCommonInfo _pm = new PWandCommonInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PWandCommonInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PWandCommonInfo ps in p) ps.write(msdata);
        }
    
    
   }
}