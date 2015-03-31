/**
 * 角色登陆信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PLoginInfo
  	{

    public uint id = 0;
    public string name = "";
    public byte job = 0;
    public byte level = 0;
    public byte sex = 0;
    public uint lastLoginTime = 0;
    public uint serverId = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        job = proto_util.readUByte(msdata);
        level = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        lastLoginTime = proto_util.readUInt(msdata);
        serverId = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUByte(msdata, level);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUInt(msdata, lastLoginTime);
        proto_util.writeUInt(msdata, serverId);
    }
    
    public static void readLoop(MemoryStream msdata, List<PLoginInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PLoginInfo _pm = new PLoginInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PLoginInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PLoginInfo ps in p) ps.write(msdata);
        }
    
    
   }
}