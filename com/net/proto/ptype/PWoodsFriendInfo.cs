/**
 * 金银岛朋友信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PWoodsFriendInfo
  	{

    public uint id = 0;
    public string name = "";
    public byte lvl = 0;
    public byte job = 0;
    public byte remainTimes = 0;
    public uint fightPoint = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        lvl = proto_util.readUByte(msdata);
        job = proto_util.readUByte(msdata);
        remainTimes = proto_util.readUByte(msdata);
        fightPoint = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUByte(msdata, remainTimes);
        proto_util.writeUInt(msdata, fightPoint);
    }
    
    public static void readLoop(MemoryStream msdata, List<PWoodsFriendInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PWoodsFriendInfo _pm = new PWoodsFriendInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PWoodsFriendInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PWoodsFriendInfo ps in p) ps.write(msdata);
        }
    
    
   }
}