/**
 * 种子信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PSeed
  	{

    public uint id = 0;
    public uint num = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        num = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, num);
    }
    
    public static void readLoop(MemoryStream msdata, List<PSeed> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PSeed _pm = new PSeed();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PSeed> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PSeed ps in p) ps.write(msdata);
        }
    
    
   }
}