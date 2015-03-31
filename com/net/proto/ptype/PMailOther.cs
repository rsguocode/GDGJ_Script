/**
 * 角色邮件其他数据 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PMailOther
  	{

    public byte type = 0;
    public string data = "";

    public void read(MemoryStream msdata)
    {
        
        type = proto_util.readUByte(msdata);
        data = proto_util.readString(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, type);
        proto_util.writeString(msdata, data);
    }
    
    public static void readLoop(MemoryStream msdata, List<PMailOther> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PMailOther _pm = new PMailOther();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PMailOther> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PMailOther ps in p) ps.write(msdata);
        }
    
    
   }
}