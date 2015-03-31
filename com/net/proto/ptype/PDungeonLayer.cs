/**
 * 世界地图信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDungeonLayer
  	{

    public uint layerid = 0;
    public List<uint> info = new List<uint>();

    public void read(MemoryStream msdata)
    {
        
        layerid = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, info);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, layerid);
        proto_util.writeLoopUInt(msdata, info);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDungeonLayer> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDungeonLayer _pm = new PDungeonLayer();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDungeonLayer> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDungeonLayer ps in p) ps.write(msdata);
        }
    
    
   }
}