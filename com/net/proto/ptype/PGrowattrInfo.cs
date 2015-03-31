/**
 * 培育类型 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGrowattrInfo
  	{

    public byte type = 0;
    public ushort freeTimes = 0;
    public uint freeCd = 0;

    public void read(MemoryStream msdata)
    {
        
        type = proto_util.readUByte(msdata);
        freeTimes = proto_util.readUShort(msdata);
        freeCd = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, type);
        proto_util.writeUShort(msdata, freeTimes);
        proto_util.writeUInt(msdata, freeCd);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGrowattrInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGrowattrInfo _pm = new PGrowattrInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGrowattrInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGrowattrInfo ps in p) ps.write(msdata);
        }
    
    
   }
}