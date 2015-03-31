/**
 * 副本奖励 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDungeonReward
  	{

    public uint exp = 0;
    public uint gold = 0;
    public byte idType = 0;
    public List<uint> goodsId = new List<uint>();

    public void read(MemoryStream msdata)
    {
        
        exp = proto_util.readUInt(msdata);
        gold = proto_util.readUInt(msdata);
        idType = proto_util.readUByte(msdata);
        proto_util.readLoopUInt(msdata, goodsId);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, exp);
        proto_util.writeUInt(msdata, gold);
        proto_util.writeUByte(msdata, idType);
        proto_util.writeLoopUInt(msdata, goodsId);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDungeonReward> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDungeonReward _pm = new PDungeonReward();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDungeonReward> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDungeonReward ps in p) ps.write(msdata);
        }
    
    
   }
}