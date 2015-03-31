/**
 * 装备鉴定属性 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PSuitAttr
  	{

    public byte id = 0;
    public ushort attr = 0;
    public byte pos = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUByte(msdata);
        attr = proto_util.readUShort(msdata);
        pos = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, id);
        proto_util.writeUShort(msdata, attr);
        proto_util.writeUByte(msdata, pos);
    }
    
    public static void readLoop(MemoryStream msdata, List<PSuitAttr> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PSuitAttr _pm = new PSuitAttr();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PSuitAttr> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PSuitAttr ps in p) ps.write(msdata);
        }
    
    
   }
}