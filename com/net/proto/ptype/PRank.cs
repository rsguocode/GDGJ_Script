/**
 * 排行榜信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PRank
  	{

    public ushort pos = 0;
    public List<PRankRole> role = new List<PRankRole>();
    public List<uint> data = new List<uint>();
    public List<PGoods> goodsList = new List<PGoods>();

    public void read(MemoryStream msdata)
    {
        
        pos = proto_util.readUShort(msdata);
        PRankRole.readLoop(msdata, role);
        proto_util.readLoopUInt(msdata, data);
        PGoods.readLoop(msdata, goodsList);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUShort(msdata, pos);
        PRankRole.writeLoop(msdata, role);
        proto_util.writeLoopUInt(msdata, data);
        PGoods.writeLoop(msdata, goodsList);
    }
    
    public static void readLoop(MemoryStream msdata, List<PRank> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PRank _pm = new PRank();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PRank> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PRank ps in p) ps.write(msdata);
        }
    
    
   }
}