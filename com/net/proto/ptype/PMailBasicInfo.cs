/**
 * 邮件基本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PMailBasicInfo
  	{

    public uint id = 0;
    public uint sendId = 0;
    public string sendName = "";
    public byte status = 0;
    public byte type = 0;
    public string title = "";
    public uint sendTime = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        sendId = proto_util.readUInt(msdata);
        sendName = proto_util.readString(msdata);
        status = proto_util.readUByte(msdata);
        type = proto_util.readUByte(msdata);
        title = proto_util.readString(msdata);
        sendTime = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, sendId);
        proto_util.writeString(msdata, sendName);
        proto_util.writeUByte(msdata, status);
        proto_util.writeUByte(msdata, type);
        proto_util.writeString(msdata, title);
        proto_util.writeUInt(msdata, sendTime);
    }
    
    public static void readLoop(MemoryStream msdata, List<PMailBasicInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PMailBasicInfo _pm = new PMailBasicInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PMailBasicInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PMailBasicInfo ps in p) ps.write(msdata);
        }
    
    
   }
}