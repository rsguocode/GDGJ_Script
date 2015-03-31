/**
 * 田地信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PLand
  	{

    public byte pos = 0;
    public byte color = 0;
    public uint seedId = 0;
    public uint remainTime = 0;
    public uint num = 0;
    public bool canSteal = false;
    public byte status = 0;

    public void read(MemoryStream msdata)
    {
        
        pos = proto_util.readUByte(msdata);
        color = proto_util.readUByte(msdata);
        seedId = proto_util.readUInt(msdata);
        remainTime = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
        canSteal = proto_util.readBool(msdata);
        status = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, pos);
        proto_util.writeUByte(msdata, color);
        proto_util.writeUInt(msdata, seedId);
        proto_util.writeUInt(msdata, remainTime);
        proto_util.writeUInt(msdata, num);
        proto_util.writeBool(msdata, canSteal);
        proto_util.writeUByte(msdata, status);
    }
    
    public static void readLoop(MemoryStream msdata, List<PLand> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PLand _pm = new PLand();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PLand> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PLand ps in p) ps.write(msdata);
        }
    
    
   }
}