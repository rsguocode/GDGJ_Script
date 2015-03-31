/**
 * kv结构 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PKeyValue
  	{

    public uint key = 0;
    public uint val = 0;

    public void read(MemoryStream msdata)
    {
        
        key = proto_util.readUInt(msdata);
        val = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, key);
        proto_util.writeUInt(msdata, val);
    }
    
    public static void readLoop(MemoryStream msdata, List<PKeyValue> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PKeyValue _pm = new PKeyValue();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PKeyValue> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PKeyValue ps in p) ps.write(msdata);
        }
    
    
   }
}