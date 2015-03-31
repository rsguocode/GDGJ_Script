/**
 * 好友信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRelationInfo
  	{

    public uint roleId = 0;
    public string name = "";
    public byte job = 0;
    public byte lvl = 0;
    public byte sex = 0;
    public uint mapid = 0;
    public uint lastLogoutTime = 0;
    public byte isOnline = 0;
    public byte vip = 0;
    public uint intimate = 0;
    public uint fightpoint = 0;

    public void read(MemoryStream msdata)
    {
        
        roleId = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        job = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        mapid = proto_util.readUInt(msdata);
        lastLogoutTime = proto_util.readUInt(msdata);
        isOnline = proto_util.readUByte(msdata);
        vip = proto_util.readUByte(msdata);
        intimate = proto_util.readUInt(msdata);
        fightpoint = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, roleId);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUInt(msdata, mapid);
        proto_util.writeUInt(msdata, lastLogoutTime);
        proto_util.writeUByte(msdata, isOnline);
        proto_util.writeUByte(msdata, vip);
        proto_util.writeUInt(msdata, intimate);
        proto_util.writeUInt(msdata, fightpoint);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRelationInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRelationInfo _pm = new PRelationInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRelationInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRelationInfo ps in p) ps.write(msdata);
        }
    
    
   }
}