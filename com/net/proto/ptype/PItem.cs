/**
 * 部分数据 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PItem
  	{

    public byte key = 0;
    public List<uint> value = new List<uint>();

    public void read(MemoryStream msdata)
    {
        
        key = proto_util.readUByte(msdata);
        proto_util.readLoopUInt(msdata, value);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUByte(msdata, key);
        proto_util.writeLoopUInt(msdata, value);
    }
    
    public static void readLoop(MemoryStream msdata, List<PItem> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PItem _pm = new PItem();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PItem> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PItem ps in p) ps.write(msdata);
        }
    
    
   }
}